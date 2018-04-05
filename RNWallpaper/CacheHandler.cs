using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;

namespace RNWallpaper
{
    public class CacheHandler
    {
        private static readonly string TempFolder;

        private static CacheHandler _instance;
        public static CacheHandler Instance => _instance ?? (_instance = new CacheHandler());

        static CacheHandler()
        {
            TempFolder = Path.Combine(Path.GetTempPath(),
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);

            Debug.WriteLine($"Temp Folder: {TempFolder}");
        }

        public BitmapImage GetImage(string url)
        {
            var filePath = ImagePath(url);
            return new BitmapImage(new Uri(filePath));
        }

        private string ImagePath(string url)
        {
            var filePathName = Path.GetFileName(url);
            var filePath = Path.Combine(TempFolder, filePathName ?? throw new InvalidOperationException());
            if (!File.Exists(filePath))
            {
                var client = new WebClient();
                client.DownloadFile(new Uri(url), filePath);
            }

            return filePath;
        }
    }
}
