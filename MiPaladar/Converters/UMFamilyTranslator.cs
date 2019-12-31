using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

using MiPaladar.Entities;

namespace MiPaladar.Converters
{
    [ValueConversion(typeof(UMFamily), typeof(string))]
    public class UMFamilyTranslator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is UMFamily)) return string.Empty;

            UMFamily um_family = (UMFamily)value;

            string inSpanish = null;

            if (um_family.Name == "Quantity") inSpanish = "Cantidad";
            if (um_family.Name == "Weight") inSpanish = "Peso";
            if (um_family.Name == "Volume") inSpanish = "Volumen";

            return inSpanish;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
