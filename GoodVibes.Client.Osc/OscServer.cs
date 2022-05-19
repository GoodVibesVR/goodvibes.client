using Rug.Osc.Core;

namespace GoodVibes.Client.Osc
{
    public class OscServer
    {
        private OscReceiver receiver;
        private Thread thread;

        public OscServer()
        {

        }

        public async Task ConnectAsync()
        {
            int port = 9001;
            receiver = new OscReceiver(port);

            thread = new Thread(ListenLoop);

            receiver.Connect();
            thread.Start();
        }

        public async Task DisconnectAsync()
        {
            receiver.Close();
            thread.Join();
        }

        private void ListenLoop()
        {
            try
            {
                var messageReceived = false;
                while (receiver.State != OscSocketState.Closed)
                {
                    if (receiver.State != OscSocketState.Connected) continue;
                    messageReceived = receiver.TryReceive(out var packet);
                    if (!messageReceived) continue;

                    var message = packet as OscMessage;
                    switch (message.Address)
                    {
                        case "/avatar/parameters/GoodVibes/ToyA/Function1":
                        case "/avatar/parameters/GoodVibes/ToyA/Function2":
                        case "/avatar/parameters/GoodVibes/ToyB/Function1":
                        case "/avatar/parameters/GoodVibes/ToyB/Function2":
                        case "/avatar/change":
                            Console.WriteLine(packet.ToString());
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                // if the socket was connected when this happens
                // then tell the user
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}