using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockToyAddedEvent : IEvent
{
    public string? ShareCode { get; set; }
    public ToyTypeEnum ToyType { get; set; }
}