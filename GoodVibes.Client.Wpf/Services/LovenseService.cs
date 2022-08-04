using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using GoodVibes.Client.Lovense;
using GoodVibes.Client.Lovense.Models;
using GoodVibes.Client.Lovense.Models.Abstractions;
using GoodVibes.Client.Wpf.Services.Abstractions;

namespace GoodVibes.Client.Wpf.Services;

public class LovenseService : ILovenseService
{
    private readonly LovenseClient _lovenseClient;

    public LovenseService(LovenseClient lovenseClient)
    {
        _lovenseClient = lovenseClient;
    }

    public BitmapImage GetToyIcon(LovenseToy toy)
    {
        var uriPackPath = toy switch
        {
            Ambi => "pack://application:,,,/GoodVibes;component/Resources/icon_ambi.png",
            Calor => "pack://application:,,,/GoodVibes;component/Resources/icon_calor.png",
            Diamo => "pack://application:,,,/GoodVibes;component/Resources/icon_diamo.png",
            Dolce => "pack://application:,,,/GoodVibes;component/Resources/icon_dolce.png",
            Domi => "pack://application:,,,/GoodVibes;component/Resources/icon_domi.png",
            Edge => "pack://application:,,,/GoodVibes;component/Resources/icon_edge.png",
            Exomoon => "pack://application:,,,/GoodVibes;component/Resources/icon_exomoon.png",
            Ferri => "pack://application:,,,/GoodVibes;component/Resources/icon_ferri.png",
            Gush => "pack://application:,,,/GoodVibes;component/Resources/icon_gush.png",
            Hush => "pack://application:,,,/GoodVibes;component/Resources/icon_hush.png",
            Hyphy => "pack://application:,,,/GoodVibes;component/Resources/icon_hyphy.png",
            Lush => "pack://application:,,,/GoodVibes;component/Resources/icon_lush.png",
            Max => "pack://application:,,,/GoodVibes;component/Resources/icon_max.png",
            Nora => "pack://application:,,,/GoodVibes;component/Resources/icon_nora.png",
            Osci => "pack://application:,,,/GoodVibes;component/Resources/icon_osci.png",
            SexMachine => "pack://application:,,,/GoodVibes;component/Resources/icon_blast.png",
            _ => "pack://application:,,,/GoodVibes;component/Resources/icon_lush.png"
        };

        return new BitmapImage(new Uri(uriPackPath));
    }

    public IEnumerable<LovenseToy> GetToys()
    {
        return _lovenseClient.Toys.Select(t => t.Value);
    }
}