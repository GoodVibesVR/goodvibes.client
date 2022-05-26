using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Dtos;

public class ResponseDataDto
{
    [JsonProperty("toys")]
    //public Dictionary<string, DetailedToyDto>? Toys { get; set; }
    public string? Toys { get; set; }

    [JsonProperty("platform")]
    public string? Platform { get; set; }

    [JsonProperty("appType")]
    public string? AppType { get; set; }
}