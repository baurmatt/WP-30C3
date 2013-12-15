using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace _30C3
{
    /// <summary>
    /// A converter which returns, based on the given integer, the Visibility (>0 -> Visible, else Collapsed)
    /// </summary>
    public class IntegerToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString()) > 0)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
