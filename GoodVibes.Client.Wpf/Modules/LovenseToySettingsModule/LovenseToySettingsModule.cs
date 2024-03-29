﻿using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.ViewModels;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule
{
    public class LovenseToySettingsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public LovenseToySettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Single function views
            ViewModelLocationProvider.Register<LushSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<AmbiSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<CalorSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<DiamoSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<DomiSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<FerriSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<GushSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<HushSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<HyphySettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<OsciSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<SexMachineSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<ExomoonSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<TeneraSettingsView, LovenseSingleFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<VulseSettingsView, LovenseSingleFunctionToySettingsViewModel>();

            containerRegistry.RegisterForNavigation<LushSettingsView>();
            containerRegistry.RegisterForNavigation<AmbiSettingsView>();
            containerRegistry.RegisterForNavigation<CalorSettingsView>();
            containerRegistry.RegisterForNavigation<DiamoSettingsView>();
            containerRegistry.RegisterForNavigation<DomiSettingsView>();
            containerRegistry.RegisterForNavigation<FerriSettingsView>();
            containerRegistry.RegisterForNavigation<GushSettingsView>();
            containerRegistry.RegisterForNavigation<HushSettingsView>();
            containerRegistry.RegisterForNavigation<HyphySettingsView>();
            containerRegistry.RegisterForNavigation<OsciSettingsView>();
            containerRegistry.RegisterForNavigation<SexMachineSettingsView>();
            containerRegistry.RegisterForNavigation<ExomoonSettingsView>();
            containerRegistry.RegisterForNavigation<TeneraSettingsView>();
            containerRegistry.RegisterForNavigation<VulseSettingsView>();

            // Multifunction views
            ViewModelLocationProvider.Register<NoraSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<MaxSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<FlexerSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<GravitySettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<RidgeSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<SolaceSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<GeminiSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<DolceSettingsView, LovenseMultiFunctionToySettingsViewModel>();
            ViewModelLocationProvider.Register<EdgeSettingsView, LovenseMultiFunctionToySettingsViewModel>();

            containerRegistry.RegisterForNavigation<NoraSettingsView>();
            containerRegistry.RegisterForNavigation<MaxSettingsView>();
            containerRegistry.RegisterForNavigation<FlexerSettingsView>();
            containerRegistry.RegisterForNavigation<GravitySettingsView>();
            containerRegistry.RegisterForNavigation<RidgeSettingsView>();
            containerRegistry.RegisterForNavigation<SolaceSettingsView>();
            containerRegistry.RegisterForNavigation<GeminiSettingsView>();
            containerRegistry.RegisterForNavigation<DolceSettingsView>();
            containerRegistry.RegisterForNavigation<EdgeSettingsView>();

            // Thricefunction views
            ViewModelLocationProvider.Register<LapisSettingsView, LovenseThriceFunctionToySettingsViewModel>();
            
            containerRegistry.RegisterForNavigation<LapisSettingsView>();
        }
    }
}
