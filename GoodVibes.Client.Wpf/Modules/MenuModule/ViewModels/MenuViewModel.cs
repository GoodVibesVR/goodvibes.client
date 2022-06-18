using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Lovense.Models;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuModule.ViewModels;

public class MenuViewModel : RegionViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly ILovenseService _lovenseService;
    
    public ObservableCollection<LovenseToyViewModel> Toys { get; set; }

    private DelegateCommand _navigateToDashboardCommand;
    public DelegateCommand NavigateToDashboardCommand =>
        _navigateToDashboardCommand ??= new DelegateCommand(NavigateToDashboard);

    private void NavigateToDashboard()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "DashboardView");
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
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "AvatarMapperView");
    }

    private DelegateCommand _navigateToWorldMappingsCommand;
    public DelegateCommand NavigateToWorldMappingsCommand =>
        _navigateToWorldMappingsCommand ??= new DelegateCommand(NavigateToWorldMappings);

    private void NavigateToWorldMappings()
    {
        _regionManager.RequestNavigate(RegionNames.ContentRegion, "WorldMapperView");
    }

    public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ILovenseService lovenseService) :
          base(regionManager)
    {
        _regionManager = regionManager;
        _lovenseService = lovenseService;

        Toys = new ObservableCollection<LovenseToyViewModel>();
        eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);

        Toys.Add(new LovenseToyViewModel()
        {
            Id = "e8b5fcfa9557",
            Battery = 87,
            DisplayName = "Brrr (Lush 2)",
            ToyIcon = _lovenseService.GetToyIcon(new Lush()),
            ToyType = LovenseToyEnum.Lush,
            Status = true
        });
        Toys.Add(new LovenseToyViewModel()
        {
            Id = "cffd248698bd",
            Battery = 43,
            DisplayName = "Nora the Explora (Nora)",
            ToyIcon = _lovenseService.GetToyIcon(new Nora()),
            ToyType = LovenseToyEnum.Nora,
            Status = true
        });
        Toys.Add(new LovenseToyViewModel()
        {
            Id = "cffd2486aaaa",
            Battery = 8,
            DisplayName = "Ferri",
            ToyIcon = _lovenseService.GetToyIcon(new Ferri()),
            ToyType = LovenseToyEnum.Ferri,
            Status = false
        });
        Toys.Add(new LovenseToyViewModel()
        {
            Id = "cffd2486bbbb",
            Battery = 100,
            DisplayName = "Exomoon",
            ToyIcon = _lovenseService.GetToyIcon(new Exomoon()),
            ToyType = LovenseToyEnum.Exomoon,
            Status = null
        });
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
                        DisplayName = lovenseToy.DisplayName,
                        ToyIcon = _lovenseService.GetToyIcon(lovenseToy),
                        ToyType = lovenseToy.ToyType
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