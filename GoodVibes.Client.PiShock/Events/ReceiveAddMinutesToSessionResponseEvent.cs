﻿using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class ReceiveAddMinutesToSessionResponseEvent : IEvent
{
    public Guid ApiKey { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public bool Result { get; set; }
}