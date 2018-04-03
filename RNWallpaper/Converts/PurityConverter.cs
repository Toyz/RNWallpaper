using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using RNWallpaper.Json;

namespace RNWallpaper.Converts
{
    public class PurityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (Purity) value != Purity.Sketchy) return new SolidColorBrush(Colors.Transparent);
            return new SolidColorBrush(Color.FromRgb(255, 200, 64));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
