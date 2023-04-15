using GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule
{
    public class PiShockToySettingsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public PiShockToySettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PiShockToySettingsView>();
            containerRegistry.RegisterForNavigation<PiVaultToySettingsView>();
        }
    }
}
