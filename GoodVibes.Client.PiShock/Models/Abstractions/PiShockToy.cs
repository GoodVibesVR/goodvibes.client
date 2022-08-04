using GoodVibes.Client.Common.Enums;

namespace GoodVibes.Client.PiShock.Models.Abstractions
{
    public abstract class PiShockToy
    {
        public virtual string? FriendlyName { get; set; }
        public virtual string? ShareCode { get; set; }
        public abstract ToyTypeEnum ToyType { get; }
    }
}
