using System.Windows.Media;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.Views;
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

            var view = ToyType switch
            {
                LovenseToyEnum.Unknown => "",
                LovenseToyEnum.Ambi => nameof(AmbiSettingsView),
                LovenseToyEnum.Calor => nameof(CalorSettingsView),
                LovenseToyEnum.Diamo => nameof(DiamoSettingsView),
                LovenseToyEnum.Dolce => nameof(DolceSettingsView),
                LovenseToyEnum.Domi => nameof(DomiSettingsView),
                LovenseToyEnum.Edge => nameof(EdgeSettingsView),
                LovenseToyEnum.Exomoon => nameof(ExomoonSettingsView),
                LovenseToyEnum.Ferri => nameof(FerriSettingsView),
                LovenseToyEnum.Gush => nameof(GushSettingsView),
                LovenseToyEnum.Hush => nameof(HushSettingsView),
                LovenseToyEnum.Hyphy => nameof(HyphySettingsView),
                LovenseToyEnum.Lush => nameof(LushSettingsView),
                LovenseToyEnum.Max => nameof(MaxSettingsView),
                LovenseToyEnum.Nora => nameof(NoraSettingsView),
                LovenseToyEnum.Osci => nameof(OsciSettingsView),
                LovenseToyEnum.SexMachine => nameof(SexMachineSettingsView),
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

    private LovenseToyEnum _toyType;
    public LovenseToyEnum ToyType
    {
        get => _toyType;
        set => SetProperty(ref _toyType, value);
    }
}
}
