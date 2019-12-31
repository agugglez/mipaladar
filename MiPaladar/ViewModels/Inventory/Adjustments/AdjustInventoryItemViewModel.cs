using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;

namespace MiPaladar.ViewModels
{
    public class AdjustInventoryItemViewModel : ViewModelBase
    {
        InventoryItem ce;
        MainWindowViewModel appvm;

        public AdjustInventoryItemViewModel(MainWindowViewModel appvm, InventoryItem ce) 
        {
            this.appvm = appvm;
            this.ce = ce;

            quantity = ce.Quantity;
            unitMeasure = ce.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

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
                OnPropertyChanged("Quantity");
            }
        }

        public InventoryItem WrappedInventoryItem 
        {
            get { return ce; }
        }

        public Inventory Inventory 
        {
            get { return ce.Inventory; }
        }

        public Product Product 
        {
            get { return ce.Product; }
        }        

        Category category;
        bool mainCategoryFound;
        public Category Category
        {
            get
            {
                if (!mainCategoryFound)
                {
                    foreach (var item in ce.Product.RelatedCategories)
                    {
                        if (item.IsMain) category = item.Category;
                    }

                    mainCategoryFound = true;
                }
                return category;
            }
        }

        double quantity;
        public double Quantity 
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        UnitMeasure unitMeasure;
        public UnitMeasure UnitMeasure
        {
            get { return unitMeasure; }
            set 
            {
                if (unitMeasure != value) 
                {
                    double rate = unitMeasure.ToBaseConversion / value.ToBaseConversion;
                    unitMeasure = value;

                    OnPropertyChanged("UnitMeasure");

                    quantity *= rate;
                    OnPropertyChanged("Quantity");
                    difference *= rate;
                    OnPropertyChanged("Difference");
                    newQuantity *= rate;
                    OnPropertyChanged("NewQuantity");
                }                
            }
        }

        double newQuantity;
        public double NewQuantity 
        {
            get
            {
                //if (newQuantity == null)                 
                //{
                //    newQuantity = new ProductQuantityViewModel(appvm, Product);
                //}
                return newQuantity; 
            }
            set
            {
                newQuantity = value;

                //if (!newQuantity.HasErrors) 
                {
                    UpdateDifference();
                }
            }
        }

        //public float NewQuantityToFloat 
        //{
        //    get 
        //    {
        //        if (newQuantity == null)                 
        //        {
        //            newQuantity = new ProductQuantityViewModel(appvm, Product);
        //        }
        //        return newQuantity.Quantity; 
        //    }
        //}

        void UpdateDifference()
        {
            difference = newQuantity - Quantity;
            OnPropertyChanged("Difference");
        }

        double difference;
        public double Difference
        {
            get 
            {
                //if (difference == null) 
                //{
                //    difference = new ProductQuantityViewModel(appvm, Product);
                //}
                return difference;
            }
            set
            {
                difference = value;

                //if (!difference.HasErrors) 
                {
                    UpdateNewQuantity();
                }
            }
        }

        //public float DifferenceToFloat 
        //{
        //    get 
        //    {
        //        if (difference == null)
        //        {
        //            difference = new ProductQuantityViewModel(appvm, Product);
        //        }
        //        return difference.Quantity;
        //    }
        //}

        void UpdateNewQuantity()
        {
            newQuantity = Quantity + difference;
            OnPropertyChanged("NewQuantity");
        }
    }
}
