﻿using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public class Edge : LovenseToy
{
    public override ToyTypeEnum ToyType => ToyTypeEnum.Edge;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate,
        LovenseCommandEnum.Vibrate1,
        LovenseCommandEnum.Vibrate2
    };

    public Edge() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Vibrate;
        Function2 = LovenseCommandEnum.None;
    }
}