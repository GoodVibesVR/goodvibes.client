using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels;

public class MenuViewModel : RegionViewModelBase
{
    private readonly IRegionManager _regionManager;
    
    public ObservableCollection<LovenseToyViewModel> Toys { get; set; }

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

    public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) :
          base(regionManager)
    {
        _regionManager = regionManager;
        Toys = new ObservableCollection<LovenseToyViewModel>();

        eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
    }

    private void LovenseToyListUpdated(LovenseToyListUpdatedEvent obj)
    {
        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            var tempList = Toys;

            foreach (var lovenseToy in obj.ToyList!)
            {
                var toy = tempList.FirstOrDefault(t => t.Id == lovenseToy.Id);
                if (toy == null)
                {
                    Toys.Add(new LovenseToyViewModel()
                    {
                        Id = lovenseToy.Id,
                        Battery = lovenseToy.Battery,
                        DisplayName = lovenseToy.DisplayName
                    });
                    continue;
                }

                toy.Battery = lovenseToy.Battery;
                toy.DisplayName = lovenseToy.DisplayName;
                toy.Status = lovenseToy.Status;
            }

            var disconnectedToys = tempList.Where(t => obj.ToyList.All(x => x.Id != t.Id));
            foreach (var disconnectedToy in disconnectedToys)
            {
                disconnectedToy.Status = false;
            }

            Toys = tempList;
        });
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        //do something
    }
}