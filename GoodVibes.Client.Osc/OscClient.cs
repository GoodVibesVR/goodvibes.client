using System.Net;
using Rug.Osc.Core;

namespace GoodVibes.Client.Osc;

public class OscClient
{
    public async Task SendMessage()
    {
        // This is the ip address we are going to send to
        IPAddress address = IPAddress.Parse("127.0.0.1");

        // This is the port we are going to send to 
        int port = 12345;

        // Create a new sender instance
        using (OscSender sender = new OscSender(address, port))
        {
            // Connect the sender socket
            sender.Connect();

            // Send a new message
            sender.Send(new OscMessage("/test", 1, 2, 3, 4));
        }
    }
}