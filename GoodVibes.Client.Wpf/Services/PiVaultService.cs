using System;
using System.Windows.Media.Imaging;
using GoodVibes.Client.Wpf.Services.Abstractions;

namespace GoodVibes.Client.Wpf.Services;

public class PiVaultService : IPiVaultService
{
    public BitmapImage GetLogoIcon(bool usingEmlaLock, bool usingChaster, bool canUnlock)
    {
        if (usingEmlaLock)
        {
            return new BitmapImage(new Uri("pack://application:,,,/GoodVibes;component/Resources/pishock_logo_emla.png"));
        }

        if (usingChaster)
        {
            return new BitmapImage(new Uri("pack://application:,,,/GoodVibes;component/Resources/pishock_logo_chaster.png"));
        }

        return canUnlock
            ? new BitmapImage(new Uri("pack://application:,,,/GoodVibes;component/Resources/pishock_logo_unlocked.png"))
            : new BitmapImage(new Uri("pack://application:,,,/GoodVibes;component/Resources/pishock_logo_locked.png"));
    }
}