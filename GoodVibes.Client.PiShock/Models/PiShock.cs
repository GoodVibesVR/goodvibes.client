using System.ComponentModel.DataAnnotations;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.PiShock.Models.Abstractions;

namespace GoodVibes.Client.PiShock.Models
{
    public class PiShock : PiShockToy
    {
        public override ToyTypeEnum ToyType => ToyTypeEnum.PiShock;

        public override int Id { get; set; }

        public override string? Name { get; set; }

        public string? ShareCode { get; set; }

        public bool Paused { get; set; }

        public bool? Online { get; set; }

        public int MaxIntensity { get; set; }

        public int MaxDuration { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int Intensity { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int Duration { get; set; }

        public virtual string? FriendlyName { get; set; }

        
    }
}
