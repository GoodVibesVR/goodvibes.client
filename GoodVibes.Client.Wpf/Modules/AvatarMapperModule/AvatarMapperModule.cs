using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule
{
    public class AvatarMapperModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AvatarMapperModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RequestNavigate(RegionNames.ContentRegion, "LushSettingsView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //ViewModelLocationProvider.Register<LushSettingsView, LovenseToySettingsViewModel>();

            containerRegistry.RegisterForNavigation<Views.AvatarMapperView>();
        }
    }
}