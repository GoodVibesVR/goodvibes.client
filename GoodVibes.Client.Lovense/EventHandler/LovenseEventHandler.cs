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
            _eventAggregator.GetEvent<LovenseStrengthChangedEventCarrier>().Subscribe(LovenseStrengthChangedReceived);
            _eventAggregator.GetEvent<RemoveLovenseToyEventCarrier>().Subscribe(RemoveLovenseToyReceived);
            _eventAggregator.GetEvent<DisconnectLovenseCommandEventCarrier>().Subscribe(DisconnectLovenseCommandReceived);
        }

        public void Unsubscribe()
        {
            _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Unsubscribe(LovenseCommandReceived);
            _eventAggregator.GetEvent<LovenseStrengthChangedEventCarrier>().Unsubscribe(LovenseStrengthChangedReceived);
            _eventAggregator.GetEvent<RemoveLovenseToyEventCarrier>().Unsubscribe(RemoveLovenseToyReceived);
            _eventAggregator.GetEvent<DisconnectLovenseCommandEventCarrier>().Unsubscribe(DisconnectLovenseCommandReceived);
        }

        private void LovenseCommandReceived(LovenseCommandEvent obj)
        {
            Task.Run(() => _lovenseClient.SendCommand(obj.Command, obj.Value, 0, obj.Toy!));
        }

        private void LovenseStrengthChangedReceived(LovenseStrengthChangedEvent obj)
        {
            Task.Run(() => _lovenseClient.SetStrength(obj.ToyId!, obj.Strength1Percentage, obj.Strength2Percentage));
        }

        private void RemoveLovenseToyReceived(RemoveLovenseToyEvent obj)
        {
            Task.Run(() => _lovenseClient.RemoveToy(obj.ToyId!));
        }

        private void DisconnectLovenseCommandReceived(DisconnectLovenseCommandEvent obj)
        {
            Task.Run(() => _lovenseClient.DisconnectAsync());
        }
    }
}
