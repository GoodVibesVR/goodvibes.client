using GoodVibes.Client.PiShock.Enums;
using Newtonsoft.Json;

namespace GoodVibes.Client.PiShock.Dtos
{
    public class HygieneSettingsDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("lockboxId")]
        public int LockboxId { get; set; }
        
        [JsonProperty("active")]
        public bool Active { get; set; }
        
        [JsonProperty("cron")]
        public string? CronExpression { get; set; }
        
        [JsonProperty("days")]
        public WeekdaysEnum[]? Days { get; set; }
        
        [JsonProperty("hours")]
        public int Hours { get; set; }
        
        [JsonProperty("minutes")]
        public int Minutes { get; set; }
        
        [JsonProperty("duration")]
        public int Duration { get; set; }
    }
}
