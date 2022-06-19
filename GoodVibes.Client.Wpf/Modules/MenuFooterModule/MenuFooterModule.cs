using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuFooterModule
{
    public class MenuFooterModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MenuFooterModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.MenuFooterView>();
        }
    }
}
