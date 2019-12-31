using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class ProductIndexViewModel : ViewModelBase
    {
        ProductViewModel parentProduct;

        public ProductIndexViewModel() { }

        public ProductIndexViewModel(ProductViewModel parentProduct) 
        {
            this.parentProduct = parentProduct;
        }
        bool ismain;
        public bool IsMain 
        {
            get { return ismain; }
            set
            {
                ismain = value;
                OnPropertyChanged("IsMain");

                if (parentProduct != null) parentProduct.HasPendingChanges = true;
            }
        }

        public Category Category { get; set; }
    }
}
