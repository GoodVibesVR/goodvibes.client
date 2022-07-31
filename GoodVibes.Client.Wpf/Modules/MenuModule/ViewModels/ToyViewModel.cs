using System.Windows.Media;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels
{
    public class ToyViewModel : ViewModelBase
    {
        private DelegateCommand _navigateToSettingsCommand;
        public DelegateCommand NavigateToSettingsCommand =>
            _navigateToSettingsCommand ??= new DelegateCommand(NavigateToSettings);

        private void NavigateToSettings()
        {
            var regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            var queryString = $"?toyId={Id}";

            var view = ToyType switch
            {
                ToyTypeEnum.Unknown => "",
                ToyTypeEnum.LovenseAmbi => nameof(AmbiSettingsView),
                ToyTypeEnum.LovenseCalor => nameof(CalorSettingsView),
                ToyTypeEnum.LovenseDiamo => nameof(DiamoSettingsView),
                ToyTypeEnum.LovenseDolce => nameof(DolceSettingsView),
                ToyTypeEnum.LovenseDomi => nameof(DomiSettingsView),
                ToyTypeEnum.LovenseEdge => nameof(EdgeSettingsView),
                ToyTypeEnum.LovenseExomoon => nameof(ExomoonSettingsView),
                ToyTypeEnum.LovenseFerri => nameof(FerriSettingsView),
                ToyTypeEnum.LovenseGush => nameof(GushSettingsView),
                ToyTypeEnum.LovenseHush => nameof(HushSettingsView),
                ToyTypeEnum.LovenseHyphy => nameof(HyphySettingsView),
                ToyTypeEnum.LovenseLush => nameof(LushSettingsView),
                ToyTypeEnum.LovenseMax => nameof(MaxSettingsView),
                ToyTypeEnum.LovenseNora => nameof(NoraSettingsView),
                ToyTypeEnum.LovenseOsci => nameof(OsciSettingsView),
                ToyTypeEnum.LovenseSexMachine => nameof(SexMachineSettingsView),
                _ => ""
            };

            regionManager.RequestNavigate(RegionNames.ContentRegion, view + queryString);
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

        private bool? _status;

        public bool? Status
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

        private ToyTypeEnum _toyType;
        public ToyTypeEnum ToyType
        {
            get => _toyType;
            set => SetProperty(ref _toyType, value);
        }
    }
}
