namespace GoodVibes.Client.SignalR
{
    public static class CommandMethodConstants
    {
        /// <summary>
        /// Used to send commands through the GoodVibes servers
        /// </summary>
        public static string SendCommand => "SendCommand";
        /// <summary>
        /// Used to ping the SignalR hub. Will keep session from timing out (60 minutes)
        /// </summary>
        public static string Ping => "Ping";

        /// <summary>
        /// Used to receive the Lovense callback from the GoodVibes servers
        /// </summary>
        public static string ReceiveCallback => "ReceiveCallback";

        /// <summary>
        /// Used to receive the QR code after a successful connection has been made to GoodVibes servers
        /// </summary>
        public static string ReceiveQrCode => "ReceiveQrCode";
    }
}
