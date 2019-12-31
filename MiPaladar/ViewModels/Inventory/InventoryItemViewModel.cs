using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class InventoryItemViewModel : ViewModelBase
    {
        InventoryItem ce;

        Action onQuantityChanged;

        //MainWindowViewModel appvm;
        public InventoryItemViewModel(MainWindowViewModel appvm, InventoryItem ce, Action onQuantityChanged) 
        {
            //this.appvm = appvm;
            this.ce = ce;
            this.onQuantityChanged = onQuantityChanged;

            //this.um = ce.Product.CostUnitMeasure;

            Quantity = ce.Quantity / UnitMeasure.ToBaseConversion;

            ce.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ce_PropertyChanged);
        }

        protected override void OnDispose()
        {
            ce.PropertyChanged -= ce_PropertyChanged;
        }

        //public void UnsubscribeEvents() 
        //{
        //    ce.PropertyChanged -= ce_PropertyChanged;
        //}

        void ce_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                Quantity = ce.Quantity / UnitMeasure.ToBaseConversion;

                if (onQuantityChanged != null) onQuantityChanged();

                //OnPropertyChanged("Difference");
                //OnPropertyChanged("RedNumbers");
            }
            else if (e.PropertyName == "Cost") 
            {
                OnPropertyChanged("Cost");
            }
        }

        //int categoryIndex;
        //public int CategoryIndex 
        //{
        //    get { return categoryIndex; }
        //    set
        //    {
        //        categoryIndex = value;
        //        OnPropertyChanged("CategoryIndex");
        //    }
        //}

        public InventoryItem InventoryItem 
        {
            get { return ce; }
        }

        //public Inventory InventoryArea 
        //{
        //    get { return ce.Inventory; }
        //}

        public Product Product 
        {
            get { return ce.Product; }
        }

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
                }
            }
            //set 
            //{
            //    ce.Quantity = value;
            //    appvm.SaveChanges();

            //    OnPropertyChanged("Difference");
            //    OnPropertyChanged("RedNumbers");
            //}
        }

        //UnitMeasure um;
        public UnitMeasure UnitMeasure 
        {
            get { return Product.CostUnitMeasure; }                                     
            //return ce.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            //set
            //{
            //    um = value;
            //    OnPropertyChanged("UnitMeasure");
            //}
        }

        //Category category;
        //bool mainCategoryFound;
        //public Category Category
        //{
        //    get
        //    {
        //        if (!mainCategoryFound)
        //        {
        //            foreach (var item in ce.Product.RelatedCategories)
        //            {
        //                if (item.IsMain) category = item.Category;
        //            }

        //            mainCategoryFound = true;
        //        }
        //        return category;
        //    }
        //}

        public decimal Cost
        {
            get { return ce.Cost; }
        }
    }
}
