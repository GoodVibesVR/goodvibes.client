using System.Collections.Generic;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels;

internal class ToyViewModel : ViewModelBase
{
    public string DisplayName { get; set; }
    public string ToyId { get; set; }
    public List<LovenseCommandEnum> Functions { get; set; }
}