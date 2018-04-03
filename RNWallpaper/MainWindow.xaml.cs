using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using RNWallpaper.Json;

namespace RNWallpaper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _searchQuery = "";
        private int _curPage = 1;
        private bool _loadingNextpage;
        private readonly string _appTitle;

        public ObservableCollection<Results> BgSearchResults { get; }

        public MainWindow()
        {
            InitializeComponent();
            _appTitle = Title;

            BgSearchResults = new ObservableCollection<Results>();

            AllBackgrounds.ItemsSource = BgSearchResults;

            Task.Run(async () =>
            {
                await HandleSearching(_curPage);
            });
        }

        private async Task HandleSearching(int page = 1)
        {
            Dispatcher.Invoke(() =>
            {
                Title = $"{_appTitle} | (Loading p.{page})";
                SearchBox.IsEnabled = false;
            });

            _loadingNextpage = true;
            var jsonFromApi =
                await (new WebClient()).DownloadStringTaskAsync(
                    new Uri($"http://127.0.0.1:8080/search?q={_searchQuery}&category=anime,people,general&purity=sketchy,sfw&sort=date_added&page={page}"));

            var res = SearchResults.FromJson(jsonFromApi);

            foreach (var item in res.Results)
            {
                BgSearchResults.AddOnUI(item);
            }
            _loadingNextpage = false;

            Dispatcher.Invoke(() =>
            {
                Title = $"{_appTitle} | (p.{page})";
                SearchBox.IsEnabled = true;
            });
        }


        private void HandleSearchEvent()
        {
           BgSearchResults.Clear();

            if (VisualTreeHelper.GetChild(AllBackgrounds, 0) is Decorator border)
            {
                ScrollViewer scroll = border.Child as ScrollViewer;
                scroll?.ScrollToTop();
            }

            _searchQuery = SearchBox.Text;
            _curPage = 1;
            Task.Run(async () =>
            {
                await HandleSearching(_curPage);
            });
        }

        private void SearchButton_Clicked(object sender, RoutedEventArgs e) => HandleSearchEvent();

        private void SearchBox_Keyup(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleSearchEvent();
            }
        }

        private void bgResults_Scroll(object sender, ScrollEventArgs e)
        {
            if (_loadingNextpage) return;

            ScrollBar sb = e.OriginalSource as ScrollBar;

            if (sb.Orientation == Orientation.Horizontal)
                return;

            if (sb.Value == sb.Maximum)
            {
                Task.Run(async () =>
                {
                    _curPage += 1;
                    await HandleSearching(_curPage);
                });
            }

        }
    }
}
