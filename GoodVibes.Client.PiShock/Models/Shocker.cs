using System.ComponentModel.DataAnnotations;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.PiShock.Models.Abstractions;

namespace GoodVibes.Client.PiShock.Models
{
    public class Shocker : PiShockToy
    {
        public override ToyTypeEnum ToyType => ToyTypeEnum.PiShockShocker;
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int Intensity { get; set; }
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int Duration { get; set; }
    }
}
