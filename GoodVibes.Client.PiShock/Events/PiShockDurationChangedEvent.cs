using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockDurationChangedEvent : IEvent
{
    public string? ToyId { get; set; }
    public float Duration { get; set; }
}