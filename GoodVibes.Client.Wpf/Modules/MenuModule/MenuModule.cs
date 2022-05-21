using GoodVibes.Client.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule
{
    internal class MenuModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MenuModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.MenuRegion, "MenuView");

            // Needs to be at the bottom
            _regionManager.RequestNavigate(RegionNames.MenuHeaderRegion, "MenuHeaderView");
            _regionManager.RequestNavigate(RegionNames.MenuFooterRegion, "MenuFooterView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.MenuView>();
        }
    }
}
