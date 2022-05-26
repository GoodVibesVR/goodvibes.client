using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Events;
using Rug.Osc.Core;

namespace GoodVibes.Client.Osc
{
    public class OscServer
    {
        private readonly IEventAggregator _eventAggregator;

        private OscReceiver _receiver = null!;
        private Thread _thread = null!;

        public OscServer(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Task ConnectAsync()
        {
            var port = 9001;
            _receiver = new OscReceiver(port);

            _thread = new Thread(ListenLoop);

            _receiver.Connect();
            _thread.Start();
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            _receiver.Close();
            _thread.Join();
            return Task.CompletedTask;
        }

        private void ListenLoop()
        {
            try
            {
                while (_receiver.State != OscSocketState.Closed)
                {
                    if (_receiver.State != OscSocketState.Connected) continue;
                    var messageReceived = _receiver.TryReceive(out var packet);
                    if (!messageReceived) continue;

                    var message = packet as OscMessage;
                    switch (message!.Address)
                    {
                        case "/avatar/parameters/GoodVibes/ToyA/Function1":
                        case "/avatar/parameters/GoodVibes/ToyA/Function2":
                        case "/avatar/parameters/GoodVibes/ToyB/Function1":
                        case "/avatar/parameters/GoodVibes/ToyB/Function2":
                            //Console.WriteLine(packet.ToString());
                            Console.WriteLine(message.ToString());
                            var test = (message.FirstOrDefault() as float? ?? 0.0f); // TODO: Dump double messages
                            var percentComplete = (int)Math.Round((double)(test / 5) * 100);
                            _eventAggregator.GetEvent<LovenseCommandEventCarrier>().Publish(new LovenseCommandEvent()
                            {
                                Command = LovenseCommandEnum.Vibrate,
                                Toy = "12345",
                                Value = percentComplete
                            });
                            break;
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
                if (_receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}