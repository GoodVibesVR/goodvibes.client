﻿using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class RemovePiShockToyEvent : IEvent
{
    public string? ToyId { get; set; }
}