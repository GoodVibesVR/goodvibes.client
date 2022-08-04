using GoodVibes.Client.Core.Mvvm;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;

public class AvatarViewModel : ViewModelBase
{
    private string _avatarId;
    public string AvatarId
    {
        get => _avatarId;
        set => SetProperty(ref _avatarId, value);
    }

    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string DisplayName => Name ?? AvatarId;
}