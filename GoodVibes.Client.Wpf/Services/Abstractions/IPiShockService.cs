using System.Windows.Media.Imaging;
using GoodVibes.Client.PiShock.Models.Abstractions;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface IPiShockService
{
    BitmapImage GetToyIcon(PiShockToy toy);
}