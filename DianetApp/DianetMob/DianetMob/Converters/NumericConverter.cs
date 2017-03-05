using System;
using System.Globalization;
using Xamarin.Forms;

namespace DianetMob.Converters
{
    public class NumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Double.Parse(value.ToString()) == 0)
                return "";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals(""))
                return 0;
            return value;
        }
    }
}
