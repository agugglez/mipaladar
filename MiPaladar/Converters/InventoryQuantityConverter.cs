//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Windows.Data;
//using System.Globalization;

//using System.Text.RegularExpressions;
//using System.Windows;

//using MiPaladar.Entities;
//using MiPaladar.ViewModels;
//using MiPaladar.Services;

//namespace MiPaladar.Converters
//{        
//    public class InventoryQuantityConverter : IMultiValueConverter
//    {
//        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (!(values[0] is double)) return string.Empty;
//            if (!(values[1] is UMFamily)) return string.Empty;

//            //float qtty = (float)values[0];
//            UMFamily um = (UMFamily)values[1];

//            return string.Format("{0:0.##}{1}", values[0], um.UnitMeasures.Single(x => x.IsFamilyBase).Caption);
//        }

//        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
