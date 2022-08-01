using GoodVibes.Client.Wpf.Views;
using Prism.Ioc;
using System.Windows;
using GoodVibes.Client.ApiCaller;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.EventDispatchers;
using GoodVibes.Client.Lovense.EventHandler;
using GoodVibes.Client.Mapper;
using GoodVibes.Client.Osc;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.EventDispatchers;
using GoodVibes.Client.PiShock.EventHandlers;
using GoodVibes.Client.Settings;
using GoodVibes.Client.Settings.Enums;
using GoodVibes.Client.Settings.Models;
using GoodVibes.Client.Wpf.Modules.AddToyModule;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule;
using GoodVibes.Client.Wpf.Modules.ContentHeaderModule;
using GoodVibes.Client.Wpf.Modules.DashboardModule;
using GoodVibes.Client.Wpf.Modules.LovenseConnectModule;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule;
using GoodVibes.Client.Wpf.Modules.MenuFooterModule;
using GoodVibes.Client.Wpf.Modules.MenuHeaderModule;
using GoodVibes.Client.Wpf.Modules.MenuModule;
using GoodVibes.Client.Wpf.Modules.PiShockConnectModule;
using GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule;
using GoodVibes.Client.Wpf.Services;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Modularity;

namespace GoodVibes.Client.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
            MainWindow = MainWindow;
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Settings
            var applicationSettingsManager = new SettingsManager<ApplicationSettings>("applicationSettings.json",
                SettingsLocationEnum.ApplicationDirectory);
            var applicationSettings = applicationSettingsManager.LoadSettings();
            containerRegistry.RegisterSingleton<ApplicationSettings>(_ => applicationSettings);

            // TODO: Add AvatarSettings, ToySettings, MappingProfile etc

            // Services
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IImageService, ImageService>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<ILovenseService, LovenseService>();
            containerRegistry.RegisterSingleton<IAvatarMapperService, AvatarMapperService>();
            containerRegistry.RegisterSingleton<IOscProfileConverterService, OscProfileConverterService>();

            // Clients
            containerRegistry.RegisterSingleton<ILovenseApiClient>(() => new ApiClient(string.Empty));
            containerRegistry.RegisterSingleton<IApiClient>(() => new ApiClient(applicationSettings!.GoodVibesRoot!));

            // Lovense
            containerRegistry.RegisterSingleton<LovenseClient>();
            containerRegistry.RegisterSingleton<LovenseEventHandler>();
            containerRegistry.RegisterSingleton<LovenseEventDispatcher>();

            // PiShock
            containerRegistry.RegisterSingleton<PiShockClient>();
            containerRegistry.RegisterSingleton<PiShockEventHandler>();
            containerRegistry.RegisterSingleton<PiShockEventDispatcher>();

            // General GoodVibes
            containerRegistry.RegisterSingleton<OscServer>();
            containerRegistry.RegisterSingleton<AvatarMapperClient>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ContentHeaderModule>();
            moduleCatalog.AddModule<MenuHeaderModule>();
            moduleCatalog.AddModule<MenuFooterModule>();
            moduleCatalog.AddModule<LovenseToySettingsModule>();
            moduleCatalog.AddModule<LovenseConnectModule>();
            moduleCatalog.AddModule<AvatarMapperModule>();
            moduleCatalog.AddModule<PiShockConnectModule>();
            moduleCatalog.AddModule<PiShockToySettingsModule>();
            moduleCatalog.AddModule<AddToyModule>();

            // Main modules need to be declared last
            moduleCatalog.AddModule<DashboardModule>();
            moduleCatalog.AddModule<MenuModule>();
        }
    }
}
