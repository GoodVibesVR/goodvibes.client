using GoodVibes.Client.Events;

namespace GoodVibes.Client.Lovense.Events;

public class LovenseDeviceAccessibilityEvent : IEvent
{
    public bool Available { get; set; }
}