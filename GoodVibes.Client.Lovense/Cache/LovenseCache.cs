using GoodVibes.Client.Cache.Abstractions;
using GoodVibes.Client.Lovense.Models.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Cache
{
    public class LovenseCache : IApplicationCache
    {
        [JsonIgnore]
        public string FileName { get; }
        public List<LovenseToy> Toys { get; set; }

        public LovenseCache()
        {
            FileName = "Lovense.json";
            Toys = new List<LovenseToy>();
        }
    }
}
