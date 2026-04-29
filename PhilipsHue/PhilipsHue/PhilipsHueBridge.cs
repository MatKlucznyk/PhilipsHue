using Avg.Communications.Net;
using Avg.ModuleFramework.Logging;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronDataStore;
using Crestron.SimplSharp.Net.Https;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PhilipsHue
{
    public delegate void SendConfig(SimplSharpString name, SimplSharpString ver);
    public delegate void SendError(SimplSharpString error);
    public delegate void SendLight(ushort id, SimplSharpString name);
    public delegate void SendGroup(ushort id, SimplSharpString name);

    static public class PhilipsHueBridge
    {
        static public string IPAddress;
        static public string Username;

        static public SendConfig SendConfigFn { get; set; }
        static public SendError SendErrorFn { get; set; }
        static public SendLight SendLightFn { get; set; }
        static public SendGroup SendGroupFn { get; set; }

        private static Dictionary<string, ClientEvents> _lightClients = new Dictionary<string, ClientEvents>();
        private static Dictionary<string, ClientEvents> _groupClients = new Dictionary<string, ClientEvents>();
        private static HttpsClientPool _httpsClientPool = new HttpsClientPool();
        private static Logger _logger = new Logger("PhilipsHue");

        internal static string BaseUrl
        {
            get
            {
                return string.Format("https://{0}/api/", IPAddress);
            }
        }

        static public void SendConfigHandler(string name, string ver)
        {
            if (SendConfigFn != null) SendConfigFn(name, ver);
        }

        static public void SendErrorHandler(string error)
        {
            if (SendErrorFn != null) SendErrorFn(error);
        }

        static public void SendLightHandler(ushort id, string name)
        {
            if (SendLightFn != null) SendLightFn(id, name);
        }

        static public bool RegisterLightClient(string Address)
        {
            try
            {
                lock (_lightClients)
                {
                    if (!_lightClients.ContainsKey(Address))
                    {
                        _lightClients.Add(Address, new ClientEvents());
                    }
                }

                return true;
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        static public bool TryGetLightClient(string Address, out ClientEvents client)
        {
            try
            {
                lock (_lightClients)
                {
                    return _lightClients.TryGetValue(Address, out client);
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
                client = null;
                return false;
            }
        }

        static public bool RegisterGroupClient(string Address)
        {
            try
            {
                lock (_groupClients)
                {
                    if (!_groupClients.ContainsKey(Address))
                    {
                        _groupClients.Add(Address, new ClientEvents());
                    }
                }

                return true;
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        static public bool TryGetGroupClient(string Address, out ClientEvents client)
        {
            try
            {
                lock (_groupClients)
                {
                    return _groupClients.TryGetValue(Address, out client);
                }
            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
                client = null;
                return false;
            }
        }

        static public void SendCommand(string url, string body, RequestType requestType, int type, int ID)
        {
            try
            {
                var request = new HttpsClientRequest()
                {
                    RequestType = requestType,
                    ContentString = body
                };

                request.Url.Parse(url);

                var response = _httpsClientPool.SendRequest(Crestron.SimplSharp.Net.AuthMethod.NONE, string.Empty, string.Empty, request, _logger);

                    if (response == null)
                        return;

                    if (response.Status != 200)
                    {
                        SendErrorFn("Error code: " + response.Status);
                        return;
                    }

                var contentString = response.Content;

                if (contentString.Contains("\"error\":{\"type\":1,\"address\":\"\",\"description\":\"unauthorized user\""))
                {
                    SendErrorFn("Unauthorized user");
                    return;
                }

                if (contentString.Contains("\"error\":{\"type\":101,\"address\":\"\",\"description\":\"link button not pressed\""))
                {
                    SendErrorFn("Press link button on bridge and try again");
                    return;
                }

                if (contentString.Contains("[{\"success\":{\"username\":"))
                {
                    var temp = contentString.Split('"');

                    Username = temp[5];

                    CrestronDataStoreStatic.SetLocalStringValue("username", temp[5]);

                    PhilipsHueBridgeConfig GL = new PhilipsHueBridgeConfig();
                    GL.GetLights();
                    return;
                }

                switch (type)
                {
                    case 1:
                        var DD = JsonConvert.DeserializeObject<DataDump>(contentString);

                        SendConfigFn(DD.config.name, DD.config.swversion);

                        foreach (var item in DD.lights)
                        {
                            SendLightFn((ushort)item.Key, DD.lights[item.Key].name);

                            if (_lightClients.ContainsKey(DD.lights[item.Key].name))
                            {
                                if (DD.lights[item.Key].state.hue > 0 || DD.lights[item.Key].state.sat > 0)
                                {
                                    _lightClients[DD.lights[item.Key].name].FireOnLightDataChange(new LightDataReceivedEventArgs(DD.lights[item.Key].name, DD.lights[item.Key].modelid, DD.lights[item.Key].type, DD.lights[item.Key].state.on,
                                        DD.lights[item.Key].state.bri, DD.lights[item.Key].state.hue, DD.lights[item.Key].state.sat, DD.lights[item.Key].state.xy, DD.lights[item.Key].state.reachable, item.Key));
                                }

                                else
                                {
                                    List<string> empty = new List<string>();
                                    empty.Add("0.0");
                                    empty.Add("0.0");

                                    _lightClients[DD.lights[item.Key].name].FireOnLightDataChange(new LightDataReceivedEventArgs(DD.lights[item.Key].name, DD.lights[item.Key].modelid, DD.lights[item.Key].type, DD.lights[item.Key].state.on,
                                        DD.lights[item.Key].state.bri, 0, 0, empty, DD.lights[item.Key].state.reachable, item.Key));
                                }
                            }
                        }

                        foreach (var item in DD.groups)
                        {
                            SendGroupFn((ushort)item.Key, DD.groups[item.Key].name);

                            if (_groupClients.ContainsKey(DD.groups[item.Key].name))
                            {
                                if (DD.groups[item.Key].action.hue > 0 || DD.groups[item.Key].action.sat > 0)
                                {
                                    _groupClients[DD.groups[item.Key].name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(DD.groups[item.Key].name, DD.groups[item.Key].type, DD.groups[item.Key].state.all_on,
                                    DD.groups[item.Key].state.any_on, DD.groups[item.Key].action.on, DD.groups[item.Key].action.bri, DD.groups[item.Key].action.hue, DD.groups[item.Key].action.sat,
                                    DD.groups[item.Key].action.xy, item.Key));
                                }

                                else
                                {
                                    List<string> empty = new List<string>();
                                    empty.Add("0.0");
                                    empty.Add("0.0");

                                    _groupClients[DD.groups[item.Key].name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(DD.groups[item.Key].name, DD.groups[item.Key].type, DD.groups[item.Key].state.all_on,
                                        DD.groups[item.Key].state.any_on, DD.groups[item.Key].action.on, DD.groups[item.Key].action.bri, 0, 0, empty, item.Key));
                                }
                            }
                        }
                        break;

                    case 3:
                        var L = JsonConvert.DeserializeObject<Lights>(contentString);

                        _lightClients[L.name].FireOnLightDataChange(new LightDataReceivedEventArgs(L.name, L.modelid, L.type, L.state.on, L.state.bri, L.state.hue, L.state.sat, L.state.xy, L.state.reachable, ID));
                        break;

                    case 4:
                        var G = JsonConvert.DeserializeObject<Groups>(contentString);

                        _groupClients[G.name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(G.name, G.type, G.state.all_on, G.state.any_on, G.action.on, G.action.bri, G.action.hue, G.action.sat, G.action.xy, ID));
                        break;
                }
            }
            catch (SocketException se)
            {
                ErrorLog.Exception(se.Message, se);
            }
            catch (HttpsException he)
            {
                ErrorLog.Exception(he.Message, he);
            }
            catch (Exception e)
            {
                ErrorLog.Exception(e.Message, e);
            }
        }
    }
}
