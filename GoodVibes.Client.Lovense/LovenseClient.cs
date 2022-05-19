using GoodVibes.Client.Lovense.EventDispatchers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense
{
    public class LovenseClient : SignalRClient
    {
        private readonly LovenseEventDispatcher _lovenseEventDispatcher;

        public LovenseClient(LovenseEventDispatcher lovenseEventDispatcher) : base()
        {
            _lovenseEventDispatcher = lovenseEventDispatcher;
        }

        public async Task ConnectAsync()
        {
            // http://localhost:5161/commandHub

            //await ConnectAsync("http://localhost:5161/commandHub", () =>
            //{
            //    Connection!.On<string>("ReceiveCallback", ReceiveCallbackHandler);
            //    Connection!.On<string>("ReceiveQrCode", ReceiveQrCodeHandler);
            //});

            await ConnectAsync("https://goodvibes.miwca.se/commandHub", () =>
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
