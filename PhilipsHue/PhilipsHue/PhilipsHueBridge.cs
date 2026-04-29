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
    /// <summary>
    /// Represents a method that handles sending configuration data identified by name and version.
    /// </summary>
    /// <param name="name">The name of the configuration to send. Cannot be null.</param>
    /// <param name="ver">The version of the configuration to send. Cannot be null.</param>
    public delegate void SendConfig(SimplSharpString name, SimplSharpString ver);

    /// <summary>
    /// Represents the method that handles error messages sent as a SimplSharpString.
    /// </summary>
    /// <param name="error">A SimplSharpString containing the error message to be handled or reported. Cannot be null.</param>
    public delegate void SendError(SimplSharpString error);

    /// <summary>
    /// Represents a method that handles sending a light command with a specified identifier and name.
    /// </summary>
    /// <param name="id">The unique identifier for the light to be sent. Must be a valid ushort value.</param>
    /// <param name="name">The name associated with the light. Cannot be null.</param>
    public delegate void SendLight(ushort id, SimplSharpString name);

    /// <summary>
    /// Represents a method that handles sending a group identifier and name.
    /// </summary>
    /// <param name="id">The unique identifier for the group to send.</param>
    /// <param name="name">The name of the group to send. Cannot be null.</param>
    public delegate void SendGroup(ushort id, SimplSharpString name);

    /// <summary>
    /// Provides static methods and properties for interacting with a Philips Hue Bridge, including sending commands,
    /// handling configuration and error events, and managing light and group clients.
    /// </summary>
    /// <remarks>This class serves as a central interface for communication with a Philips Hue Bridge. It
    /// exposes delegates for handling configuration, error, light, and group events, and provides methods for
    /// registering and retrieving client event handlers. All members are static, and the class is not intended to be
    /// instantiated. Thread safety is ensured for client registration and retrieval methods. Ensure that the IP address
    /// and username are set before sending commands to the bridge.</remarks>
    static public class PhilipsHueBridge
    {
        /// <summary>
        /// Gets or sets the IP address as a string representation.
        /// </summary>
        static public string IPAddress;
        
        /// <summary>
        /// Represents the username associated with the current context.
        /// </summary>
        static public string Username;

        /// <summary>
        /// Gets or sets the delegate used to configure and send data using the SendConfig mechanism.
        /// </summary>
        /// <remarks>Assign this property to customize how configuration data is sent. The delegate should
        /// implement the logic for sending configuration according to application requirements.</remarks>
        static public SendConfig SendConfigFn { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to handle error reporting for send operations.
        /// </summary>
        /// <remarks>Assign a custom delegate to modify how errors are processed or logged during send
        /// operations. This property is static and affects error handling globally for all send operations in the
        /// application.</remarks>
        static public SendError SendErrorFn { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to send light commands to the hardware interface.
        /// </summary>
        /// <remarks>Assign this property to customize how light signals are transmitted. This delegate
        /// should encapsulate the logic for communicating with the underlying hardware. Thread safety depends on the
        /// implementation of the assigned delegate.</remarks>
        static public SendLight SendLightFn { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to send a group of messages as a single operation.
        /// </summary>
        /// <remarks>Use this property to customize how message groups are sent. Assigning a custom
        /// delegate allows integration with different messaging backends or batching strategies.</remarks>
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

        /// <summary>
        /// Invokes the configuration handler delegate with the specified name and version.
        /// </summary>
        /// <remarks>If no configuration handler is assigned, this method performs no action.</remarks>
        /// <param name="name">The name of the configuration to send to the handler. Cannot be null.</param>
        /// <param name="ver">The version of the configuration to send to the handler. Cannot be null.</param>
        static public void SendConfigHandler(string name, string ver)
        {
            if (SendConfigFn != null) SendConfigFn(name, ver);
        }

        /// <summary>
        /// Invokes the configured error handler delegate with the specified error message.
        /// </summary>
        /// <remarks>If no error handler delegate is configured, this method performs no action.</remarks>
        /// <param name="error">The error message to pass to the error handler. Cannot be null.</param>
        static public void SendErrorHandler(string error)
        {
            if (SendErrorFn != null) SendErrorFn(error);
        }

        /// <summary>
        /// Invokes the registered light handler delegate with the specified identifier and name.
        /// </summary>
        /// <remarks>If no handler is registered, this method performs no action.</remarks>
        /// <param name="id">The unique identifier for the light to be handled.</param>
        /// <param name="name">The name associated with the light to be handled.</param>
        static public void SendLightHandler(ushort id, string name)
        {
            if (SendLightFn != null) SendLightFn(id, name);
        }

        /// <summary>
        /// Registers a new light client with the specified address if it is not already registered.
        /// </summary>
        /// <param name="Address">The unique address of the light client to register. Cannot be null or empty.</param>
        /// <returns>true if the client was successfully registered or was already present; otherwise, false if an error
        /// occurred.</returns>
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

        /// <summary>
        /// Attempts to retrieve a light client associated with the specified address.
        /// </summary>
        /// <remarks>This method is thread-safe. If an exception occurs during the lookup, the method
        /// returns false and sets the out parameter to null.</remarks>
        /// <param name="Address">The address used to locate the light client. Cannot be null.</param>
        /// <param name="client">When this method returns, contains the light client associated with the specified address, if found;
        /// otherwise, null. This parameter is passed uninitialized.</param>
        /// <returns>true if a light client with the specified address is found; otherwise, false.</returns>
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

        /// <summary>
        /// Registers a new group client with the specified address if it does not already exist.
        /// </summary>
        /// <remarks>If a client with the specified address is already registered, this method does
        /// nothing and returns true. If an error occurs during registration, the method returns false.</remarks>
        /// <param name="Address">The unique address of the group client to register. Cannot be null or empty.</param>
        /// <returns>true if the group client was successfully registered or already exists; otherwise, false.</returns>
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

        /// <summary>
        /// Attempts to retrieve a group client associated with the specified address.
        /// </summary>
        /// <remarks>This method is thread-safe. If an exception occurs during the lookup, the method
        /// returns false and sets the out parameter to null.</remarks>
        /// <param name="Address">The address used to identify the group client. Cannot be null.</param>
        /// <param name="client">When this method returns, contains the group client associated with the specified address, if found;
        /// otherwise, null. This parameter is passed uninitialized.</param>
        /// <returns>true if a group client with the specified address is found; otherwise, false.</returns>
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

        /// <summary>
        /// Sends a command to the specified URL using the given request type and body, and processes the response based
        /// on the provided type and ID.
        /// </summary>
        /// <remarks>If the response indicates an error, such as unauthorized access or a required action
        /// not being performed, an error handler is invoked. The method processes different response types based on the
        /// value of the type parameter, which may trigger updates to light or group clients. This method does not
        /// return a value and handles exceptions internally by logging them.</remarks>
        /// <param name="url">The URL to which the command is sent. Must be a valid HTTP or HTTPS endpoint.</param>
        /// <param name="body">The request body to include with the command. The format and content depend on the API being targeted.</param>
        /// <param name="requestType">The HTTP request type to use for the command, such as GET, POST, or PUT.</param>
        /// <param name="type">An integer indicating the type of operation to perform with the response. The meaning of each value is
        /// determined by the application logic.</param>
        /// <param name="ID">An identifier used to correlate the command with a specific resource or operation. Its usage depends on the
        /// value of the type parameter.</param>
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

                if (contentString.Contains("unauthorized user"))
                {
                    SendErrorFn("Unauthorized user");
                    return;
                }

                if (contentString.Contains("link button not pressed"))
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
