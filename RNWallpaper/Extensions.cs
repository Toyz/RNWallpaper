using System;
using System.Collections.Generic;
using System.Windows;

namespace RNWallpaper
{
    public static class Extensions
    {
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
        }
    }
}
