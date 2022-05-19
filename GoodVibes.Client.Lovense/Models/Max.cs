using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Max : LovenseToy
{
    protected override LovenseCommandEnum Function1 => LovenseCommandEnum.Vibrate;
    protected override LovenseCommandEnum Function2 => LovenseCommandEnum.AirAuto;
    public override LovenseCommandEnum[] SpecialFunctions => new[]
    {
        LovenseCommandEnum.AVibrate,
        LovenseCommandEnum.AirIn,
        LovenseCommandEnum.AirOut,
        LovenseCommandEnum.AAirLevel,
        LovenseCommandEnum.AVibAir
    };

    public Max() : base()
    {
    }
}