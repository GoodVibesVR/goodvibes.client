using System;
using GoodVibes.Client.Core.Mvvm;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels;

public class SelectToyTypeViewModel : RegionViewModelBase
{
    public SelectToyTypeViewModel(IRegionManager regionManager) : base(regionManager)
    {
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        Console.WriteLine("Now navigated to SelectToyType form");

        // TODO: This should reset the form completely
    }
}