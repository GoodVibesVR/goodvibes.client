using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Exomoon : LovenseToy
{
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate
    };

    public Exomoon() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Vibrate;
        Function2 = LovenseCommandEnum.None;
    }
}