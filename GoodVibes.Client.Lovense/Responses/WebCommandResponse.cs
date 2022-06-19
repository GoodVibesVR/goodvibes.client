using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Responses
{
    public class WebCommandResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
