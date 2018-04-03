using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace RNWallpaper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#if DEBUG
        protected override void OnStartup(StartupEventArgs e)
        {
            Debug.WriteLine($"Running at Tier {RenderCapability.Tier}");
            Debug.WriteLine($"Running in mode {RenderOptions.ProcessRenderMode}");
        }
#endif
    }
}
