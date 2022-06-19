using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface IImageService
{
    Task<BitmapImage> GetQrCodeFromGoodVibesServers(string imageKey);
    Bitmap DrawFilledRectangle(int x, int y);
}