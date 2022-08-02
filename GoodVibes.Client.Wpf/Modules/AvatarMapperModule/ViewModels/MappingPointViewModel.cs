using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using GoodVibes.Client.Common;
using GoodVibes.Client.Common.Extensions;
using Prism.Ioc;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using GoodVibes.Client.PiShock.EventCarriers;
using GoodVibes.Client.PiShock.Events;
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
            set => SetProperty(ref _toyMappings, value);
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

            ToyMappings = new ObservableCollection<ToyFunctionViewModel>();
            ToyMappings.CollectionChanged += ToyMappings_CollectionChanged;

            _oscAddress = string.Empty;

            //_eventAggregator.GetEvent<RemoveLovenseToyEventCarrier>().Subscribe(RemoveLovenseToy);
            //_eventAggregator.GetEvent<RemovePiShockToyEventCarrier>().Subscribe(RemovePiShockToy);
        }

        //private void RemoveLovenseToy(RemoveLovenseToyEvent obj)
        //{
        //    RemoveToyFunctions(obj.ToyId);
        //}

        //private void RemovePiShockToy(RemovePiShockToyEvent obj)
        //{
        //    RemoveToyFunctions(obj.ToyId);
        //}

        //private void RemoveToyFunctions(string toyId)
        //{
        //    //var toyMappings = new ObservableCollection<ToyFunctionViewModel>(ToyMappings.Where(tm => tm.ToyId != toyId).ToList());
        //    var toyFunctions = ToyMappings.Where(tm => tm.ToyId == toyId).ToList();
        //    foreach (var toyFunction in toyFunctions)
        //    {
        //        ToyMappings.Remove(toyFunction);
        //    }

        //    ToyMappings = new ObservableCollection<ToyFunctionViewModel>(ToyMappings);
        //    ToyMappings.CollectionChanged += ToyMappings_CollectionChanged;
        //}

        private void ToyMappings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
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