using System;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;

namespace PhilipsHue
{
    public class PhilipsHueGroup
    {
        public GroupInfo newGroupInfo { get; set; }
        public delegate void GroupInfo(SimplSharpString name, SimplSharpString type, SimplSharpString allon, SimplSharpString anyOn, SimplSharpString on,
            ushort bri, ushort hue, ushort sat, ushort red, ushort green, ushort blue);

        private int ID;

        private event EventHandler<GroupDataReceivedEventArgs> _OnGroupDataReceived = delegate { };

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

        public string Address
        {
            get { return _address; }
            set
            {
                if (this._address != value)
                {
                    if (PhilipsHueBridge.GroupClient.ContainsKey(Address))
                        PhilipsHueBridge.GroupClient[Address].OnGroupDataReceived -= HandleReceiveData;

                    this._address = value;

                    if (PhilipsHueBridge.RegisterGroupClient(Address))
                        PhilipsHueBridge.GroupClient[Address].OnGroupDataReceived += HandleReceiveData;
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

                //CrestronConsole.PrintLine(body);

                PhilipsHueBridge.SendCommand(String.Format("http://{0}/api/{1}/groups/{2}/action", PhilipsHueBridge.IPAddress, PhilipsHueBridge.Username, ID),
                    body, RequestType.Put, 2, 0);
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public void GetGroup()
        {
            PhilipsHueBridge.SendCommand(String.Format("http://{0}/api/{1}/groups/{2}", PhilipsHueBridge.IPAddress, PhilipsHueBridge.Username, ID), "",
                RequestType.Get, 4, ID);
        }
    }
}