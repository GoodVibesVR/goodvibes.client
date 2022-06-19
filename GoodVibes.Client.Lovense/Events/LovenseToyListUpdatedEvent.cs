using GoodVibes.Client.Events;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Events;

public class LovenseToyListUpdatedEvent : IEvent
{
    public List<LovenseToy>? ToyList { get; set; }
}