using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Data;
using MiPaladar.ViewModels;

namespace MiPaladar.Validation
{
    class ProductValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
          System.Globalization.CultureInfo cultureInfo)
        {
            ProductViewModel prod = (value as BindingGroup).Items[0] as ProductViewModel;

            //if (prod.MainCategory == null) return new ValidationResult(false,
            //        "Tienes que especificar una categoría");

            if (prod.Name == null) return new ValidationResult(false,
                      "Tienes que especificar un nombre");

            return ValidationResult.ValidResult;
        }
    }
}
