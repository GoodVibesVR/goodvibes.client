namespace GoodVibes.Client.Settings.Models
{
    public class ApplicationSettings
    {
        public string? GoodVibesRoot { get; set; }
        public SignalRSettings? SignalRSettings { get; set; }
        public LovenseSettings? LovenseSettings { get; set; }
        public bool LowPerformanceMode { get; set; }
    }
}
