using GoodVibes.Client.Core.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.ContentHeaderModule.ViewModels
{
    public class ContentHeaderViewModel : RegionViewModelBase
    {
        public ContentHeaderViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
