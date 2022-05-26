using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Events;

namespace GoodVibes.Client.Lovense.EventHandler
{
    public class LovenseEventHandler
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly LovenseClient _lovenseClient;

        public LovenseEventHandler(IEventAggregator eventAggregator, LovenseClient lovenseClient)
        {
            _eventAggregator = eventAggregator;
            _lovenseClient = lovenseClient;
        }

        public void Subscribe()
        {
            _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Subscribe(LovenseCommandReceived);
        }

        public void Unsubscribe()
        {
            _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Unsubscribe(LovenseCommandReceived);
        }

        private void LovenseCommandReceived(LovenseCommandEvent obj)
        {
            Task.Run(() => _lovenseClient.SendCommand(obj.Command.ToString(), obj.Value, 0, obj.Toy!));
        }
    }
}
