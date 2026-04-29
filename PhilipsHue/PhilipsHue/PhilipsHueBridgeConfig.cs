using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronDataStore;
using Crestron.SimplSharp.Net.Https;
using Newtonsoft.Json;
using System;

namespace PhilipsHue
{
    /// <summary>
    /// Represents the configuration and control interface for a Philips Hue Bridge device, providing methods to link a
    /// user and retrieve available lights.
    /// </summary>
    /// <remarks>This class provides high-level operations for interacting with a Philips Hue Bridge, such as
    /// user registration and querying the bridge for connected lights. It is intended to be used as part of a larger
    /// system that manages Philips Hue devices.</remarks>
    public class PhilipsHueBridgeConfig
    {
        /// <summary>
        /// Links the specified user to the Philips Hue Bridge by creating a new user entry if one does not already
        /// exist.
        /// </summary>
        /// <remarks>If the user is already linked, the method retrieves the available lights instead of
        /// creating a new user entry. This method handles exceptions by logging error details to the console.</remarks>
        /// <param name="user">The user name to associate with the Philips Hue Bridge. This value is used to generate a unique device type
        /// identifier.</param>
        public void Link(string user)
        {
            try
            {
                if (CrestronDataStoreStatic.GetLocalStringValue("username", out PhilipsHueBridge.Username) == CrestronDataStore.CDS_ERROR.CDS_RECORD_NOT_FOUND)
                {
                    User u = new User();
                    u.devicetype = string.Format("crestron#{0}", user);
                    string body = JsonConvert.SerializeObject(u);

                    PhilipsHueBridge.SendCommand(String.Format("{0}", PhilipsHueBridge.BaseUrl), body, RequestType.Post, 1, 0);
                }
                else GetLights();
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Sends a request to the Philips Hue Bridge to retrieve information about all available lights.
        /// </summary>
        /// <remarks>This method initiates a GET request to the configured Philips Hue Bridge using the
        /// current username. Any exceptions encountered during the request are printed to the console. The method does
        /// not return the retrieved data directly; callers should ensure that subsequent processing or event handling
        /// is implemented as needed.</remarks>
        public void GetLights()
        {
            try
            {
                PhilipsHueBridge.SendCommand(String.Format("{0}{1}", PhilipsHueBridge.BaseUrl, PhilipsHueBridge.Username), "", RequestType.Get, 1, 0);
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
