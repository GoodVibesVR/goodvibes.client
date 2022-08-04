using GoodVibes.Client.Events;

namespace GoodVibes.Client.Wpf.Events;

public class RemoveToyEvent : IEvent
{
    public string ToyId { get; set; }
}