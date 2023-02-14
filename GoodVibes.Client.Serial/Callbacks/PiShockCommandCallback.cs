using Newtonsoft.Json;

namespace GoodVibes.Client.Serial.Callbacks
{
    internal class PiShockSerialCallback
    {
        [JsonProperty("Commands")]
        public List<CommandCallback> Commands { get; set; } = null!;

        [JsonProperty("ShockersId")]
        public List<int> ShockerIds { get; set; } = null!;

        [JsonProperty("ShockersType")]
        public List<int> ShockerTypes { get; set; } = null!;

        [JsonProperty("LastCommandId")]
        public int LastCommandId { get; set; }

        [JsonProperty("Poll")]
        public int Poll { get; set; }

        [JsonProperty("RequestRefresh")]
        public bool RequestRefresh { get; set; }
    }
}
