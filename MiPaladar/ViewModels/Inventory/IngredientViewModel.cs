using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.MVVM;


namespace MiPaladar.ViewModels
{
    public class IngredientViewModel : ViewModelBase  //ProductQuantityViewModel
    {
        Action onModified;
        bool designTime;

        //ProductViewModel parentProduct;

        public IngredientViewModel()// : base()
        {
            designTime = true;
        }

        public IngredientViewModel(double qtty, UnitMeasure um, Product prod, Action onModified)
        {
            this.quantity = qtty;
            this.umId = um.Id;
            this.productId = prod.Id;
            this.productName = prod.Name;

            this.onModified = onModified;

            UpdateAvailableUMs();

            UpdateCost();            
        }

        public IngredientViewModel(Ingredient original, Action onModified) :
            this(original.Quantity, original.UnitMeasure, original.IngredientProduct, onModified)
        // : base(ingredientProduct, quantity, unitMeasure)
        {
            //this.quantity = quantity;
            //this.unitMeasure = unitMeasure;
            //IngredientProduct = ingredientProduct;
            //this.parentProduct = parentProduct;
            Id = original.Id;
            //itemCost = ProductManager.GetCurrentCost(original.IngredientProduct, original.Quantity, original.UnitMeasure);
            //itemCost = Product.PurchasePrice * (decimal)(UnitMeasure.ToBaseConversion * Quantity);
        }

        public int Id { get; set; }

        double quantity;
        public double Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value && value > 0)
                {
                    quantity = value;

                    UpdateCost();
                    if (onModified != null) onModified();

                    OnPropertyChanged("Quantity");
                }
            }
        }

        List<UnitMeasure> availableUMs;
        public List<UnitMeasure> AvailableUMs
        {
            get { return availableUMs; }
        }

        int umId;
        public int UnitMeasureId
        {
            get { return umId; }
            set
            {
                if (umId != value)
                {
                    umId = value;

                    UpdateCost();
                    if (onModified != null) onModified();

                    OnPropertyChanged("UnitMeasureId");
                }
            }
        }

        int productId;

        public int ProductId
        {
            get { return productId; }            
        }
                
        string productName;
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        decimal itemCost;
        public decimal ItemCost
        {
            get { return itemCost; }
            set
            {
                itemCost = value;

                if (onModified != null) onModified();

                OnPropertyChanged("ItemCost");
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

        //protected override void OnQuantityChanged()
        //{
        //    if (designTime) return;

        //    UpdateCost();
        //}

        //protected override void OnUnitMeasureChanged()
        //{
        //    if (designTime) return;

        //    UpdateCost();
        //}

        void UpdateCost()
        {
            if (designTime) return;

            var invSVC = base.GetService<IInventoryService>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Product ingredientProduct = unitOfWork.ProductRepository.GetById(productId);
                ItemCost = invSVC.GetProductCost(ingredientProduct, quantity, availableUMs.Single(x => x.Id == umId));
            }            
            //ItemCost = Product.PurchasePrice * (decimal)(UnitMeasure.ToBaseConversion * Quantity);

            //if (parentProduct != null)
            //{
            //    parentProduct.HasPendingChanges = true;
            //}
        }

        private void UpdateAvailableUMs()
        {
            availableUMs = new List<UnitMeasure>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var allUMs = unitOfWork.UMRepository.Get();

                Product ingredientProduct = unitOfWork.ProductRepository.GetById(productId);

                foreach (var item in allUMs)
                {
                    if (item.UMFamilyId == ingredientProduct.UMFamilyId) availableUMs.Add(item);
                }
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