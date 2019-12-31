using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Data;

using MiPaladar.Classes;

//namespace MiPaladar.Validation
//{
//    class ProductQuantityItemValidationRule : ValidationRule
//    {
//        public override ValidationResult Validate(object value,
//          System.Globalization.CultureInfo cultureInfo)
//        {
//            ProductQuantityViewModel pqItem = (value as BindingGroup).Items[0] as ProductQuantityViewModel;

//            //if (ing.UnitMeasure == null) return new ValidationResult(false, "El especifique una unidad de medida");

//            if (pqItem.HasErrors) return new ValidationResult(false, pqItem.ErrorMessage);

//            //if (ing.UnitMeasure == null)
//            //    return new ValidationResult(false, "La unidad de medida no existe");

//            //if (ing.UnitMeasure.UMFamily != ing.Product.UMFamily)
//            //{
//            //    return new ValidationResult(false, "El producto y la unidad de medida no coinciden");
//            //}

//            return ValidationResult.ValidResult;
//        }
//    }
//}
