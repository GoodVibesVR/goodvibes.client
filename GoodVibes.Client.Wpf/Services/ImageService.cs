using System.Drawing;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Brushes = System.Drawing.Brushes;

namespace GoodVibes.Client.Wpf.Services;

public class ImageService : IImageService
{
    private readonly IApiClient _apiClient;

    public ImageService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BitmapImage> GetQrCodeFromGoodVibesServers(string imageKey)
    {
        var qrStream = await _apiClient.GetImageAsync($"/api/v1/lovense/qrcode?imageKey={HttpUtility.UrlEncode(imageKey)}");

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = qrStream;
        bitmap.EndInit();
        bitmap.Freeze();

        return bitmap;
    }

    public Bitmap DrawFilledRectangle(int x, int y)
    {
        var bmp = new Bitmap(x, y);
        using var graph = Graphics.FromImage(bmp);
        var ImageSize = new Rectangle(0, 0, x, y);
        graph.FillRectangle(Brushes.White, ImageSize);
        
        return bmp;
    }
}