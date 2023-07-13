using GoodVibes.Client.Lovense.EventHandler;
using GoodVibes.Client.PiShock.EventHandlers;
using GoodVibes.Client.Settings.Models;
using Prism.Mvvm;

namespace GoodVibes.Client.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly LovenseEventHandler _lovenseEventHandler;
        private readonly PiShockEventHandler _piShockEventHandler;

        private string _title = "GoodVibes 1.0-alpha1.5";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel(ApplicationSettings appSettings, LovenseEventHandler lovenseEventHandler, 
            PiShockEventHandler piShockEventHandler)
        {

            _lovenseEventHandler = lovenseEventHandler;
            _lovenseEventHandler.Subscribe();

            _piShockEventHandler = piShockEventHandler;
            _piShockEventHandler.Subscribe();
        }
    }
}
