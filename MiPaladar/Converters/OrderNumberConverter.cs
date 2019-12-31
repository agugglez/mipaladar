using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Globalization;

using MiPaladar.ViewModels;
using MiPaladar.Entities;
using MiPaladar.Enums;

namespace MiPaladar.Converters
{
    [ValueConversion(typeof(Order),typeof(string))]
    public class OrderNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string stringParameter = (string)parameter;

            if (value is Sale) 
            {
                Sale s = (Sale)value;
                return "Vale " + s.Number;
            }
            else if (value is Purchase)
            {
                Purchase p = (Purchase)value;
                return "Compra>> ";
            }
            else if (value is Adjustment)
            {
                Adjustment p = (Adjustment)value;
                return "Ajuste>>";
            }
            else if (value is Transfer)
            {
                Transfer p = (Transfer)value;
                return "Transferencia>>";
            }
            else if (value is Production)
            {
                Production p = (Production)value;
                return "Producción>>";
            }
            //else if (value is Faena)
            //{
            //    Faena p = (Faena)value;
            //    return "Faena>>";
            //}

            return string.Empty;

            //OrderType orderType;
            //int number;
            //if (stringParameter == "order") 
            //{
            //    OrderReportViewModel order = (OrderReportViewModel)value;
            //    orderType = order.OrderType;
            //    number = order.Number;
            //}
            //else if (stringParameter == "followproduct") 
            //{
            //    FollowProductItemViewModel lineitem = (FollowProductItemViewModel)value;
            //    number = lineitem.Order.TheNumber;

            //    return lineitem.OperationType == InventoryOperationType.Salida ? "Vale " + number : "Compra " + number;
            //}
            //else
            //{
            //    OperationItemViewModel lineitem = (OperationItemViewModel)value;
            //    orderType = lineitem.OrderType;
            //    number = lineitem.Order.TheNumber;
            //    //if (lineitem.Order is Sale) number = ((Sale)lineitem.Order).Number;
            //    //else
            //    //{
            //    //    int? pNumber = ((Purchase)lineitem.Order).Number;
            //    //    number = pNumber.HasValue ? pNumber.Value : 0;
            //    //}
            //}            

            //return orderType == OrderType.Sale ? "Vale " + number : "Compra " + number;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
