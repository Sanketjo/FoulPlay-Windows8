﻿using System;
using Windows.UI.Xaml.Data;

namespace FoulPlay_Windows8.Tools
{
    public class TrophyHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;
            //TODO: Add Translation
            return string.IsNullOrEmpty((string) value) ? "Hidden" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}