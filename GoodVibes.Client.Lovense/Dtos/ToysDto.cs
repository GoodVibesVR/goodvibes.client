using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Dtos
{
    public class ToyDto
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("nickName")]
        public string? Nickname { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class CommandDto
    {
        public string? Command { get; set; }
        public int Value { get; set; }
    }
}