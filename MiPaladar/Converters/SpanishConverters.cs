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

    [ValueConversion(typeof(ProductType), typeof(string))]
    public class ProductTypeInSpanishConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ProductType pType = (ProductType)value;

            string inSpanish;

            switch (pType)
            {
                case ProductType.FinishedGoods:
                    inSpanish= "Venta";
                    break;
                case ProductType.WorkInProcess:
                    inSpanish= "Elaboración";
                    break;
                case ProductType.RawMaterials:
                    inSpanish= "Primario";
                    break;
                case ProductType.CompraVenta:
                    inSpanish= "Compra-Venta";
                    break;
                default:
                    inSpanish=string.Empty;
                    break;
            }

            return inSpanish;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
