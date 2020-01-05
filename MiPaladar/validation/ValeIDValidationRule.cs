using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using MiPaladar.Classes;
using MiPaladar.Entities;

namespace MiPaladar.Validation
{
    class ValeIDValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //App app = Application.Current as App;

            //CollectionView collection = (CollectionView)CollectionViewSource.GetDefaultView(App.Ctx.Vales);

            Sale currentVale = (Sale)value;

            int valeID = 0;

            try
            {
                if (((string)value).Length > 0)
                    valeID = Int32.Parse((String)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "entre un numero");
            }

            //if the ID doesnt change, then it's ok
            if (valeID == currentVale.Number) return new ValidationResult(true, null);

            //now look the rest of vales to see if the ID is in use
            //foreach (SalesOrder vale in App.Ctx.Vales)
            //{
            //    if (vale.Number == valeID)
            //        return new ValidationResult(false, "Ya hay otro vale con este número");
            //}

            //if the id isnt used delete previous file and return true
            //Accounter.DeleteValeFile(currentVale);

            return ValidationResult.ValidResult;
        }
    }
    //class ValeListaPrecioVR : ValidationRule 
    //{
    //    public override ValidationResult Validate(object value, CultureInfo cultureInfo) 
    //    {
    //        CollectionView collection = (CollectionView)CollectionViewSource.GetDefaultView(App.ObjectContext.Vales);

    //        Order currentVale = collection.CurrentItem as Order;

    //        //look if after changing price list, there is another vale with that ID

    //        ListaPrecio lp = (ListaPrecio)value;

    //        foreach (Order v3 in App.ObjectContext.Vales)
    //        {
    //            if (v3 == currentVale) continue;
    //            if (v3.ListaPrecio == lp) 
    //            {
    //                if (v3.ID == currentVale.ID) 
    //                    return new ValidationResult(false, "ya existe un vale con este ID");
    //            }
    //        }

    //        return new ValidationResult(true, null);
    //    }
    //}

    class ValeItemProductValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Product product = (Product)value;

            if (product == null) return new ValidationResult(false, "seleccione un producto");

            return new ValidationResult(true, null);
        }
    }
}