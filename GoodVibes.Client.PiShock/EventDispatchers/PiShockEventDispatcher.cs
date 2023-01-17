using GoodVibes.Client.Events;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Newtonsoft.Json;
using Prism.Events;

namespace GoodVibes.Client.PiShock.EventDispatchers
{
    public class PiShockEventDispatcher : IDispatchEvent<PiShockConnectionAckEvent>, IDispatchEvent<ReceiveShockResponseEvent>, 
        IDispatchEvent<ReceiveVibrateResponseEvent>, IDispatchEvent<ReceiveBeepResponseEvent>, IDispatchEvent<PiShockToyListUpdatedEvent>,
        IDispatchEvent<PiShockDisconnectedEvent>, IDispatchEvent<ReceivePiVaultLockBoxStatusResponseEvent>, IDispatchEvent<ReceivePiVaultLockBoxStatusResponseErrorEvent>,
        IDispatchEvent<ReceivePiVaultApiKeyPermissionsResponseEvent>, IDispatchEvent<ReceivePiVaultApiKeyPermissionsResponseErrorEvent>, IDispatchEvent<ReceiveSetUnlockTimeResponseEvent>, IDispatchEvent<ReceiveClearCurrentSessionResponseEvent>,
        IDispatchEvent<ReceiveUnlockPiVaultResponseEvent>, IDispatchEvent<ReceiveAddMinutesToSessionResponseEvent>, IDispatchEvent<ReceiveRetractMinutesFromSessionResponseEvent>,
        IDispatchEvent<ReceiveAddHoursToSessionResponseEvent>, IDispatchEvent<ReceiveRetractHoursFromSessionResponseEvent>, IDispatchEvent<ReceiveAddDaysToSessionResponseEvent>,
        IDispatchEvent<ReceiveRetractDaysFromSessionResponseEvent>
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

        public void Dispatch(PiShockToyListUpdatedEvent e)
        {
            Console.WriteLine($"PiShock ToyList updated: {JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<PiShockToyListUpdatedEventCarrier>().Publish(e);
        }

        public void Dispatch(PiShockDisconnectedEvent e)
        {
            Console.WriteLine("PiShock disconnected event received");

            _eventAggregator.GetEvent<PiShockDisconnectedEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceivePiVaultLockBoxStatusResponseEvent e)
        {
            Console.WriteLine($"PiVault Lock Box Status Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceivePiVaultLockBoxStatusResponseErrorEvent e)
        {
            Console.WriteLine($"PiVault Lock Box Error Status Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseErrorEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceivePiVaultApiKeyPermissionsResponseEvent e)
        {
            Console.WriteLine($"PiVault Api Key Permissions Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceivePiVaultApiKeyPermissionsResponseErrorEvent e)
        {
            Console.WriteLine($"PiVault Api Key Permissions Error Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseErrorEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveSetUnlockTimeResponseEvent e)
        {
            Console.WriteLine($"PiVault Set Unlock Time Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveSetUnlockTimeResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveClearCurrentSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Clear Current Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveClearCurrentSessionResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveUnlockPiVaultResponseEvent e)
        {
            Console.WriteLine($"PiVault Unlock Pi Vault Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveUnlockPiVaultResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveAddMinutesToSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Add Minutes To Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveAddMinutesToSessionResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveRetractMinutesFromSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Retract Minutes From Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveRetractMinutesFromSessionResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveAddHoursToSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Add Hours To Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveAddHoursToSessionResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveRetractHoursFromSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Retract Hours From Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveRetractHoursFromSessionResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveAddDaysToSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Add Days To Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveAddDaysToSessionResponseEventCarrier>().Publish(e);
        }

        public void Dispatch(ReceiveRetractDaysFromSessionResponseEvent e)
        {
            Console.WriteLine($"PiVault Retract Days From Session Response event received:\n{JsonConvert.SerializeObject(e)}");

            _eventAggregator.GetEvent<ReceiveRetractDaysFromSessionResponseEventCarrier>().Publish(e);
        }
    }
}
