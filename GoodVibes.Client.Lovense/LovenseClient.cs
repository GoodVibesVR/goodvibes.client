using GoodVibes.Client.Lovense.EventDispatchers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Settings.Models;
using GoodVibes.Client.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense
{
    public class LovenseClient : SignalRClient
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly LovenseEventDispatcher _lovenseEventDispatcher;

        public LovenseClient(ApplicationSettings applicationSettings, LovenseEventDispatcher lovenseEventDispatcher) : base()
        {
            _applicationSettings = applicationSettings;
            _lovenseEventDispatcher = lovenseEventDispatcher;
        }

        public async Task ConnectAsync()
        {
            await ConnectAsync(
                $"{_applicationSettings.GoodVibesRoot}{_applicationSettings.SignalRSettings!.CommandHubPath}", () =>
                {
                    Connection!.On<string>("ReceiveCallback", ReceiveCallbackHandler);
                    Connection!.On<string>("ReceiveQrCode", ReceiveQrCodeHandler);
                });
        }

        public async Task SendLovenseCommand(string command, int value1, int value2, int seconds, string toy)
        {
            await Connection!.InvokeAsync("SendCommand", command, value1, value2, seconds, toy);
        }

        private void ReceiveCallbackHandler(string message)
        {
            Console.WriteLine($"onReceiveMessage: {message}");
            var @event = JsonConvert.DeserializeObject<LovenseCallbackReceivedEvent>(message)!;
            _lovenseEventDispatcher.Dispatch(@event);
        }

        private void ReceiveQrCodeHandler(string message)
        {
            Console.WriteLine($"onReceiveQrCode: {message}");
            var @event = JsonConvert.DeserializeObject<LovenseQrCodeReceivedEvent>(message)!;
            _lovenseEventDispatcher.Dispatch(@event);
        }
    }
}
