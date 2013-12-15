using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using _30C3.scheduleModel;

namespace _30C3
{
    /// <summary>
    /// A converter which converters an list of person names into an comma separated string
    /// </summary>
    public class PersonsStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is List<person>))
                throw new NotSupportedException("PersonsStringConverter should only be used on List<person> values.");

            if (((List<person>)value) != null && ((List<person>)value).Count > 0)
            {
                string ReturnString = "";
                foreach (person p in (value as List<person>))
                {
                    ReturnString += p.Name + ", ";
                }

                return ReturnString.Substring(0, ReturnString.Length - 2);
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
