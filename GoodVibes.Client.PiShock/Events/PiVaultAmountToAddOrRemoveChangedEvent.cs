using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PiVaultAmountToAddOrRemoveChangedEvent : IEvent
{
    public Guid ApiKey { get; set; }
    public int Amount { get; set; }
}