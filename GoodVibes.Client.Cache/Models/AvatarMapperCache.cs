using GoodVibes.Client.Cache.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.Cache.Models
{
    public class AvatarMapperCache : IApplicationCache
    {
        [JsonIgnore]
        public string FileName { get; }
        public Dictionary<string, AvatarCache> AvatarMapper { get; set; }

        public AvatarMapperCache()
        {
            FileName = "AvatarMapper.json";
            AvatarMapper = new Dictionary<string, AvatarCache>();
        }
    }
}
