using Newtonsoft.Json;

namespace GoodVibes.Client.Vrchat.Dtos;

public class OscAddressDto
{
    [JsonProperty("address")]
    public string? Address { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}