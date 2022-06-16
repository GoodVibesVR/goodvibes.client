﻿using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Nora : LovenseToy
{
    public override string? Id { get; set; }
    public override string? Nickname { get; set; }
    public override string? Name { get; set; }
    public override bool Status { get; set; }
    public override string? Version { get; set; }
    public override int? Battery { get; set; }
    public override bool Enabled { get; set; }
    
    private LovenseCommandEnum TempFunction1 { get; set; }
    public override LovenseCommandEnum Function1
    {
        get => LovenseCommandEnum.Vibrate;
        set => TempFunction1 = value;
    }
    private LovenseCommandEnum TempFunction2 { get; set; }
    public override LovenseCommandEnum Function2
    {
        get => LovenseCommandEnum.Rotate;
        set => TempFunction2 = value;
    }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate,
        LovenseCommandEnum.Rotate,
        LovenseCommandEnum.RotateAntiClockwise
    };
    
    public Nora() : base()
    {
    }
}