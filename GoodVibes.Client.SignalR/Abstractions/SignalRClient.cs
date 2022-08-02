using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace GoodVibes.Client.SignalR.Abstractions
{
    public abstract class SignalRClient
    {
        public HubConnection? Connection;

        protected SignalRClient()
        {
        }

        public async Task ConnectAsync(string url, Delegate eventDelegate)
        {
            Connection = new HubConnectionBuilder().WithUrl(url).Build();
            eventDelegate.DynamicInvoke();

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

        public async Task DisconnectAsync(bool disconnect)
        {
            if (!disconnect) return;

            try
            {
                await Connection!.StopAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to disconnect from SignalR hub.", e);
                throw;
            }
        }
    }
}