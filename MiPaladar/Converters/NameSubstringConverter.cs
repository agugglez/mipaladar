using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Windows.Data;

namespace MiPaladar.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class NameSubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringParameter = (string)value;

            return stringParameter.Substring(0, 4);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
