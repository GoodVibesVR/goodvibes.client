using System;
using System.Threading.Tasks;
using System.Windows;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockConnectModule.ViewModels
{
    public class PiShockConnectViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly PiShockClient _piShockClient;

        public PiShockConnectViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, PiShockClient piShockClient) : base(regionManager)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _piShockClient = piShockClient;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<PiShockConnectionAckEventCarrier>().Subscribe(PiShockConnectionAckEventHandler);
            Task.Run(() => _piShockClient.ConnectAsync());
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<PiShockConnectionAckEventCarrier>().Unsubscribe(PiShockConnectionAckEventHandler);
        }

        private void PiShockConnectionAckEventHandler(PiShockConnectionAckEvent obj)
        {
            // TODO: Do we need to send something back to the Dashboard here or are we assuming connected?

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var contentRegion = _regionManager.Regions[RegionNames.ContentRegion];
                contentRegion.NavigationService.Journal.GoBack();
            });
        }
    }
}
