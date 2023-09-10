using GoodVibes.Client.Mapper;
using Rug.Osc.Core;
using VRC.OSCQuery;

namespace GoodVibes.Client.Osc
{
    public class OscServer
    {
        private readonly AvatarMapperClient _avatarMapper;
        private readonly OSCQueryService _oscQueryService;

        private OscReceiver _receiver = null!;
        private Thread _thread = null!;

        public OscServer(OSCQueryService oscQueryService, AvatarMapperClient avatarMapper)
        {
            //_applicationSettings = applicationSettings;
            _oscQueryService = oscQueryService;
            _avatarMapper = avatarMapper;
        }

        public Task ConnectAsync()
        {
            Console.WriteLine($"Starting OSC Server. \nDiscoverable on http://127.0.0.1:{_oscQueryService.TcpPort}. \nOsc port: {_oscQueryService.OscPort}");

            var port = _oscQueryService.OscPort;
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
                    if (typedMessage == null) continue;

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