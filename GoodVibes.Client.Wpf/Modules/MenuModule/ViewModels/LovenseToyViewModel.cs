using System;
using System.Windows.Media;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
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
                LovenseToyEnum.Ambi => "",
                LovenseToyEnum.Calor => "",
                LovenseToyEnum.Diamo => "",
                LovenseToyEnum.Dolce => "",
                LovenseToyEnum.Domi => "",
                LovenseToyEnum.Edge => "",
                LovenseToyEnum.Exomoon => "",
                LovenseToyEnum.Ferri => "",
                LovenseToyEnum.Gush => "",
                LovenseToyEnum.Hush => "",
                LovenseToyEnum.Hyphy => "",
                LovenseToyEnum.Lush => "LushSettingsView",
                LovenseToyEnum.Max => "",
                LovenseToyEnum.Nora => "NoraSettingsView",
                LovenseToyEnum.Osci => "",
                LovenseToyEnum.SexMachine => "",
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

    private LovenseToyEnum _toyType;
    public LovenseToyEnum ToyType
    {
        get => _toyType;
        set => SetProperty(ref _toyType, value);
    }
}
}
