using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace _30C3
{
    /// <summary>
    /// A converter which cuts the given string after X chars (parameter) and adds "..."
    /// </summary>
    public class LongTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                throw new NotSupportedException("LongTextConverter should only be used on string values.");

            int StringLength;
            bool IsInt = int.TryParse(parameter.ToString(),out StringLength);

            if (IsInt == false || parameter == null)
                throw new ArgumentNullException("You need to provide the desired length of the output string via the parameter");

            if (((string)value).Length > StringLength)
            {
                return ((string)value).Substring(0, StringLength) + "...";
            }
            else
            {
                return (string)value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
