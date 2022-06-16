using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoodVibes.Client.Mapper.Annotations;

namespace GoodVibes.Client.Mapper.Dtos;

public class AvatarMappingDto : INotifyPropertyChanged
{
    public string? AvatarId { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}