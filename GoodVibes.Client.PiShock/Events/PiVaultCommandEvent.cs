using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.Enums;

namespace GoodVibes.Client.PiShock.Events;

public class PiVaultCommandEvent : IEvent
{
    public PiVaultCommandEnum Command { get; set; }
    public Guid ApiKey { get; set; }
}