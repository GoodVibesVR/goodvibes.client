using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Calor : LovenseToy
{
    protected override LovenseCommandEnum Function1 => LovenseCommandEnum.Vibrate;
    protected override LovenseCommandEnum Function2 => LovenseCommandEnum.None;
    public override LovenseCommandEnum[] SpecialFunctions => Array.Empty<LovenseCommandEnum>();

    public Calor() : base()
    {
    }
}