using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace GoodVibes.Client.SignalR.Abstractions
{
    public abstract class SignalRClient
    {
        public HubConnection? Connection;

        public SignalRClient()
        {
        }

        public async Task ConnectAsync(string url, Delegate addDelegate)
        {
            Connection = new HubConnectionBuilder().WithUrl(url).Build();
            addDelegate.DynamicInvoke();

            try
            {
                await Connection.StartAsync();
                Console.WriteLine("SignalR connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred in SignalR client\n{JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}