using GoodVibes.Client.Lovense.EventHandler;
using GoodVibes.Client.Settings.Models;
using Prism.Mvvm;

namespace GoodVibes.Client.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly LovenseEventHandler _lovenseEventHandler;

        private string _title = "GoodVibes 1.0-alpha1";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel(ApplicationSettings appSettings, LovenseEventHandler lovenseEventHandler)
        {
            _lovenseEventHandler = lovenseEventHandler;
            _lovenseEventHandler.Subscribe();
        }
    }
}
