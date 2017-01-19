using System;
using System.Globalization;
using Xamarin.Forms;

namespace DianetMob.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            string formattedDate = date.ToString("yyyy-M-dd") + " " + DateTime.UtcNow.ToString("HH:mm:ss");
            return formattedDate;
        }        
    }
}
