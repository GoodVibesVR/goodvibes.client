using GoodVibes.Client.Wpf.Views;
using Prism.Ioc;
using System.Windows;
using GoodVibes.Client.ApiCaller;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.EventHandler;
using GoodVibes.Client.Mapper;
using GoodVibes.Client.Osc;
using GoodVibes.Client.Settings;
using GoodVibes.Client.Settings.Enums;
using GoodVibes.Client.Settings.Models;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule;
using GoodVibes.Client.Wpf.Modules.ContentHeaderModule;
using GoodVibes.Client.Wpf.Modules.DashboardModule;
using GoodVibes.Client.Wpf.Modules.LovenseConnectModule;
using GoodVibes.Client.Wpf.Modules.LovenseToySettingsModule;
using GoodVibes.Client.Wpf.Modules.MenuFooterModule;
using GoodVibes.Client.Wpf.Modules.MenuHeaderModule;
using GoodVibes.Client.Wpf.Modules.MenuModule;
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

            // Clients
            containerRegistry.RegisterSingleton<ILovenseApiClient>(() => new ApiClient(string.Empty));
            containerRegistry.RegisterSingleton<IApiClient>(() => new ApiClient(applicationSettings!.GoodVibesRoot!));

            containerRegistry.RegisterSingleton<LovenseClient>();
            containerRegistry.RegisterSingleton<OscServer>();
            containerRegistry.RegisterSingleton<LovenseEventHandler>();
            containerRegistry.RegisterSingleton<AvatarMapper>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ContentHeaderModule>();
            moduleCatalog.AddModule<MenuHeaderModule>();
            moduleCatalog.AddModule<MenuFooterModule>();
            moduleCatalog.AddModule<LovenseToySettingsModule>();
            moduleCatalog.AddModule<LovenseConnectModule>();
            moduleCatalog.AddModule<AvatarMapperModule>();

            // Main modules need to be declared last
            moduleCatalog.AddModule<DashboardModule>();
            moduleCatalog.AddModule<MenuModule>();
        }
    }
}
