using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace _30C3
{
    /// <summary>
    /// A converter which returns the full name of the given language (de/en/default)
    /// </summary>
    public class LanguageStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                throw new NotSupportedException("LongTextConverter should only be used on string values.");

            switch (value as string)
            {
                case "de":
                    return "German";
                case "en":
                    return "English";
                default:
                    return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
