using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class ReceivePausePiShockResponseEvent : IEvent
{
    public string? ShareCode { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public bool Result { get; set; }
}

public class ReceivePiShockInformationResponseEvent : IEvent
{
    public string? ShareCode { get; set; }
    public int ClientId { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Paused { get; set; }
    public int MaxIntensity { get; set; }
    public int MaxDuration { get; set; }
    public bool Online { get; set; }
}

public class ReceivePiShockInformationErrorResponseEvent : IEvent
{
    public string? ShareCode { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public bool Result { get; set; }
}