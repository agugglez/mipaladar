using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;
using MiPaladar.MVVM;

using System.Windows.Input;
using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class OfflineLineItemViewModel : ViewModelBase
    {
        //MainWindowViewModel appvm;

        Action onQuantityChanged;
        Action onPriceChanged;

        //public LineItemViewModel() { }

        public OfflineLineItemViewModel(Product prod, double qtty, decimal amount, decimal cost,
            Action onQuantityChanged, Action onPriceChanged)
        {
            //this.appvm = appvm;
            this.quantity = qtty;
            this.product = prod;
            this.amount = amount;
            this.cost = cost;

            this.onQuantityChanged = onQuantityChanged;
            this.onPriceChanged = onPriceChanged;
        }

        public OfflineLineItemViewModel(SaleLineItem li, Action onQuantityChanged, Action onPriceChanged) :
            this(li.Product, li.Quantity, li.Amount, li.Cost, onQuantityChanged, onPriceChanged)
        {
            this.liId = li.Id;
        }

        protected void OnQuantityChanged()
        {
            if (onQuantityChanged != null) onQuantityChanged();

            //this saves changes
            Price = (decimal)Quantity * Product.SalePrice;

            var invSVC = base.GetService<IInventoryService>();
            Cost = invSVC.GetProductCost(Product, Quantity, null);

            if (onPriceChanged != null) onPriceChanged();
        }

        //bool printed;
        //public bool Printed
        //{
        //    get { return printed; }
        //    set 
        //    {
        //        printed = value;
        //        OnPropertyChanged("Printed");
        //    }
        //}

        double quantity;
        public double Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged("Quantity");
                    OnQuantityChanged();
                }
            }
        }

        Product product;
        public Product Product
        {
            get { return product; }
            set
            {
                if (product != value)
                {
                    //OnProductChanging();
                    product = value;
                    OnPropertyChanged("Product");
                    //if (product != null) umFamily = product.UMFamily;
                    //ParseQuantityExpression(quantityExpression);
                    //OnProductChanged();
                }
            }
        }

        public int ProductId
        {
            get { return product.Id; }
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

        decimal cost;
        public decimal Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        int liId;
        public int Id
        {
            get { return liId; }
        }
    }
}
