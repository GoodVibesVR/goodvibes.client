using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Osc;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.DashboardModule.ViewModels
{
    public class DashboardViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;

        private readonly LovenseClient _lovenseClient;
        private readonly OscServer _oscServer;

        private DelegateCommand _connectToCommandHubCommand;
        public DelegateCommand ConnectToCommandHubCommand =>
            _connectToCommandHubCommand ??= new DelegateCommand(ConnectToLovense);

        private DelegateCommand _connectToOscCommand;
        public DelegateCommand ConnectToOscCommand =>
            _connectToOscCommand ??= new DelegateCommand(ConnectToOsc);

        public DashboardViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, LovenseClient lovenseClient, OscServer oscServer) :
            base(regionManager)
        {
            _regionManager = regionManager;

            _lovenseClient = lovenseClient;
            _oscServer = oscServer;
            _oscServer.ConnectAsync();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        private void ConnectToLovense()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "LovenseConnectView");
        }

        private void ConnectToOsc()
        {
            _oscServer.ConnectAsync();
        }
    }
}
