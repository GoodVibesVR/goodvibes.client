using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
using GoodVibes.Client.Wpf.EventCarriers;
using GoodVibes.Client.Wpf.Events;
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
    private readonly IEventAggregator _eventAggregator;
    private readonly ILovenseService _lovenseService;
    private readonly IPiShockService _piShockService;

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

    public MenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ILovenseService lovenseService, IPiShockService piShockService) :
          base(regionManager)
    {
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        _lovenseService = lovenseService;
        _piShockService = piShockService;

        Toys = new ObservableCollection<ToyViewModel>();
        eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
        eventAggregator.GetEvent<RemoveToyEventCarrier>().Subscribe(ToyRemoved);
        eventAggregator.GetEvent<PiShockToyListUpdatedEventCarrier>().Subscribe(PiShockToyListUpdated);
    }

    private void ToyRemoved(RemoveToyEvent obj)
    {
        var toy = Toys.FirstOrDefault(t => t.Id == obj.ToyId);
        if (toy != null)
        {
            switch (toy.ToyType)
            {
                
                case ToyTypeEnum.Ambi:
                case ToyTypeEnum.Calor:
                case ToyTypeEnum.Diamo:
                case ToyTypeEnum.Dolce:
                case ToyTypeEnum.Domi:
                case ToyTypeEnum.Edge:
                case ToyTypeEnum.Exomoon:
                case ToyTypeEnum.Ferri:
                case ToyTypeEnum.Gush:
                case ToyTypeEnum.Hush:
                case ToyTypeEnum.Hyphy:
                case ToyTypeEnum.Lush:
                case ToyTypeEnum.Max:
                case ToyTypeEnum.Nora:
                case ToyTypeEnum.Osci:
                case ToyTypeEnum.Tenera:
                case ToyTypeEnum.Gravity:
                case ToyTypeEnum.Flexer:
                case ToyTypeEnum.Gemini:
                case ToyTypeEnum.SexMachine:
                    _eventAggregator.GetEvent<RemoveLovenseToyEventCarrier>().Publish(new RemoveLovenseToyEvent()
                    {
                        ToyId = toy.Id
                    });
                    break;
                case ToyTypeEnum.PiShock:
                    _eventAggregator.GetEvent<RemovePiShockToyEventCarrier>().Publish(new RemovePiShockToyEvent()
                    {
                        ToyId = toy.Id
                    });
                    break;
                case ToyTypeEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Toys.Remove(toy);
        }
    }
    private void PiShockToyListUpdated(PiShockToyListUpdatedEvent obj)
    {
        Application.Current.Dispatcher.Invoke((Action)delegate
        {
            var tempList = Toys;
            foreach (var piShockToy in obj.ToyList!)
            {
                var toy = tempList.FirstOrDefault(t => t.Id == piShockToy.ShareCode);
                if (toy == null)
                {
                    Toys.Add(new ToyViewModel()
                    {
                        Id = piShockToy.ShareCode,
                        DisplayName = piShockToy.FriendlyName,
                        ToyIcon = _piShockService.GetToyIcon(piShockToy),
                        Status = true,
                        ToyType = ToyTypeEnum.PiShock
                    });
                }
            }
        });
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
        var existingLovenseToys = _lovenseService.GetToys();
        foreach (var lovenseToy in existingLovenseToys)
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
        }

        var existingPiShockToys = _piShockService.GetToys();
        foreach (var piShockToy in existingPiShockToys)
        {
            Toys.Add(new ToyViewModel()
            {
                Id = piShockToy.ShareCode,
                DisplayName = piShockToy.FriendlyName,
                ToyIcon = _piShockService.GetToyIcon(piShockToy),
                Status = true,
                ToyType = ToyTypeEnum.PiShock
            });
        }
    }
}