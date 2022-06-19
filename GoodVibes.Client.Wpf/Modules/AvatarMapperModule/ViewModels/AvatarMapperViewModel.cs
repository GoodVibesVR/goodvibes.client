using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.EventCarriers;
using GoodVibes.Client.Mapper.Events;
using GoodVibes.Client.Wpf.EventCarriers;
using GoodVibes.Client.Wpf.Events;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    internal class AvatarMapperViewModel : RegionViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly AvatarMapperClient _avatarMapper;

        private Dictionary<string, ObservableCollection<MappingPointViewModel>> _avatarMappingPoints;

        public ObservableCollection<AvatarMappingDto> Avatars { get; set; }
        private ObservableCollection<MappingPointViewModel> _mappingPoints;
        public ObservableCollection<MappingPointViewModel> MappingPoints
        {
            get => _mappingPoints;
            set
            {
                ChangeMapperMappings(_mappingPoints, value);
                SetProperty(ref _mappingPoints, value);
            }
        }

        public ObservableCollection<ToyViewModel> AvailableToyFunctions { get; set; }

        //public ObservableCollection<ToyMappingDto> Toys { get; set; }
        //public ObservableCollection<ToyMappingDto> SelectedToys { get; set; }

        private AvatarMappingDto _selectedAvatar;
        public AvatarMappingDto SelectedAvatar
        {
            get => _selectedAvatar;
            set
            {
                SelectedAvatarChanged(_selectedAvatar, value);
                SetProperty(ref _selectedAvatar, value);
            }
        }

        private Dictionary<string, List<ToyMappingDto>> _mappings;
        public Dictionary<string, List<ToyMappingDto>> Mappings
        {
            get => _mappings;
            set => SetProperty(ref _mappings, value);
        }

        private DelegateCommand _openImportDialog;
        public DelegateCommand OpenImportDialog =>
            _openImportDialog ??= new DelegateCommand(ImportProfile);

        private DelegateCommand _openExportDialog;
        public DelegateCommand OpenExportDialog =>
            _openExportDialog ??= new DelegateCommand(ExportProfile);

        private DelegateCommand<object> _removeMappingPointCommand;
        public DelegateCommand<object> RemoveMappingPointCommand =>
            _removeMappingPointCommand ??= new DelegateCommand<object>(argument =>
            {
                MappingPoints.Remove(argument as MappingPointViewModel);
            });

        public AvatarMapperViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService, AvatarMapperClient avatarMapper) : base(regionManager)
        {
            _dialogService = dialogService;
            _avatarMapper = avatarMapper;

            _avatarMappingPoints = new Dictionary<string, ObservableCollection<MappingPointViewModel>>();
            AvailableToyFunctions = new ObservableCollection<ToyViewModel>();
            Avatars = new ObservableCollection<AvatarMappingDto>(); // TODO: Fix this

            MappingPoints = new ObservableCollection<MappingPointViewModel>()
            {
                new MappingPointViewModel()
            };

            eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
            eventAggregator.GetEvent<AvatarChangedEventCarrier>().Subscribe(AvatarChanged);
            eventAggregator.GetEvent<RemoveAvatarMappingEventCarrier>().Subscribe(RemoveMappingPoint);
            eventAggregator.GetEvent<AddEmptyMappingEventCarrier>().Subscribe(AddEmptyMapping);
        }

        private void AddEmptyMapping(AddEmptyMappingEvent obj)
        {
            MappingPoints.Add(new MappingPointViewModel());
        }

        private void RemoveMappingPoint(RemoveAvatarMappingEvent obj)
        {
            _avatarMapper.RemoveMapping(obj.MappingPoint.OscAddress);
            MappingPoints.Remove(obj.MappingPoint);
        }

        private void SelectedAvatar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("Selected avatar changed");
        }

        private void SelectedAvatarChanged(AvatarMappingDto oldAvatarMapping, AvatarMappingDto newAvatarMapping)
        {
            if (oldAvatarMapping != null)
            {
                var oldMappingExists =
                    _avatarMappingPoints.TryGetValue(oldAvatarMapping.AvatarId!, out var oldAvatarMappingPoints);
                if (oldMappingExists)
                {
                    oldAvatarMappingPoints = MappingPoints;
                }
                else
                {
                    _avatarMappingPoints.Add(oldAvatarMapping.AvatarId!, MappingPoints);
                }
            }

            var newMappingExists = _avatarMappingPoints.TryGetValue(newAvatarMapping.AvatarId!, out var newAvatarMappingPoints);
            if (newMappingExists)
            {
                MappingPoints = newAvatarMappingPoints;
            }
            else
            {
                MappingPoints = new ObservableCollection<MappingPointViewModel>()
                {
                    new MappingPointViewModel()
                    {
                        OscAddress = string.Empty,
                        AvailableToyFunctions = AvailableToyFunctions.ToArray()
                    }
                };
            }
        }

        private void ChangeMapperMappings(ObservableCollection<MappingPointViewModel> oldMappings, ObservableCollection<MappingPointViewModel> newMappings)
        {
            var oldMappingDtos = oldMappings == null
                ? new List<MappingDto>()
                : oldMappings.Select(mappingPointViewModel => new MappingDto()
                {
                    OscAddress = mappingPointViewModel.OscAddress,
                    ToyMappings = mappingPointViewModel.ToyMappings.Select(tm => new ToyMappingDto()
                        { Id = tm.ToyId, Function = tm.Function, IsChecked = tm.IsChecked, Name = tm.Name }).ToList()
                });

            var newMappingDtos = newMappings.Select(mappingPointViewModel => new MappingDto()
            {
                OscAddress = mappingPointViewModel.OscAddress,
                ToyMappings = mappingPointViewModel.ToyMappings.Select(tm => new ToyMappingDto()
                    { Id = tm.ToyId, Function = tm.Function, IsChecked = tm.IsChecked, Name = tm.Name }).ToList()
            });

            _avatarMapper.ChangeMappings(oldMappingDtos, newMappingDtos);
        }

        private void ImportProfile()
        {
            var jsonProfile = _dialogService.OpenJsonFileDialog();
            if (string.IsNullOrEmpty(jsonProfile))
            {
                return;
            }

            // TODO: Do things
        }

        private void ExportProfile()
        {
            // TODO: Fix
        }

        private void AvatarChanged(AvatarChangedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Console.WriteLine("AvatarChanged event received");

                var avatarDto = new AvatarMappingDto()
                {
                    AvatarId = obj.AvatarId
                };

                var exists = Avatars.Any(a => a.AvatarId == obj.AvatarId);
                if (!exists)
                {
                    Console.WriteLine($"Adding new avatar with ID {obj.AvatarId} to Avatar list");
                    Avatars.Add(avatarDto);
                }

                SelectedAvatar = avatarDto;
            });
        }

        private void LovenseToyListUpdated(LovenseToyListUpdatedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var tempList = AvailableToyFunctions;
                var tempList2 = new List<ToyViewModel>();
                foreach (var lovenseToy in obj.ToyList!)
                {
                    if (lovenseToy.Status == false || !lovenseToy.Enabled)
                    {
                        continue;
                    }
                    if (lovenseToy.Function1 != LovenseCommandEnum.None)
                    {
                        tempList2.Add(new ToyViewModel()
                        {
                            Name = lovenseToy.DisplayName,
                            Function = lovenseToy.Function1,
                            ToyId = lovenseToy.Id!
                        });
                    }
                    if (lovenseToy.Function2 != LovenseCommandEnum.None)
                    {
                        tempList2.Add(new ToyViewModel()
                        {
                            Name = lovenseToy.DisplayName,
                            Function = lovenseToy.Function2,
                            ToyId = lovenseToy.Id!
                        });
                    }
                }
                
                foreach (var toy in tempList2.ToList())
                {
                    var exists = tempList.Any(t => t.ToyId == toy.ToyId && t.Function == toy.Function);
                    if (!exists)
                    {
                        AvailableToyFunctions.Add(toy);
                    }

                    // TODO: Actually remove as well.
                }

                // TODO: Add to avatar mappingPoints as well
                foreach (var mappingPoint in MappingPoints)
                {
                    mappingPoint.AvailableToyFunctions = AvailableToyFunctions.ToArray();
                }

                // TODO: We need to handle mapped toy types as well as IDs here... or?
                //var disconnectedToys = tempList.Where(t => obj.ToyList.All(x => x.Id != t.Id));
                //var test = tempList.Except(Toys);
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}