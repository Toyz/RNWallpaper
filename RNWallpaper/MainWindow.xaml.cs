﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private readonly ContextBinding _contextData;

        public ObservableCollection<Results> BgSearchResults { get; }

        public MainWindow()
        {
            InitializeComponent();
            _contextData = new ContextBinding
            {
                ImageRatioSizes = new List<string>
                {
                    "4x3",
                    "5x4",
                    "16x9",
                    "16x10",
                    "21x9",
                    "32x9",
                    "49x9",
                    "9x16",
                    "10x16"
                }
            };

            _contextData.SelectedImageRation = _contextData.ImageRatioSizes[2]; // 16x9

            DataContext = _contextData;

            _appTitle = Title;

            BgSearchResults = new ObservableCollection<Results>();

            AllBackgrounds.ItemsSource = BgSearchResults;

            Task.Run(async () =>
            {
                await HandleSearching();
            });
        }

        private async Task HandleSearching()
        {
            Dispatcher.Invoke(() =>
            {
                Title = $"{_appTitle} | (Loading p.{_curPage})";
                SearchBox.IsEnabled = false;
            });

            _loadingNextpage = true;
            var jsonFromApi =
                await (new WebClient()).DownloadStringTaskAsync(
                    new Uri(createURL()));

            var res = SearchResults.FromJson(jsonFromApi);

            foreach (var item in res.Results)
            {
                BgSearchResults.AddOnUi(item);
            }
            _loadingNextpage = false;

            Dispatcher.Invoke(() =>
            {
                Title = $"{_appTitle} | (p.{_curPage})";
                SearchBox.IsEnabled = true;
            });
        }

        private string createURL()
        {
            var purity = new List<string>(2);
            var category = new List<string>(3);

            var nvc = new NameValueCollection
            {
                { "purity", "" },
                { "q", _searchQuery },
                { "category", "" },
                { "sort", "date_added" },
                { "page", $"{_curPage}" },
                { "ratio", "16x9" }
            };

            #region Purity
            if (_contextData.PuritySfw)
            {
                purity.Add("sfw");
            }
            if (_contextData.PuritySketchy)
            {
                purity.Add("sketchy");
            }
            nvc["purity"] = string.Join(",", purity);
            #endregion

            #region Category
            if (_contextData.CatAnime)
            {
                category.Add("anime");
            }
            if (_contextData.CatPeople)
            {
                category.Add("people");
            }
            if (_contextData.CatGeneral)
            {
                category.Add("general");
            }
            nvc["category"] = string.Join(",", category);
            #endregion

            #region Ratio
            nvc["ratio"] = _contextData.SelectedImageRation;
            #endregion

            var r = $"http://127.0.0.1:8080/search{nvc.ToQueryString()}";
            return r;
        }

        private void HandleSearchEvent()
        {
           BgSearchResults.Clear();

            if (VisualTreeHelper.GetChild(AllBackgrounds, 0) is Decorator border)
            {
                var scroll = border.Child as ScrollViewer;
                scroll?.ScrollToTop();
            }

            _searchQuery = SearchBox.Text;
            _curPage = 1;
            Task.Run(async () =>
            {
                await HandleSearching();
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

            var sb = e.OriginalSource as ScrollBar;

            if (sb.Orientation == Orientation.Horizontal)
                return;

            if (sb.Value < sb.Maximum - (sb.Maximum * 0.05)) return;
            _loadingNextpage = true; // HACK: Forces newpage to be loading regradless
            Task.Run(async () =>
            {
                _curPage += 1;
                await HandleSearching();
            });

        }

        private void AdvSearchOk_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtonOptions.IsChecked = !ToggleButtonOptions.IsChecked;
        }
    }
}
