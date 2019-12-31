using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

using MiPaladar.ViewModels;
using MiPaladar.Entities;
using System.Windows;

namespace MiPaladar.Converters
{
    public class TableFormatterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {            
            if (values.Length < 3) return string.Empty;
            if (values[0] == DependencyProperty.UnsetValue) return string.Empty;
            if (values[1] == DependencyProperty.UnsetValue) return string.Empty;
            if (values[2] == DependencyProperty.UnsetValue) return string.Empty;

            //if (!(values[0] is bool)) return string.Empty;
            bool isBar = (bool)values[0];

            //if (!(values[1] is int)) return string.Empty;
            int number = (int)values[1];
            
            //if (!(values[2] is string)) return string.Empty;
            string description = (string)values[2];

            return (isBar ? "B" : "M") + number +
                (string.IsNullOrWhiteSpace(description) ? string.Empty : " - " + description);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
