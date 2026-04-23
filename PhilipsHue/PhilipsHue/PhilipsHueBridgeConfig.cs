using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronDataStore;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;
using System;

namespace PhilipsHue
{
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
