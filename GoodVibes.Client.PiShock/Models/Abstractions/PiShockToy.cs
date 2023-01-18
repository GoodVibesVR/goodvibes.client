using GoodVibes.Client.Common.Enums;

namespace GoodVibes.Client.PiShock.Models.Abstractions
{
    public abstract class PiShockToy
    {
        public abstract int Id { get; set; }
        public abstract string? Name { get; set; }
        public abstract ToyTypeEnum ToyType { get; }
    }
}
