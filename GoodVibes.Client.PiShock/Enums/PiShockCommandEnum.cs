﻿namespace GoodVibes.Client.PiShock.Enums
{
    public enum PiShockCommandEnum
    {
        Unknown = 0,
        Shock = 10,
        Vibrate = 11,
        Beep = 12,

        // A variant of Shock with different duration
        MiniShock = 50,

        // Command to pause the PiShock from receiving actions
        Pause = 60
    }
}
