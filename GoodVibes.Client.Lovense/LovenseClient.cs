using System.Text;
using GoodVibes.Client.ApiCaller;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.Lovense.Dtos;
using GoodVibes.Client.Lovense.EventDispatchers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Lovense.Models;
using GoodVibes.Client.Lovense.Models.Abstractions;
using GoodVibes.Client.Lovense.Responses;
using GoodVibes.Client.Settings.Models;
using GoodVibes.Client.SignalR;
using GoodVibes.Client.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense
{
    public class LovenseClient : SignalRClient
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly LovenseEventDispatcher _lovenseEventDispatcher;
        
        private ILovenseApiClient? _lovenseApiClient;
        private bool Connected { get; set; }

        public string? Uid { get; private set; }
        public string? AppVersion { get; private set; }
        public string? WssPort { get; private set; }
        public string? HttpPort { get; private set; }
        public string? WsPort { get; private set; }
        public string? AppType { get; private set; }
        public string? Domain { get; private set; }
        public string? UToken { get; private set; }
        public string? HttpsPort { get; private set; }
        public int Version { get; private set; }
        public string? Platform { get; private set; }
        public Dictionary<string, LovenseToy>? Toys { get; private set; }


        public LovenseClient(ApplicationSettings applicationSettings, LovenseEventDispatcher lovenseEventDispatcher) : base()
        {
            _applicationSettings = applicationSettings;
            _lovenseEventDispatcher = lovenseEventDispatcher;

            Toys = new Dictionary<string, LovenseToy>();
        }

        public async Task ConnectAsync()
        {
            await ConnectAsync(
                $"{_applicationSettings.GoodVibesRoot}{_applicationSettings.SignalRSettings!.CommandHubPath}", () =>
                {
                    Connection!.On<string>(CommandMethodConstants.ReceiveCallback, ReceiveCallbackHandler);
                    Connection!.On<string>(CommandMethodConstants.ReceiveQrCode, ReceiveQrCodeHandler);
                });

#pragma warning disable CS4014
            Task.Run(ApiCallerTask);
#pragma warning restore CS4014
        }

        public Task SendCommand(string command, int value, int seconds, string toy)
        {
            if (!Connected) return Task.CompletedTask;
            
            //var toyObj = Toys[toy];
            var toyObj = Toys!.First().Value;

            // TODO: Do we need to validate anything here?
            toyObj.AddCommandToList(command, value);

            return Task.CompletedTask;
        }

        private void ReceiveCallbackHandler(string messageStr)
        {
            Console.WriteLine($"onReceiveMessage: {messageStr}");
            var callback = JsonConvert.DeserializeObject<LovenseCallbackReceivedDto>(messageStr)!;

            var deviceAvailable = !string.IsNullOrEmpty(callback.Domain);
            _lovenseApiClient = !deviceAvailable ? null
                : new ApiClient($"http://{callback.Domain}:{callback.HttpPort}");

            Toys = Task.Run(() => BuildLovenseToys(callback.Toys!)).Result;
            Uid = callback.Uid;
            AppVersion = callback.AppVersion;
            WssPort = callback.WssPort;
            HttpPort = callback.HttpPort;
            WsPort = callback.WsPort;
            AppType = callback.AppType;
            Domain = callback.Domain;
            UToken = callback.UToken;
            HttpsPort = callback.HttpsPort;
            Version = callback.Version;
            Platform = callback.Platform;

            Connected = true;
            _lovenseEventDispatcher.Dispatch(new LovenseDeviceAccessibilityEvent()
            {
                Available = deviceAvailable
            });
        }

        private void ReceiveQrCodeHandler(string message)
        {
            Console.WriteLine($"onReceiveQrCode: {message}");
            var @event = JsonConvert.DeserializeObject<LovenseQrCodeReceivedEvent>(message)!;
            _lovenseEventDispatcher.Dispatch(@event);
        }

        private async Task<Dictionary<string, LovenseToy>> BuildLovenseToys(List<ToyDto> toyList)
        {
            var detailedToys = await GetDetailedToyList();
            var tempList = new Dictionary<string, LovenseToy>();
            foreach (var toyDto in toyList)
            {
                var toyExists = Toys!.TryGetValue(toyDto.Id!, out var toy);
                if (toyExists && toy != null)
                {
                    toy.Nickname = toyDto.Nickname;
                    toy.Status = toyDto.Status == 1;
                    toy.Battery = detailedToys?[toyDto.Id!].Battery ?? null;
                }
                else
                {
                    toy = toyDto.Name!.ToLower() switch
                    {
                        ToyTypeConstants.Ambi => new Ambi(),
                        ToyTypeConstants.Calor => new Calor(),
                        ToyTypeConstants.Diamo => new Diamo(),
                        ToyTypeConstants.Dolce => new Dolce(),
                        ToyTypeConstants.Domi => new Domi(),
                        ToyTypeConstants.Edge => new Edge(),
                        ToyTypeConstants.Ferri => new Ferri(),
                        ToyTypeConstants.Gush => new Gush(),
                        ToyTypeConstants.Hush => new Hush(),
                        ToyTypeConstants.Hyphy => new Hyphy(),
                        ToyTypeConstants.Lush => new Lush(),
                        ToyTypeConstants.Max => new Max(),
                        ToyTypeConstants.Nora => new Nora(),
                        ToyTypeConstants.Osci => new Osci(),
                        ToyTypeConstants.SexMachine => new SexMachine(),
                        _ => throw new ArgumentOutOfRangeException("Unsupported toy")
                    };

                    toy.Id = toyDto.Id;
                    toy.Nickname = toyDto.Nickname;
                    toy.Name = toyDto.Name;
                    toy.Status = toyDto.Status == 1;
                    toy.Battery = detailedToys?[toyDto.Id!].Battery ?? null;
                }
                
                tempList.Add(toyDto.Id!, toy);
            }

            Console.WriteLine($"Modified toys list: {JsonConvert.SerializeObject(tempList)}");
            return tempList;
        }

        private async Task ApiCallerTask()
        {
            while (!Connected)
            {
            }

            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));

            while (await timer.WaitForNextTickAsync())
            {
                foreach (var lovenseToy in Toys!)
                {
                    if (_lovenseApiClient == null)
                    {
                        var commandList = lovenseToy.Value.GetCommandList();
                        if (commandList.Any())
                        {
                            lovenseToy.Value.ClearCommandList();
                        }
                        foreach (var command in commandList)
                        {
                            await Connection!.InvokeAsync(CommandMethodConstants.SendCommand, command.Command,
                                command.Value, 0, 0, lovenseToy.Value.Id);
                        }

                        return;
                    }

                    var commandStr = lovenseToy.Value.GetCommandString();
                    if (string.IsNullOrEmpty(commandStr)) continue;
                    lovenseToy.Value.ClearCommandList();
                    await _lovenseApiClient.PostAsync<WebCommandResponse>("/command", new StringContent(JsonConvert.SerializeObject(new
                    {
                        command = "Function",
                        action = commandStr,
                        timeSec = 10,
                        stopPrevious = 0,
                        //loopRunningSec = 10,
                        //loopPauseSec = 0,
                        apiVer = 1,
                        toy = lovenseToy.Value.Id
                    }), Encoding.UTF8, "application/json"));

                }
            }
        }

        private async Task<Dictionary<string, DetailedToyDto>?> GetDetailedToyList()
        {
            if (_lovenseApiClient == null) return null;

            var detailedToysResponse = await _lovenseApiClient.PostAsync<GetToysResponse>("/command",
                new StringContent(JsonConvert.SerializeObject(new
                {
                    command = "GetToys"
                }), Encoding.UTF8, "application/json"));

            var toys = JsonConvert.DeserializeObject<Dictionary<string, DetailedToyDto>>(detailedToysResponse?.Data!.Toys!);
            return toys;
        }
    }
}
