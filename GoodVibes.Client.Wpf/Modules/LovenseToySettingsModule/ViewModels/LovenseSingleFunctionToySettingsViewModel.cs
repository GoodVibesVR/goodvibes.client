using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.Enums;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.ViewModels
{
    internal class LovenseSingleFunctionToySettingsViewModel : RegionViewModelBase
    {
        private readonly LovenseClient _lovenseClient;

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

        private void ChangeStrength(double value)
        {
            Console.WriteLine($"Strength2 changed to: {value}");
        }

        public LovenseSingleFunctionToySettingsViewModel(IRegionManager regionManager, LovenseClient lovenseClient) : base(regionManager)
        {
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
            Battery = toy.Battery;
            Enabled = toy.Enabled;
        }
    }
}
