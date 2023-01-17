using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.Models.Abstractions;

namespace GoodVibes.Client.PiShock.Models;

public class PiVault : PiShockToy
{
    public override int Id { get; set; }

    public Guid ApiKey { get; set; }

    public override ToyTypeEnum ToyType => ToyTypeEnum.PiVault;

    public bool Online { get; set; }

    public string? Name { get; set; }

    public int KeyHoldersCount { get; set; }

    public string? Username { get; set; }

    public int TimesForced { get; set; }

    public bool SelfLocking { get; set; }

    public int MaxMinutesOverall { get; set; }

    public int MaxMinutesSelfBondage { get; set; }

    public bool NormallyUnlocked { get; set; }

    public string? TimeZone { get; set; }

    public bool HygieneActive { get; set; }

    public bool UsingEmlalock { get; set; }

    public bool UsingChaster { get; set; }

    public bool CanUnlock { get; set; }

    public HygieneSettings? HygieneSettings { get; set; }

    public DateTime LastPolled { get; set; }

    public DateTime? LastUnlocked { get; set; }

    public DateTime? LastOpened { get; set; }

    public DateTime? LastClosed { get; set; }

    public DateTime? LockedSince { get; set; }

    public DateTime? LockedUntil { get; set; }
}

public class HygieneSettings
{
    public bool Active { get; set; }

    public string? CronExpression { get; set; }

    public WeekdaysEnum[]? Days { get; set; }

    public int Hours { get; set; }

    public int Minutes { get; set; }

    public int Duration { get; set; }
}