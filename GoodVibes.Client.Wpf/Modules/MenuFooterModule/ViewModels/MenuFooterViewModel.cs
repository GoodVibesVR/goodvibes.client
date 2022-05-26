using System.Diagnostics;
using GoodVibes.Client.Core.Mvvm;
using Prism.Commands;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.MenuFooterModule.ViewModels
{
    public class MenuFooterViewModel : RegionViewModelBase
    {
        private DelegateCommand _openDiscordCommand;
        public DelegateCommand OpenDiscordCommand =>
            _openDiscordCommand ??= new DelegateCommand(OpenDiscord);

        private void OpenDiscord()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/zS7R78zXSG",
                UseShellExecute = true
            });
        }

        private DelegateCommand _openGithubCommand;
        public DelegateCommand OpenGithubCommand =>
            _openGithubCommand ??= new DelegateCommand(OpenGithub);

        private void OpenGithub()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Miwca/GoodVibes.Client",
                UseShellExecute = true
            });
        }

        public MenuFooterViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
