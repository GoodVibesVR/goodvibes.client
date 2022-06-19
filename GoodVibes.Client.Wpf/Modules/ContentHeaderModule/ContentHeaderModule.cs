using GoodVibes.Client.Core;
using GoodVibes.Client.Wpf.Modules.ContentHeaderModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.ContentHeaderModule
{
    internal class ContentHeaderModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ContentHeaderModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentHeaderRegion, "ContentHeaderView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ContentHeaderView>();
        }
    }
}
