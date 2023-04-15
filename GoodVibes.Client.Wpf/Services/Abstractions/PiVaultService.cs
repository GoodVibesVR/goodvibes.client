using System.Windows.Media.Imaging;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface IPiVaultService
{
    BitmapImage GetLogoIcon(bool usingEmlaLock, bool usingChaster, bool canUnlock);
}