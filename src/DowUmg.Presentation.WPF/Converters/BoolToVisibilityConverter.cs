using System;
using System.Globalization;
using System.Windows.Data;

namespace DowUmg.Presentation.WPF.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value.ToString().ToLower()) switch
            {
                "false" => System.Windows.Visibility.Collapsed,
                _ => System.Windows.Visibility.Visible,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((System.Windows.Visibility)value) switch
            {
                System.Windows.Visibility.Collapsed => "false",
                _ => "true"
            };
        }
    }
}
