using GoodVibes.Client.Serial.Enums;
using Newtonsoft.Json;

namespace GoodVibes.Client.Serial.Callbacks;

internal class CommandCallback
{
    [JsonProperty("Id")]
    public int Id { get; set; }

    [JsonProperty("Mac")] 
    public string MacAddress { get; set; } = null!;

    [JsonProperty("At")]
    public DateTime SentAt { get; set; }

    [JsonProperty("Status")]
    public int Status { get; set; }

    [JsonProperty("Type")]
    public CallbackCommandEnum Type { get; set; }

    [JsonProperty("Values")]
    public ValuesCallback Values { get; set; } = null!;
}