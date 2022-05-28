using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.ViewModels
{
    internal class LovenseToySettingsViewModel : RegionViewModelBase
    {
        private readonly LovenseClient _lovenseClient;

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public LovenseToySettingsViewModel(IRegionManager regionManager, LovenseClient lovenseClient) : base(regionManager)
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
            // TODO: Populate everything needed for the view
        }
    }
}
