using Newtonsoft.Json;

namespace GoodVibes.Client.Vrchat.Dtos;

public class OscParameterDto
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("input")]
    public OscAddressDto? Input { get; set; }

    [JsonProperty("output")]
    public OscAddressDto? Output { get; set; }
}