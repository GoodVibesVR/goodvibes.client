using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Dolce : LovenseToy
{
    public override LovenseToyEnum ToyType => LovenseToyEnum.Dolce;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate,
        LovenseCommandEnum.Vibrate1,
        LovenseCommandEnum.Vibrate2
    };

    public Dolce() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Vibrate1;
        Function2 = LovenseCommandEnum.Vibrate2;
    }
}

