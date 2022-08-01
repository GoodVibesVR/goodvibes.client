using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.Models.Abstractions;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockToyListUpdatedEvent : IEvent
{
    public List<PiShockToy>? ToyList { get; set; }
}