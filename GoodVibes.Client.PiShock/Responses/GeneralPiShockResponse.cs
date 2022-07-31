using Newtonsoft.Json;

namespace GoodVibes.Client.PiShock.Responses
{
    public class GeneralPiShockResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
