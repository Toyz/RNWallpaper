using System;
using System.Globalization;
using System.Windows.Data;

namespace RNWallpaper.Converts
{
    public class CacheImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? CacheHandler.Instance.GetImage((string) value) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
