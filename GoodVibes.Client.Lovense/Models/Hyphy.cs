using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Hyphy : LovenseToy
{
    protected override LovenseCommandEnum Function1 => LovenseCommandEnum.Vibrate1;
    protected override LovenseCommandEnum Function2 => LovenseCommandEnum.Vibrate2;
    public override LovenseCommandEnum[] SpecialFunctions => new[]
    {
        LovenseCommandEnum.Vibrate,
        LovenseCommandEnum.AVibrate,
        LovenseCommandEnum.AVibrate1,
        LovenseCommandEnum.AVibrate2
    };

    public Hyphy() : base()
    {
    }
}