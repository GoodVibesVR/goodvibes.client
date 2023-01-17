using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class SetPiShockMiniShockEvent : IEvent
{
    public string? ShareCode { get; set; }
    public bool MiniShock { get; set; }
}