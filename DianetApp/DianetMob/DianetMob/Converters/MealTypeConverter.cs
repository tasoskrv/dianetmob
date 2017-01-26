using System;
using System.Globalization;
using Xamarin.Forms;

namespace DianetMob.Converters
{
    public class MealTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "1")
            {
                return "Breakfast";
            }
            else if (value.ToString() == "2")
            {
                return "Lunch";
            }
            else if (value.ToString() == "3")
            {
                return "Dinner";
            }
            else if (value.ToString() == "4")
            {
                return "Snack";
            }                       
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }        
    }
}
