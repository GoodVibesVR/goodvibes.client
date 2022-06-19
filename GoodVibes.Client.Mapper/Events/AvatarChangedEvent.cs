using GoodVibes.Client.Events;

namespace GoodVibes.Client.Mapper.Events
{
    public class AvatarChangedEvent : IEvent
    {
        public string? AvatarId { get; set; }
    }
}
