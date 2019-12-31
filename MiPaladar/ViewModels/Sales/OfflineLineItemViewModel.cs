using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;

using System.Windows.Input;
using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class OfflineLineItemViewModel : ProductQuantityViewModel
    {
        //MainWindowViewModel appvm;

        Action onQuantityChanged;
        Action onPriceChanged;

        //public LineItemViewModel() { }

        public OfflineLineItemViewModel(Product prod, double qtty, UnitMeasure um, decimal amount,
            Action onQuantityChanged, Action onPriceChanged)
            : base(prod, qtty, um)
        {
            //this.appvm = appvm;
            this.amount = amount;

            this.onQuantityChanged = onQuantityChanged;
            this.onPriceChanged = onPriceChanged;
        }

        public OfflineLineItemViewModel(SaleLineItem li, Action onQuantityChanged, Action onPriceChanged)
            : base(li.Product, li.Quantity, li.UnitMeasure)
        {
            this.sli = li;

            this.amount = li.Amount;

            this.onQuantityChanged = onQuantityChanged;
            this.onPriceChanged = onPriceChanged;
        }

        protected override void OnQuantityChanged()
        {
            if (onQuantityChanged != null) onQuantityChanged();

            //this saves changes
            Price = (decimal)Quantity * Product.SalePrice;

            if (onPriceChanged != null) onPriceChanged();
        }

        bool printed;
        public bool Printed
        {
            get { return printed; }
            set 
            {
                printed = value;
                OnPropertyChanged("Printed");
            }
        }

        decimal amount;
        public decimal Price
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged("Price");
            }
        }

        SaleLineItem sli;
        public SaleLineItem WrappedLineItem 
        {
            get { return sli; }
            set { sli = value; }
        }
    }
}
