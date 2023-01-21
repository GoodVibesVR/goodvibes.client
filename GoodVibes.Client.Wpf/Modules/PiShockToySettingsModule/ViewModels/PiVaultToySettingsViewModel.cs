using System;
using System.Windows;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.ViewModels;

public class PiVaultToySettingsViewModel : RegionViewModelBase
{
    private readonly PiShockClient _piShockClient;
    private readonly IEventAggregator _eventAggregator;

    private string _toyId;
    public string ToyId
    {
        get => _toyId;
        set => SetProperty(ref _toyId, value);
    }

    private string _displayName;
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    private bool _online;
    public bool Online
    {
        get => _online;
        set => SetProperty(ref _online, value);
    }

    private int _keyHoldersCount;
    public int KeyHoldersCount
    {
        get => _keyHoldersCount;
        set => SetProperty(ref _keyHoldersCount, value);
    }

    private string _ownerUsername;
    private string OwnerUsername
    {
        get => _ownerUsername;
        set => SetProperty(ref _ownerUsername, value);
    }

    private int _timesForced;
    private int TimesForced
    {
        get => _timesForced;
        set => SetProperty(ref _timesForced, value);
    }

    private bool _selfLocking;
    private bool SelfLocking
    {
        get => _selfLocking;
        set => SetProperty(ref _selfLocking, value);
    }

    private int _maxMinutesOverall;
    private int MaxMinutesOverall
    {
        get => _maxMinutesOverall;
        set => SetProperty(ref _maxMinutesOverall, value);
    }

    private int _maxMinutesSelfBondage;
    private int MaxMinutesSelfBondage
    {
        get => _maxMinutesSelfBondage;
        set => SetProperty(ref _maxMinutesSelfBondage, value);
    }

    private bool _normallyUnlocked;
    private bool NormallyUnlocked
    {
        get => _normallyUnlocked;
        set => SetProperty(ref _normallyUnlocked, value);
    }

    private string _timeZone;
    private string TimeZone
    {
        get => _timeZone;
        set => SetProperty(ref _timeZone, value);
    }

    private bool _hygieneActive;
    private bool HygieneActive
    {
        get => _hygieneActive;
        set => SetProperty(ref _hygieneActive, value);
    }

    private bool _usingEmlaLock;
    private bool UsingEmlaLock
    {
        get => _usingEmlaLock;
        set => SetProperty(ref _usingEmlaLock, value);
    }

    private bool _usingChaster;
    private bool UsingChaster
    {
        get => _usingChaster;
        set => SetProperty(ref _usingChaster, value);
    }

    private bool _canUnlock;
    private bool CanUnlock
    {
        get => _canUnlock;
        set => SetProperty(ref _canUnlock, value);
    }

    private DateTime _lastPolled;
    private DateTime LastPolled
    {
        get => _lastPolled;
        set => SetProperty(ref _lastPolled, value);
    }

    private DateTime? _lastUnlocked;
    private DateTime? LastUnlocked
    {
        get => _lastUnlocked;
        set => SetProperty(ref _lastUnlocked, value);
    }

    private DateTime? _lastOpened;
    private DateTime? LastOpened
    {
        get => _lastOpened;
        set => SetProperty(ref _lastOpened, value);
    }

    private DateTime? _lastClosed;
    private DateTime? LastClosed
    {
        get => _lastClosed;
        set => SetProperty(ref _lastClosed, value);
    }

    private DateTime? _lockedSince;
    private DateTime? LockedSince
    {
        get => _lockedSince;
        set => SetProperty(ref _lockedSince, value);
    }

    private DateTime? _lockedUntil;
    private DateTime? LockedUntil
    {
        get => _lockedUntil;
        set => SetProperty(ref _lockedUntil, value);
    }

    private WeekdaysEnum[] _hygieneDays;
    private WeekdaysEnum[] HygieneDays
    {
        get => _hygieneDays;
        set => SetProperty(ref _hygieneDays, value);
    }

    private int? _hygieneHour;
    private int? HygieneHour
    {
        get => _hygieneHour;
        set => SetProperty(ref _hygieneHour, value);
    }

    private int? _hygieneMinute;
    private int? HygieneMinute
    {
        get => _hygieneMinute;
        set => SetProperty(ref _hygieneMinute, value);
    }

    private int? _hygieneDuration;
    private int? HygieneDuration
    {
        get => _hygieneDuration;
        set => SetProperty(ref _hygieneDuration, value);
    }

    public PiVaultToySettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, PiShockClient piShockClient) : base(regionManager)
    {
        _eventAggregator = eventAggregator;
        _piShockClient = piShockClient;

        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseEventCarrier>().Subscribe(ReceivePiVaultLockBoxStatus);
    }

    private void ReceivePiVaultLockBoxStatus(ReceivePiVaultLockBoxStatusResponseEvent obj)
    {
        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            DisplayName = obj.Name;
            Online = obj.Online;
            KeyHoldersCount = obj.KeyHoldersCount;
            OwnerUsername = obj.Username;
            TimesForced = obj.TimesForced;
            SelfLocking = obj.SelfLocking;
            MaxMinutesOverall = obj.MaxMinutesOverall;
            MaxMinutesSelfBondage = obj.MaxMinutesSelfBondage;
            NormallyUnlocked = obj.NormallyUnlocked;
            TimeZone = obj.TimeZone;
            HygieneActive = obj.HygieneActive;
            UsingEmlaLock = obj.UsingEmlalock;
            UsingChaster = obj.UsingChaster;
            CanUnlock = obj.CanUnlock;
            LastPolled = obj.LastPolled;
            LastUnlocked = obj.LastUnlocked;
            LastOpened = obj.LastOpened;
            LastClosed = obj.LastClosed;
            LockedSince = obj.LockedSince;
            LockedUntil = obj.LockedUntil;
            HygieneDays = obj.HygieneSettings?.Days;
            HygieneHour = obj.HygieneSettings?.Hours;
            HygieneDuration = obj.HygieneSettings?.Duration;
        });
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        var toyId = (string)navigationContext.Parameters["toyId"];
        var toy = _piShockClient.Toys![toyId];

        if (toy == null)
        {
            // TODO: Do something
        }

        if (toy is not PiShock.Models.PiVault piVault) return;

        ToyId = piVault.ApiKey.ToString();
        DisplayName = piVault.Name;
        Online = piVault.Online;
        KeyHoldersCount = piVault.KeyHoldersCount;
        OwnerUsername = piVault.Username;
        TimesForced = piVault.TimesForced;
        SelfLocking = piVault.SelfLocking;
        MaxMinutesOverall = piVault.MaxMinutesOverall;
        MaxMinutesSelfBondage = piVault.MaxMinutesSelfBondage;
        NormallyUnlocked = piVault.NormallyUnlocked;
        TimeZone = piVault.TimeZone;
        HygieneActive = piVault.HygieneActive;
        UsingEmlaLock = piVault.UsingEmlalock;
        UsingChaster = piVault.UsingChaster;
        CanUnlock = piVault.CanUnlock;
        LastPolled = piVault.LastPolled;
        LastUnlocked = piVault.LastUnlocked;
        LastOpened = piVault.LastOpened;
        LastClosed = piVault.LastClosed;
        LockedSince = piVault.LockedSince;
        LockedUntil = piVault.LockedUntil;
        HygieneDays = piVault.HygieneSettings?.Days;
        HygieneHour = piVault.HygieneSettings?.Hours;
        HygieneDuration = piVault.HygieneSettings?.Duration;
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _eventAggregator.GetEvent<SavePiShockCacheEventCarrier>().Publish(new SavePiShockCacheEvent());
    }
}