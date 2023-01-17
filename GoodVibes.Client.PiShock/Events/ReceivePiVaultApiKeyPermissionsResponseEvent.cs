using GoodVibes.Client.Events;
using Newtonsoft.Json;

namespace GoodVibes.Client.PiShock.Events;

public class ReceivePiVaultApiKeyPermissionsResponseEvent : IEvent
{
    [JsonProperty("apiKey")]
    public Guid ApiKey { get; set; }

    [JsonProperty("change")]
    public bool AllowTimeChange { get; set; }
    
    [JsonProperty("reduction")]
    public bool AllowTimeReduction { get; set; }
    
    [JsonProperty("sessionStart")]
    public bool SessionStart { get; set; }
    
    [JsonProperty("unlock")]
    public bool CanUnlock { get; set; }
}