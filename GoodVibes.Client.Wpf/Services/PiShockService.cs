using System;
using System.Windows.Media.Imaging;
using GoodVibes.Client.PiShock.Models;
using GoodVibes.Client.PiShock.Models.Abstractions;
using GoodVibes.Client.Wpf.Services.Abstractions;

namespace GoodVibes.Client.Wpf.Services;

public class PiShockService : IPiShockService
{
    public BitmapImage GetToyIcon(PiShockToy toy)
    {
        var uriPackPath = toy switch
        {
            PiShock.Models.PiShock => "pack://application:,,,/GoodVibes;component/Resources/icon_pishock_shocker.png",
            _ => "pack://application:,,,/GoodVibes;component/Resources/icon_pishock_shocker.png"
        };

        return new BitmapImage(new Uri(uriPackPath));
    }
}