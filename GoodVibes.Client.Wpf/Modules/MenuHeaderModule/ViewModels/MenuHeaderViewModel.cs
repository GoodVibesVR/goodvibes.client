using GoodVibes.Client.Core.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuHeaderModule.ViewModels
{
    public class MenuHeaderViewModel : RegionViewModelBase
    {
        public MenuHeaderViewModel(IRegionManager regionManager) : 
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
