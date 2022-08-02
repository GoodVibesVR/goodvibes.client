using System;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels;

public class AddPiShockViewModel : RegionViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private readonly PiShockClient _piShockClient;

    private string _friendlyName;

    public string FriendlyName
    {
        get => _friendlyName;
        set => SetProperty(ref _friendlyName, value);
    }

    private string _shareCode;

    public string ShareCode
    {
        get => _shareCode;
        set => SetProperty(ref _shareCode, value);
    }

    private string _errorMessage;

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    private DelegateCommand _addPiShockCommand;
    public DelegateCommand AddPiShockCommand =>
        _addPiShockCommand ??= new DelegateCommand(AddPiShockHandler);

    public AddPiShockViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, PiShockClient piShockClient) : base(regionManager)
    {
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        _piShockClient = piShockClient;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Console.WriteLine("Now navigated to AddPiShock form");
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        FriendlyName = string.Empty;
        ShareCode = string.Empty;
        ErrorMessage = string.Empty;
    }

    private void AddPiShockHandler()
    {
        if (_friendlyName == string.Empty || _shareCode == string.Empty)
            return;

        if (_piShockClient.Toys!.TryGetValue(_shareCode, out var shocker))
        {
            ErrorMessage = "ShareCode already added to GoodVibes";
            return;
        }

        _eventAggregator.GetEvent<PiShockToyAddedEventCarrier>().Publish(new PiShockToyAddedEvent()
        {
            FriendlyName = _friendlyName,
            ShareCode = _shareCode,
            ToyType = ToyTypeEnum.PiShock
        });

        var queryString = $"?toyId={_shareCode}";
        _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(PiShockToySettingsView) + queryString);
    }
}