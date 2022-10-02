using System.Text;
using GoodVibes.Client.ApiCaller;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.Cache;
using GoodVibes.Client.Lovense.Cache;
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
        private readonly GoodVibesCacheManager<LovenseCache> _cacheManager;

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
        public Dictionary<string, LovenseToy> Toys { get; private set; }

        public LovenseClient(ApplicationSettings applicationSettings, LovenseEventDispatcher lovenseEventDispatcher, GoodVibesCacheManager<LovenseCache> cacheManager) : base()
        {
            _applicationSettings = applicationSettings;
            _lovenseEventDispatcher = lovenseEventDispatcher;
            _cacheManager = cacheManager;

            Toys = SetupToyList();
        }

        private Dictionary<string, LovenseToy> SetupToyList()
        {
            var toysDict = new Dictionary<string, LovenseToy>();
            var lovenseCache = _cacheManager.GetCache();
            if (lovenseCache.Toys.Count < 1) return toysDict;

            var toys = lovenseCache.Toys;
            foreach (var lovenseToy in toys)
            {
                toysDict.Add(lovenseToy.Id!, lovenseToy);
            }

            _lovenseEventDispatcher.Dispatch(new LovenseToyListUpdatedEvent()
            {
                ToyList = toysDict.Select(t => t.Value).ToList()
            });
            return toysDict;
        }

        public async Task ConnectAsync()
        {
            Console.WriteLine($"Lovense ConnectAsync called...");

            await ConnectAsync(
                $"{_applicationSettings.GoodVibesRoot}{_applicationSettings.SignalRSettings!.CommandHubPath}", () =>
                {
                    Connection!.On<string>(LovenseCommandMethodConstants.ReceiveCallback, ReceiveCallbackHandler);
                    Connection!.On<string>(LovenseCommandMethodConstants.ReceiveQrCode, ReceiveQrCodeHandler);
                });
            
            Console.WriteLine("Starting ApiCallerTask");
            await Task.Run(ApiCallerTask).ConfigureAwait(false);
            // TODO: Make a connection checker task
        }

        public async Task DisconnectAsync()
        {
            Console.WriteLine($"Lovense DisconnectAsync called...");

            Connected = false;
            await DisconnectAsync(true);

            _lovenseEventDispatcher.Dispatch(new LovenseDisconnectedEvent());
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

            SaveToysToCache(Toys, true);
            Console.WriteLine($"Strength now changed:\nStrength1: {strength1}\nStrength2: {strength2}");

            return Task.CompletedTask;
        }

        public Task RemoveToy(string toyId)
        {
            var toyFound = Toys!.TryGetValue(toyId, out var toy);
            if (!toyFound) return Task.CompletedTask;

            Toys.Remove(toyId);
            SaveToysToCache(Toys, true);

            return Task.CompletedTask;
        }

        private void SaveToysToCache(Dictionary<string, LovenseToy> toys, bool ignoreCheck = false)
        {
            var equal = toys.OrderBy(t => t.Key).SequenceEqual(Toys.OrderBy(t => t.Key));
            if (!equal || ignoreCheck)
            {
                _cacheManager.SaveCache(new LovenseCache()
                {
                    Toys = toys.Select(t => t.Value).ToList()
                });
            }
        }

        private void ReceiveCallbackHandler(string messageStr)
        {
            Console.WriteLine($"onReceiveMessage: {messageStr}");
            var callback = JsonConvert.DeserializeObject<LovenseCallbackReceivedDto>(messageStr)!;

            var deviceAvailable = !string.IsNullOrEmpty(callback.Domain);
            _lovenseApiClient = !deviceAvailable ? null
                : new ApiClient($"http://{callback.Domain}:{callback.HttpPort}");

            var toys = Task.Run(() => BuildLovenseToys(callback.Toys!)).Result;
            SaveToysToCache(toys);

            Toys = toys;
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
                            ToyTypeConstants.Tenera => new Tenera(),
                            // ReSharper disable once NotResolvedInText
                            _ => throw new ArgumentOutOfRangeException("Unsupported toy")
                        };

                        toy.Id = toyDto.Id;
                        toy.Nickname = toyDto.Nickname;
                        toy.Name = toyDto.Name;
                        toy.Status = toyDto.Status == 1;
                        toy.Function1MaxStrengthPercentage = 100;
                        toy.Function2MaxStrengthPercentage = 100;
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
                if (!Connected)
                {
                    return;
                }

                try
                {
                    Console.WriteLine("ApiCallerTask triggered");

                    // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var (_, lovenseToy) in Toys!)
                    {
                        if (lovenseToy.Status == false)
                            continue;

                        if (_lovenseApiClient == null)
                        {
                            var commandList = lovenseToy.GetCommandList();
                            if (commandList.Any())
                            {
                                lovenseToy.ClearCommandList();
                            }
                            foreach (var command in commandList)
                            {
                                await Connection!.InvokeAsync(LovenseCommandMethodConstants.SendCommand, command.Command,
                                    command.Value, 0, 0, lovenseToy.Id);
                            }

                            continue;
                        }

                        var commandStr = lovenseToy.GetCommandString();
                        if (string.IsNullOrEmpty(commandStr)) continue;
                        lovenseToy.ClearCommandList();

                        var requestBody = JsonConvert.SerializeObject(new
                        {
                            command = "Function",
                            action = commandStr,
                            stopPrevious = 0,
                            timeSec = 0,
                            apiVer = 1,
                            toy = lovenseToy.Id
                        });

                        try
                        {
                            Console.WriteLine($"Request body being sent: {requestBody}");
                            await DoLocalCommandCall(requestBody);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async Task DoLocalCommandCall(string requestBody)
        {
            if (_lovenseApiClient == null) return;
            var response = await _lovenseApiClient.PostAsync<WebCommandResponse>("/command", new StringContent(requestBody, Encoding.UTF8, "application/json"));
            Console.WriteLine($"API Response: {JsonConvert.SerializeObject(response)}");
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
