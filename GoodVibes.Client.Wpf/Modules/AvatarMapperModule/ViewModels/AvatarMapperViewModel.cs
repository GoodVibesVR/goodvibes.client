using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;
using GoodVibes.Client.Lovense.EventCarriers;
using GoodVibes.Client.Lovense.Events;
using GoodVibes.Client.Mapper.Dtos;
using Prism.Events;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    internal class AvatarMapperViewModel : RegionViewModelBase
    {
        public ObservableCollection<ToyMappingDto> Toys { get; set; }

        public AvatarMapperViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager)
        {
            Toys = new ObservableCollection<ToyMappingDto>();

            eventAggregator.GetEvent<LovenseToyListUpdatedEventCarrier>().Subscribe(LovenseToyListUpdated);
        }

        private void LovenseToyListUpdated(LovenseToyListUpdatedEvent obj)
        {
            var tempList = Toys;
            var tempList2 = new List<ToyMappingDto>();
            foreach (var lovenseToy in obj.ToyList!)
            {
                if (!lovenseToy.Status || !lovenseToy.Enabled)
                {
                    continue;
                }

                if (lovenseToy.Function1 != LovenseCommandEnum.None)
                {
                    tempList2.Add(new ToyMappingDto()
                    {
                        DisplayName = lovenseToy.DisplayName,
                        Function = lovenseToy.Function1,
                        Id = lovenseToy.Id!
                    });
                }

                if (lovenseToy.Function2 != LovenseCommandEnum.None)
                {
                    tempList2.Add(new ToyMappingDto()
                    {
                        DisplayName = lovenseToy.DisplayName,
                        Function = lovenseToy.Function2,
                        Id = lovenseToy.Id!
                    });
                }
            }

            foreach (var toy in Toys)
            {
                var exists = tempList2.Any(t => t.Id == toy.Id && t.Function == toy.Function);
                if (!exists)
                {
                    Toys.Add(toy);
                }

                // TODO: Actually remove as well.
            }

            // TODO: We need to handle mapped toy types as well as IDs here... or?
            //var disconnectedToys = tempList.Where(t => obj.ToyList.All(x => x.Id != t.Id));
            //var test = tempList.Except(Toys);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}