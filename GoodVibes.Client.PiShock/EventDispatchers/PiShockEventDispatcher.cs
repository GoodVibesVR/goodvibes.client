﻿using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Newtonsoft.Json;
using Prism.Events;

namespace GoodVibes.Client.PiShock.EventDispatchers
{
    public class PiShockEventDispatcher : IDispatchEvent<PiShockConnectionAckEvent>, IDispatchEvent<ReceiveShockResponseEvent>, IDispatchEvent<ReceiveVibrateResponseEvent>, IDispatchEvent<ReceiveBeepResponseEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        public PiShockEventDispatcher(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Dispatch(PiShockConnectionAckEvent e)
        {
            Console.WriteLine("Connection to PiShock Command Hub acknowledged...");

            _eventAggregator.GetEvent<PiShockConnectionAckEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveShockResponseEvent e)
        {
            Console.WriteLine($"Shock response received from PiShock:\n{JsonConvert.SerializeObject(e)}");
            
            // TODO: Do things
        }

        public void Dispatch(ReceiveVibrateResponseEvent e)
        {
            Console.WriteLine($"Vibrate response received from PiShock:\n{JsonConvert.SerializeObject(e)}");

            // TODO: Do things
        }

        public void Dispatch(ReceiveBeepResponseEvent e)
        {
            Console.WriteLine($"Beep response received from PiShock:\n{JsonConvert.SerializeObject(e)}");

            // TODO: Do things
        }
    }
}