using System;
using System.Windows;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using GoodVibes.Client.Wpf.Modules.AddToyModule.Views;
using GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels;

public class AddPiVaultViewModel : RegionViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private readonly PiShockClient _piShockClient;

    private Visibility _piShockNotConnectedVisibility;
    public Visibility PiShockNotConnectedVisibility
    {
        get => _piShockNotConnectedVisibility;
        set => SetProperty(ref _piShockNotConnectedVisibility, value);
    }

    private Visibility _addPiVaultFormVisibility;
    public Visibility AddPiVaultFormVisibility
    {
        get => _addPiVaultFormVisibility;
        set => SetProperty(ref _addPiVaultFormVisibility, value);
    }

    private Visibility _checkPermissionsButtonVisibility;
    public Visibility CheckPermissionsButtonVisibility
    {
        get => _checkPermissionsButtonVisibility;
        set => SetProperty(ref _checkPermissionsButtonVisibility, value);
    }

    private Visibility _loadingSpinnerVisibility;
    public Visibility LoadingSpinnerVisibility
    {
        get => _loadingSpinnerVisibility;
        set => SetProperty(ref _loadingSpinnerVisibility, value);
    }

    private Visibility _addPiVaultButtonVisibility;
    public Visibility AddPiVaultButtonVisibility
    {
        get => _addPiVaultButtonVisibility;
        set => SetProperty(ref _addPiVaultButtonVisibility, value);
    }

    private Visibility _errorMessageVisibility;
    public Visibility ErrorMessageVisibility
    {
        get => _errorMessageVisibility;
        set => SetProperty(ref _errorMessageVisibility, value);
    }

    private Visibility _permissionsVisibility;
    public Visibility PermissionsVisibility 
    { 
        get => _permissionsVisibility;
        set => SetProperty(ref _permissionsVisibility, value);
    }

    private Visibility _timeChangeTrueVisibility;
    public Visibility TimeChangeTrueVisibility
    {
        get => _timeChangeTrueVisibility;
        set => SetProperty(ref _timeChangeTrueVisibility, value);
    }

    private Visibility _timeChangeFalseVisibility;
    public Visibility TimeChangeFalseVisibility
    {
        get => _timeChangeFalseVisibility;
        set => SetProperty(ref _timeChangeFalseVisibility, value);
    }

    private Visibility _reduceTimeTrueVisibility;
    public Visibility ReduceTimeTrueVisibility
    {
        get => _reduceTimeTrueVisibility;
        set => SetProperty(ref _reduceTimeTrueVisibility, value);
    }

    private Visibility _reduceTimeFalseVisibility;
    public Visibility ReduceTimeFalseVisibility

    {
        get => _reduceTimeFalseVisibility;
        set => SetProperty(ref _reduceTimeFalseVisibility, value);
    }

    private Visibility _startSessionTrueVisibility;
    public Visibility StartSessionTrueVisibility
    {
        get => _startSessionTrueVisibility;
        set => SetProperty(ref _startSessionTrueVisibility, value);
    }

    private Visibility _startSessionFalseVisibility;
    public Visibility StartSessionFalseVisibility
    {
        get => _startSessionFalseVisibility;
        set => SetProperty(ref _startSessionFalseVisibility, value);
    }

    private Visibility _unlockPiVaultTrueVisibility;
    public Visibility UnlockPiVaultTrueVisibility
    {
        get => _unlockPiVaultTrueVisibility;
        set => SetProperty(ref _unlockPiVaultTrueVisibility, value);
    }

    private Visibility _unlockPiVaultFalseVisibility;
    public Visibility UnlockPiVaultFalseVisibility
    {
        get => _unlockPiVaultFalseVisibility;
        set => SetProperty(ref _unlockPiVaultFalseVisibility, value);
    }

    private string _apiKey;
    public string ApiKey
    {
        get => _apiKey;
        set
        {
            if (value != _apiKey)
            {
                OnApiKeyChangedHandler();
            }

            SetProperty(ref _apiKey, value);
        }
    }

    private bool _allowTimeChange;
    public bool AllowTimeChange
    {
        get => _allowTimeChange;
        set => SetProperty(ref _allowTimeChange, value);
    }

    private bool _allowTimeReduction;
    public bool AllowTimeReduction
    {
        get => _allowTimeReduction;
        set => SetProperty(ref _allowTimeReduction, value);
    }

    private bool _sessionStart;
    public bool SessionStart
    {
        get => _sessionStart;
        set => SetProperty(ref _sessionStart, value);
    }

    private bool _canUnlock;
    public bool CanUnlock
    {
        get => _canUnlock;
        set => SetProperty(ref _canUnlock, value);
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    private DelegateCommand _connectToPiShockCommand;

    public DelegateCommand ConnectToPiShockCommand =>
        _connectToPiShockCommand ??= new DelegateCommand(ConnectToPiShockHandler);

    private DelegateCommand _onApiKeyChangedCommand;
    public DelegateCommand OnApiKeyChangedCommand =>
        _onApiKeyChangedCommand ??= new DelegateCommand(OnApiKeyChangedHandler);

    private DelegateCommand _checkPermissionsCommand;
    public DelegateCommand CheckPermissionsCommand =>
        _checkPermissionsCommand ??= new DelegateCommand(CheckApiKeyPermissionsHandler);

    private DelegateCommand _addPiVaultCommand;
    public DelegateCommand AddPiVaultCommand =>
        _addPiVaultCommand ??= new DelegateCommand(AddPiVaultHandler);

    public AddPiVaultViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, PiShockClient piShockClient) : base(regionManager)
    {
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        _piShockClient = piShockClient;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Console.WriteLine("Now navigated to AddPiVault form");

        if (!_piShockClient.Connected)
        {
            PiShockNotConnectedVisibility = Visibility.Visible;
            AddPiVaultFormVisibility = Visibility.Collapsed;

            return;
        }

        // Permissions check
        _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseEventCarrier>().Subscribe(ReceivePiVaultAPiKeyPermissions);
        _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseErrorEventCarrier>().Subscribe(ReceivePiVaultAPiKeyPermissionsError); ;

        // PiVault information
        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseEventCarrier>().Subscribe(ReceivePiVaultLockBoxStatus);
        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseErrorEventCarrier>().Subscribe(ReceivePiVaultLockBoxStatusError);

        PiShockNotConnectedVisibility = Visibility.Collapsed;
        AddPiVaultFormVisibility = Visibility.Visible;
        PermissionsVisibility = Visibility.Collapsed;

        CheckPermissionsButtonVisibility = Visibility.Visible;
        LoadingSpinnerVisibility = Visibility.Collapsed;
        AddPiVaultButtonVisibility = Visibility.Collapsed;
        ErrorMessageVisibility = Visibility.Collapsed;
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        // Permissions check
        _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseEventCarrier>().Unsubscribe(ReceivePiVaultAPiKeyPermissions);
        _eventAggregator.GetEvent<ReceivePiVaultApiKeyPermissionsResponseErrorEventCarrier>().Unsubscribe(ReceivePiVaultAPiKeyPermissionsError); ;

        // PiVault information
        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseEventCarrier>().Unsubscribe(ReceivePiVaultLockBoxStatus);
        _eventAggregator.GetEvent<ReceivePiVaultLockBoxStatusResponseErrorEventCarrier>().Unsubscribe(ReceivePiVaultLockBoxStatusError);

        ApiKey = string.Empty;
        ErrorMessage = string.Empty;
    }

    private void OnApiKeyChangedHandler()
    {
        ErrorMessage = string.Empty;

        ErrorMessageVisibility = Visibility.Collapsed;
        CheckPermissionsButtonVisibility = Visibility.Visible;
        LoadingSpinnerVisibility = Visibility.Collapsed;
        AddPiVaultButtonVisibility = Visibility.Collapsed;
        PermissionsVisibility = Visibility.Collapsed;
    }

    private void CheckApiKeyPermissionsHandler()
    {
        if (!Guid.TryParse(_apiKey, out var keyGuid))
        {
            ErrorMessage = "API key is not valid.";
            ErrorMessageVisibility = Visibility.Visible;
            CheckPermissionsButtonVisibility = Visibility.Visible;
            AddPiVaultButtonVisibility = Visibility.Collapsed;
            LoadingSpinnerVisibility = Visibility.Collapsed;
            PermissionsVisibility = Visibility.Collapsed;

            return;
        }

        ErrorMessageVisibility = Visibility.Collapsed;
        CheckPermissionsButtonVisibility = Visibility.Collapsed;
        AddPiVaultButtonVisibility = Visibility.Collapsed;
        LoadingSpinnerVisibility = Visibility.Visible;
        PermissionsVisibility = Visibility.Collapsed;

        _eventAggregator.GetEvent<GetPiVaultApiKeyPermissionsEventCarrier>().Publish(new GetPiVaultApiKeyPermissionsEvent()
        {
            ApiKey = new Guid(_apiKey)
        });
    }

    private void ConnectToPiShockHandler()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "PiShockConnectView");
    }

    private void AddPiVaultHandler()
    {
        if (string.IsNullOrEmpty(_apiKey))
            return;

        if (_piShockClient.Toys!.TryGetValue(_apiKey, out var piVault))
        {
            ErrorMessage = "PiVault already added to GoodVibes";

            PermissionsVisibility = Visibility.Collapsed;
            ErrorMessageVisibility = Visibility.Visible;
            CheckPermissionsButtonVisibility = Visibility.Visible;
            AddPiVaultButtonVisibility = Visibility.Collapsed;
            LoadingSpinnerVisibility = Visibility.Collapsed;

            return;
        }

        PermissionsVisibility = Visibility.Collapsed;
        ErrorMessageVisibility = Visibility.Collapsed;
        CheckPermissionsButtonVisibility = Visibility.Collapsed;
        AddPiVaultButtonVisibility = Visibility.Collapsed;
        LoadingSpinnerVisibility = Visibility.Visible;

        _eventAggregator.GetEvent<PiShockToyAddedEventCarrier>().Publish(new PiShockToyAddedEvent()
        {
            ApiKey = new Guid(_apiKey),
            ToyType = ToyTypeEnum.PiVault
        });
    }

    private void ReceivePiVaultAPiKeyPermissions(ReceivePiVaultApiKeyPermissionsResponseEvent obj)
    {
        if (obj.ApiKey.ToString() != _apiKey)
        {
            return;
        }

        PermissionsVisibility = Visibility.Visible;
        TimeChangeTrueVisibility = obj.AllowTimeChange ? Visibility.Visible : Visibility.Collapsed;
        TimeChangeFalseVisibility = obj.AllowTimeChange ? Visibility.Collapsed : Visibility.Visible;

        ReduceTimeTrueVisibility = obj.AllowTimeReduction ? Visibility.Visible : Visibility.Collapsed;
        ReduceTimeFalseVisibility = obj.AllowTimeReduction ? Visibility.Collapsed : Visibility.Visible;

        StartSessionTrueVisibility = obj.SessionStart ? Visibility.Visible : Visibility.Collapsed;
        StartSessionFalseVisibility = obj.SessionStart ? Visibility.Collapsed : Visibility.Visible;

        UnlockPiVaultTrueVisibility = obj.CanUnlock ? Visibility.Visible : Visibility.Collapsed;
        UnlockPiVaultFalseVisibility = obj.CanUnlock ? Visibility.Collapsed : Visibility.Visible;

        ErrorMessageVisibility = Visibility.Collapsed;
        CheckPermissionsButtonVisibility = Visibility.Collapsed;
        AddPiVaultButtonVisibility = Visibility.Visible;
        LoadingSpinnerVisibility = Visibility.Collapsed;
    }

    private void ReceivePiVaultAPiKeyPermissionsError(ReceivePiVaultApiKeyPermissionsResponseErrorEvent obj)
    {
        if (obj.ApiKey.ToString() != _apiKey)
        {
            return;
        }

        ErrorMessage = obj.Message;
        ErrorMessageVisibility = Visibility.Visible;
        CheckPermissionsButtonVisibility = Visibility.Visible;
        AddPiVaultButtonVisibility = Visibility.Collapsed;
        LoadingSpinnerVisibility = Visibility.Collapsed;
        PermissionsVisibility = Visibility.Collapsed;
    }

    private void ReceivePiVaultLockBoxStatus(ReceivePiVaultLockBoxStatusResponseEvent obj)
    {
        if (obj.ApiKey.ToString() != _apiKey)
        {
            return;
        }

        var queryString = $"?toyId={_apiKey}";

        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(PiVaultToySettingsView) + queryString);
            _regionManager.RequestNavigate(RegionNames.AddToyForm, nameof(SelectTypeView));
        });
    }

    private void ReceivePiVaultLockBoxStatusError(ReceivePiVaultLockBoxStatusResponseErrorEvent obj)
    {
        if (obj.ApiKey.ToString() != _apiKey)
        {
            return;
        }

        ErrorMessage = obj.Message;
        ErrorMessageVisibility = Visibility.Visible;
        CheckPermissionsButtonVisibility = Visibility.Visible;
        AddPiVaultButtonVisibility = Visibility.Collapsed;
        LoadingSpinnerVisibility = Visibility.Collapsed;
        PermissionsVisibility = Visibility.Collapsed;
    }
}