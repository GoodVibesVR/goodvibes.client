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
            _eventAggregator.GetEvent<PausePiShockEventCarrier>().Subscribe(PausePiShockEventHandler);
            _eventAggregator.GetEvent<PiShockToyAddedEventCarrier>().Subscribe(PiShockToyAddedEventHandler);
            _eventAggregator.GetEvent<RemovePiShockToyEventCarrier>().Subscribe(RemovePiSHockToyEventHandler);
            _eventAggregator.GetEvent<PiShockIntensityChangedEventCarrier>().Subscribe(IntensityChangedEventHandler);
            _eventAggregator.GetEvent<PiShockDurationChangedEventCarrier>().Subscribe(DurationChangedEventHandler);
            _eventAggregator.GetEvent<DisconnectPiShockCommandEventCarrier>().Subscribe(DisconnectPiShockCommandEventHandler);
            _eventAggregator.GetEvent<PiShockSettingsDurationChangedEventCarrier>().Subscribe(PiShockSettingsDurationChangedEventHandler);
            _eventAggregator.GetEvent<PiShockSettingsIntensityChangedEventCarrier>().Subscribe(PiShockSettingsIntensityChangedEventHandler);
            _eventAggregator.GetEvent<SavePiShockCacheEventCarrier>().Subscribe(PiShockSaveCacheEventHandler);
            _eventAggregator.GetEvent<GetPiVaultApiKeyPermissionsEventCarrier>().Subscribe(GetPiVaultApiKeyPermissionsEventHandler);
            _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Subscribe(PiVaultCommandReceived);
            _eventAggregator.GetEvent<PiVaultAmountToAddOrRemoveChangedEventCarrier>().Subscribe(PiVaultAmountToAddOrRemoveEventHandler);
        }

        public void Unsubscribe()
        {
            _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Unsubscribe(PiShockCommandReceived);
            _eventAggregator.GetEvent<PausePiShockEventCarrier>().Unsubscribe(PausePiShockEventHandler);
            _eventAggregator.GetEvent<PiShockToyAddedEventCarrier>().Unsubscribe(PiShockToyAddedEventHandler);
            _eventAggregator.GetEvent<RemovePiShockToyEventCarrier>().Unsubscribe(RemovePiSHockToyEventHandler);
            _eventAggregator.GetEvent<PiShockIntensityChangedEventCarrier>().Unsubscribe(IntensityChangedEventHandler);
            _eventAggregator.GetEvent<PiShockDurationChangedEventCarrier>().Unsubscribe(DurationChangedEventHandler);
            _eventAggregator.GetEvent<DisconnectPiShockCommandEventCarrier>().Unsubscribe(DisconnectPiShockCommandEventHandler);
            _eventAggregator.GetEvent<PiShockSettingsDurationChangedEventCarrier>().Unsubscribe(PiShockSettingsDurationChangedEventHandler);
            _eventAggregator.GetEvent<PiShockSettingsIntensityChangedEventCarrier>().Unsubscribe(PiShockSettingsIntensityChangedEventHandler);
            _eventAggregator.GetEvent<SavePiShockCacheEventCarrier>().Unsubscribe(PiShockSaveCacheEventHandler);
            _eventAggregator.GetEvent<GetPiVaultApiKeyPermissionsEventCarrier>().Unsubscribe(GetPiVaultApiKeyPermissionsEventHandler);
            _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Unsubscribe(PiVaultCommandReceived);
            _eventAggregator.GetEvent<PiVaultAmountToAddOrRemoveChangedEventCarrier>().Unsubscribe(PiVaultAmountToAddOrRemoveEventHandler);
        }

        private void PiVaultCommandReceived(PiVaultCommandEvent obj)
        {
            Console.WriteLine($"PIVault command received: {obj.Command.ToString()}");

            switch (obj.Command)
            {
                case PiVaultCommandEnum.Unlock:
                    Task.Run(() => _piShockClient.UnlockPiVault(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.ClearSession:
                    Task.Run(() => _piShockClient.ClearPiVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.AddMinutes:
                    Task.Run(() => _piShockClient.AddMinutesToPiVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.AddHours:
                    Task.Run(() => _piShockClient.AddHoursToPiVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.AddDays:
                    Task.Run(() => _piShockClient.AddDaysToFromVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.RemoveMinutes:
                    Task.Run(() => _piShockClient.RemoveMinutesFromPiVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.RemoveHours:
                    Task.Run(() => _piShockClient.RemoveHoursFromPiVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.RemoveDays:
                    Task.Run(() => _piShockClient.RemoveDaysFromPiVaultSession(obj.ApiKey));
                    break;
                case PiVaultCommandEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PiShockCommandReceived(PiShockCommandEvent obj)
        {
            Console.WriteLine($"PiShock command received: {obj.Command.ToString()}");

            switch (obj.Command)
            {
                case PiShockCommandEnum.Shock:
                    Task.Run(() => _piShockClient.Shock(obj.ShareCode!));
                    break;
                case PiShockCommandEnum.MiniShock:
                    Task.Run(() => _piShockClient.MiniShock(obj.ShareCode!));
                    break;
                case PiShockCommandEnum.Vibrate:
                    Task.Run(() => _piShockClient.Vibrate(obj.ShareCode!));
                    break;
                case PiShockCommandEnum.Beep:
                    Task.Run(() => _piShockClient.Beep(obj.ShareCode!));
                    break;
                case PiShockCommandEnum.Pause:
                case PiShockCommandEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PausePiShockEventHandler(PausePiShockEvent obj)
        {
            Task.Run(() => _piShockClient.PausePiShock(obj.ShareCode!, obj.Pause));
        }

        private void PiShockToyAddedEventHandler(PiShockToyAddedEvent obj)
        {
            Task.Run(() => _piShockClient.AddToy(obj.FriendlyName!, obj.ShareCode, obj.ApiKey, obj.ToyType, obj.Permissions));
        }

        private void RemovePiSHockToyEventHandler(RemovePiShockToyEvent obj)
        {
            Task.Run(() => _piShockClient.RemoveToy(obj.ToyId!));
        }

        private void IntensityChangedEventHandler(PiShockIntensityChangedEvent obj)
        {
            Task.Run(() => _piShockClient.ChangeIntensity(obj.ToyId!, obj.Intensity));
        }

        private void PiShockSettingsIntensityChangedEventHandler(PiShockIntensityChangedEvent obj)
        {
            Task.Run(() => _piShockClient.ChangeIntensity(obj.ToyId!, obj.Intensity));
        }

        private void DurationChangedEventHandler(PiShockDurationChangedEvent obj)
        {
            Task.Run(() => _piShockClient.ChangeDuration(obj.ToyId!, obj.Duration));
        }

        private void PiShockSettingsDurationChangedEventHandler(PiShockDurationChangedEvent obj)
        {
            Task.Run(() => _piShockClient.ChangeDuration(obj.ToyId!, obj.Duration));
        }

        private void DisconnectPiShockCommandEventHandler(DisconnectPiShockCommandEvent obj)
        {
            Task.Run(() => _piShockClient.DisconnectAsync());
        }

        private void PiShockSaveCacheEventHandler(SavePiShockCacheEvent obj)
        {
            _piShockClient.SaveCache();
        }

        private void GetPiVaultApiKeyPermissionsEventHandler(GetPiVaultApiKeyPermissionsEvent obj)
        {
            Task.Run(() => _piShockClient.GetApiKeyPermissions(obj.ApiKey));
        }

        private void PiVaultAmountToAddOrRemoveEventHandler(PiVaultAmountToAddOrRemoveChangedEvent obj)
        {
            Task.Run(() => _piShockClient.ChangeAmountToAddOrRemove(obj.ApiKey, obj.Amount));
        }
    }
}
