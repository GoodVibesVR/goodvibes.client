using GoodVibes.Client.Core.Mvvm;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.ViewModels
{
    public class ToyFunctionViewModel : ViewModelBase
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

        private string _function;
        public string Function
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

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string DisplayName => $"{Name} / {Function}";
    }
}