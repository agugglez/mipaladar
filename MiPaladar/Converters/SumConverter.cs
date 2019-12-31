using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

using MiPaladar.Classes;

namespace MiPaladar.Converters
{
    [ValueConversion(typeof(IEnumerable<TotalByWaiterByProduct>), typeof(double))]
    public class TotalSumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var totals = value as IEnumerable<object>;
            if (totals == null)
                return "0";

            double sum = 0;

            foreach (var u in totals)
            {
                sum += ((TotalByWaiterByProduct)u).Total;
            }

            return sum.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
