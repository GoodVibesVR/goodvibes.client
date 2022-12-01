﻿using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Tenera : LovenseToy
{
    public override ToyTypeEnum ToyType => ToyTypeEnum.Tenera;
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Suction
    };

    public Tenera() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Suction;
        Function2 = LovenseCommandEnum.None;
    }
}