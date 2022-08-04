using GoodVibes.Client.Events;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Events;

public class DisconnectLovenseCommandEvent : IEvent
{
}

public class UpdateLovenseCacheEvent : IEvent
{
    public List<LovenseToy>? Toys { get; set; }
}