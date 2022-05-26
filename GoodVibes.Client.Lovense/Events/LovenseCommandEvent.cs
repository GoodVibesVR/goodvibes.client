using GoodVibes.Client.Events;
using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Lovense.Events;

public class LovenseCommandEvent : IEvent
{
    public LovenseCommandEnum Command { get; set; }
    public int Value { get; set; }
    public string? Toy { get; set; }
}