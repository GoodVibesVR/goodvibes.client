using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.Enums;

namespace GoodVibes.Client.PiShock.Events
{
    public class PiShockCommandEvent : IEvent
    {
        public PiShockCommandEnum Command { get; set; }
        public string? Username { get; set; }
        public string? ShareCode { get; set; }
        public int Duration { get; set; }
        public int Intensity { get; set; }
    }
}
