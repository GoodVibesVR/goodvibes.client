using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockToyAddedEvent : IEvent
{
    public string? FriendlyName { get; set; }
    public string? ShareCode { get; set; }
    public ToyTypeEnum ToyType { get; set; }
}