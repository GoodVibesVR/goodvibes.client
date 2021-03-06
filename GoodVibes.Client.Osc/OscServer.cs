using GoodVibes.Client.Mapper;
using Rug.Osc.Core;

namespace GoodVibes.Client.Osc
{
    public class OscServer
    {
        private readonly AvatarMapperClient _avatarMapper;

        private OscReceiver _receiver = null!;
        private Thread _thread = null!;

        public OscServer(AvatarMapperClient avatarMapper)
        {
            _avatarMapper = avatarMapper;
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

                    var typedMessage = message!.ToDto();
                    if(typedMessage == null) continue;
                    
                    _avatarMapper.MapAndPublishEvent(typedMessage);
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