using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

using MiPaladar.Classes;

namespace MiPaladar.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class NotSetDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateValue = (DateTime)value;

            if (dateValue.Year == 1) return "-";
            else return string.Format("{0:d/MMM h:mm tt}", dateValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
