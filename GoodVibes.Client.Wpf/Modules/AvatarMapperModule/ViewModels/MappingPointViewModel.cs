using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Prism.Ioc;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Mapper;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.Wpf.EventCarriers;
using GoodVibes.Client.Wpf.Events;
using Prism.Commands;
using Prism.Events;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    public class MappingPointViewModel : ViewModelBase
    {
        private readonly AvatarMapperClient _avatarMapper;
        private readonly IEventAggregator _eventAggregator;

        private string _oscAddress;

        public string OscAddress
        {
            get => _oscAddress;
            set
            {
                ChangeOscAddress(_oscAddress, value);
                SetProperty(ref _oscAddress, value);
            }
        }

        private ToyViewModel[] _availableToyFunctions;
        public ToyViewModel[] AvailableToyFunctions
        {
            get => _availableToyFunctions;
            set => SetProperty(ref _availableToyFunctions, value);
        }
    
        public ObservableCollection<ToyViewModel> ToyMappings { get; set; }

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
            _avatarMapper = ContainerLocator.Container.Resolve<AvatarMapperClient>();
            _eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();

            ToyMappings = new ObservableCollection<ToyViewModel>();
            ToyMappings.CollectionChanged += ToyMappings_CollectionChanged;

            _oscAddress = string.Empty;
        }

        private void ToyMappings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems!)
                    {
                        var mapping = newItem as ToyViewModel;
                        _avatarMapper.AddMapping(OscAddress, new ToyMappingDto()
                        {
                            Id = mapping.ToyId,
                            Function = mapping.Function,
                            IsChecked = mapping.IsChecked,
                            Name = mapping.Name
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        var mapping = oldItem as ToyViewModel;
                        _avatarMapper.RemoveMapping(OscAddress, new ToyMappingDto()
                        {
                            Id = mapping.ToyId,
                            Function = mapping.Function,
                            IsChecked = mapping.IsChecked,
                            Name = mapping.Name
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
            switch (oldAddress)
            {
                case null:
                case "" when newAddress == string.Empty:
                    return;
                case "" when newAddress != string.Empty:
                    _eventAggregator.GetEvent<AddEmptyMappingEventCarrier>().Publish(new AddEmptyMappingEvent());
                    break;
            }

            _avatarMapper.ChangeOrAddMappingAddress(oldAddress, newAddress);
        }

        private void RemoveMapping()
        {
            if (string.IsNullOrEmpty(OscAddress))
                return;

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