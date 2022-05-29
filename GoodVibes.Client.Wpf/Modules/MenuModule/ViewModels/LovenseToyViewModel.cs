using System.Windows.Media;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels
{
    public class LovenseToyViewModel : ViewModelBase
    {
        private DelegateCommand _navigateToSettingsCommand;
        public DelegateCommand NavigateToSettingsCommand =>
            _navigateToSettingsCommand ??= new DelegateCommand(NavigateToSettings);

        private void NavigateToSettings()
        {
            var regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            var queryString = $"?toyId={Id}";

            regionManager.RequestNavigate(RegionNames.ContentRegion, "LushSettingsView" + queryString);
        }

        private string _id;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private int? _battery;

        public int? Battery
        {
            get => _battery;
            set => SetProperty(ref _battery, value);
        }

        private bool _status;

        public bool Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private ImageSource _toyIcon;

        public ImageSource ToyIcon
        {
            get => _toyIcon;
            set => SetProperty(ref _toyIcon, value);
        }
    }
}
