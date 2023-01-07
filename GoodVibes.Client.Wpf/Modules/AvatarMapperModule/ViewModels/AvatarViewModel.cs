using GoodVibes.Client.Core.Mvvm;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;

public class AvatarViewModel : ViewModelBase
{
    private string _avatarId;
    public string AvatarId
    {
        get => _avatarId;
        set
        {
            SetProperty(ref _avatarId, value);
            DisplayName = value;
        }
    }

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
            DisplayName = value;
        }
    }

    private string _displayName;

    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, _name ?? _avatarId);
    }
}