using Prism.Mvvm;

namespace GoodVibes.Client.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "GoodVibes V0.1";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
