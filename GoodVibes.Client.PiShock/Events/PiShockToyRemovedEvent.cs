using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockToyRemovedEvent : IEvent
{
    public string? ShareCode { get; set; }
}