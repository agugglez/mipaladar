using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class SoldProductsByEmployeeItemViewModel : ViewModelBase
    {
        public SoldProductsByEmployeeItemViewModel() { }

        public SoldProductsByEmployeeItemViewModel(SaleLineItem li)
        {
            Product = li.Product;            
        }

        public Product Product { get; set; }

        Category category;
        public Category Category
        {
            get
            {
                if (category == null && Product != null)
                {
                    foreach (var item in Product.RelatedCategories)
                        if (item.IsMain)
                        {
                            category = item.Category;
                            break;
                        }
                }
                
                return category;
            }
            set 
            {
                category = value;
            }
        }

        PassthruDictionary<int, ProductQuantityViewModel> quantityItems = 
            new PassthruDictionary<int, ProductQuantityViewModel>();

        public PassthruDictionary<int, ProductQuantityViewModel> QuantityItems
        {
            get { return quantityItems; }
        }
    }
}