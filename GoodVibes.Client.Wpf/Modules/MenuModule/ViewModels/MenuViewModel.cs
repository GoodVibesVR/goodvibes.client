using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Wpf.Modules.AddToyModule.Views;
using GoodVibes.Client.Wpf.Modules.AvatarMapperModule.Views;
using GoodVibes.Client.Wpf.Modules.DashboardModule.Views;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels;

public class MenuViewModel : RegionViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly ILovenseService _lovenseService;

    private ObservableCollection<ToyViewModel> _toys;
    public ObservableCollection<ToyViewModel> Toys
    {
        get => _toys;
        set => SetProperty(ref _toys, value);
    }

    private DelegateCommand _navigateToDashboardCommand;
    public DelegateCommand NavigateToDashboardCommand =>
        _navigateToDashboardCommand ??= new DelegateCommand(NavigateToDashboard);

    private void NavigateToDashboard()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(DashboardView));
    }

    private DelegateCommand _navigateToGroupsCommand;
    public DelegateCommand NavigateToGroupsCommand =>
        _navigateToGroupsCommand ??= new DelegateCommand(NavigateToGroups);

    private void NavigateToGroups()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "GroupsView");
    }

    private DelegateCommand _navigateToAvatarMappingsCommand;
    public DelegateCommand NavigateToAvatarMappingsCommand =>
        _navigateToAvatarMappingsCommand ??= new DelegateCommand(NavigateToAvatarMappings);

    private void NavigateToAvatarMappings()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(AvatarMapperView));
    }

    private DelegateCommand _navigateToWorldMappingsCommand;
    public DelegateCommand NavigateToWorldMappingsCommand =>
        _navigateToWorldMappingsCommand ??= new DelegateCommand(NavigateToWorldMappings);

    private void NavigateToWorldMappings()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "WorldMapperView");
    }

    private DelegateCommand _navigateToAddToyCommand;
    public DelegateCommand NavigateToAddToyCommand =>
        _navigateToAddToyCommand ??= new DelegateCommand(NavigateToAddToy);

    private void NavigateToAddToy()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(AddToyView));
    }

    public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ILovenseService lovenseService) :
          base(regionManager)
    {
        _regionManager = regionManager;
        _lovenseService = lovenseService;

        Toys = new ObservableCollection<ToyViewModel>();
        eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
    }

    private void LovenseToyListUpdated(LovenseToyListUpdatedEvent obj)
    {
        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            Console.WriteLine($"ToyList update in menu received");
            var tempList = Toys;
            foreach (var lovenseToy in obj.ToyList!)
            {
                var toy = tempList.FirstOrDefault(t => t.Id == lovenseToy.Id);
                if (toy == null)
                {
                    Toys.Add(new ToyViewModel()
                    {
                        Id = lovenseToy.Id,
                        Battery = lovenseToy.Battery,
                        DisplayName = lovenseToy.DisplayName,
                        ToyIcon = _lovenseService.GetToyIcon(lovenseToy),
                        ToyType = lovenseToy.ToyType,
                        Status = lovenseToy.Status
                    });

                    continue;
                }

                toy.Battery = lovenseToy.Battery;
                toy.DisplayName = lovenseToy.DisplayName;
                toy.Status = lovenseToy.Status;
                
            }

            // TODO: Fix sorting
            var test = from t in tempList
                       orderby t.Status == null, t.Status.ToString() descending
                       select t;
            Toys = new ObservableCollection<ToyViewModel>(test);
        });
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        //do something
    }
}