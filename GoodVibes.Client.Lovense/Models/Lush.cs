using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Lush : LovenseToy
{
    protected override LovenseCommandEnum Function1 => LovenseCommandEnum.Vibrate;
    protected override LovenseCommandEnum Function2 => LovenseCommandEnum.None;
    public override LovenseCommandEnum[] SpecialFunctions => Array.Empty<LovenseCommandEnum>();

    public Lush() : base()
    {
    }
}