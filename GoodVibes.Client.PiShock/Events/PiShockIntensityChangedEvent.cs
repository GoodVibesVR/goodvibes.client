﻿using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class PiShockIntensityChangedEvent : IEvent
{
    public string? ToyId { get; set; }
    public float Intensity { get; set; }
}