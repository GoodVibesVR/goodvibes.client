using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.ViewModels
{
    internal class LovenseSingleFunctionToySettingsViewModel : RegionViewModelBase
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

        private LovenseCommandEnum _function;
        public LovenseCommandEnum Function
        {
            get => _function;
            set => SetProperty(ref _function, value);
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

        private double _strength;
        public double Strength
        {
            get => _strength;
            set
            {
                SetProperty(ref _strength, value);
                ChangeStrength(value);
            }
        }

        public LovenseSingleFunctionToySettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, LovenseClient lovenseClient) : base(regionManager)
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
            Function = toy.Function1;
            Strength = toy.Function1MaxStrengthPercentage;
            Battery = toy.Battery;
            Enabled = toy.Enabled;
            ToyId = toyId;
        }

        private void ChangeStrength(double value)
        {
            if (ToyId == null) return;

            _eventAggregator.GetEvent<LovenseStrengthChangedEventCarrier>().Publish(new LovenseStrengthChangedEvent()
            {
                ToyId = ToyId,
                Strength1Percentage = (int)value,
                Strength2Percentage = 0,
                Strength3Percentage = 0
            });
        }
    }
}
