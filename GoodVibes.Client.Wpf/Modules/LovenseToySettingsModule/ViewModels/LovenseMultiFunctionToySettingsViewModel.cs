using System;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.ViewModels;

internal class LovenseMultiFunctionToySettingsViewModel : RegionViewModelBase
{
    private readonly IEventAggregator _eventAggregator;
    private readonly LovenseClient _lovenseClient;

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

    private LovenseCommandEnum _function1;
    public LovenseCommandEnum Function1
    {
        get => _function1;
        set => SetProperty(ref _function1, value);
    }

    private LovenseCommandEnum _function2;
    public LovenseCommandEnum Function2
    {
        get => _function2;
        set => SetProperty(ref _function2, value);
    }

    private int? _battery;
    public int? Battery
    {
        get => _battery;
        set => SetProperty(ref _battery, value);
    }

    private bool _enabled;
    public bool Enabled
    {
        get => _enabled;
        set => SetProperty(ref _enabled, value);
    }

    private double _strength1;
    public double Strength1
    {
        get => _strength1;
        set
        {
            SetProperty(ref _strength1, value);
            ChangeStrength1(value);
        }
    }
    
    private double _strength2;
    public double Strength2
    {
        get => _strength2;
        set
        {
            SetProperty(ref _strength2, value);
            ChangeStrength2(value);
        }
    }

    public LovenseMultiFunctionToySettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, LovenseClient lovenseClient) : base(regionManager)
    {
        _eventAggregator = eventAggregator;
        _lovenseClient = lovenseClient;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        var toyId = (string)navigationContext.Parameters["toyId"];
        var toy = _lovenseClient.Toys?[toyId];
        if (toy == null)
        {
            // TODO: Do something
        }
        
        DisplayName = toy.DisplayName;
        Function1 = toy.Function1;
        Function2 = toy.Function2;
        Strength1 = toy.Function1MaxStrengthPercentage;
        Strength2 = toy.Function2MaxStrengthPercentage;
        Battery = toy.Battery;
        Enabled = toy.Enabled;
        ToyId = toyId;
    }

    private void ChangeStrength1(double value)
    {
        if (ToyId == null) return;
        
        _eventAggregator.GetEvent<LovenseStrengthChangedEventCarrier>().Publish(new LovenseStrengthChangedEvent()
        {
            ToyId = ToyId,
            Strength1Percentage = (int)value,
            Strength2Percentage = (int)Strength2,
            Strength3Percentage = 0
        });

        Console.WriteLine($"Strength1 changed to: {value}");
    }

    private void ChangeStrength2(double value)
    {
        if (ToyId == null) return;

        _eventAggregator.GetEvent<LovenseStrengthChangedEventCarrier>().Publish(new LovenseStrengthChangedEvent()
        {
            ToyId = ToyId,
            Strength1Percentage = (int)Strength1,
            Strength2Percentage = (int)value,
            Strength3Percentage = 0
        });

        Console.WriteLine($"Strength2 changed to: {value}");
    }
}