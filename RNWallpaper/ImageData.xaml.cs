using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using RNWallpaper.Json;

namespace RNWallpaper
{
    /// <summary>
    /// Interaction logic for ImageData.xaml
    /// </summary>
    public partial class ImageData : UserControl
    {
        public static bool isSetting { get; private set; }

        public ImageData()
        {
            InitializeComponent();
        }

        private void OpenOnWH_Click(object sender, RoutedEventArgs e)
        {
            Process.Start($"https://whvn.cc/{((Results) DataContext).ImageId}");
        }

        private async void SetWallPaper_Click(object sender, RoutedEventArgs e)
        {
            if (isSetting) return;
            Dispatcher.Invoke(() => { ((Button) sender).IsEnabled = false; });

            isSetting = true;
            var wp = new Wallpaper(((Results)DataContext).ImageId);
            await wp.Set();
            isSetting = false;

            Dispatcher.Invoke(() => { ((Button)sender).IsEnabled = true; });
        }

        private async void CopyToClipBoard_Click(object sender, RoutedEventArgs e)
        {
            if (isSetting) return;
            Dispatcher.Invoke(() => { ((Button)sender).IsEnabled = false; });

            isSetting = true;
            var wp = new Wallpaper(((Results)DataContext).ImageId);
            await wp.SetClipboard();
            isSetting = false;

            Dispatcher.Invoke(() => { ((Button)sender).IsEnabled = true; });
        }
    }
}
