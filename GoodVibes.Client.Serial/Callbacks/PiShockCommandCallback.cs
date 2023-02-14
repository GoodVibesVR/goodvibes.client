namespace GoodVibes.Client.Serial.Callbacks
{
    internal class PiShockSerialCallback
    {
        public List<CommandCallback> Commands { get; set; } = null!;
        public List<int> ShockersId { get; set; } = null!;
        public List<int> ShockersType { get; set; } = null!;
        public int LastCommandId { get; set; }
        public int Poll { get; set; }
        public bool RequestRefresh { get; set; }
    }
}
