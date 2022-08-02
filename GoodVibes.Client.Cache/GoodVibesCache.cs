using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Cache
{
    public class GoodVibesCache
    {
        public LovenseCache? LovenseCache { get; set; }
        public PiShockCache? PiShockCache { get; set; }
        public AvatarMapperCache? AvatarMapperCache { get; set; }
    }

    public class LovenseCache
    {
        public List<LovenseToy>? Toys { get; set; }
    }

    public class PiShockCache
    {

    }

    public class AvatarMapperCache
    {

    }
}