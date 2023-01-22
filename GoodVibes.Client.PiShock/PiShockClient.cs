using GoodVibes.Client.Cache;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Common.Extensions;
using GoodVibes.Client.PiShock.Cache;
using GoodVibes.Client.PiShock.EventDispatchers;
using GoodVibes.Client.PiShock.Events;
using GoodVibes.Client.PiShock.Models;
using GoodVibes.Client.PiShock.Models.Abstractions;
using GoodVibes.Client.Settings.Models;
using GoodVibes.Client.SignalR;
using GoodVibes.Client.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace GoodVibes.Client.PiShock
{
    public class PiShockClient : SignalRClient
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly PiShockEventDispatcher _piShockEventDispatcher;
        private readonly GoodVibesCacheManager<PiShockCache> _cacheManager;

        public Dictionary<string, PiShockToy> Toys { get; }

        public bool Connected { get; set; }

        public PiShockClient(ApplicationSettings applicationSettings, PiShockEventDispatcher piShockEventDispatcher, GoodVibesCacheManager<PiShockCache> cacheManager)
        {
            _applicationSettings = applicationSettings;
            _piShockEventDispatcher = piShockEventDispatcher;
            _cacheManager = cacheManager;

            Toys = SetupToyList();
            Connected = false;
        }

        private Dictionary<string, PiShockToy> SetupToyList()
        {
            var toysDict = new Dictionary<string, PiShockToy>();
            var piShockCache = _cacheManager.GetCache();
            if (piShockCache.Toys.Count < 1) return toysDict;

            var toys = piShockCache.Toys;
            foreach (var piShockToy in toys)
            {
                switch (piShockToy)
                {
                    case Models.PiShock piShock:
                        toysDict.Add(piShock.ShareCode!, piShock);
                        break;
                    case PiVault piVault:
                        toysDict.Add(piVault.ApiKey.ToString()!, piVault);
                        break;
                }
            }
            
            return toysDict;
        }

        public async Task ConnectAsync()
        {
            Console.WriteLine($"PiShock ConnectAsync called...");

            await ConnectAsync(
                $"{_applicationSettings.GoodVibesRoot}{_applicationSettings.SignalRSettings!.PiShockHubPath}", () =>
                {
                    Connection!.On<string>(PiShockCommandMethodConstants.ConnectionAck, ReceiveConnectionAcknowledgedHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.Pong, ReceivePongResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.GetPiShockInformationResponse, ReceivePiShockInformationResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.PausePiShockResponse, ReceivePausePiShockResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.ShockResponse, ReceiveShockResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.VibrateResponse, ReceiveVibrateResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.BeepResponse, ReceiveBeepResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.GetPiVaultStatusResponse, ReceivePiVaultStatusResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.GetApiKeyPermissionsResponse, ReceiveApiKeyPermissionsResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.SetUnlockTimeResponse, ReceiveSetUnlockTimeResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.ClearCurrentSessionResponse, ReceiveClearCurrentSessionResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.UnlockPiVaultResponse, ReceiveUnlockPiVaultResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.AddMinutesToSessionResponse, ReceiveAddMinutesToSessionResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.RetractMinutesFromSessionResponse, ReceiveRetractMinutesFromSessionResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.AddHoursToSessionResponse, ReceiveAddHoursToSessionResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.RetractHoursFromSessionResponse, ReceiveRetractHoursFromSessionResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.AddDaysToSessionResponse, ReceiveAddDaysToSessionResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.RetractDaysFromSessionResponse, ReceiveRetractDaysFromSessionResponseHandler);
                });

            Connected = true;
            await Task.Run(HealthCheckTask).ConfigureAwait(false);
        }

        public async Task DisconnectAsync()
        {
            Console.WriteLine($"PiShock DisconnectAsync called...");

            Connected = false;
            await DisconnectAsync(true);

            _piShockEventDispatcher.Dispatch(new PiShockDisconnectedEvent());
        }

        public async Task AddToy(string friendlyName, string? shareCode, Guid? apiKey, ToyTypeEnum toyType, Permissions? permissions)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (toyType)
            {
                case ToyTypeEnum.PiShock:
                    Toys!.Add(shareCode!, new Models.PiShock()
                    {
                        FriendlyName = friendlyName,
                        ShareCode = shareCode,
                        Duration = 2,
                        Intensity = 50
                    });

                    _piShockEventDispatcher.Dispatch(new PiShockToyListUpdatedEvent()
                    {
                        ToyList = Toys.Select(t => t.Value).ToList()
                    });

                    SaveCache();
                    break;
                case ToyTypeEnum.PiVault:
                    Toys!.Add(apiKey!.ToString()!, new PiVault()
                    {
                        ApiKey = apiKey!.Value,
                        Permissions = permissions!
                    });

                    await Connection!.InvokeAsync(PiShockCommandMethodConstants.GetPiVaultStatus,
                        apiKey!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toyType), toyType, null);
            }
        }

        public async Task GetApiKeyPermissions(Guid apiKey)
        {
            await Connection!.InvokeAsync(PiShockCommandMethodConstants.GetApiKeyPermissions, apiKey);
        }

        public Task RemoveToy(string toyId)
        {
            var toyFound = Toys!.TryGetValue(toyId, out var toy);
            if (toyFound)
            {
                Toys.Remove(toyId);
            }

            SaveCache();

            return Task.CompletedTask;
        }

        public Task ChangeIntensity(string toyId, float intensity)
        {
            if (!Toys!.TryGetValue(toyId, out var toy)) return Task.CompletedTask;

            if (toy is Models.PiShock shocker)
            {
                shocker.Intensity = (int)Math.Round((double)(intensity * 100));
            }

            SaveCache();

            return Task.CompletedTask;
        }

        public Task ChangeDuration(string toyId, float duration)
        {
            if (!Toys!.TryGetValue(toyId, out var toy)) return Task.CompletedTask;

            if (toy is Models.PiShock shocker)
            {
                shocker.Duration = (int)Math.Round((double)(duration * 10));
            }

            SaveCache();

            return Task.CompletedTask;
        }

        public async Task Shock(string shareCode)
        {
            if (!Connected) return;
            var found = Toys!.TryGetValue(shareCode, out var toy);
            if (!found) return;

            if (toy is Models.PiShock shocker)
            {
                await Connection!.InvokeAsync(PiShockCommandMethodConstants.Shock, shareCode, shocker.Duration, shocker.Intensity);
            }
        }

        public async Task MiniShock(string shareCode)
        {
            if (!Connected) return;
            var found = Toys!.TryGetValue(shareCode, out var toy);
            if (!found) return;

            if (toy is Models.PiShock shocker)
            {
                await Connection!.InvokeAsync(PiShockCommandMethodConstants.Shock, shareCode, shocker.Duration * 100, shocker.Intensity);
            }
        }

        public async Task Vibrate(string shareCode)
        {
            if (!Connected) return;
            var found = Toys!.TryGetValue(shareCode, out var toy);
            if (!found) return;

            if (toy is Models.PiShock shocker)
            {
                await Connection!.InvokeAsync(PiShockCommandMethodConstants.Vibrate, shareCode, shocker.Duration, shocker.Intensity);
            }
        }

        public async Task PausePiShock(string shareCode, bool pause)
        {
            if (!Connected) return;
            var found = Toys!.TryGetValue(shareCode, out var toy);
            if (!found) return;

            if (toy is Models.PiShock shocker)
            {
                await Connection!.InvokeAsync(PiShockCommandMethodConstants.PausePiShock, shareCode, pause);
            }
        }

        public async Task Beep(string shareCode)
        {
            if (!Connected) return;
            var found = Toys!.TryGetValue(shareCode, out var toy);
            if (!found) return;

            if (toy is Models.PiShock shocker)
            {
                await Connection!.InvokeAsync(PiShockCommandMethodConstants.Beep, shareCode, shocker.Duration);
            }
        }

        private void ReceiveConnectionAcknowledgedHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(new PiShockConnectionAckEvent());
        }

        private void ReceivePongResponseHandler(string messageStr)
        {
            Console.WriteLine($"PING -> Pong response from PiShock hub");
        }

        private void ReceivePiShockInformationResponseHandler(string messageStr)
        {
            var successfulResponse = messageStr.TryParseJson<ReceivePiShockInformationResponseEvent>(out var piShockInformationResponseEvent);
            if (!successfulResponse)
            {
                _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceivePiShockInformationErrorResponseEvent>(messageStr)!);
            }

            var piShockFound = Toys.TryGetValue(piShockInformationResponseEvent.ShareCode!, out var piShockToy);
            if (!piShockFound)
            {
                piShockToy = new Models.PiShock()
                {
                    Id = piShockInformationResponseEvent.Id,
                    ShareCode = piShockInformationResponseEvent.ShareCode
                };
                Toys.Add(piShockInformationResponseEvent.ShareCode!, piShockToy);
            }

            if (piShockToy is not Models.PiShock piShock) return;
            piShock.Id = piShockInformationResponseEvent.Id;
            piShock.Name = piShockInformationResponseEvent.Name;
            piShock.Paused = piShockInformationResponseEvent.Paused;
            piShock.Online = piShockInformationResponseEvent.Online;
            piShock.MaxIntensity = piShockInformationResponseEvent.MaxIntensity;
            piShock.MaxDuration = piShockInformationResponseEvent.MaxDuration;

            _piShockEventDispatcher.Dispatch(piShockInformationResponseEvent);
            _piShockEventDispatcher.Dispatch(new PiShockToyListUpdatedEvent()
            {
                ToyList = Toys.Select(t => t.Value).ToList()
            });
        }

        private void ReceivePausePiShockResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceivePausePiShockResponseEvent>(messageStr)!);
        }

        private void ReceiveShockResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveShockResponseEvent>(messageStr)!);
        }

        private void ReceiveVibrateResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveVibrateResponseEvent>(messageStr)!);
        }

        private void ReceiveBeepResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveBeepResponseEvent>(messageStr)!);
        }

        private void ReceivePiVaultStatusResponseHandler(string messageStr)
        {
            var successfulResponse = messageStr.TryParseJson<ReceivePiVaultLockBoxStatusResponseEvent>(out var piVaultStatusResponseEvent);
            if (!successfulResponse)
            {
                _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceivePiVaultLockBoxStatusResponseErrorEvent>(messageStr)!);
            }

            var piVaultFound = Toys.TryGetValue(piVaultStatusResponseEvent.ApiKey.ToString(), out var piVaultToy);
            if (!piVaultFound)
            {
                piVaultToy = new PiVault()
                {
                    Id = piVaultStatusResponseEvent.Id,
                    ApiKey = piVaultStatusResponseEvent.ApiKey
                };
                Toys.Add(piVaultStatusResponseEvent.ApiKey.ToString(), piVaultToy);
            }

            if (piVaultToy is not PiVault piVault) return;
            piVault.Online = piVaultStatusResponseEvent.Online;
            piVault.Name = piVaultStatusResponseEvent.Name;
            piVault.KeyHoldersCount = piVaultStatusResponseEvent.KeyHoldersCount;
            piVault.Username = piVaultStatusResponseEvent.Username;
            piVault.TimesForced = piVaultStatusResponseEvent.TimesForced;
            piVault.SelfLocking = piVaultStatusResponseEvent.SelfLocking;
            piVault.MaxMinutesOverall = piVaultStatusResponseEvent.MaxMinutesOverall;
            piVault.MaxMinutesSelfBondage = piVaultStatusResponseEvent.MaxMinutesSelfBondage;
            piVault.NormallyUnlocked = piVaultStatusResponseEvent.NormallyUnlocked;
            piVault.TimeZone = piVaultStatusResponseEvent.TimeZone;
            piVault.TimeZoneOffset = piVaultStatusResponseEvent.TzOffset;
            piVault.HygieneActive = piVaultStatusResponseEvent.HygieneActive;
            piVault.UsingEmlalock = piVaultStatusResponseEvent.UsingEmlalock;
            piVault.UsingChaster = piVaultStatusResponseEvent.UsingChaster;
            piVault.CanUnlock = piVaultStatusResponseEvent.CanUnlock;
            piVault.HygieneSettings = piVaultStatusResponseEvent.HygieneSettings != null ? new HygieneSettings()
            {
                Active = piVaultStatusResponseEvent.HygieneSettings.Active,
                CronExpression = piVaultStatusResponseEvent.HygieneSettings.CronExpression,
                Days = piVaultStatusResponseEvent.HygieneSettings.Days,
                Hours = piVaultStatusResponseEvent.HygieneSettings.Hours,
                Minutes = piVaultStatusResponseEvent.HygieneSettings.Minutes,
                Duration = piVaultStatusResponseEvent.HygieneSettings.Duration
            } : null;
            piVault.LastPolled = piVaultStatusResponseEvent.LastPolled;
            piVault.LastUnlocked = piVaultStatusResponseEvent.LastUnlocked;
            piVault.LastOpened = piVaultStatusResponseEvent.LastOpened;
            piVault.LastClosed = piVaultStatusResponseEvent.LastClosed;
            piVault.LockedSince = piVaultStatusResponseEvent.LockedSince;
            piVault.LockedUntil = piVaultStatusResponseEvent.LockedUntil;

            _piShockEventDispatcher.Dispatch(piVaultStatusResponseEvent);
            _piShockEventDispatcher.Dispatch(new PiShockToyListUpdatedEvent()
            {
                ToyList = Toys.Select(t => t.Value).ToList()
            });

            SaveCache();
        }

        private void ReceiveApiKeyPermissionsResponseHandler(string messageStr)
        {
            var successfulResponse = messageStr.TryParseJson<ReceivePiVaultApiKeyPermissionsResponseEvent>(out var piVaultApiKeyPermissionsResponseEvent);
            if (!successfulResponse)
            {
                _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceivePiVaultApiKeyPermissionsResponseErrorEvent>(messageStr)!);
            }

            _piShockEventDispatcher.Dispatch(piVaultApiKeyPermissionsResponseEvent);
        }

        private void ReceiveSetUnlockTimeResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveSetUnlockTimeResponseEvent>(messageStr)!);
        }

        private void ReceiveClearCurrentSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveClearCurrentSessionResponseEvent>(messageStr)!);
        }

        private void ReceiveUnlockPiVaultResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveUnlockPiVaultResponseEvent>(messageStr)!);
        }

        private void ReceiveAddMinutesToSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveAddMinutesToSessionResponseEvent>(messageStr)!);
        }

        private void ReceiveRetractMinutesFromSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveRetractMinutesFromSessionResponseEvent>(messageStr)!);
        }

        private void ReceiveAddHoursToSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveAddHoursToSessionResponseEvent>(messageStr)!);
        }

        private void ReceiveRetractHoursFromSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveRetractHoursFromSessionResponseEvent>(messageStr)!);
        }

        private void ReceiveAddDaysToSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveAddDaysToSessionResponseEvent>(messageStr)!);
        }

        private void ReceiveRetractDaysFromSessionResponseHandler(string messageStr)
        {
            _piShockEventDispatcher.Dispatch(JsonConvert.DeserializeObject<ReceiveRetractDaysFromSessionResponseEvent>(messageStr)!);
        }

        private async Task HealthCheckTask()
        {
            while (!Connected)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("PiShock - HealthCheck Task is starting");
            await GetPiShockToyInformation();
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

            while (await timer.WaitForNextTickAsync())
            {
                if (!Connected)
                {
                    return;
                }

                await GetPiShockToyInformation();
            }
        }

        private async Task GetPiShockToyInformation()
        {
            if (Toys.Count == 0)
            {
                await Connection!.InvokeAsync(PiShockCommandMethodConstants.Ping);
            }
            else
            {
                foreach (var toy in Toys.Values)
                {
                    if (toy is Models.PiShock piShock)
                    {
                        await Connection!.InvokeAsync(PiShockCommandMethodConstants.GetPiShockInformation,
                            piShock.ShareCode);
                    }

                    if (toy is PiVault piVault)
                    {
                        await Connection!.InvokeAsync(PiShockCommandMethodConstants.GetPiVaultStatus,
                            piVault.ApiKey);
                    }
                }
            }
        }

        public void SaveCache()
        {
            _cacheManager.SaveCache(new PiShockCache()
            {
                Toys = Toys!.Select(t => t.Value).ToList()
            });
        }
    }
}