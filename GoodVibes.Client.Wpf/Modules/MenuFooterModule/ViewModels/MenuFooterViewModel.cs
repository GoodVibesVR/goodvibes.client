using GoodVibes.Client.Core.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuFooterModule.ViewModels
{
    public class MenuFooterViewModel : RegionViewModelBase
    {
        public MenuFooterViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
