using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Lapis : LovenseToy
{
    public override ToyTypeEnum ToyType => ToyTypeEnum.Lapis;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum Function3 { get; set; }

    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate1,
        LovenseCommandEnum.Vibrate2,
        LovenseCommandEnum.Vibrate3
    };

    public Lapis() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Vibrate1;
        Function2 = LovenseCommandEnum.Vibrate2;
        Function3 = LovenseCommandEnum.Vibrate3;
    }
}