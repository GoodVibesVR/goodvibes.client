﻿using GoodVibes.Client.Core;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule.Views;
using GoodVibes.Client.Wpf.Modules.DashboardModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.DashboardModule
{
    internal class DashboardModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public DashboardModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(AvatarMapperView)); // TODO: This is a hack
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(DashboardView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //ViewModelLocationProvider.Register<ContentView, ContentViewModel>();
            //ViewModelLocationProvider.Register<LovenseConnectView, LovenseConnectViewModel>();

            containerRegistry.RegisterForNavigation<DashboardView>();
            //containerRegistry.RegisterForNavigation<ContentModule.Views.LovenseConnectView>();
        }
    }
}
