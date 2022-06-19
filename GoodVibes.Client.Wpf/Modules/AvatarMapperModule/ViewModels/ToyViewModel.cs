using System.Collections.Generic;
using System.Windows.Navigation;
using GoodVibes.Client.Core.Mvvm;
using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    public class ToyViewModel : ViewModelBase
    {
        private string _toyId;
        public string ToyId
        {
            get => _toyId;
            set => SetProperty(ref _toyId, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private LovenseCommandEnum _function;
        public LovenseCommandEnum Function
        {
            get => _function;
            set => SetProperty(ref _function, value);
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public string DisplayName => $"{Name} / {Function}";
    }
}