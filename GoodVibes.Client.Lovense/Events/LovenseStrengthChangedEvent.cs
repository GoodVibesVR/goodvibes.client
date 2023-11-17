using GoodVibes.Client.Events;

namespace GoodVibes.Client.Lovense.Events;

public class LovenseStrengthChangedEvent : IEvent
{
    public string? ToyId { get; set; }
    public int Strength1Percentage { get; set; }
    public int Strength2Percentage { get; set; }
    public int Strength3Percentage { get; set; }
}