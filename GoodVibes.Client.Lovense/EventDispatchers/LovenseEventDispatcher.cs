using GoodVibes.Client.Events;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Events;

namespace GoodVibes.Client.Lovense.EventDispatchers
{
    public class LovenseEventDispatcher : IDispatchEvent<LovenseQrCodeReceivedEvent>, IDispatchEvent<LovenseDeviceAccessibilityEvent>, IDispatchEvent<LovenseToyListUpdatedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        public LovenseEventDispatcher(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Dispatch(LovenseQrCodeReceivedEvent e)
        {
            _eventAggregator.GetEvent<LovenseQrCodeReceivedEventCarrier>().Publish(e);
        }

        public void Dispatch(LovenseDeviceAccessibilityEvent e)
        {
            _eventAggregator.GetEvent<LovenseDeviceAccessibilityEventCarrier>().Publish(e);
        }

        public void Dispatch(LovenseToyListUpdatedEvent e)
        {
            _eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Publish(e);
        }
    }
}
