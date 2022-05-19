using GoodVibes.Client.Events;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Events
{
    public class LovenseCallbackReceivedEvent : IEvent
    {
        [JsonProperty("uid")]
        public string? Uid { get; set; }

        [JsonProperty("appVersion")]
        public string? AppVersion { get; set; }

        [JsonProperty("toys")]
        public List<ToysDto>? Toys { get; set; }

        [JsonProperty("wssPort")]
        public string? WssPort { get; set; }

        [JsonProperty("httpPort")]
        public string? HttpPort { get; set; }

        [JsonProperty("wsPort")]
        public string? WsPort { get; set; }

        [JsonProperty("appType")]
        public string? AppType { get; set; }

        [JsonProperty("domain")]
        public string? Domain { get; set; }

        [JsonProperty("utoken")]
        public string? UToken { get; set; }

        [JsonProperty("httpsPort")]
        public string? HttpsPort { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("platform")]
        public string? Platform { get; set; }
    }
}
