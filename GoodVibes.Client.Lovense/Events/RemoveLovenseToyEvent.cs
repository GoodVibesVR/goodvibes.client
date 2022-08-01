using GoodVibes.Client.Events;

namespace GoodVibes.Client.Lovense.Events;

public class RemoveLovenseToyEvent : IEvent
{
    public string? ToyId { get; set; }
}