using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Gravity : LovenseToy
{
    public override ToyTypeEnum ToyType => ToyTypeEnum.Gravity;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum Function3 { get; set; }

    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate,
        LovenseCommandEnum.Thrusting
    };

    public Gravity() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Vibrate;
        Function2 = LovenseCommandEnum.Thrusting;
        Function3 = LovenseCommandEnum.None;
    }
}