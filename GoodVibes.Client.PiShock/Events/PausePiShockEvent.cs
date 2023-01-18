using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PausePiShockEvent : IEvent
{
    public string? ShareCode { get; set; }
    public bool Pause { get; set; }
}