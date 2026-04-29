using System;
using System.Collections.Generic;
using System.Linq;

namespace PhilipsHue
{
    /// <summary>
    /// Provides data for an event that is raised when information about a light device is received.
    /// </summary>
    /// <remarks>This class encapsulates the state and attributes of a light device, such as its name, model,
    /// type, brightness, color settings, and reachability. It is typically used in event handlers to access the latest
    /// data received from a light device.</remarks>
    public class LightDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the model name or identifier.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the type associated with this instance.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the feature is enabled.
        /// </summary>
        public bool On { get; set; }

        /// <summary>
        /// Gets or sets the brightness level.
        /// </summary>
        /// <remarks>The valid range for brightness is typically 0 to 65535, where higher values represent
        /// greater brightness. The exact interpretation of the value may depend on the context in which it is
        /// used.</remarks>
        public ushort Bri { get; set; }

        /// <summary>
        /// Gets or sets the hue value for the color, typically representing the degree on the color wheel in degrees
        /// (0–360).
        /// </summary>
        /// <remarks>The hue determines the base color component and is commonly used in color models such
        /// as HSL or HSV. The valid range for hue is usually 0 to 360, where 0 and 360 both represent red. Values
        /// outside this range may be wrapped or clamped depending on usage.</remarks>
        public ushort Hue { get; set; }

        /// <summary>
        /// Gets or sets the satellite identifier associated with this instance.
        /// </summary>
        public ushort Sat { get; set; }

        /// <summary>
        /// Gets or sets the collection of XY values.
        /// </summary>
        public IList<string> XY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the resource is currently reachable.
        /// </summary>
        public bool Reachable { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Initializes a new instance of the LightDataReceivedEventArgs class with the specified light properties.
        /// </summary>
        /// <param name="name">The name of the light.</param>
        /// <param name="model">The model identifier of the light.</param>
        /// <param name="type">The type of the light (for example, bulb, strip, etc.).</param>
        /// <param name="on">A value indicating whether the light is turned on. Set to <see langword="true"/> if the light is on;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="bri">The brightness level of the light, typically in the range 0 to 65535.</param>
        /// <param name="hue">The hue value of the light, typically in the range 0 to 65535.</param>
        /// <param name="sat">The saturation value of the light, typically in the range 0 to 65535.</param>
        /// <param name="xy">The CIE color coordinates of the light as a list of strings.</param>
        /// <param name="reachable">A value indicating whether the light is currently reachable. <see langword="true"/> if the light can be
        /// controlled; otherwise, <see langword="false"/>.</param>
        /// <param name="id">The unique identifier for the light.</param>
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

    /// <summary>
    /// Provides data for events that report the current state and attributes of a group, such as a collection of
    /// devices or lights.
    /// </summary>
    /// <remarks>This class is typically used as the event data for group state update notifications. It
    /// contains properties describing the group's name, type, power state, brightness, color settings, and identifier.
    /// All properties are populated with the latest values at the time the event is raised.</remarks>
    public class GroupDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type associated with the current instance.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all items are enabled.
        /// </summary>
        public bool AllOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether any items are currently enabled.
        /// </summary>
        public bool AnyOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the feature is enabled.
        /// </summary>
        public bool On { get; set; }

        /// <summary>
        /// Gets or sets the brightness level.
        /// </summary>
        /// <remarks>The valid range for brightness is typically 0 to 65535, where higher values represent
        /// greater brightness. The exact interpretation of the value may depend on the context in which it is
        /// used.</remarks>
        public ushort Bri { get; set; }

        /// <summary>
        /// Gets or sets the hue value for the color, typically representing the degree on the color wheel (0–360).
        /// </summary>
        public ushort Hue { get; set; }

        /// <summary>
        /// Gets or sets the saturation value for the color component.
        /// </summary>
        public ushort Sat { get; set; }

        /// <summary>
        /// Gets or sets the collection of XY values.
        /// </summary>
        public IList<string> XY { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Initializes a new instance of the GroupDataReceivedEventArgs class with the specified group state and
        /// attributes.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <param name="type">The type of the group.</param>
        /// <param name="allOn">true if all lights in the group are on; otherwise, false.</param>
        /// <param name="anyOn">true if any light in the group is on; otherwise, false.</param>
        /// <param name="on">true if the group is considered on; otherwise, false.</param>
        /// <param name="bri">The brightness level of the group. Valid values are typically in the range 0 to 254.</param>
        /// <param name="hue">The hue value of the group. Valid values are typically in the range 0 to 65535.</param>
        /// <param name="sat">The saturation value of the group. Valid values are typically in the range 0 to 254.</param>
        /// <param name="xy">The CIE color coordinates of the group as a list of strings.</param>
        /// <param name="id">The unique identifier for the group.</param>
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

    /// <summary>
    /// Provides events for receiving data updates related to lights and groups from the client.
    /// </summary>
    /// <remarks>Subscribe to the events exposed by this class to handle incoming data for lights or groups as
    /// it is received. Event handlers are only added if they are not already registered, preventing duplicate
    /// subscriptions.</remarks>
    public class ClientEvents
    {
        private event EventHandler<LightDataReceivedEventArgs> _OnLightDataReceived = delegate { };
        private event EventHandler<GroupDataReceivedEventArgs> _OnGroupDataReceived = delegate { };

        /// <summary>
        /// Occurs when new light data is received from the device.
        /// </summary>
        /// <remarks>Subscribe to this event to handle incoming light data as it becomes available. The
        /// event provides a <see cref="LightDataReceivedEventArgs"/> instance containing the received data.</remarks>
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