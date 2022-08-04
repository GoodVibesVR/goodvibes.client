using GoodVibes.Client.Core.Mvvm;

namespace GoodVibes.Client.Wpf.Modules.AddToyModule.ViewModels;

public class AddToyTypeViewModel : ViewModelBase
{
    private string _displayName;

    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }
}