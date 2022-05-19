using GoodVibes.Client.Wpf.Views;
using Prism.Ioc;
using System.Windows;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Osc;
using GoodVibes.Client.SignalR;
using GoodVibes.Client.SignalR.Abstractions;
using GoodVibes.Client.Wpf.Modules.Menu;
using GoodVibes.Client.Wpf.Modules.SignalR;
using GoodVibes.Client.Wpf.Modules.Top;
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
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<LovenseClient>();
            containerRegistry.RegisterSingleton<OscServer>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<SignalRModule>();
            moduleCatalog.AddModule<MenuModule>();
            moduleCatalog.AddModule<TopModule>();
        }
    }
}
