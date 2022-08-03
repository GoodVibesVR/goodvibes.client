using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.PiShock.EventDispatchers;
using GoodVibes.Client.PiShock.Events;
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

        public Dictionary<string, PiShockToy>? Toys { get; }

        public bool Connected { get; set; }

        public PiShockClient(ApplicationSettings applicationSettings, PiShockEventDispatcher piShockEventDispatcher)
        {
            _applicationSettings = applicationSettings;
            _piShockEventDispatcher = piShockEventDispatcher;

            Toys = new Dictionary<string, PiShockToy>();
            Connected = false;
        }

        public async Task ConnectAsync()
        {
            Console.WriteLine($"PiShock ConnectAsync called...");

            await ConnectAsync(
                $"{_applicationSettings.GoodVibesRoot}{_applicationSettings.SignalRSettings!.PiShockHubPath}", () =>
                {
                    Connection!.On<string>(PiShockCommandMethodConstants.ConnectionAck, ReceiveConnectionAcknowledgedHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.Pong, ReceivePongResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.ShockResponse, ReceiveShockResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.VibrateResponse, ReceiveVibrateResponseHandler);
                    Connection!.On<string>(PiShockCommandMethodConstants.BeepResponse, ReceiveBeepResponseHandler);
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

        public Task AddToy(string friendlyName, string shareCode, ToyTypeEnum toyType)
        {
            Toys!.Add(shareCode, new Models.PiShock()
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

            return Task.CompletedTask;
        }

        public Task RemoveToy(string toyId)
        {
            var toyFound = Toys!.TryGetValue(toyId, out var toy);
            if (toyFound)
            {
                Toys.Remove(toyId);
            }

            return Task.CompletedTask;
        }

        public Task ChangeIntensity(string toyId, float intensity)
        {
            if (!Connected) return Task.CompletedTask;
            if (!Toys!.TryGetValue(toyId, out var toy)) return Task.CompletedTask;

            if (toy is Models.PiShock shocker)
            {
                shocker.Intensity = (int)Math.Round((double)(intensity * 100));
            }

            return Task.CompletedTask;
        }

        public Task ChangeDuration(string toyId, float duration)
        {
            if (!Connected) return Task.CompletedTask;
            if (!Toys!.TryGetValue(toyId, out var toy)) return Task.CompletedTask;

            if (toy is Models.PiShock shocker)
            {
                shocker.Duration = (int)Math.Round((double)(duration * 10));
            }

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

        private async Task HealthCheckTask()
        {
            while (!Connected)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("PiShock - HealthCheck Task is starting");
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            while (await timer.WaitForNextTickAsync())
            {
                if (!Connected)
                {
                    return;
                }

                await Connection!.InvokeAsync(PiShockCommandMethodConstants.Ping);
            }
        }
    }
}