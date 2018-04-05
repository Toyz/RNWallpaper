using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;

namespace RNWallpaper
{
    public static class Extensions
    {
        public static void AddOnUi<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
        }

        public static string ToQueryString(this NameValueCollection nvc)
        {
            var sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                var values = nvc.GetValues(key);
                if (values == null) continue;

                foreach (var value in values)
                {
                    sb.Append(sb.Length == 0 ? "?" : "&");
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                }
            }

            return sb.ToString();
        }
    }
}
