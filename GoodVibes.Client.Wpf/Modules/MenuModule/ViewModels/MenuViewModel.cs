using GoodVibes.Client.Core.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels;

public class MenuViewModel : RegionViewModelBase
{
    public MenuViewModel(IRegionManager regionManager) :
          base(regionManager)
    {
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        //do something
    }
}