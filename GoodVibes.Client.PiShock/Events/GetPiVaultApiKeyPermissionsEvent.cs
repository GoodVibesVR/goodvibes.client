﻿using GoodVibes.Client.Events;

namespace GoodVibes.Client.PiShock.Events;

public class GetPiVaultApiKeyPermissionsEvent : IEvent
{
    public Guid ApiKey { get; set; }
}