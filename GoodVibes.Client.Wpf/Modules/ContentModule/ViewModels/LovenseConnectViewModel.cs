using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.ContentModule.ViewModels
{
    public class LovenseConnectViewModel : RegionViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IImageService _imageService;

        private readonly LovenseClient _lovenseClient;

        private string _uniqueCode;
        public string UniqueCode
        {
            get => _uniqueCode;
            set => SetProperty(ref _uniqueCode, value);
        }

        private ImageSource _qrSource;
        public ImageSource QrSource
        {
            get => _qrSource;
            set => SetProperty(ref _qrSource, value);
        }

        private float _qrSourceOpacity;

        public float QrSourceOpacity
        {
            get => _qrSourceOpacity;
            set => SetProperty(ref _qrSourceOpacity, value);
        }

        public LovenseConnectViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
            IImageService imageService, LovenseClient lovenseClient) : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _imageService = imageService;

            _lovenseClient = lovenseClient;
            QrSourceOpacity = 0;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<LovenseQrCodeReceivedEventCarrier>().Subscribe(LovenseQrCodeReceived);
            _eventAggregator.GetEvent<LovenseDeviceAccessibilityEventCarrier>().Subscribe(LovenseDeviceAccessibilityEventReceived);
            Task.Run(() => _lovenseClient.ConnectAsync());
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _eventAggregator.GetEvent<LovenseQrCodeReceivedEventCarrier>().Unsubscribe(LovenseQrCodeReceived);
            _eventAggregator.GetEvent<LovenseDeviceAccessibilityEventCarrier>().Unsubscribe(LovenseDeviceAccessibilityEventReceived);
        }

        private void LovenseDeviceAccessibilityEventReceived(LovenseDeviceAccessibilityEvent obj)
        {
            // TODO: If accessibility is false, we need to instruct the users to connect device to wi-fi.

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var contentRegion = _regionManager.Regions[RegionNames.ContentRegion];
                contentRegion.NavigationService.Journal.GoBack();
            });
        }

        private void LovenseQrCodeReceived(LovenseQrCodeReceivedEvent obj)
        {
            Console.WriteLine($"onReceiveQrCode: {JsonConvert.SerializeObject(obj)}");

            Application.Current.Dispatcher.Invoke(delegate
            {
                var bitmap = Task.Run(() => _imageService.GetQrCodeFromGoodVibesServers(obj.ImageKey)).Result;

                QrSource = bitmap;
                UniqueCode = obj.UniqueCode;
                QrSourceOpacity = 1;
            });
        }
    }
}