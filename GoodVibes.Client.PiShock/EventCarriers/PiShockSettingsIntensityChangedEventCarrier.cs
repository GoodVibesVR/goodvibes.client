﻿using GoodVibes.Client.PiShock.Events;
using Prism.Events;

namespace GoodVibes.Client.PiShock.EventCarriers;

public class PiShockSettingsIntensityChangedEventCarrier : PubSubEvent<PiShockIntensityChangedEvent>
{
}