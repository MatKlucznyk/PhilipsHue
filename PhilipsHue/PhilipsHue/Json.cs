using System.Collections.Generic;
// For Basic SIMPL# Classes
using Newtonsoft.Json;

namespace PhilipsHue
{
    public class State
    {
        [JsonProperty("on")]
        public bool on { get; set; }
        [JsonProperty("bri")]
        public ushort bri { get; set; }
        [JsonProperty("hue")]
        public ushort hue { get; set; }
        [JsonProperty("sat")]
        public ushort sat { get; set; }
        [JsonProperty("xy")]
        public IList<string> xy { get; set; }
        [JsonProperty("reachable")]
        public bool reachable { get; set; }
    }

    public class Lights
    {
        [JsonProperty("state")]
        public State state { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("modelid")]
        public string modelid { get; set; }
    }

    public class GroupState
    {
        [JsonProperty("all_on")]
        public bool all_on { get; set; }
        [JsonProperty("any_on")]
        public bool any_on { get; set; }
    }

    public class Groups
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("state")]
        public GroupState state { get; set; }
        [JsonProperty("action")]
        public State action { get; set; }
    }

    public class Config
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("swversion")]
        public string swversion { get; set; }
    }

    public class DataDump
    {
        [JsonProperty("lights")]
        public IDictionary<int, Lights> lights { get; set; }
        [JsonProperty("groups")]
        public IDictionary<int, Groups> groups { get; set; }
        [JsonProperty("config")]
        public Config config { get; set; }
    }

    public class User
    {
        [JsonProperty("devicetype")]
        public string devicetype { get; set; }
    }
}