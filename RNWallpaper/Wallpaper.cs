using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RNWallpaper.Json;

namespace RNWallpaper
{
    public class Wallpaper
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private readonly long _id;
        readonly WebClient _client = new WebClient();

        public Wallpaper(long id)
        {
            _id = id;
        }

        public async Task Set()
        {
            var jsonFromApi =
                await _client.DownloadStringTaskAsync(
                    new Uri($"http://127.0.0.1:8080/detail/{_id}"));

            var res = Details.FromJson(jsonFromApi);

            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(res.Url));

            if(!File.Exists(tempPath))
                await _client.DownloadFileTaskAsync(new Uri(res.Url), tempPath);
             
            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            Debug.WriteLine($"Saved image to {tempPath}");
        }

        public async Task SetClipboard()
        {
            var jsonFromApi =
                await _client.DownloadStringTaskAsync(
                    new Uri($"http://127.0.0.1:8080/detail/{_id}"));

            var res = Details.FromJson(jsonFromApi);

            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(res.Url));

            if (!File.Exists(tempPath))
                await _client.DownloadFileTaskAsync(new Uri(res.Url), tempPath);

            Clipboard.SetImage(new BitmapImage(new Uri(tempPath)));

            MessageBox.Show("Copied image to clipboard", "Copied to clipboard", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
