using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.Models;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockToyAddedEvent : IEvent
{
    public string? FriendlyName { get; set; }
    public string? ShareCode { get; set; }
    public Guid? ApiKey { get; set; }
    public ToyTypeEnum ToyType { get; set; }
    public Permissions? Permissions { get; set; }
}