using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.Dtos;
using Newtonsoft.Json;

namespace GoodVibes.Client.PiShock.Events;

public class ReceivePiVaultLockBoxStatusResponseEvent : IEvent
{
    [JsonProperty("apiKey")]
    public Guid ApiKey { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("online")]
    public bool Online { get; set; }
    
    [JsonProperty("lastPolled")]
    public DateTime LastPolled { get; set; }
    
    [JsonProperty("lastUnlocked")]
    public DateTime? LastUnlocked { get; set; }
    
    [JsonProperty("name")]
    public string? Name { get; set; }
    
    // ReSharper disable once StringLiteralTypo
    [JsonProperty("keyholdersCount")]
    public int KeyHoldersCount { get; set; }
    
    [JsonProperty("userName")]
    public string? Username { get; set; }
    
    [JsonProperty("timesForced")]
    public int TimesForced { get; set; }
    
    [JsonProperty("selfLocking")]
    public bool SelfLocking { get; set; }
    
    [JsonProperty("maxMinutes")]
    public int MaxMinutesOverall { get; set; }
    
    [JsonProperty("maxMinutesSB")]
    public int MaxMinutesSelfBondage { get; set; }
    
    [JsonProperty("normallyUnlocked")]
    public bool NormallyUnlocked { get; set; }
    
    [JsonProperty("timezone")]
    public string? TimeZone { get; set; }
    
    [JsonProperty("hygieneActive")]
    public bool HygieneActive { get; set; }
    
    [JsonProperty("lockedSince")]
    public DateTime? LockedSince { get; set; }
    
    [JsonProperty("lockedUntil")]
    public DateTime? LockedUntil { get; set; }
    
    [JsonProperty("usingEmlalock")]
    public bool UsingEmlalock { get; set; }
    
    [JsonProperty("usingChaster")]
    public bool UsingChaster { get; set; }
    
    [JsonProperty("canUnlock")]
    public bool CanUnlock { get; set; }
    
    [JsonProperty("hygieneSettings")]
    public HygieneSettingsDto? HygieneSettings { get; set; }
    
    [JsonProperty("lastOpened")]
    public DateTime? LastOpened { get; set; }
    
    [JsonProperty("lastClosed")]
    public DateTime? LastClosed { get; set; }
}