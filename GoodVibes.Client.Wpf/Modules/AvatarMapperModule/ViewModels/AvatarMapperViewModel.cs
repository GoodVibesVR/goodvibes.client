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
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    internal class AvatarMapperViewModel : RegionViewModelBase
    {
        public ObservableCollection<ToyViewModel> Toys { get; set; }
        public ObservableCollection<AvatarMappingDto> Avatars { get; set; }

        private AvatarMappingDto _selectedAvatar;
        public AvatarMappingDto SelectedAvatar
        {
            get => _selectedAvatar;
            set => SetProperty(ref _selectedAvatar, value);
        }


        private Dictionary<string, List<ToyMappingDto>> _mappings;
        public Dictionary<string, List<ToyMappingDto>> Mappings
        {
            get => _mappings;
            set => SetProperty(ref _mappings, value);
        }

        public AvatarMapperViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager)
        {
            Toys = new ObservableCollection<ToyViewModel>()
            {
                new ToyViewModel()
                {
                    DisplayName = "Brrr (Lush 2)",
                    Id = "12341",
                    Functions = new List<LovenseCommandEnum>()
                    {
                        LovenseCommandEnum.Vibrate
                    }
                },
                new ToyViewModel()
                {
                    DisplayName = "Nora",
                    Id = "12341",
                    Functions = new List<LovenseCommandEnum>()
                    {
                        LovenseCommandEnum.Vibrate,
                        LovenseCommandEnum.Rotate
                    }
                },
            };
            Avatars = new ObservableCollection<AvatarMappingDto>()
            {
                new AvatarMappingDto()
                {
                    AvatarId = "avtr_740A58AC-1C52-450F-99B5-B66F7387FF78"
                },
                new AvatarMappingDto()
                {
                    AvatarId = "avtr_CAB4271A-3F48-40CC-94D3-5FBD95D889D9"
                },
                new AvatarMappingDto()
                {
                    AvatarId = "avtr_B55FA4C2-8749-4F6B-8F3C-F01432D27AE6"
                },
                new AvatarMappingDto()
                {
                    AvatarId = "avtr_182C8B2F-D1A6-4FBE-AAB0-B42F51A0D7AE"
                }
            };

            // SelectedAvatar?.PropertyChanged += SelectedAvatar_PropertyChanged;

            eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
            eventAggregator.GetEvent<AvatarChangedEventCarrier>().Subscribe(AvatarChanged);
        }

        private void SelectedAvatar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("Selected avatar changed");
        }

        private void AvatarChanged(AvatarChangedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Console.WriteLine("AvatarChanged event received");

                var exists = Avatars.Any(a => a.AvatarId == obj.AvatarId);
                if (!exists)
                {
                    Console.WriteLine($"Adding new avatar with ID {obj.AvatarId} to Avatar list");
                    Avatars.Add(new AvatarMappingDto()
                    {
                        AvatarId = obj.AvatarId
                    });
                }
            });
        }

        private void LovenseToyListUpdated(LovenseToyListUpdatedEvent obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var tempList = Toys;
                var tempList2 = new List<ToyViewModel>();
                foreach (var lovenseToy in obj.ToyList!)
                {
                    if (!lovenseToy.Status || !lovenseToy.Enabled)
                    {
                        continue;
                    }

                    var toy = new ToyViewModel()
                    {
                        Id = lovenseToy.Id,
                        DisplayName = lovenseToy.DisplayName,
                        Functions = new List<LovenseCommandEnum>()
                        {
                            lovenseToy.Function1
                        }
                    };
                    if (lovenseToy.Function2 != LovenseCommandEnum.None)
                    {
                        toy.Functions.Add(lovenseToy.Function2);
                    }
                    
                    tempList2.Add(toy);
                }

                foreach (var toy in tempList2.ToList())
                {
                    var exists = tempList.Any(t => t.Id == toy.Id);
                    if (!exists)
                    {
                        Toys.Add(toy);
                    }
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