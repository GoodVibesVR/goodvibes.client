using Newtonsoft.Json;

namespace GoodVibes.Client.ApiCaller.Responses
{
    internal class CommandResponse
    {
        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
