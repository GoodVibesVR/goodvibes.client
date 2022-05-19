using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.Top.ViewModels
{
    public class TopViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public TopViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
