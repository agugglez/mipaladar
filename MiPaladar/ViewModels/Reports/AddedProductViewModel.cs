using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class AddedProductViewModel
    {
        Action onCheckedChanged;
        public AddedProductViewModel(Product p, Action onCheckedChanged) 
        {
            Product = p;
            isChecked = true;
            this.onCheckedChanged = onCheckedChanged;
        }
        public Product Product { get; set; }

        bool isChecked;
        public bool IsChecked 
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value; 
                    onCheckedChanged();
                }
            }
        }
    }
}
