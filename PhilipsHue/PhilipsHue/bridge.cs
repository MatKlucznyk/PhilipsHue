using System;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronDataStore;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;

namespace PhilipsHue
{
    public delegate void SendConfig(SimplSharpString name, SimplSharpString ver);
    public delegate void SendError(SimplSharpString error);
    public delegate void SendLight(ushort id, SimplSharpString name);
    public delegate void SendGRoup(ushort id, SimplSharpString name);

    static public class PhilipsHueBridge
    {
        static public string IPAddress;
        static public string Username;

        static public SendConfig SendConfigFn { get; set; }
        static public SendError SendErrorFn { get; set; }
        static public SendLight SendLightFn { get; set; }
        static public SendGRoup SendGroupFn { get; set; }

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

        static public void SedndLightHandler(ushort id, string name)
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
                using (HttpClient client = new HttpClient())
                {
                    HttpClientRequest request = new HttpClientRequest();

                    client.TimeoutEnabled = true;
                    client.Timeout = 10;
                    request.RequestType = requestType;
                    request.Url.Parse(url);
                    request.ContentString = body;

                    HttpClientResponse response = client.Dispatch(request);

                    if (response.ContentString.Contains("\"error\":{\"type\":1,\"address\":\"\",\"description\":\"unauthorized user\""))
                    {
                        SendErrorFn("Unauthorized user");
                    }

                    else if (response.ContentString.Contains("\"error\":{\"type\":101,\"address\":\"\",\"description\":\"link button not pressed\""))
                    {
                        SendErrorFn("Press link button on bridge and try again");
                    }

                    else if (response.ContentString.Contains("[{\"success\":{\"username\":"))
                    {
                        var temp = response.ContentString.Split('"');

                        Username = temp[5];

                        CrestronDataStoreStatic.SetLocalStringValue("username", temp[5]);

                        PhilipsHueBridgeConfig GL = new PhilipsHueBridgeConfig();
                        GL.GetLights();
                    }

                    else if (type == 1)
                    {
                        DataDump DD = JsonConvert.DeserializeObject<DataDump>(response.ContentString);

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
                    }

                    else if (type == 3)
                    {
                        Lights L = JsonConvert.DeserializeObject<Lights>(response.ContentString);

                        LightClient[L.name].FireOnLightDataChange(new LightDataReceivedEventArgs(L.name, L.modelid, L.type, L.state.on, L.state.bri, L.state.hue, L.state.sat, L.state.xy, L.state.reachable, ID));
                    }

                    else if (type == 4)
                    {
                        Groups G = JsonConvert.DeserializeObject<Groups>(response.ContentString);

                        GroupClient[G.name].FireOnGroupDataChange(new GroupDataReceivedEventArgs(G.name, G.type, G.state.all_on, G.state.any_on, G.action.on, G.action.bri, G.action.hue, G.action.sat, G.action.xy, ID));
                    }
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

    public class PhilipsHueBridgeConfig
    {   
        public void Link(string user)
        {
            try
            {
                if (CrestronDataStoreStatic.GetLocalStringValue("username", out PhilipsHueBridge.Username) == CrestronDataStore.CDS_ERROR.CDS_RECORD_NOT_FOUND)
                {
                    User u = new User();
                    u.devicetype = string.Format("crestron#{0}", user);
                    string body = JsonConvert.SerializeObject(u);

                    PhilipsHueBridge.SendCommand(String.Format("http://{0}/api", PhilipsHueBridge.IPAddress/*, PhilipsHueBridge.Username*/), body, RequestType.Post, 1, 0);
                }
                else GetLights();
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public void GetLights()
        {
            try
            {
                PhilipsHueBridge.SendCommand(String.Format("http://{0}/api/{1}", PhilipsHueBridge.IPAddress, PhilipsHueBridge.Username), "", RequestType.Get, 1, 0);
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
