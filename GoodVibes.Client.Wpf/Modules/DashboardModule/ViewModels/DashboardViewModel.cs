using System;
using System.Windows;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Osc;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using GoodVibes.Client.Serial;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.DashboardModule.ViewModels
{
    public class DashboardViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private readonly LovenseClient _lovenseClient;
        private readonly SerialClient _serialClient;
        private readonly OscServer _oscServer;

        private bool _lovenseConnected;
        public bool LovenseConnected
        {
            get => _lovenseConnected;
            set => SetProperty(ref _lovenseConnected, value);
        }

        private bool _piShockConnected;
        public bool PiShockConnected
        {
            get => _piShockConnected;
            set => SetProperty(ref _piShockConnected, value);
        }

        private DelegateCommand _connectToLovenseCommandHubCommand;
        public DelegateCommand ConnectToLovenseCommandHubCommand =>
            _connectToLovenseCommandHubCommand ??= new DelegateCommand(ConnectToLovense);

        private DelegateCommand _connectToPiShockCommandHubCommand;
        public DelegateCommand ConnectToPiShockCommandHubCommand =>
            _connectToPiShockCommandHubCommand ??= new DelegateCommand(ConnectToPiShock);

        private DelegateCommand _connectToOscCommand;
        public DelegateCommand ConnectToOscCommand =>
            _connectToOscCommand ??= new DelegateCommand(ConnectToOsc);

        public DashboardViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, 
            LovenseClient lovenseClient, SerialClient serialClient, OscServer oscServer) :
            base(regionManager)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _lovenseClient = lovenseClient;
            _serialClient = serialClient;
            _oscServer = oscServer;
            _oscServer.ConnectAsync();

            _eventAggregator.GetEvent<LovenseDeviceAccessibilityEventCarrier>().Subscribe(LovenseDeviceAccessibilityEventReceived);
            _eventAggregator.GetEvent<LovenseDisconnectedEventCarrier>().Subscribe(LovenseDisconnectedEventReceived);

            _eventAggregator.GetEvent<PiShockConnectionAckEventCarrier>().Subscribe(PiShockConnectionAckEventReceived);
            _eventAggregator.GetEvent<PiShockDisconnectedEventCarrier>().Subscribe(PiShockDisconnectedEventReceived);
        }

        private void LovenseDeviceAccessibilityEventReceived(LovenseDeviceAccessibilityEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                LovenseConnected = true;
            });
        }

        private void LovenseDisconnectedEventReceived(LovenseDisconnectedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                LovenseConnected = false;
            });
        }

        private void PiShockConnectionAckEventReceived(PiShockConnectionAckEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                PiShockConnected = true;
            });
        }

        private void PiShockDisconnectedEventReceived(PiShockDisconnectedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                PiShockConnected = false;
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        private void ConnectToLovense()
        {
            if (LovenseConnected)
            {
                _eventAggregator.GetEvent<DisconnectLovenseCommandEventCarrier>().Publish(new DisconnectLovenseCommandEvent());
                return;
            }

            _regionManager.RequestNavigate(RegionNames.ContentRegion, "LovenseConnectView");
        }

        private void ConnectToPiShock()
        {
            if (PiShockConnected)
            {
                _eventAggregator.GetEvent<DisconnectPiShockCommandEventCarrier>().Publish(new DisconnectPiShockCommandEvent());
                return;
            }

            _regionManager.RequestNavigate(RegionNames.ContentRegion, "PiShockConnectView");
        }

        private void ConnectToOsc()
        {
            _oscServer.ConnectAsync();
        }
    }
}
