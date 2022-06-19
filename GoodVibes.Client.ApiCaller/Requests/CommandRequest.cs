using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoodVibes.Client.ApiCaller.Requests;

public class CommandRequest
{
    //[JsonProperty("command")]
    //[JsonConverter(typeof(StringEnumConverter))]
    //public LovenseCommandEnum Command { get; set; }

    [Range(0, 20)]
    [JsonProperty("value1")]
    public int Value1 { get; set; }

    [Range(0, 20)]
    [JsonProperty("value2")]
    public int Value2 { get; set; }

    [JsonProperty("seconds")]
    public int Seconds { get; set; }

    [JsonProperty("toy")]
    public string? Toy { get; set; }
}