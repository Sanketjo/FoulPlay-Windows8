﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FoulPlay_Windows8.Tools
{
    public class HasTrophiesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}