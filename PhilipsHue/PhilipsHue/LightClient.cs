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
    public class PhilipsHueLight
    {
        public LightInfo newLightInfo { get; set; }
        public OnTrace newTrace { get; set; }
        public delegate void LightInfo(SimplSharpString name, SimplSharpString type, SimplSharpString on, ushort bri, ushort hue, ushort sat,
            ushort red, ushort green, ushort blue, SimplSharpString reachable);
        public delegate void OnTrace(SimplSharpString trace);

        private int ID;
        private string model;


        private event EventHandler<LightDataReceivedEventArgs> _OnLightDataReceived = delegate { };

        public event EventHandler<LightDataReceivedEventArgs> OnLightDataReceived
        {
            add
            {
                if (!_OnLightDataReceived.GetInvocationList().Contains(value))
                    _OnLightDataReceived += value;
            }
            remove
            {
                _OnLightDataReceived -= value;
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
                    if (PhilipsHueBridge.LightClient.ContainsKey(Address))
                        PhilipsHueBridge.LightClient[Address].OnLightDataReceived -= HandleReceiveData;

                    this._address = value;

                    if (PhilipsHueBridge.RegisterLightClient(Address))
                        PhilipsHueBridge.LightClient[Address].OnLightDataReceived += HandleReceiveData;
                }
            }
        }

        internal void HandleReceiveData(Object sender, LightDataReceivedEventArgs Data)
        {
            ushort red = 0;
            ushort green = 0;
            ushort blue = 0;

            if (Data.XY != null)
            {
                if (float.Parse(Data.XY[0]) > 0)
                {
                    ColourConverter.Point point = new ColourConverter.Point(double.Parse(Data.XY[0]), double.Parse(Data.XY[1]));
                    ColourConverter.RGBColour RGB = ColourConverter.XYtoRGB(point, Data.Model);
                    red = Convert.ToUInt16(RGB.red * 255);
                    green = Convert.ToUInt16(RGB.green * 255);
                    blue = Convert.ToUInt16(RGB.blue * 255);

                }
            }

            newLightInfo(Data.Name, Data.Type, Data.On.ToString(), Data.Bri, Data.Hue, Data.Sat, red, green,
                blue, Data.Reachable.ToString());
            ID = Data.ID;
            model = Data.Model;
        }

        public void LightState(string type, ushort state1, ushort state2, ushort state3)
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
                    double red = state1 / 255.0;
                    double green = state2 / 255.0;
                    double blue = state3 / 255.0;

                    ColourConverter.RGBColour RGB = new ColourConverter.RGBColour(red, green, blue);
                    ColourConverter.Point point = ColourConverter.RgbToXY(RGB, model); 

                    body = String.Format("{{\"xy\":[{0}, {1}]}}", point.x, point.y);
                }

                newTrace(body);

                PhilipsHueBridge.SendCommand(String.Format("http://{0}/api/{1}/lights/{2}/state", PhilipsHueBridge.IPAddress, PhilipsHueBridge.Username, ID),
                    body, RequestType.Put, 2, 0);
            }

            catch (Exception e)
            {
                CrestronConsole.PrintLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public void GetLight()
        {
            PhilipsHueBridge.SendCommand(String.Format("http://{0}/api/{1}/lights/{2}", PhilipsHueBridge.IPAddress, PhilipsHueBridge.Username, ID), "",
                RequestType.Get, 3, ID);
        }
    }
}