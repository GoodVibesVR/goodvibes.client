using GoodVibes.Client.Cache.Abstractions;
using GoodVibes.Client.PiShock.Models.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.PiShock.Cache
{
    public class PiShockCache : IApplicationCache
    {
        [JsonIgnore]
        public string FileName { get; }
        public List<PiShockToy> Toys { get; set; }

        public PiShockCache()
        {
            FileName = "PiShock.json";
            Toys = new List<PiShockToy>();
        }
    }
}
