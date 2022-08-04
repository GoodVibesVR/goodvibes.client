using System;
using GoodVibes.Client.Core.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels;

public class AddLovenseViewModel : RegionViewModelBase
{
    public AddLovenseViewModel(IRegionManager regionManager) : base(regionManager)
    {
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Console.WriteLine("Now navigated to Add Lovense form");

        // TODO: This should reset the form completely
    }
}