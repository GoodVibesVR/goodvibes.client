using GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels;
using GoodVibes.Client.Wpf.Modules.AddToyModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule
{
    internal class AddToyModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AddToyModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<AddPiShockView, AddPiShockViewModel>();
            ViewModelLocationProvider.Register<AddLovenseView, AddLovenseViewModel>();
            ViewModelLocationProvider.Register<SelectTypeView, SelectToyTypeViewModel>();
            ViewModelLocationProvider.Register<AddToyView, AddToyViewModel>();

            containerRegistry.RegisterForNavigation<AddPiShockView>();
            containerRegistry.RegisterForNavigation<AddLovenseView>();
            containerRegistry.RegisterForNavigation<SelectTypeView>();
            containerRegistry.RegisterForNavigation<AddToyView>();
        }
    }
}
