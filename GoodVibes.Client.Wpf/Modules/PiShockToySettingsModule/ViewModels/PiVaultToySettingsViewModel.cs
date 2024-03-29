﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.ViewModels;

public class PiVaultToySettingsViewModel : RegionViewModelBase
{
    private readonly PiShockClient _piShockClient;
    private readonly IPiVaultService _piVaultService;
    private readonly IEventAggregator _eventAggregator;

    private DispatcherTimer _dispatcherTimer;
    private bool _eventLocked;

    private ImageSource _logoImage;
    public ImageSource LogoImage
    {
        get => _logoImage;
        set => SetProperty(ref _logoImage, value);
    }

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
    public string OwnerUsername
    {
        get => _ownerUsername;
        set => SetProperty(ref _ownerUsername, value);
    }

    private int _timesForced;
    public int TimesForced
    {
        get => _timesForced;
        set => SetProperty(ref _timesForced, value);
    }

    private bool _selfLocking;
    public bool SelfLocking
    {
        get => _selfLocking;
        set => SetProperty(ref _selfLocking, value);
    }

    private int _maxMinutesOverall;
    public int MaxMinutesOverall
    {
        get => _maxMinutesOverall;
        set => SetProperty(ref _maxMinutesOverall, value);
    }

    private int _maxMinutesSelfBondage;
    public int MaxMinutesSelfBondage
    {
        get => _maxMinutesSelfBondage;
        set => SetProperty(ref _maxMinutesSelfBondage, value);
    }

    private bool _normallyUnlocked;
    public bool NormallyUnlocked
    {
        get => _normallyUnlocked;
        set => SetProperty(ref _normallyUnlocked, value);
    }

    private string _timeZone;
    public string TimeZone
    {
        get => _timeZone;
        set => SetProperty(ref _timeZone, value);
    }

    private bool _hygieneActive;
    public bool HygieneActive
    {
        get => _hygieneActive;
        set => SetProperty(ref _hygieneActive, value);
    }

    private bool _usingEmlaLock;
    public bool UsingEmlaLock
    {
        get => _usingEmlaLock;
        set
        {
            SetProperty(ref _usingEmlaLock, value);
            PermissionTimeChange = value; // Not really doing anything;
            PermissionTimeReduction = value; // Not really doing anything
        }
    }

    private bool _usingChaster;
    public bool UsingChaster
    {
        get => _usingChaster;
        set
        {
            SetProperty(ref _usingChaster, value);
            PermissionTimeChange = value; // Not really doing anything;
            PermissionTimeReduction = value; // Not really doing anything
        }
    }

    private bool _canUnlock;
    public bool CanUnlock
    {
        get => _canUnlock;
        set => SetProperty(ref _canUnlock, value);
    }

    private DateTime _lastPolled;
    public DateTime LastPolled
    {
        get => _lastPolled;
        set => SetProperty(ref _lastPolled, value);
    }

    private DateTime? _lastUnlocked;
    public DateTime? LastUnlocked
    {
        get => _lastUnlocked;
        set => SetProperty(ref _lastUnlocked, value);
    }

    private DateTime? _lastOpened;
    public DateTime? LastOpened
    {
        get => _lastOpened;
        set => SetProperty(ref _lastOpened, value);
    }

    private DateTime? _lastClosed;
    public DateTime? LastClosed
    {
        get => _lastClosed;
        set => SetProperty(ref _lastClosed, value);
    }

    private DateTime? _lockedSince;
    public DateTime? LockedSince
    {
        get => _lockedSince;
        set
        {
            SetProperty(ref _lockedSince, value);
            LockedSinceFormatted = ""; // Stupid hack, can this be done any other way?
        }
    }

    private string _lockedSinceFormatted;
    public string LockedSinceFormatted
    {
        get => _lockedSinceFormatted;
        set => SetProperty(ref _lockedSinceFormatted,
            LockedSince.HasValue
                ? $"{LockedSince.Value:d}\n{LockedSince.Value:t}"
                : "");
    }

    private DateTime? _lockedUntil;
    public DateTime? LockedUntil
    {
        get => _lockedUntil;
        set
        {
            SetProperty(ref _lockedUntil, value);
            LockedUntilFormatted = "";   // Stupid hack, can this be done any other way?
        }
    }

    private string _lockedUntilFormatted;
    public string LockedUntilFormatted
    {
        get => _lockedUntilFormatted;
        set => SetProperty(ref _lockedUntilFormatted, LockedUntil.HasValue ? $"{LockedUntil.Value:d}\n{LockedUntil.Value:t}" : "");
    }

    private WeekdaysEnum[] _hygieneDays;
    public WeekdaysEnum[] HygieneDays
    {
        get => _hygieneDays;
        set
        {
            SetProperty(ref _hygieneDays, value ?? new WeekdaysEnum[0]);
            HygieneDaysFormatted = ""; // Stupid hack, can this be done any other way?
        }
    }

    private string _hygieneDaysFormatted;
    public string HygieneDaysFormatted
    {
        get => _hygieneDaysFormatted;
        set => SetProperty(ref _hygieneDaysFormatted,
            _hygieneDays.Length > 0
                ? string.Join(", ", _hygieneDays.Select(day => day.ToString().Substring(0, 3)))
                : "");
    }

    private int? _hygieneHour;
    public int? HygieneHour
    {
        get => _hygieneHour;
        set
        {
            SetProperty(ref _hygieneHour, value);
            HygieneTimeFormatted = "";  // Stupid hack, can this be done any other way?
        }
    }

    private int? _hygieneMinute;
    public int? HygieneMinute
    {
        get => _hygieneMinute;
        set
        {
            SetProperty(ref _hygieneMinute, value);
            HygieneTimeFormatted = "";  // Stupid hack, can this be done any other way?
        }
    }

    private int? _hygieneDuration;
    public int? HygieneDuration
    {
        get => _hygieneDuration;
        set
        {
            SetProperty(ref _hygieneDuration, value);
            HygieneTimeFormatted = "";
        }
    }

    private string _hygieneTimeFormatted;
    public string HygieneTimeFormatted
    {
        get => _hygieneTimeFormatted;
        set
        {
            var dt = new DateTime(1970, 1, 1, HygieneHour.GetValueOrDefault(0), HygieneMinute.GetValueOrDefault(0), 0, DateTimeKind.Utc);
            var future = dt.AddMinutes(HygieneDuration.GetValueOrDefault(0));
            SetProperty(ref _hygieneTimeFormatted, $"{dt.ToShortTimeString()} - {future.ToShortTimeString()}");
        }
    }

    private bool _permissionAllowTimeChange;
    public bool PermissionAllowTimeChange
    {
        get => _permissionAllowTimeChange;
        set
        {
            SetProperty(ref _permissionAllowTimeChange, value);
            PermissionTimeChange = value; // Not really doing anything;
        }
    }

    private bool _permissionsAllowTimeReduction;
    public bool PermissionsAllowTimeReduction
    {
        get => _permissionsAllowTimeReduction;
        set
        {
            SetProperty(ref _permissionsAllowTimeReduction, value);
            PermissionTimeReduction = value; // Not really doing anything
        }
    }

    private bool _permissionSessionStart;
    public bool PermissionSessionStart
    {
        get => _permissionSessionStart;
        set => SetProperty(ref _permissionSessionStart, value);
    }

    private bool _permissionCanUnlock;
    public bool PermissionCanUnlock
    {
        get => _permissionCanUnlock;
        set
        {
            SetProperty(ref _permissionCanUnlock, value);
            PermissionClearSession = value;
            ClearSessionOpacity = PermissionClearSession ? 1 : (float)0.2;
            UnlockOpacity = value ? 1 : (float)0.2;
        }
    }

    private int _amountToAddOrRemove;
    public int AmountToAddOrRemove
    {
        get => _amountToAddOrRemove;
        set
        {
            SetAmountToAddOrRemove(value);
            SetProperty(ref _amountToAddOrRemove, value);
        }
    }

    private bool _permissionTimeChange;
    public bool PermissionTimeChange
    {
        get => _permissionTimeChange;
        set
        {
            var perm = !_usingChaster && !_usingEmlaLock && PermissionAllowTimeChange;
            SetProperty(ref _permissionTimeChange, perm);
            AddOpacity = perm ? 1 : (float)0.2;
        }
    }

    private bool _permissionTimeReduction;
    public bool PermissionTimeReduction
    {
        get => _permissionTimeReduction;
        set
        {
            var perm = !_usingChaster && !_usingEmlaLock && PermissionsAllowTimeReduction;
            SetProperty(ref _permissionTimeReduction, perm);
            RemoveOpacity = perm ? 1 : (float)0.2;
        }
    }

    private bool _permissionClearSession;

    public bool PermissionClearSession
    {
        get => _permissionClearSession;
        set => SetProperty(ref _permissionClearSession,
            !UsingEmlaLock && !UsingChaster && PermissionAllowTimeChange && PermissionsAllowTimeReduction &&
            PermissionCanUnlock && PermissionSessionStart);
    }

    private int _timeGauge;

    public int TimeGauge
    {
        get => _timeGauge;
        set => SetProperty(ref _timeGauge, value);
    }

    private float _unlockOpacity;
    public float UnlockOpacity
    {
        get => _unlockOpacity;
        set => SetProperty(ref _unlockOpacity, value);
    }

    private float _clearSessionOpacity;
    public float ClearSessionOpacity
    {
        get => _clearSessionOpacity;
        set => SetProperty(ref _clearSessionOpacity, value);
    }

    private float _addOpacity;
    public float AddOpacity
    {
        get => _addOpacity;
        set => SetProperty(ref _addOpacity, value);
    }

    private float _removeOpacity;
    public float RemoveOpacity
    {
        get => _removeOpacity;
        set => SetProperty(ref _removeOpacity, value);
    }

    private DelegateCommand _unlockCommand;
    public DelegateCommand UnlockCommand =>
        _unlockCommand ??= new DelegateCommand(Unlock);

    private DelegateCommand _clearSessionCommand;
    public DelegateCommand ClearSessionCommand =>
        _clearSessionCommand ??= new DelegateCommand(ClearSession);

    private DelegateCommand _addMinutesToSessionCommand;
    public DelegateCommand AddMinutesToSessionCommand =>
        _addMinutesToSessionCommand ??= new DelegateCommand(AddMinutesToSession);

    private DelegateCommand _addHoursToSessionCommand;
    public DelegateCommand AddHoursToSessionCommand =>
        _addHoursToSessionCommand ??= new DelegateCommand(AddHoursToSession);

    private DelegateCommand _addDaysToSessionCommand;
    public DelegateCommand AddDaysToSessionCommand =>
        _addDaysToSessionCommand ??= new DelegateCommand(AddDaysToSession);

    private DelegateCommand _removeMinutesFromSessionCommand;
    public DelegateCommand RemoveMinutesFromSessionCommand =>
        _removeMinutesFromSessionCommand ??= new DelegateCommand(RemoveMinutesFromSession);

    private DelegateCommand _removeHoursFromSessionCommand;
    public DelegateCommand RemoveHoursFromSessionCommand =>
        _removeHoursFromSessionCommand ??= new DelegateCommand(RemoveHoursFromSession);

    private DelegateCommand _removeDaysFromSessionCommand;
    public DelegateCommand RemoveDaysFromSessionCommand =>
        _removeDaysFromSessionCommand ??= new DelegateCommand(RemoveDaysFromSession);

    public PiVaultToySettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
        PiShockClient piShockClient, IPiVaultService piVaultService) : base(regionManager)
    {
        _eventAggregator = eventAggregator;
        _piShockClient = piShockClient;
        _piVaultService = piVaultService;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        var toyId = (string)navigationContext.Parameters["toyId"];
        GetAndSetParameters(toyId);

        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseEventCarrier>().Subscribe(ReceivePiVaultLockBoxStatus);
        _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseEventCarrier>().Subscribe(ReceivePiVaultAPiKeyPermissions);
        _eventAggregator.GetEvent<PiVaultAmountToAddOrRemoveChangedEventCarrier>().Subscribe(PiVaultAmountToAddOrRemoveEventHandler);

        UpdateTimeGauge();
        _dispatcherTimer = new DispatcherTimer();
        _dispatcherTimer.Tick += new EventHandler(UpdateTimeGaugeTick);
        _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        _dispatcherTimer.Start();
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _dispatcherTimer.Stop();

        _eventAggregator.GetEvent<SavePiShockCacheEventCarrier>().Publish(new SavePiShockCacheEvent());
        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseEventCarrier>().Unsubscribe(ReceivePiVaultLockBoxStatus);
        _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseEventCarrier>().Unsubscribe(ReceivePiVaultAPiKeyPermissions);
    }

    private void ReceivePiVaultAPiKeyPermissions(ReceivePiVaultApiKeyPermissionsResponseEvent obj)
    {
        if (ToyId != obj.ApiKey.ToString())
        {
            return;
        }

        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            PermissionAllowTimeChange = obj.AllowTimeChange;
            PermissionsAllowTimeReduction = obj.AllowTimeReduction;
            PermissionSessionStart = obj.SessionStart;
            PermissionCanUnlock = obj.CanUnlock;
        });
    }

    private void ReceivePiVaultLockBoxStatus(ReceivePiVaultLockBoxStatusResponseEvent obj)
    {
        if (ToyId != obj.ApiKey.ToString())
        {
            return;
        }

        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            GetAndSetParameters(obj.ApiKey.ToString());
        });
    }

    private void GetAndSetParameters(string toyId)
    {
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
        HygieneMinute = piVault.HygieneSettings?.Minutes;
        HygieneDuration = piVault.HygieneSettings?.Duration;
        PermissionAllowTimeChange = piVault.Permissions.AllowTimeChange;
        PermissionsAllowTimeReduction = piVault.Permissions.AllowTimeReduction;
        PermissionSessionStart = piVault.Permissions.SessionStart;
        PermissionCanUnlock = piVault.Permissions.CanUnlock;

        _eventLocked = true;
        AmountToAddOrRemove = piVault.AmountToAddRemove;
        _eventLocked = false;

        LogoImage = _piVaultService.GetLogoIcon(UsingEmlaLock, UsingChaster, CanUnlock);
    }

    private void UpdateTimeGaugeTick(object sender, EventArgs e)
    {
        UpdateTimeGauge();
    }

    private void UpdateTimeGauge()
    {
        if (!LockedSince.HasValue || !LockedUntil.HasValue)
        {
            TimeGauge = 0;
            return;
        }

        var totalDuration = LockedUntil.Value.Subtract(LockedSince.Value);
        var durationLeft = LockedUntil.Value.Subtract(DateTime.Now);

        TimeGauge = (int)Math.Round((totalDuration.TotalSeconds - durationLeft.TotalSeconds) / totalDuration.TotalSeconds * 100 * 1.8);
    }

    private void Unlock()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.Unlock
        });
    }

    private void ClearSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.ClearSession
        });
    }

    private void AddMinutesToSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.AddMinutes
        });
    }

    private void AddHoursToSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.AddHours
        });
    }

    private void AddDaysToSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.AddDays
        });
    }

    private void RemoveMinutesFromSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.RemoveMinutes
        });
    }

    private void RemoveHoursFromSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.RemoveHours
        });
    }

    private void RemoveDaysFromSession()
    {
        _eventAggregator.GetEvent<PiVaultCommandEventCarrier>().Publish(new PiVaultCommandEvent()
        {
            ApiKey = new Guid(ToyId),
            Command = PiVaultCommandEnum.RemoveDays
        });
    }

    private void PiVaultAmountToAddOrRemoveEventHandler(PiVaultAmountToAddOrRemoveChangedEvent obj)
    {
        if (ToyId != obj.ApiKey.ToString()) return;

        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            _eventLocked = true;
            AmountToAddOrRemove = obj.Amount;
            _eventLocked = false;
        });
    }

    private void SetAmountToAddOrRemove(int value)
    {
        if (_eventLocked) return;
        _eventAggregator.GetEvent<PiVaultAmountToAddOrRemoveChangedEventCarrier>().Publish(new PiVaultAmountToAddOrRemoveChangedEvent()
        {
            ApiKey = new Guid(ToyId),
            Amount = value
        });

    }
}