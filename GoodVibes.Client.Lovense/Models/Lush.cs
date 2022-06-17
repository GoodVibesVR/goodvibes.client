using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Lovense.Models;

public sealed class Lush : LovenseToy
{
    public override string? Id { get; set; }
    public override string? Nickname { get; set; }
    public override string? Name { get; set; }
    public override bool Status { get; set; }
    public override string? Version { get; set; }
    public override int? Battery { get; set; }
    public override bool Enabled { get; set; }
    public override LovenseCommandEnum Function1 { get; set; }
    public override LovenseCommandEnum Function2 { get; set; }
    public override LovenseCommandEnum[] ToyFunctions => new[]
    {
        LovenseCommandEnum.Vibrate
    };

    public Lush() : base()
    {
        Enabled = true;
        Function1 = LovenseCommandEnum.Vibrate;
        Function2 = LovenseCommandEnum.None;
    }
}