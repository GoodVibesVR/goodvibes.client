using GoodVibes.Client.Lovense.EventHandler;
using Prism.Mvvm;

namespace GoodVibes.Client.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly LovenseEventHandler _lovenseEventHandler;

        private string _title = "GoodVibes V0.1";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel(LovenseEventHandler lovenseEventHandler)
        {
            _lovenseEventHandler = lovenseEventHandler;
            _lovenseEventHandler.Subscribe();
        }
    }
}
