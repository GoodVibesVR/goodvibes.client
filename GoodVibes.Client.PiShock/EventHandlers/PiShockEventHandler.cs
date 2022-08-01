﻿using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Prism.Events;

namespace GoodVibes.Client.PiShock.EventHandlers
{
    public class PiShockEventHandler
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly PiShockClient _piShockClient;

        public PiShockEventHandler(IEventAggregator eventAggregator, PiShockClient piShockClient)
        {
            _eventAggregator = eventAggregator;
            _piShockClient = piShockClient;
        }

        public void Subscribe()
        {
            _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Subscribe(PiShockCommandReceived);
            _eventAggregator.GetEvent<PiShockToyAddedEventCarrier>().Subscribe(PiShockToyAddedEventHandler);
            _eventAggregator.GetEvent<PiShockToyRemovedEventCarrier>().Subscribe(PiShockToyRemovedEventHandler);
        }

        public void Unsubscribe()
        {
            _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Unsubscribe(PiShockCommandReceived);
            _eventAggregator.GetEvent<PiShockToyAddedEventCarrier>().Unsubscribe(PiShockToyAddedEventHandler);
            _eventAggregator.GetEvent<PiShockToyRemovedEventCarrier>().Unsubscribe(PiShockToyRemovedEventHandler);
        }

        private void PiShockCommandReceived(PiShockCommandEvent obj)
        {
            Console.WriteLine($"PiShock command received: {obj.Command.ToString()}");

            switch (obj.Command)
            {
                case PiShockCommandEnum.Shock:
                    Task.Run(() => _piShockClient.Shock(obj.ShareCode!, obj.Duration, obj.Intensity));
                    break;
                case PiShockCommandEnum.Vibrate:
                    Task.Run(() => _piShockClient.Vibrate(obj.ShareCode!, obj.Duration, obj.Intensity));
                    break;
                case PiShockCommandEnum.Beep:
                    Task.Run(() => _piShockClient.Beep(obj.ShareCode!, obj.Duration));
                    break;
                case PiShockCommandEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PiShockToyAddedEventHandler(PiShockToyAddedEvent obj)
        {
            _piShockClient.AddToy(obj.FriendlyName!, obj.ShareCode!, obj.ToyType);
        }

        private void PiShockToyRemovedEventHandler(PiShockToyRemovedEvent obj)
        {
            _piShockClient.RemoveToy(obj.ShareCode!);
        }
    }
}