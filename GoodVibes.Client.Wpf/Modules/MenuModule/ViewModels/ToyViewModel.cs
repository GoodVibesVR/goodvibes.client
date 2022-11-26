using System.Windows.Media;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Wpf.EventCarriers;
using GoodVibes.Client.Wpf.Events;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.Views;
using GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels
{
    public class ToyViewModel : ViewModelBase
    {
        private DelegateCommand _navigateToSettingsCommand;
        public DelegateCommand NavigateToSettingsCommand =>
            _navigateToSettingsCommand ??= new DelegateCommand(NavigateToSettings);

        private DelegateCommand _removeToyCommand;

        public DelegateCommand RemoveToyCommand =>
            _removeToyCommand ??= new DelegateCommand(RemoveToy);

        private void RemoveToy()
        {
            var eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<RemoveToyEventCarrier>().Publish(new RemoveToyEvent()
            {
                ToyId = Id
            });
        }

        private void NavigateToSettings()
        {
            var regionManager = ContainerLocator.Container.Resolve<IRegionManager>();
            var queryString = $"?toyId={Id}";

            var view = ToyType switch
            {
                ToyTypeEnum.Unknown => "",
                ToyTypeEnum.Ambi => nameof(AmbiSettingsView),
                ToyTypeEnum.Calor => nameof(CalorSettingsView),
                ToyTypeEnum.Diamo => nameof(DiamoSettingsView),
                ToyTypeEnum.Dolce => nameof(DolceSettingsView),
                ToyTypeEnum.Domi => nameof(DomiSettingsView),
                ToyTypeEnum.Edge => nameof(EdgeSettingsView),
                ToyTypeEnum.Exomoon => nameof(ExomoonSettingsView),
                ToyTypeEnum.Ferri => nameof(FerriSettingsView),
                ToyTypeEnum.Gush => nameof(GushSettingsView),
                ToyTypeEnum.Hush => nameof(HushSettingsView),
                ToyTypeEnum.Hyphy => nameof(HyphySettingsView),
                ToyTypeEnum.Lush => nameof(LushSettingsView),
                ToyTypeEnum.Max => nameof(MaxSettingsView),
                ToyTypeEnum.Nora => nameof(NoraSettingsView),
                ToyTypeEnum.Osci => nameof(OsciSettingsView),
                ToyTypeEnum.SexMachine => nameof(SexMachineSettingsView),
                ToyTypeEnum.Tenera => nameof(TeneraSettingsView),
                ToyTypeEnum.Flexer => nameof(FlexerSettingsView),
                ToyTypeEnum.Gravity => nameof(GravitySettingsView),
                ToyTypeEnum.Gemini => nameof(GeminiSettingsView),
                ToyTypeEnum.PiShock => nameof(PiShockToySettingsView),
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
