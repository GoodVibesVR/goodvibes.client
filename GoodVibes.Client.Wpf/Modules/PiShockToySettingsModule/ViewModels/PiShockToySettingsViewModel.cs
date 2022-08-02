using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using Prism.Commands;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.ViewModels
{
    public class PiShockToySettingsViewModel : RegionViewModelBase
    {
        private readonly PiShockClient _piShockClient;

        private int _duration;

        public int Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private int _intensity;

        public int Intensity
        {
            get => _intensity;
            set => SetProperty(ref _intensity, value);
        }

        private DelegateCommand _shockCommand;
        public DelegateCommand ShockCommand =>
            _shockCommand ??= new DelegateCommand(Shock);

        private DelegateCommand _vibrateCommand;
        public DelegateCommand VibrateCommand =>
            _vibrateCommand ??= new DelegateCommand(Vibrate);

        private DelegateCommand _beepCommand;
        public DelegateCommand beepCommand =>
            _beepCommand ??= new DelegateCommand(Beep);

        public PiShockToySettingsViewModel(IRegionManager regionManager, PiShockClient piShockClient) : base(regionManager)
        {
            _piShockClient = piShockClient;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var toyId = (string)navigationContext.Parameters["toyId"];
            var shocker = _piShockClient.Toys?[toyId];
            if (shocker == null)
            {
                // TODO: Do something
            }

            // TODO: ....
        }

        private void Shock()
        {

        }

        private void Vibrate()
        {

        }

        private void Beep()
        {

        }
    }
}
