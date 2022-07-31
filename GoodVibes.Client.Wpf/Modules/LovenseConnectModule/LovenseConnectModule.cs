using GoodVibes.Client.Wpf.Modules.LovenseConnectModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseConnectModule
{
    internal class LovenseConnectModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public LovenseConnectModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LovenseConnectView>();
        }
    }
}
