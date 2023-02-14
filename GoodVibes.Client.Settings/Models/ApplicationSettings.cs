namespace GoodVibes.Client.Settings.Models
{
    public class ApplicationSettings
    {
        public string? GoodVibesRoot { get; set; }
        public SignalRSettings? SignalRSettings { get; set; }
        public OscSettings? OscSettings { get; set; }
        public SerialSettings? SerialSettings { get; set; }
    }
}
