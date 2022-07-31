using GoodVibes.Client.Wpf.Modules.PiShockConnectModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockConnectModule
{
    public class PiShockConnectModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public PiShockConnectModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PiShockConnectView>();
        }
    }
}
