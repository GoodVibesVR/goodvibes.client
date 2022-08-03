using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using GoodVibes.Client.PiShock;
using GoodVibes.Client.PiShock.Models.Abstractions;
using GoodVibes.Client.Wpf.Services.Abstractions;

namespace GoodVibes.Client.Wpf.Services;

public class PiShockService : IPiShockService
{
    private readonly PiShockClient _piShockClient;

    public PiShockService(PiShockClient piShockClient)
    {
        _piShockClient = piShockClient;
    }

    public BitmapImage GetToyIcon(PiShockToy toy)
    {
        var uriPackPath = toy switch
        {
            PiShock.Models.PiShock => "pack://application:,,,/GoodVibes;component/Resources/icon_pishock_shocker.png",
            _ => "pack://application:,,,/GoodVibes;component/Resources/icon_pishock_shocker.png"
        };

        return new BitmapImage(new Uri(uriPackPath));
    }

    public IEnumerable<PiShockToy> GetToys()
    {
        return _piShockClient.Toys.Select(t => t.Value);
    }
}