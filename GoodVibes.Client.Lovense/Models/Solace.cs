using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Solace : LovenseToy
{
    public override ToyTypeEnum ToyType => ToyTypeEnum.Solace;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum Function3 { get; set; }

    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Thrusting,
        LovenseCommandEnum.Depth
    };

    public Solace() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Thrusting;
        Function2 = LovenseCommandEnum.Depth;
        Function3 = LovenseCommandEnum.None;
    }
}