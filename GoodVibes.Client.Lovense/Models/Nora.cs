﻿using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Nora : LovenseToy
{
    protected override LovenseCommandEnum Function1 => LovenseCommandEnum.Vibrate;
    protected override LovenseCommandEnum Function2 => LovenseCommandEnum.Rotate;
    public override LovenseCommandEnum[] SpecialFunctions => new[]
    {
        LovenseCommandEnum.AVibrate,
        LovenseCommandEnum.ARotate,
        LovenseCommandEnum.AVibRotate,
        LovenseCommandEnum.RotateAntiClockwise,
        LovenseCommandEnum.RotateClockwise,
        LovenseCommandEnum.RotateChange
    };

    public Nora() : base()
    {
    }
}