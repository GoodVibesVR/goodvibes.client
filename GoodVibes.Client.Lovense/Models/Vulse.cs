using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Vulse : LovenseToy
{
    public override ToyTypeEnum ToyType => ToyTypeEnum.Vulse;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override bool IsActivityBased { get; set; }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate
    };

    public Vulse() : base()
    {
        Enabled = true;
        IsActivityBased = true;
        Function1 = LovenseCommandEnum.Vibrate;
        Function2 = LovenseCommandEnum.None;
    }
}