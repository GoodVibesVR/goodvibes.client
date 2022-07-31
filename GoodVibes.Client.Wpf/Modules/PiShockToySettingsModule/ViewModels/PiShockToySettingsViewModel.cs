using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.PiShock;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.PiShockToySettingsModule.ViewModels
{
    public class PiShockToySettingsViewModel : RegionViewModelBase
    {
        private readonly PiShockClient _piShockClient;

        public PiShockToySettingsViewModel(IRegionManager regionManager, PiShockClient piShockClient) : base(regionManager)
        {
            _piShockClient = piShockClient;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var toyId = (string)navigationContext.Parameters["toyId"];
            var shocker = _piShockClient.Toys?[toyId];
            if (shocker == null)
            {
                // TODO: Do something
            }

            // TODO: ....
        }
    }
}
