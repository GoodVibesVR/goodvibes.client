using System.Collections.Generic;
using System.Windows.Media.Imaging;
using GoodVibes.Client.Lovense.Models.Abstractions;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface ILovenseService
{
    BitmapImage GetToyIcon(LovenseToy toy);
    IEnumerable<LovenseToy> GetToys();
}