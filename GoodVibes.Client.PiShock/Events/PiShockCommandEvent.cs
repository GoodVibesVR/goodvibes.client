using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.Enums;

namespace GoodVibes.Client.PiShock.Events
{
    public class PiShockCommandEvent : IEvent
    {
        public PiShockCommandEnum Command { get; set; }
        public string? ShareCode { get; set; }
    }
}
