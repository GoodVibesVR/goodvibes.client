using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using Prism.Commands;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels;

public class MenuViewModel : RegionViewModelBase
{
    private readonly IRegionManager _regionManager;

    private DelegateCommand _navigateToHomeCommand;
    public DelegateCommand NavigateToHomeCommand =>
        _navigateToHomeCommand ??= new DelegateCommand(NavigateToHome);

    private void NavigateToHome()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "DashboardView");
    }

    private DelegateCommand _navigateToSessionsCommand;
    public DelegateCommand NavigateToSessionsCommand =>
        _navigateToSessionsCommand ??= new DelegateCommand(NavigateToSessions);

    private void NavigateToSessions()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "SessionsView");
    }

    private DelegateCommand _navigateToAvatarsCommand;
    public DelegateCommand NavigateToAvatarsCommand =>
        _navigateToAvatarsCommand ??= new DelegateCommand(NavigateToAvatars);

    private void NavigateToAvatars()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "AvatarsView");
    }

    public MenuViewModel(IRegionManager regionManager) :
          base(regionManager)
    {
        _regionManager = regionManager;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        //do something
    }
}