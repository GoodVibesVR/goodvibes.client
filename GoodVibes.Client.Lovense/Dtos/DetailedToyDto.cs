using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Dtos;

public class DetailedToyDto
{
    [JsonProperty("version")]
    public string? Version { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("battery")]
    public int? Battery { get; set; }

    [JsonProperty("nickName")]
    public string? Nickname { get; set; }

    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }
}