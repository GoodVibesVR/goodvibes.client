using GoodVibes.Client.Serial.Enums;
using Newtonsoft.Json;

namespace GoodVibes.Client.Serial.Callbacks;

internal class ValuesCallback
{
    [JsonProperty("ShockerId")]
    public int ShockerId { get; set; }

    [JsonProperty("Duration")]
    public int Duration { get; set; }

    [JsonProperty("Intensity")]
    public int Intensity { get; set; }

    [JsonProperty("Method")]
    public CallbackCommandEnum Method { get; set; }

    [JsonProperty("Type")]
    public int Type { get; set; }

    [JsonProperty("Milliseconds")]
    public bool Milliseconds { get; set; }
}