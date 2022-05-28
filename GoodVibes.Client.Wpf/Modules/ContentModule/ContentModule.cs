using GoodVibes.Client.Core;
using GoodVibes.Client.Wpf.Modules.ContentModule.ViewModels;
using GoodVibes.Client.Wpf.Modules.ContentModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.ContentModule
{
    public class ContentModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ContentModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "ContentView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<ContentView, ContentViewModel>();
            ViewModelLocationProvider.Register<LovenseConnectView, LovenseConnectViewModel>();

            containerRegistry.RegisterForNavigation<Views.ContentView>();
            containerRegistry.RegisterForNavigation<Views.LovenseConnectView>();
        }
    }
}
