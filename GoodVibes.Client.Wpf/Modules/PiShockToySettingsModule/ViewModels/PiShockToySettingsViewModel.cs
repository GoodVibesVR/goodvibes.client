using System;
using System.Windows;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.Enums;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.ViewModels
{
    public class PiShockToySettingsViewModel : RegionViewModelBase
    {
        private readonly PiShockClient _piShockClient;
        private readonly IEventAggregator _eventAggregator;

        private bool _eventLocked;

        private string _toyId;
        public string ToyId
        {
            get => _toyId;
            set => SetProperty(ref _toyId, value);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                SetDuration(value);
                SetProperty(ref _duration, value);
            }
        }

        private int _intensity;
        public int Intensity
        {
            get => _intensity;
            set
            {
                SetIntensity(value);
                SetProperty(ref _intensity, value);
            }
        }

        private int _intensityGauge;

        public int IntensityGauge
        {
            get => _intensityGauge;
            set => SetProperty(ref _intensityGauge, value);
        }

        private DelegateCommand _shockCommand;
        public DelegateCommand ShockCommand =>
            _shockCommand ??= new DelegateCommand(Shock);

        private DelegateCommand _vibrateCommand;
        public DelegateCommand VibrateCommand =>
            _vibrateCommand ??= new DelegateCommand(Vibrate);

        private DelegateCommand _beepCommand;
        public DelegateCommand BeepCommand =>
            _beepCommand ??= new DelegateCommand(Beep);

        public PiShockToySettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, PiShockClient piShockClient) : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _piShockClient = piShockClient;

            _eventAggregator.GetEvent<PiShockDurationChangedEventCarrier>().Subscribe(DurationChangedEventHandler);
            _eventAggregator.GetEvent<PiShockIntensityChangedEventCarrier>().Subscribe(IntensityChangedEventHandler);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var toyId = (string)navigationContext.Parameters["toyId"];
            var toy = _piShockClient.Toys?[toyId];

            if (toy == null)
            {
                // TODO: Do something
            }

            if (toy is not PiShock.Models.PiShock shocker) return;

            _eventLocked = true;

            ToyId = shocker.ShareCode;
            DisplayName = shocker.FriendlyName;
            Duration = shocker.Duration;
            Intensity = shocker.Intensity;
            
            _eventLocked = false;
        }

        private void Shock()
        {
            _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
            {
                Command = PiShockCommandEnum.Shock,
                ShareCode = ToyId
            });
        }

        private void Vibrate()
        {
            _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
            {
                Command = PiShockCommandEnum.Vibrate,
                ShareCode = ToyId
            });
        }

        private void Beep()
        {
            _eventAggregator.GetEvent<PiShockCommandEventCarrier>().Publish(new PiShockCommandEvent()
            {
                Command = PiShockCommandEnum.Beep,
                ShareCode = ToyId
            });
        }

        private void SetDuration(int value)
        {
            if (_eventLocked) return;
            _eventAggregator.GetEvent<PiShockSettingsDurationChangedEventCarrier>().Publish(new PiShockDurationChangedEvent()
            {
                ToyId = ToyId,
                Duration = (float)Duration / 10
            });
        }

        private void SetIntensity(int value)
        {
            IntensityGauge = (int)Math.Round(value * 1.8);

            if (_eventLocked) return;
            _eventAggregator.GetEvent<PiShockSettingsIntensityChangedEventCarrier>().Publish(new PiShockIntensityChangedEvent()
            {
                ToyId = ToyId,
                Intensity = (float)Intensity / 100
            });
        }

        private void DurationChangedEventHandler(PiShockDurationChangedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                _eventLocked = true;
                Duration = (int)Math.Round(obj.Duration * 10);
                _eventLocked = false;
            });
        }

        private void IntensityChangedEventHandler(PiShockIntensityChangedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                _eventLocked = true;
                Intensity = (int)Math.Round(obj.Intensity * 100);
                _eventLocked = false;
            });
        }
    }
}
