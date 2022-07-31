using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.PiShock.Models.Abstractions;

namespace GoodVibes.Client.PiShock.Models
{
    public class Shocker : PiShockToy
    {
        public override ToyTypeEnum ToyType => ToyTypeEnum.PiShockShocker;
    }
}
