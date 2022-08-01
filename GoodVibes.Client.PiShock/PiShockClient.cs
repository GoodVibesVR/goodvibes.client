using GoodVibes.Client.Common.Enums;
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

        public Dictionary<string, PiShockToy>? Toys { get; }


        public PiShockClient(ApplicationSettings applicationSettings, PiShockEventDispatcher piShockEventDispatcher)
        {
            _applicationSettings = applicationSettings;
            _piShockEventDispatcher = piShockEventDispatcher;

            Toys = new Dictionary<string, PiShockToy>();
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
        }

        public Task AddToy(string friendlyName, string shareCode, ToyTypeEnum toyType)
        {
            Toys!.Add(shareCode, new Shocker()
            {
                FriendlyName = friendlyName,
                ShareCode = shareCode
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

        public async Task Shock(string shareCode, int duration, int intensity)
        {
            await Connection!.InvokeAsync(PiShockCommandMethodConstants.Shock, shareCode, duration, intensity);
        }

        public async Task Vibrate(string shareCode, int duration, int intensity)
        {
            await Connection!.InvokeAsync(PiShockCommandMethodConstants.Vibrate, shareCode, duration, intensity);
        }

        public async Task Beep(string shareCode, int duration)
        {
            await Connection!.InvokeAsync(PiShockCommandMethodConstants.Vibrate, shareCode, duration);
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
    }
}