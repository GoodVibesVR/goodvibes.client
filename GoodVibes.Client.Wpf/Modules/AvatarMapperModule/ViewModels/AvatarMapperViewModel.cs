using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Cache;
using GoodVibes.Client.Cache.Models;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Mapper.EventCarriers;
using GoodVibes.Client.Mapper.Events;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
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
        private readonly ILovenseService _lovenseService;
        private readonly IPiShockService _piShockService;

        private readonly OscProfileConverterService _oscProfileService;
        private readonly GoodVibesCacheManager<AvatarMapperCache> _cacheManager;

        private readonly Dictionary<string, ObservableCollection<MappingPointViewModel>> _avatarMappingPoints;

        public ObservableCollection<AvatarViewModel> Avatars { get; set; }


        private ObservableCollection<ToyFunctionViewModel> _availableToyFunctions;
        public ObservableCollection<ToyFunctionViewModel> AvailableToyFunctions
        {
            get
            {
                return _availableToyFunctions;
            }
            set
            {
                _availableToyFunctions = value;
            }
        }

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
            IAvatarMapperService mapperService, ILovenseService lovenseService, IPiShockService piShockService,
            OscProfileConverterService oscProfileService, GoodVibesCacheManager<AvatarMapperCache> cacheManager) : base(regionManager)
        {
            _dialogService = dialogService;
            _mapperService = mapperService;
            _oscProfileService = oscProfileService;
            _lovenseService = lovenseService;
            _piShockService = piShockService;
            _cacheManager = cacheManager;

            _avatarMappingPoints = new Dictionary<string, ObservableCollection<MappingPointViewModel>>();
            Avatars = new ObservableCollection<AvatarViewModel>();
            MappingPoints = new ObservableCollection<MappingPointViewModel>() { new() };

            BuildAvatarMappingPoints();
            BuildAvailableToyFunctions();

            eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
            eventAggregator.GetEvent<PiShockToyListUpdatedEventCarrier>().Subscribe(PiShockToyListUpdated);
            eventAggregator.GetEvent<AvatarChangedEventCarrier>().Subscribe(AvatarChanged);
            eventAggregator.GetEvent<RemoveAvatarMappingEventCarrier>().Subscribe(RemoveMappingPoint);
            eventAggregator.GetEvent<AddEmptyMappingEventCarrier>().Subscribe(AddEmptyMapping);

            eventAggregator.GetEvent<RemoveLovenseToyEventCarrier>().Subscribe(RemoveLovenseToy);
            eventAggregator.GetEvent<RemovePiShockToyEventCarrier>().Subscribe(RemovePiShockToy);
        }

        private void BuildAvatarMappingPoints()
        {
            var cache = _cacheManager.GetCache();
            foreach (var avatarCache in cache.AvatarMapper)
            {
                var avatarId = avatarCache.Key;
                var name = avatarCache.Value.Name;
                var oscMappings = avatarCache.Value.OscMappings;
                var mappingPoints = new List<MappingPointViewModel>();
                foreach (var oscMapping in oscMappings)
                {
                    var oscAddress = oscMapping.Key;
                    var test = oscMapping.Value;
                    var mappedFunctions = test.Select(m => new ToyFunctionViewModel()
                    {
                        Function = m.Function,
                        Name = m.Name,
                        ToyId = m.ToyId,
                        Type = m.Type
                    });
                    var mappingPoint = new MappingPointViewModel()
                    {
                        BeingImported = true,
                        AvatarId = avatarId,
                        OscAddress = oscAddress,
                        HintVisible = string.IsNullOrEmpty(oscAddress) ? Visibility.Visible : Visibility.Hidden
                    };

                    foreach (var mappedFunction in mappedFunctions)
                    {
                        mappingPoint.AddToyFunction(mappedFunction);
                    }

                    mappingPoint.BeingImported = false;
                    mappingPoints.Add(mappingPoint);
                }

                if (!mappingPoints.Any(mp => string.IsNullOrEmpty(mp.OscAddress)))
                {
                    mappingPoints.Add(new MappingPointViewModel()
                    {
                        AvatarId = avatarId
                    });
                }

                _avatarMappingPoints.Add(avatarId, new ObservableCollection<MappingPointViewModel>(mappingPoints));

                Avatars.Add(new AvatarViewModel()
                {
                    AvatarId = avatarId,
                    Name = name
                });
            }
        }

        private void BuildAvailableToyFunctions()
        {
            AvailableToyFunctions = new ObservableCollection<ToyFunctionViewModel>();

            var lovenseToys = _lovenseService.GetToys();
            var lovenseToyFunctions = _mapperService.BuildToyFunctionViewModels(lovenseToys);
            BuildToyFunctions(lovenseToyFunctions);

            var piShockToys = _piShockService.GetToys();
            var piShockToyFunctions = _mapperService.BuildToyFunctionViewModels(piShockToys);
            BuildToyFunctions(piShockToyFunctions);
        }

        private void RemoveLovenseToy(RemoveLovenseToyEvent obj)
        {
            RemoveToyFunctions(obj.ToyId);
        }

        private void RemovePiShockToy(RemovePiShockToyEvent obj)
        {
            RemoveToyFunctions(obj.ToyId);
        }

        private void RemoveToyFunctions(string toyId)
        {
            var availableFunctions = AvailableToyFunctions
                .Where(f => f.ToyId == toyId).ToList();
            foreach (var function in availableFunctions)
            {
                AvailableToyFunctions.Remove(function);
            }

            foreach (var mappingPoint in MappingPoints)
            {
                mappingPoint.AvailableToyFunctions = AvailableToyFunctions.ToArray();
            }
        }

        private void AddEmptyMapping(AddEmptyMappingEvent obj)
        {
            MappingPoints.Add(new MappingPointViewModel()
            {
                AvatarId = SelectedAvatar?.AvatarId,
                AvailableToyFunctions = AvailableToyFunctions.ToArray()
            });
        }

        private void RemoveMappingPoint(RemoveAvatarMappingEvent obj)
        {
            _mapperService.RemoveMappingPoint(obj.MappingPoint.OscAddress);
            MappingPoints.Remove(obj.MappingPoint);

            if (MappingPoints.Count == 0)
            {
                MappingPoints.Add(new MappingPointViewModel()
                {
                    AvatarId = SelectedAvatar.AvatarId
                });
            }
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
                foreach (var newAvatarMappingPoint in newAvatarMappingPoints)
                {
                    newAvatarMappingPoint.AvailableToyFunctions = AvailableToyFunctions.ToArray();
                }
                MappingPoints = newAvatarMappingPoints;
            }
            else
            {
                MappingPoints = new ObservableCollection<MappingPointViewModel>()
                {
                    new MappingPointViewModel()
                    {
                        AvatarId = SelectedAvatar?.AvatarId,
                        OscAddress = string.Empty,
                        AvailableToyFunctions = AvailableToyFunctions.ToArray()
                    }
                };
            }
        }

        private void AddGoodVibesFileMappingPoints(GoodVibesProfileDto goodVibesProfile)
        {
            if (SelectedAvatar == null)
            {
                // TODO: Show warning of no selected avatar
                return;
            }

            if (goodVibesProfile?.Parameters == null ||
                goodVibesProfile.Parameters.Length == 0)
            {
                // No need to import nothing.
                return;
            }

            var dropDownAvatar = Avatars.First(a => a.AvatarId == SelectedAvatar.AvatarId);
            dropDownAvatar.Name = goodVibesProfile.AvatarName;
            SelectedAvatar.Name = goodVibesProfile.AvatarName;
            SelectedAvatar = SelectedAvatar;

            var results = goodVibesProfile.Parameters.Where(p => MappingPoints.All(p2 => p2.OscAddress != p)).ToArray();
            if (results.Any())
            {
                var newMappings = new ObservableCollection<MappingPointViewModel>();
                newMappings.AddRange(MappingPoints.Where(mp => mp.OscAddress != string.Empty));
                foreach (var result in results)
                {
                    newMappings.Add(new MappingPointViewModel()
                    {
                        AvatarId = SelectedAvatar.AvatarId,
                        OscAddress = result.Replace("/avatar/parameters/", ""),
                        AvailableToyFunctions = AvailableToyFunctions.ToArray(),
                        HintVisible = Visibility.Hidden
                    });
                }

                // Also add empty
                newMappings.Add(new MappingPointViewModel()
                {
                    AvatarId = SelectedAvatar.AvatarId,
                    AvailableToyFunctions = AvailableToyFunctions.ToArray()
                });

                MappingPoints = newMappings;
            }

            var cache = _cacheManager.GetCache();
            var avatarFound = cache.AvatarMapper.TryGetValue(SelectedAvatar.AvatarId, out var cachedAvatar);
            if (!avatarFound)
            {
                // We shouldn't be here...
                return;
            }

            cachedAvatar.Name = goodVibesProfile.AvatarName;
            _cacheManager.SaveCache(cache);
        }

        private void AddOscProfileMappingPoints(VrcOscProfileDto oscProfile)
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

            var oldMappings = MappingPoints.ToList();
            foreach (var oldMapping in oldMappings)
            {
                oldMapping.RemoveMapping();
            }

            var newMappings = new ObservableCollection<MappingPointViewModel>();
            newMappings.AddRange(oscProfile.Parameters!.Where(p => p.Output != null).Select(p => new MappingPointViewModel()
            {
                AvatarId = oscProfile.Id,
                OscAddress = p.Output.Address!.Replace("/avatar/parameters/", ""),
                AvailableToyFunctions = AvailableToyFunctions.ToArray(),
                HintVisible = Visibility.Hidden
            }));

            // Also add empty
            newMappings.Add(new MappingPointViewModel()
            {
                AvatarId = SelectedAvatar.AvatarId,
                AvailableToyFunctions = AvailableToyFunctions.ToArray()
            });

            MappingPoints = newMappings;
            var cache = _cacheManager.GetCache();
            var avatarFound = cache.AvatarMapper.TryGetValue(SelectedAvatar.AvatarId, out var cachedAvatar);
            if (!avatarFound)
            {
                // We shouldn't be here...
                return;
            }

            cachedAvatar.Name = oscProfile.Name;
            _cacheManager.SaveCache(cache);
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

            var goodVibesProfile = _mapperService.DeserializeAvatarMappingProfile(jsonProfile);
            if (goodVibesProfile != null)
            {
                AddGoodVibesFileMappingPoints(goodVibesProfile);
            }
            else
            {
                var oscProfile = _oscProfileService.DeserializeOscProfile(jsonProfile);
                AddOscProfileMappingPoints(oscProfile);
            }
        }

        private void ExportProfile()
        {
            if (SelectedAvatar == null)
                return; // Do nothing

            var profile = _mapperService.SerializeAvatarMappingProfile(new GoodVibesProfileDto()
            {
                AvatarName = SelectedAvatar.Name,
                Parameters = MappingPoints.Select(mp => mp.OscAddress).
                    Where(address => address != string.Empty).ToArray()
            });

            _dialogService.OpenJsonFileSaveDialog(SelectedAvatar.AvatarId, profile);
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
            if (SelectedAvatar != null && SelectedAvatar.AvatarId == avatar.AvatarId)
            {
                var cache = _cacheManager.GetCache();
                var dropDownAvatar = Avatars.First(a => a.AvatarId == avatar.AvatarId);
                dropDownAvatar.Name = avatar.Name;

                SelectedAvatar.Name = avatar.Name;


                var cachedAvatar = cache.AvatarMapper[avatar.AvatarId];
                cachedAvatar.Name = avatar.Name;
                _cacheManager.SaveCache(cache);

                return;
            }

            var existingAvatar = Avatars.FirstOrDefault(a => a.AvatarId == avatar.AvatarId);
            if (existingAvatar == null)
            {
                Console.WriteLine($"Adding new avatar with ID {avatar.AvatarId} to Avatar list");

                var cache = _cacheManager.GetCache();
                cache.AvatarMapper.Add(avatar.AvatarId, new AvatarCache()
                {
                    Name = avatar.Name,
                    OscMappings = new Dictionary<string, List<MappedFunctionsCache>>()
                });
                _cacheManager.SaveCache(cache);

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
                BuildToyFunctions(toyFunctions);
            });
        }

        private void PiShockToyListUpdated(PiShockToyListUpdatedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var toyFunctions = _mapperService.BuildToyFunctionViewModels(obj.ToyList);
                BuildToyFunctions(toyFunctions);
            });
        }

        private void BuildToyFunctions(IEnumerable<ToyFunctionViewModel> toyFunctions)
        {
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
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}