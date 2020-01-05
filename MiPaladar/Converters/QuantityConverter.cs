using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

using System.Text.RegularExpressions;
using System.Windows;

using MiPaladar.Entities;
using MiPaladar.ViewModels;
using MiPaladar.Services;

//namespace MiPaladar.Converters
//{
//    //Adds unit measure postfix to quantity
        
//    public class QuantityConverter : IMultiValueConverter
//    {
//        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (!(values[0] is float)) return string.Empty;
//            if (!(values[1] is UnitMeasure)) return string.Empty;

//            //float qtty = (float)values[0];
//            UnitMeasure um = (UnitMeasure)values[1];

//            return string.Format("{0}{1}", values[0], um.Caption);
//        }

//        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
//        {
//            MainWindowViewModel appvm = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

//            PatternMatcher pm = appvm.PatternMatcher;

//            float quantity;
//            UnitMeasure unit_measure;
//            pm.ParseQuantityString((string)value, out quantity, out unit_measure);

//            return new object[] { quantity, unit_measure };
//        }
//    }
//}
