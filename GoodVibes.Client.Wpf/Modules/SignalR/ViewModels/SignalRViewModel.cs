﻿using System;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Osc;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.SignalR.ViewModels
{
    public class SignalRViewModel : RegionViewModelBase
    {
        private readonly LovenseClient _lovenseClient;
        private readonly OscServer _oscServer;

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private DelegateCommand _connectToCommandHubCommand;
        public DelegateCommand ConnectToCommandHubCommand =>
            _connectToCommandHubCommand ??= new DelegateCommand(ConnectToLovense);

        private DelegateCommand _connectToOscCommand;
        public DelegateCommand ConnectToOscCommand =>
            _connectToOscCommand ??= new DelegateCommand(ConnectToOsc);

        public SignalRViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, LovenseClient lovenseClient, OscServer oscServer) :
            base(regionManager)
        {
            _lovenseClient = lovenseClient;
            _oscServer = oscServer;
            
            eventAggregator.GetEvent<LovenseCallbackReceivedEventCarrier>().Subscribe(LovenseCallbackReceived);

            // TODO: ObservableCollection for toys
            //this.AllMedicines = new ObservableCollection<Medicine>();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        private void ConnectToLovense()
        {
            _lovenseClient.ConnectAsync();
        }

        private void ConnectToOsc()
        {
            _oscServer.ConnectAsync();
        }

        private void LovenseCallbackReceived(LovenseCallbackReceivedEvent callback)
        {
            // TODO: Update toys list
            var callbackJson = JsonConvert.SerializeObject(callback);
            Console.WriteLine($"SignalRViewModel: {callbackJson}");
            Message = callbackJson;
        }
    }
}
