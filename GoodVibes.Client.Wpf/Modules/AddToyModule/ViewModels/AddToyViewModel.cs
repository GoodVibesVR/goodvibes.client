using System;
using System.Collections.ObjectModel;
using GoodVibes.Client.Core;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Wpf.Modules.AddToyModule.Constants;
using GoodVibes.Client.Wpf.Modules.AddToyModule.Views;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels
{
    public class AddToyViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<AddToyTypeViewModel> ToyTypes { get; set; }

        private AddToyTypeViewModel _selectedToyType;

        public AddToyTypeViewModel SelectedToyType
        {
            get => _selectedToyType;
            set
            {
                SelectedToyTypeChanged(value?.DisplayName);
                SetProperty(ref _selectedToyType, value);
            }
        }

        public AddToyViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            ToyTypes = new ObservableCollection<AddToyTypeViewModel>()
            {
                new() { DisplayName = string.Empty },
                new() { DisplayName = AddToyTypeConstants.PiShock },
                new () {DisplayName = AddToyTypeConstants.PiVault},
                new() { DisplayName = AddToyTypeConstants.LovenseToy }
            };
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Console.WriteLine("Navigating to Add Pi Shock form");
            SelectedToyType = null;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void SelectedToyTypeChanged(string toyType)
        {
            switch (toyType)
            {
                case AddToyTypeConstants.PiShock:
                    _regionManager.RequestNavigate(RegionNames.AddToyForm, nameof(AddPiShockView));
                    break;
                case AddToyTypeConstants.PiVault:
                    _regionManager.RequestNavigate(RegionNames.AddToyForm, nameof(AddPiVaultView));
                    break;
                case AddToyTypeConstants.LovenseToy:
                    _regionManager.RequestNavigate(RegionNames.AddToyForm, nameof(AddLovenseView));
                    break;
                default:
                    _regionManager.RequestNavigate(RegionNames.AddToyForm, nameof(SelectTypeView));
                    break;
            }
        }
    }
}
