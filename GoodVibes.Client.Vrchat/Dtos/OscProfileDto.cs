using Newtonsoft.Json;

namespace GoodVibes.Client.Vrchat.Dtos
{
    public class VrcOscProfileDto
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("parameters")]
        public OscParameterDto[]? Parameters { get; set; }
    }
}