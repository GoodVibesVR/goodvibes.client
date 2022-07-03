using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.EventCarriers;
using GoodVibes.Client.Mapper.Events;
using GoodVibes.Client.Vrchat.Dtos;
using GoodVibes.Client.Wpf.EventCarriers;
using GoodVibes.Client.Wpf.Events;
using GoodVibes.Client.Wpf.Services;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    internal class AvatarMapperViewModel : RegionViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IAvatarMapperService _mapperService;
        private readonly OscProfileConverterService _oscProfileService;
        private readonly Dictionary<string, ObservableCollection<MappingPointViewModel>> _avatarMappingPoints;

        public ObservableCollection<AvatarViewModel> Avatars { get; set; }
        public ObservableCollection<ToyFunctionViewModel> AvailableToyFunctions { get; set; }

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

        private AvatarViewModel _selectedAvatar;
        public AvatarViewModel SelectedAvatar
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

        public AvatarMapperViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService,
            IAvatarMapperService mapperService, OscProfileConverterService oscProfileService) : base(regionManager)
        {
            _dialogService = dialogService;
            _mapperService = mapperService;
            //_avatarMapper = avatarMapper;
            _oscProfileService = oscProfileService;

            _avatarMappingPoints = new Dictionary<string, ObservableCollection<MappingPointViewModel>>();
            AvailableToyFunctions = new ObservableCollection<ToyFunctionViewModel>();
            Avatars = new ObservableCollection<AvatarViewModel>();

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
            _mapperService.RemoveMappingPoint(obj.MappingPoint.OscAddress);
            MappingPoints.Remove(obj.MappingPoint);
        }

        private void SelectedAvatarChanged(AvatarViewModel oldAvatarMapping, AvatarViewModel newAvatarMapping)
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

        private void AddOscProfileMappingPoints(OscProfileDto oscProfile)
        {
            if (oscProfile.Id == null)
            {
                // TODO: Show warning of invalid profile
                return;
            }
            
            ChangeAvatar(new AvatarViewModel()
            {
                AvatarId = oscProfile.Id,
                Name = oscProfile.Name
            });

            var newMappings = new ObservableCollection<MappingPointViewModel>();
            newMappings.AddRange(oscProfile.Parameters!.Where(p => p.Output != null).Select(p => new MappingPointViewModel()
            {
                OscAddress = p.Output.Address!.Replace("/avatar/parameters/", ""),
                AvailableToyFunctions = AvailableToyFunctions.ToArray(),
                HintVisible = Visibility.Hidden
            }));

            MappingPoints = newMappings;
        }

        private void ChangeMapperMappings(IReadOnlyCollection<MappingPointViewModel> oldMappings, IEnumerable<MappingPointViewModel> newMappings)
        {
            _mapperService.ChangeMappings(oldMappings, newMappings);
        }

        private void ImportProfile()
        {
            var jsonProfile = _dialogService.OpenJsonFileDialog();
            if (string.IsNullOrEmpty(jsonProfile))
                return;

            // TODO: OscProfile or GoodVibes Profile?
            var oscProfile = _oscProfileService.DeserializeOscProfile(jsonProfile);
            AddOscProfileMappingPoints(oscProfile);
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

                var avatarDto = new AvatarViewModel()
                {
                    AvatarId = obj.AvatarId
                };

                ChangeAvatar(avatarDto);
            });
        }

        private void ChangeAvatar(AvatarViewModel avatar)
        {
            var existingAvatar = Avatars.FirstOrDefault(a => a.AvatarId == avatar.AvatarId);
            if (existingAvatar == null)
            {
                Console.WriteLine($"Adding new avatar with ID {avatar.AvatarId} to Avatar list");
                Avatars.Add(avatar);
            }
            else
            {
                avatar = existingAvatar;
            }

            SelectedAvatar = avatar;
        }

        private void LovenseToyListUpdated(LovenseToyListUpdatedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var toyFunctions = _mapperService.BuildToyFunctionViewModels(obj.ToyList);
                foreach (var toy in toyFunctions)
                {
                    var exists = AvailableToyFunctions.Any(t => t.ToyId == toy.ToyId && t.Function == toy.Function);
                    if (!exists)
                    {
                        AvailableToyFunctions.Add(toy);
                    }
                }
                
                foreach (var mappingPoint in MappingPoints)
                {
                    mappingPoint.AvailableToyFunctions = AvailableToyFunctions.ToArray();
                }
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}