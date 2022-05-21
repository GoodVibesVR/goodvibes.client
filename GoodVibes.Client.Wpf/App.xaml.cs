using GoodVibes.Client.Wpf.Views;
using Prism.Ioc;
using System.Windows;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Osc;
using GoodVibes.Client.Settings;
using GoodVibes.Client.Settings.Enums;
using GoodVibes.Client.Settings.Models;
using GoodVibes.Client.Wpf.Modules.ContentHeaderModule;
using GoodVibes.Client.Wpf.Modules.ContentModule;
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
            containerRegistry.RegisterSingleton<ApplicationSettings>(_ => applicationSettingsManager.LoadSettings());

            // TODO: Add AvatarSettings, ToySettings, MappingProfile etc

            // Services
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<LovenseClient>();
            containerRegistry.RegisterSingleton<OscServer>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ContentHeaderModule>();
            moduleCatalog.AddModule<MenuHeaderModule>();
            moduleCatalog.AddModule<MenuFooterModule>();

            // Main modules need to be declared last
            moduleCatalog.AddModule<ContentModule>();
            moduleCatalog.AddModule<MenuModule>();
        }
    }
}
