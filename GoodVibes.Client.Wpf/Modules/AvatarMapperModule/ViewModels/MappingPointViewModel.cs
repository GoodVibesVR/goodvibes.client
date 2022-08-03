using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Cache;
using GoodVibes.Client.Cache.Models;
using GoodVibes.Client.Common.Extensions;
using Prism.Ioc;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Wpf.EventCarriers;
using GoodVibes.Client.Wpf.Events;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Prism.Commands;
using Prism.Events;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    public class MappingPointViewModel : ViewModelBase
    {
        private readonly IAvatarMapperService _mapperService;
        private readonly IEventAggregator _eventAggregator;

        private readonly GoodVibesCacheManager<AvatarMapperCache> _cacheManager;

        private bool _beingImported;
        public bool BeingImported
        {
            get => _beingImported;
            set => SetProperty(ref _beingImported, value);
        }

        private string _avatarId;
        public string AvatarId
        {
            get => _avatarId;
            set => SetProperty(ref _avatarId, value);
        }

        private string _oscAddress;
        public string OscAddress
        {
            get => _oscAddress;
            set
            {
                ChangeOscAddress(_oscAddress, value);
                SetProperty(ref _oscAddress, value);
                UpdateCache();
            }
        }

        private ToyFunctionViewModel[] _availableToyFunctions;
        public ToyFunctionViewModel[] AvailableToyFunctions
        {
            get => _availableToyFunctions;
            set => SetProperty(ref _availableToyFunctions, value);
        }

        private ObservableCollection<ToyFunctionViewModel> _toyMappings;
        public ObservableCollection<ToyFunctionViewModel> ToyMappings
        {
            get => _toyMappings;
            set
            {
                SetProperty(ref _toyMappings, value);
                UpdateCache();
            }
        }

        private Visibility _hintVisible;
        public Visibility HintVisible
        {
            get => _hintVisible;
            set => SetProperty(ref _hintVisible, value);
        }

        private DelegateCommand _removeMappingCommand;
        public DelegateCommand RemoveMappingCommand =>
            _removeMappingCommand ??= new DelegateCommand(RemoveMapping);

        private DelegateCommand _gotFocusCommand;
        public DelegateCommand GotFocusCommand =>
            _gotFocusCommand ??= new DelegateCommand(OscAddressFieldFocused);

        private DelegateCommand _lostFocusCommand;
        public DelegateCommand LostFocusCommand =>
            _lostFocusCommand ??= new DelegateCommand(OscAddressFieldFocusedLost);

        public MappingPointViewModel()
        {
            _mapperService = ContainerLocator.Container.Resolve<IAvatarMapperService>();
            _eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            _cacheManager = ContainerLocator.Container.Resolve<GoodVibesCacheManager<AvatarMapperCache>>();

            ToyMappings = new ObservableCollection<ToyFunctionViewModel>();
            ToyMappings.CollectionChanged += ToyMappings_CollectionChanged;

            _oscAddress = string.Empty;
        }

        public void AddToyFunction(ToyFunctionViewModel toyFunction)
        {
            ToyMappings.Add(toyFunction);
        }

        private void UpdateCache()
        {
            if (_beingImported) return;
            if (AvatarId == null) return;
            if (string.IsNullOrEmpty(OscAddress)) return;

            var cache = _cacheManager.GetCache();
            var avatar = cache.AvatarMapper[AvatarId];
            var mappingExists = avatar.OscMappings.TryGetValue(OscAddress, out var functions);
            if (!mappingExists)
            {
                avatar.OscMappings.Add(OscAddress, new List<MappedFunctionsCache>());
            }

            avatar.OscMappings[OscAddress] = ToyMappings.Select(tm => new MappedFunctionsCache()
            {
                Function = tm.Function,
                Name = tm.Name,
                ToyId = tm.ToyId,
                Type = tm.Type
            }).ToList();

            _cacheManager.SaveCache(cache);
        }

        private void RemoveFromCache()
        {
            if (AvatarId == null) return;

            var cache = _cacheManager.GetCache();
            var avatar = cache.AvatarMapper[AvatarId];
            avatar.OscMappings.Remove(OscAddress);

            _cacheManager.SaveCache(cache);
        }

        private void ToyMappings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateCache();
                    if (_beingImported) return;

                    foreach (var newItem in e.NewItems!)
                    {
                        var mapping = newItem as ToyFunctionViewModel;
                        _mapperService.AddMapping(OscAddress, new ToyMappingDto()
                        {
                            Id = mapping.ToyId,
                            Function = mapping.Function,
                            IsChecked = mapping.IsChecked,
                            Name = mapping.Name,
                            ToyType = ToyTypeExtensions.GetToyTypeFromTypeString(mapping.Type)
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateCache();
                    if (_beingImported) return;

                    foreach (var oldItem in e.OldItems!)
                    {
                        var mapping = oldItem as ToyFunctionViewModel;
                        _mapperService.RemoveMapping(OscAddress, new ToyMappingDto()
                        {
                            Id = mapping.ToyId,
                            Function = mapping.Function,
                            IsChecked = mapping.IsChecked,
                            Name = mapping.Name,
                            ToyType = ToyTypeExtensions.GetToyTypeFromTypeString(mapping.Type)
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    // Do nothing.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeOscAddress(string oldAddress, string newAddress)
        {
            if (_beingImported) return;

            if (!string.IsNullOrEmpty(oldAddress))
            {
                RemoveFromCache();
            }

            switch (oldAddress)
            {
                case null:
                case "" when newAddress == string.Empty:
                    return;
                case "" when newAddress != string.Empty:
                    _eventAggregator.GetEvent<AddEmptyMappingEventCarrier>().Publish(new AddEmptyMappingEvent());
                    break;
            }

            _mapperService.ChangeOrAddMappingAddress(oldAddress, newAddress);
        }

        public void RemoveMapping()
        {
            if (string.IsNullOrEmpty(OscAddress))
                return;

            RemoveFromCache();

            _eventAggregator.GetEvent<RemoveAvatarMappingEventCarrier>().Publish(new RemoveAvatarMappingEvent()
            {
                MappingPoint = this
            });
        }

        private void OscAddressFieldFocused()
        {
            HintVisible = Visibility.Hidden;
        }

        private void OscAddressFieldFocusedLost()
        {
            if (!string.IsNullOrEmpty(OscAddress))
            {
                HintVisible = Visibility.Hidden;
            }
            else
            {
                HintVisible = Visibility.Visible;
            }
        }
    }
}