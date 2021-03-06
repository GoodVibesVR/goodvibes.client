using System.Text;
using GoodVibes.Client.ApiCaller;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.Lovense.Dtos;
using GoodVibes.Client.Lovense.Enums;
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
            Console.WriteLine($"ConnectAsync called...");

            await ConnectAsync(
                $"{_applicationSettings.GoodVibesRoot}{_applicationSettings.SignalRSettings!.CommandHubPath}", () =>
                {
                    Connection!.On<string>(CommandMethodConstants.ReceiveCallback, ReceiveCallbackHandler);
                    Connection!.On<string>(CommandMethodConstants.ReceiveQrCode, ReceiveQrCodeHandler);
                });
            
            Console.WriteLine("Starting ApiCallerTask");
            Task.Run(ApiCallerTask).ConfigureAwait(false);
            // TODO: Make a connection checker task
        }

        public Task SendCommand(LovenseCommandEnum command, float value, int seconds, string toy)
        {
            if (!Connected || !Toys!.TryGetValue(toy, out var toyObj)) return Task.CompletedTask;

            toyObj.AddCommand(command, toyObj.ConvertPercentageByCommand(command, value));
            return Task.CompletedTask;
        }

        public Task SetStrength(string toyId, int strength1, int strength2)
        {
            var toyExists = Toys!.TryGetValue(toyId, out var toy);
            if (toyExists && toy != null)
            {
                toy.SetStrengthPercentage(strength1, strength2);
            }

            Console.WriteLine($"Strength now changed:\nStrength1: {strength1}\nStrength2: {strength2}");

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

            Console.WriteLine("Setting Connected to true...");
            Connected = true;
            Console.WriteLine("Connected set to true...");
            _lovenseEventDispatcher.Dispatch(new LovenseDeviceAccessibilityEvent()
            {
                Available = deviceAvailable
            });
            _lovenseEventDispatcher.Dispatch(new LovenseToyListUpdatedEvent()
            {
                ToyList = Toys.Select(t => t.Value).ToList()
            });
        }

        private void ReceiveQrCodeHandler(string message)
        {
            var @event = JsonConvert.DeserializeObject<LovenseQrCodeReceivedEvent>(message)!;
            _lovenseEventDispatcher.Dispatch(@event);
        }

        private async Task<Dictionary<string, LovenseToy>> BuildLovenseToys(List<ToyDto> toyList)
        {
            try
            {
                var detailedToys = await GetDetailedToyList();
                var tempList = new Dictionary<string, LovenseToy>();
                var handledIds = new List<string>();
                foreach (var toyDto in toyList)
                {
                    DetailedToyDto? detailedToy = null;
                    detailedToys?.TryGetValue(toyDto.Id!, out detailedToy);

                    var toyExists = Toys!.TryGetValue(toyDto.Id!, out var toy);
                    if (toyExists && toy != null)
                    {
                        toy.Nickname = toyDto.Nickname;
                        toy.Status = toyDto.Status == 1;
                        toy.Battery = detailedToy?.Battery ?? null;
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
                            ToyTypeConstants.Exomoon => new Exomoon(),
                            _ => throw new ArgumentOutOfRangeException("Unsupported toy")
                        };

                        toy.Id = toyDto.Id;
                        toy.Nickname = toyDto.Nickname;
                        toy.Name = toyDto.Name;
                        toy.Status = toyDto.Status == 1;
                        toy.Battery = detailedToy?.Battery ?? null;
                        toy.Version = detailedToy?.Version ?? null;
                    }

                    handledIds.Add(toyDto.Id!);
                    tempList.Add(toyDto.Id!, toy);
                }

                var notHandledIds = Toys!.Keys.Except(handledIds);
                foreach (var notHandledId in notHandledIds)
                {
                    var toyExists = Toys!.TryGetValue(notHandledId, out var toy);
                    if (!toyExists || toy == null) continue;

                    toy.Status = null;
                    tempList.Add(toy.Id!, toy);
                }

                Console.WriteLine($"Modified toys list: {JsonConvert.SerializeObject(tempList)}");
                return tempList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ApiCallerTask()
        {
            Console.WriteLine("ApiCallerTask waiting to be connected...");

            while (!Connected)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("ApiCallerTask connected!");

            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));

            while (await timer.WaitForNextTickAsync())
            {
                Console.WriteLine("ApiCallerTask triggered");

                foreach (var lovenseToy in Toys!)
                {
                    if (lovenseToy.Value.Status == false)
                        continue;

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

                    var requestBody = JsonConvert.SerializeObject(new
                    {
                        command = "Function",
                        action = commandStr,
                        stopPrevious = 0,
                        timeSec = 0,
                        apiVer = 1,
                        toy = lovenseToy.Value.Id
                    });

                    try
                    {
                        Console.WriteLine($"Request body being sent: {requestBody}");
                        var response = await _lovenseApiClient.PostAsync<WebCommandResponse>("/command", new StringContent(requestBody, Encoding.UTF8, "application/json"));
                        Console.WriteLine($"API Response: {JsonConvert.SerializeObject(response)}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }

        private async Task<Dictionary<string, DetailedToyDto>?> GetDetailedToyList()
        {
            if (_lovenseApiClient == null) return null;

            try
            {
                var detailedToysResponse = await _lovenseApiClient.PostAsync<GetToysResponse>("/command",
                    new StringContent(JsonConvert.SerializeObject(new
                    {
                        command = "GetToys"
                    }), Encoding.UTF8, "application/json"));

                Console.WriteLine($"Detailed Toy Response: {JsonConvert.SerializeObject(detailedToysResponse)}");

                var toys = JsonConvert.DeserializeObject<Dictionary<string, DetailedToyDto>>(detailedToysResponse?.Data!.Toys!);
                return toys;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
    }
}
