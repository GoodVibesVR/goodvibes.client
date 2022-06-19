using System.Net;
using Rug.Osc.Core;

namespace GoodVibes.Client.Osc;

public class OscClient
{
    public Task SendMessage()
    {
        var address = IPAddress.Parse("127.0.0.1");
        var port = 12345;
        using var sender = new OscSender(address, port);
        
        sender.Connect();
        sender.Send(new OscMessage("/test", 1, 2, 3, 4));
        return Task.CompletedTask;
    }
}