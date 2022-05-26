using GoodVibes.Client.Events;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Events;

public class LovenseQrCodeReceivedEvent : IEvent
{
    [JsonProperty("imageKey")]
    public string? ImageKey { get; set; }

    [JsonProperty("result")]
    public bool Result { get; set; }

    public string? UniqueCode { get; set; }
}