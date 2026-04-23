using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronDataStore;
using Crestron.SimplSharp.Net.Http;
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

        internal static Dictionary<string, ClientEvents> LightClient = new Dictionary<string, ClientEvents>();
        internal static Dictionary<string, ClientEvents> GroupClient = new Dictionary<string, ClientEvents>();

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
                lock (LightClient)
                {
                    if (!LightClient.ContainsKey(Address))
                    {
                        LightClient.Add(Address, new ClientEvents());
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

        static public bool RegisterGroupClient(string Address)
        {
            try
            {
                lock (GroupClient)
                {
                    if (!GroupClient.ContainsKey(Address))
                    {
                        GroupClient.Add(Address, new ClientEvents());
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

        static public void SendCommand(string url, string body, RequestType requestType, int type, int ID)
        {
            try
            {
                var contentString = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    HttpClientRequest request = new HttpClientRequest();

                    client.TimeoutEnabled = true;
                    client.Timeout = 10;
                    request.RequestType = requestType;
                    request.Url.Parse(url);
                    request.ContentString = body;

                    HttpClientResponse response = client.Dispatch(request);

                    if (response == null)
                        return;

                    if (response.Code != 200)
                    {
                        SendErrorFn("Error code: " + response.Code);
                        return;
                    }

                    contentString = response.ContentString;
                }

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

                            if (LightClient.ContainsKey(DD.lights[item.Key].name))
                            {
                                if (DD.lights[item.Key].state.hue > 0 || DD.lights[item.Key].state.sat > 0)
                                {
                                    LightClient[DD.lights[item.Key].name].FireOnLightDataChange(new LightDataReceivedEventArgs(DD.lights[item.Key].name, DD.lights[item.Key].modelid, DD.lights[item.Key].type, DD.lights[item.Key].state.on,
                                        DD.lights[item.Key].state.bri, DD.lights[item.Key].state.hue, DD.lights[item.Key].state.sat, DD.lights[item.Key].state.xy, DD.lights[item.Key].state.reachable, item.Key));
                                }

                                else
                                {
                                    List<string> empty = new List<string>();
                                    empty.Add("0.0");
                                    empty.Add("0.0");

                                    LightClient[DD.lights[item.Key].name].FireOnLightDataChange(new LightDataReceivedEventArgs(DD.lights[item.Key].name, DD.lights[item.Key].modelid, DD.lights[item.Key].type, DD.lights[item.Key].state.on,
                                        DD.lights[item.Key].state.bri, 0, 0, empty, DD.lights[item.Key].state.reachable, item.Key));
                                }
                            }
                        }

                        foreach (var item in DD.groups)
                        {
                            SendGroupFn((ushort)item.Key, DD.groups[item.Key].name);

                            if (GroupClient.ContainsKey(DD.groups[item.Key].name))
                            {
                                if (DD.groups[item.Key].action.hue > 0 || DD.groups[item.Key].action.sat > 0)
                                {
                                    GroupClient[DD.groups[item.Key].name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(DD.groups[item.Key].name, DD.groups[item.Key].type, DD.groups[item.Key].state.all_on,
                                    DD.groups[item.Key].state.any_on, DD.groups[item.Key].action.on, DD.groups[item.Key].action.bri, DD.groups[item.Key].action.hue, DD.groups[item.Key].action.sat,
                                    DD.groups[item.Key].action.xy, item.Key));
                                }

                                else
                                {
                                    List<string> empty = new List<string>();
                                    empty.Add("0.0");
                                    empty.Add("0.0");

                                    GroupClient[DD.groups[item.Key].name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(DD.groups[item.Key].name, DD.groups[item.Key].type, DD.groups[item.Key].state.all_on,
                                        DD.groups[item.Key].state.any_on, DD.groups[item.Key].action.on, DD.groups[item.Key].action.bri, 0, 0, empty, item.Key));
                                }
                            }
                        }
                        break;

                    case 3:
                        var L = JsonConvert.DeserializeObject<Lights>(contentString);

                        LightClient[L.name].FireOnLightDataChange(new LightDataReceivedEventArgs(L.name, L.modelid, L.type, L.state.on, L.state.bri, L.state.hue, L.state.sat, L.state.xy, L.state.reachable, ID));
                        break;

                    case 4:
                        var G = JsonConvert.DeserializeObject<Groups>(contentString);

                        GroupClient[G.name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(G.name, G.type, G.state.all_on, G.state.any_on, G.action.on, G.action.bri, G.action.hue, G.action.sat, G.action.xy, ID));
                        break;
                }
            }
            catch (SocketException se)
            {
                ErrorLog.Exception(se.Message, se);
            }
            catch (HttpException he)
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
