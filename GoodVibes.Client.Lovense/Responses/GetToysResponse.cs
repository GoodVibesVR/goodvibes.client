using GoodVibes.Client.Lovense.Dtos;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Responses;

public class GetToysResponse
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("data")]
    public ResponseDataDto? Data { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}