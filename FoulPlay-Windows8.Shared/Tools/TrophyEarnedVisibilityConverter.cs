﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Foulplay_Windows8.Core.Entities;

namespace FoulPlay_Windows8.Tools
{
    public class TrophyEarnedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = value as TrophyDetailEntity.Trophy;
            if (item == null) return Visibility.Collapsed;
            if (item.ComparedUser != null)
            {
                return item.ComparedUser.Earned ? Visibility.Visible : Visibility.Collapsed;
            }
            if (item.FromUser != null)
            {
                return item.FromUser.Earned ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}