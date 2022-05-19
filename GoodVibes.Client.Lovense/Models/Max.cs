using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Max : LovenseToy
{
    protected override LovenseCommandEnum Function1 => LovenseCommandEnum.Vibrate;
    protected override LovenseCommandEnum Function2 => LovenseCommandEnum.Pump;
    public override LovenseCommandEnum[] SpecialFunctions => Array.Empty<LovenseCommandEnum>();

    public Max() : base()
    {
    }
}