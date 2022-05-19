using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.Menu.ViewModels;

public class MenuViewModel : RegionViewModelBase
{
    private string _message;
    public string Message
    {
        get { return _message; }
        set { SetProperty(ref _message, value); }
    }

    public MenuViewModel(IRegionManager regionManager, IMessageService messageService) :
        base(regionManager)
    {
        Message = messageService.GetMessage();
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        //do something
    }
}