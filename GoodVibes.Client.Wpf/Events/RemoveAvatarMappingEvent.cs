using GoodVibes.Client.Events;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;

namespace GoodVibes.Client.Wpf.Events
{
    public class RemoveAvatarMappingEvent : IEvent
    {
        public MappingPointViewModel MappingPoint { get; set; }
    }
}
