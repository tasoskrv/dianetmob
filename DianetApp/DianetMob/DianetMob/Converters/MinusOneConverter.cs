﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace DianetMob.Converters
{
    public class MinusOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(value) - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(value) + 1;
        }



    }
}
