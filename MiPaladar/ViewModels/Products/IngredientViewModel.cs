using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class IngredientViewModel : ProductQuantityViewModel
    {
        Action onCostChanged;
        bool designTime;

        ProductViewModel parentProduct;

        public IngredientViewModel() : base()
        {
            designTime = true;
        }

        public IngredientViewModel(ProductViewModel parentProduct, double quantity, UnitMeasure unitMeasure, Product ingredientProduct, Action onCostChanged)
            : base(ingredientProduct, quantity, unitMeasure)
        {
            //this.quantity = quantity;
            //this.unitMeasure = unitMeasure;
            //IngredientProduct = ingredientProduct;
            this.parentProduct = parentProduct;
            this.onCostChanged = onCostChanged;

            //UpdateCost();
            itemCost = ProductManager.GetCurrentCost(ingredientProduct, quantity, unitMeasure);
            //itemCost = Product.PurchasePrice * (decimal)(UnitMeasure.ToBaseConversion * Quantity);
        }

        decimal itemCost;
        public decimal ItemCost 
        {
            get { return itemCost; }
            set
            {
                itemCost = value;
                OnPropertyChanged("ItemCost");

                if (onCostChanged != null) onCostChanged();
            }
        }

        //float quantity;
        //public float Quantity         
        //{
        //    get { return quantity; }
        //    set 
        //    {
        //        quantity = value;
        //        if (onPropertyChanged != null) onPropertyChanged();
                
        //    }
        //}

        protected override void OnQuantityChanged()
        {
            if (designTime) return;

            UpdateCost();
        }

        protected override void OnUnitMeasureChanged()
        {
            if (designTime) return;

            UpdateCost();
        }

        void UpdateCost()
        {
            ItemCost = ProductManager.GetCurrentCost(Product, Quantity, UnitMeasure);
            //ItemCost = Product.PurchasePrice * (decimal)(UnitMeasure.ToBaseConversion * Quantity);

            if (parentProduct != null)
            {
                parentProduct.HasPendingChanges = true;
            }
        }

        //UnitMeasure unitMeasure;
        //public UnitMeasure UnitMeasure 
        //{
        //    get { return unitMeasure; }
        //    set
        //    {
        //        unitMeasure = value;
        //    }
        //}

        //protected override void OnUnitMeasureChanged() { }

        //public Product IngredientProduct { get; set; }

        //protected override void OnProductChanged() { }
    }
}