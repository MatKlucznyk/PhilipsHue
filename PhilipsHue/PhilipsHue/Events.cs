using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PhilipsHue
{
    public class LightDataReceivedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public bool On { get; set; }
        public ushort Bri { get; set; }
        public ushort Hue { get; set; }
        public ushort Sat { get; set; }
        public IList<string> XY { get; set; }
        public bool Reachable { get; set; }
        public int ID { get; set; }

        /*public LightDataReceivedEventArgs()
        {
        }*/

        public LightDataReceivedEventArgs(string name, string model, string type, bool on, ushort bri, ushort hue, ushort sat, IList<string> xy, bool reachable, int id)
        {
            this.Name = name;
            this.Model = model;
            this.Type = type;
            this.On = on;
            this.Bri = bri;
            this.Hue = hue;
            this.Sat = sat;
            this.XY = xy;
            this.Reachable = reachable;
            this.ID = id;
        }
    }

    public class GroupDataReceivedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool AllOn { get; set; }
        public bool AnyOn { get; set; }
        public bool On { get; set; }
        public ushort Bri { get; set; }
        public ushort Hue { get; set; }
        public ushort Sat { get; set; }
        public IList<string> XY { get; set; }
        public int ID { get; set; }

        /* public GroupDataReceivedEventArgs()
         {
         }*/

        public GroupDataReceivedEventArgs(string name, string type, bool allOn, bool anyOn, bool on, ushort bri, ushort hue, ushort sat, IList<string> xy, int id)
        {
            this.Name = name;
            this.Type = type;
            this.AllOn = allOn;
            this.AnyOn = anyOn;
            this.On = on;
            this.Bri = bri;
            this.Hue = hue;
            this.Sat = sat;
            this.XY = xy;
            this.ID = id;
        }

    }

    public class ClientEvents
    {
        private event EventHandler<LightDataReceivedEventArgs> _OnLightDataReceived = delegate { };
        private event EventHandler<GroupDataReceivedEventArgs> _OnGroupDataReceived = delegate { };

        public event EventHandler<LightDataReceivedEventArgs> OnLightDataReceived
        {
            add
            {
                if (!_OnLightDataReceived.GetInvocationList().Contains(value))
                {
                    _OnLightDataReceived += value;
                }
            }

            remove
            {
                _OnLightDataReceived -= value;
            }
        }

        public event EventHandler<GroupDataReceivedEventArgs> OnGroupDataReceived
        {
            add
            {
                if (!_OnGroupDataReceived.GetInvocationList().Contains(value))
                {
                    _OnGroupDataReceived += value;
                }
            }

            remove
            {
                _OnGroupDataReceived -= value;
            }
        }

        internal void FireOnLightDataChange(LightDataReceivedEventArgs e)
        {
            _OnLightDataReceived(null, e);
        }

        internal void FireOnGroupDataChange(GroupDataReceivedEventArgs e)
        {
            _OnGroupDataReceived(null, e);
        }

    }
}