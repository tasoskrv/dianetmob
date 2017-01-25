using System;
using System.Globalization;
using Xamarin.Forms;

namespace DianetMob.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if (value.ToString().Equals("0"))
            {
                return "Off";
            }
            return "On";
        }        
    }
}
