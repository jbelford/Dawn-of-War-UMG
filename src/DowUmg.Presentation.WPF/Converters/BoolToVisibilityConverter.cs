using System;
using System.Globalization;
using System.Windows.Data;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter, IBindingTypeConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value.ToString().ToLower()) switch
            {
                "false" => System.Windows.Visibility.Collapsed,
                _ => System.Windows.Visibility.Visible,
            };
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            return ((System.Windows.Visibility)value) switch
            {
                System.Windows.Visibility.Collapsed => "false",
                _ => "true"
            };
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            return 1;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            result = (from.ToString().ToLower()) switch
            {
                "false" => System.Windows.Visibility.Collapsed,
                _ => System.Windows.Visibility.Visible,
            };
            return true;
        }
    }
}
