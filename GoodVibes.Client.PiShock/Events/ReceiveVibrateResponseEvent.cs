using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class ReceiveVibrateResponseEvent : IEvent
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public bool Result { get; set; }
}