using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.ViewModels;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule
{
    public class LovenseToySettingsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public LovenseToySettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RequestNavigate(RegionNames.ContentRegion, "LushSettingsView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<LushSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<NoraSettingsView, LovenseMultiFunctionToySettingsViewModel>();

            containerRegistry.RegisterForNavigation<LushSettingsView>();
            containerRegistry.RegisterForNavigation<NoraSettingsView>();
        }
    }
}
