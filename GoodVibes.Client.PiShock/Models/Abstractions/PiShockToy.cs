using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodVibes.Client.Common.Enums;

namespace GoodVibes.Client.PiShock.Models.Abstractions
{
    public abstract class PiShockToy
    {
        public virtual string? ShareCode { get; set; }
        public abstract ToyTypeEnum ToyType { get; }
    }
}
