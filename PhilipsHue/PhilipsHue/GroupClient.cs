using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.Net.Https;
using System;
using System.Linq;

namespace PhilipsHue
{
    /// <summary>
    /// Represents a Philips Hue group and provides methods and events for interacting with and controlling the group's
    /// state via a Philips Hue Bridge.
    /// </summary>
    /// <remarks>This class enables communication with a specific Philips Hue group, allowing retrieval and
    /// modification of group state such as power, brightness, hue, saturation, and color. It exposes events for
    /// receiving group data updates and provides integration points for handling group information changes. Thread
    /// safety is not guaranteed; callers should ensure appropriate synchronization if accessing instances from multiple
    /// threads.</remarks>
    public class PhilipsHueGroup
    {
        /// <summary>
        /// Represents a method that receives detailed information about a lighting group, including its name, type,
        /// on/off status, and color attributes.
        /// </summary>
        /// <remarks>This delegate is typically used to provide status updates or query results for
        /// lighting groups in smart lighting systems. The color and brightness parameters reflect the current state of
        /// the group and may be averaged or representative values if the group contains multiple lights.</remarks>
        /// <param name="name">The name of the group.</param>
        /// <param name="type">The type of the group, such as 'Room' or 'Zone'.</param>
        /// <param name="allon">A string indicating whether all lights in the group are on.</param>
        /// <param name="anyOn">A string indicating whether any light in the group is on.</param>
        /// <param name="on">A string representing the overall on/off state of the group.</param>
        /// <param name="bri">The brightness level of the group, as a value between 0 and 254.</param>
        /// <param name="hue">The hue value of the group, as a value between 0 and 65535.</param>
        /// <param name="sat">The saturation level of the group, as a value between 0 and 254.</param>
        /// <param name="red">The red component of the group's color, as a value between 0 and 255.</param>
        /// <param name="green">The green component of the group's color, as a value between 0 and 255.</param>
        /// <param name="blue">The blue component of the group's color, as a value between 0 and 255.</param>
        public delegate void GroupInfo(SimplSharpString name, SimplSharpString type, SimplSharpString allon, SimplSharpString anyOn, SimplSharpString on,
            ushort bri, ushort hue, ushort sat, ushort red, ushort green, ushort blue);

        /// <summary>
        /// Gets or sets information about the newly created group.
        /// </summary>
        public GroupInfo newGroupInfo { get; set; }

        private int ID;

        private event EventHandler<GroupDataReceivedEventArgs> _OnGroupDataReceived = delegate { };

        /// <summary>
        /// Occurs when group data is received from the data source.
        /// </summary>
        /// <remarks>Subscribe to this event to handle incoming group data as it becomes available. Event
        /// handlers receive a <see cref="GroupDataReceivedEventArgs"/> instance containing the group data
        /// details.</remarks>
        public event EventHandler<GroupDataReceivedEventArgs> OnGroupDataReceived
        {
            add
            {
                if (!_OnGroupDataReceived.GetInvocationList().Contains(value))
                    _OnGroupDataReceived += value;
            }
            remove
            {
                _OnGroupDataReceived -= value;
            }
        }

        private string _address = "";

        /// <summary>
        /// Gets or sets the network address associated with the group client connection.
        /// </summary>
        /// <remarks>Changing the address will unregister the previous group client and register a new one
        /// for the specified address. This may affect event subscriptions and group data reception.</remarks>
        public string Address
        {
            get { return _address; }
            set
            {
                var address = _address;
                if (address != value)
                {
                    if (PhilipsHueBridge.TryGetGroupClient(address, out var client))
                        client.OnGroupDataReceived -= HandleReceiveData;

                    _address = value;

                    if (PhilipsHueBridge.RegisterGroupClient(value))
                    {
                        PhilipsHueBridge.TryGetGroupClient(value, out var newClient);
                        newClient.OnGroupDataReceived += HandleReceiveData;
                    }
                }
            }
        }

        internal void HandleReceiveData(Object sender, GroupDataReceivedEventArgs Data)
        {
            ushort red = 0;
            ushort green = 0;
            ushort blue = 0;

            if (float.Parse(Data.XY[0]) > 0)
            {
                ColourConverter.Point point = new ColourConverter.Point(double.Parse(Data.XY[0]), double.Parse(Data.XY[1]));
                ColourConverter.RGBColour RGB = ColourConverter.XYtoRGB(point, "LCT001");
                red = Convert.ToUInt16(RGB.red * 255);
                green = Convert.ToUInt16(RGB.green * 255);
                blue = Convert.ToUInt16(RGB.blue * 255);

            }

            newGroupInfo(Data.Name, Data.Type, Data.AllOn.ToString(), Data.AnyOn.ToString(), Data.On.ToString(), Data.Bri, Data.Hue, Data.Sat, red, green, blue);
            ID = Data.ID;

        }

        /// <summary>
        /// Sends a state change command to a Philips Hue group based on the specified type and state values.
        /// </summary>
        /// <remarks>This method constructs and sends a command to the Philips Hue Bridge to update the
        /// state of a group. For color changes using the "xy" type, all three state parameters are required and
        /// interpreted as RGB color components. For other types, only <paramref name="state1"/> is used. The method
        /// does not throw exceptions but logs errors to the console.</remarks>
        /// <param name="type">The type of group state to set. Supported values are "on", "bri", "hue", "sat", and "xy". Each type
        /// determines how the state parameters are interpreted.</param>
        /// <param name="state1">The primary state value to apply. Its meaning depends on the value of <paramref name="type"/>. For example,
        /// for "on", use 0 for off and 1 for on; for "bri", "hue", or "sat", provide the corresponding value; for "xy",
        /// represents the red color component (0–255).</param>
        /// <param name="state2">The secondary state value, used only when <paramref name="type"/> is "xy". Represents the green color
        /// component (0–255). Ignored for other types.</param>
        /// <param name="state3">The tertiary state value, used only when <paramref name="type"/> is "xy". Represents the blue color
        /// component (0–255). Ignored for other types.</param>
        public void GroupState(string type, ushort state1, ushort state2, ushort state3)
        {
            try
            {
                string body = "";

                if (type == "on")
                {
                    if (state1 == 0) body = "{\"on\":false}";
                    else if (state1 == 1) body = "{\"on\":true}";
                }
                else if (type == "bri") body = String.Format("{{\"bri\":{0}}}", state1);
                else if (type == "hue") body = String.Format("{{\"hue\":{0}}}", state1);
                else if (type == "sat") body = String.Format("{{\"sat\":{0}}}", state1);
                else if (type == "xy")
                {
                    double red = state1 / 255;
                    double green = state2 / 255;
                    double blue = state3 / 255;

                    ColourConverter.RGBColour RGB = new ColourConverter.RGBColour(red, green, blue);
                    ColourConverter.Point point = ColourConverter.RgbToXY(RGB, "LCT001");

                    body = String.Format("{{\"xy\":[{0}, {1}]}}", point.x, point.y);
                }

                PhilipsHueBridge.SendCommand(String.Format("{0}{1}/groups/{2}/action", PhilipsHueBridge.BaseUrl, PhilipsHueBridge.Username, ID),
                    body, RequestType.Put, 2, 0);
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// Sends a request to retrieve information about the current group from the Philips Hue Bridge.
        /// </summary>
        /// <remarks>This method communicates with the Philips Hue Bridge using the configured base URL,
        /// username, and group ID. It does not return a value; any response handling must be implemented separately.
        /// This method may trigger network activity and should be used accordingly in performance-sensitive
        /// scenarios.</remarks>
        public void GetGroup()
        {
            PhilipsHueBridge.SendCommand(String.Format("{0}{1}/groups/{2}", PhilipsHueBridge.BaseUrl, PhilipsHueBridge.Username, ID), "",
                RequestType.Get, 4, ID);
        }
    }
}