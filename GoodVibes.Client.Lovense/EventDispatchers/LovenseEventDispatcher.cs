using GoodVibes.Client.Events;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Events;

namespace GoodVibes.Client.Lovense.EventDispatchers
{
    public class LovenseEventDispatcher : IDispatchEvent<LovenseCallbackReceivedEvent>, IDispatchEvent<LovenseQrCodeReceivedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        public LovenseEventDispatcher(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Dispatch(LovenseCallbackReceivedEvent e)
        {
            _eventAggregator.GetEvent<LovenseCallbackReceivedEventCarrier>().Publish(e);
            // Toys event here
        }

        public void Dispatch(LovenseQrCodeReceivedEvent e)
        {
            _eventAggregator.GetEvent<LovenseQrCodeReceivedEventCarrier>().Publish(e);
        }
    }
}
