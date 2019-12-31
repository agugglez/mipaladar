using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class InventoryAdvancedSearchViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public InventoryAdvancedSearchViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "BUSQUEDA AVANZADA"; }
        }

        public bool PassesFilter(Product prod)
        {
            if (!ignoreInventory && prod.IsStorable != isInventoryItem) return false;            

            //if (!ignoreIngredient && prod.IsIngredient != isIngredient) return false;

            if (!ignoreSales && prod.NotInMenu != !inSales) return false;

            //if (!ignorePurchases && prod.IsPurchasable != inPurchases) return false;

            if (!ignoreRecipe && prod.IsRecipe != isRecipe) return false;

            if (isRecipe && selectedIngredient != null) 
            {
                //chech for selected ingredient
                bool contains = false;

                foreach (var item in prod.Ingredients)
                {
                    if (item.IngredientProduct == selectedIngredient)
                    { contains = true; break; }
                }

                if (!contains) return false;
            }

            if (minPrice != 0 && prod.SalePrice < minPrice) return false;

            if (maxPrice != 0 && prod.SalePrice > maxPrice) return false;

            if (selectedUMFamily != null && prod.UMFamily != selectedUMFamily) return false;

            if (selectedProductionArea != null && (prod.IsProduced != true ||
                prod.ProductionArea != selectedProductionArea)) return false;

            return true;
        }

        #region Inventory
        bool ignoreInventory = true;

        public bool IgnoreInventory
        {
            get { return ignoreInventory; }
            set 
            {
                ignoreInventory = value;
                OnPropertyChanged("IgnoreInventory");
            }
        }

        bool isInventoryItem;

        public bool IsInventoryItem
        {
            get { return isInventoryItem; }
            set 
            {
                isInventoryItem = value;
                OnPropertyChanged("IsInventoryItem");
            }
        }
        #endregion

        //#region Ingredient
        //bool ignoreIngredient = true;

        //public bool IgnoreIngredient
        //{
        //    get { return ignoreIngredient; }
        //    set
        //    {
        //        ignoreIngredient = value;
        //        OnPropertyChanged("IgnoreIngredient");
        //    }
        //}

        //bool isIngredient;

        //public bool IsIngredient
        //{
        //    get { return isIngredient; }
        //    set
        //    {
        //        isIngredient = value;
        //        OnPropertyChanged("IsIngredient");
        //    }
        //}
        //#endregion

        #region Sales
        bool ignoreSales = true;

        public bool IgnoreSales
        {
            get { return ignoreSales; }
            set
            {
                ignoreSales = value;
                OnPropertyChanged("IgnoreSales");
            }
        }

        bool inSales;

        public bool InSales
        {
            get { return inSales; }
            set
            {
                inSales = value;
                OnPropertyChanged("InSales");
            }
        }
        #endregion

        //#region Purchases
        //bool ignorePurchases = true;

        //public bool IgnorePurchases
        //{
        //    get { return ignorePurchases; }
        //    set
        //    {
        //        ignorePurchases = value;
        //        OnPropertyChanged("IgnorePurchases");
        //    }
        //}

        //bool inPurchases;

        //public bool InPurchases
        //{
        //    get { return inPurchases; }
        //    set
        //    {
        //        inPurchases = value;
        //        OnPropertyChanged("InPurchases");
        //    }
        //}
        //#endregion

        #region Recipe
        bool ignoreRecipe = true;

        public bool IgnoreRecipe
        {
            get { return ignoreRecipe; }
            set
            {
                ignoreRecipe = value;
                OnPropertyChanged("IgnoreRecipe");
            }
        }

        bool isRecipe;

        public bool IsRecipe
        {
            get { return isRecipe; }
            set
            {
                isRecipe = value;
                OnPropertyChanged("IsRecipe");
            }
        }
        #endregion

        #region Contains Ingredient

        Product selectedIngredient;
        public Product SelectedIngredient 
        {
            get { return selectedIngredient; }
            set
            {
                selectedIngredient = value;
                OnPropertyChanged("SelectedIngredient");
            }
        }

        public ObservableCollection<Product> Ingredients 
        {
            get { return appvm.ProductsOC; }
        }

        #endregion

        #region Price

        decimal minPrice;

        public decimal MinPrice
        {
            get { return minPrice; }
            set 
            {
                minPrice = value;
                OnPropertyChanged("MinPrice");
            }
        }

        decimal maxPrice;

        public decimal MaxPrice
        {
            get { return maxPrice; }
            set 
            {
                maxPrice = value;
                OnPropertyChanged("MaxPrice");
            }
        }

        #endregion

        #region UnitMeasure Family

        UMFamily selectedUMFamily;
        public UMFamily SelectedUMFamily 
        {
            get { return selectedUMFamily; }
            set
            {
                selectedUMFamily = value;
                OnPropertyChanged("SelectedUMFamily");
            }
        }

        public ObservableCollection<UMFamily> UMFamilies 
        {
            get { return new ObservableCollection<UMFamily>(appvm.Context.UMFamilies); }
        }

        #endregion

        #region Production Area

        ProductionArea selectedProductionArea;
        public ProductionArea SelectedProductionArea 
        {
            get { return selectedProductionArea; }
            set
            {
                selectedProductionArea = value;
                OnPropertyChanged("SelectedProductionArea");
            }
        }

        public ObservableCollection<ProductionArea> ProductionAreas
        {
            get { return appvm.ProductionAreasOC; }
        }

        #endregion

        //#region Reset Command

        //RelayCommand resetCommand;
        //public ICommand ResetCommand
        //{
        //    get 
        //    {
        //        if (resetCommand == null)
        //            resetCommand = new RelayCommand(x => this.Reset());
        //        return resetCommand; 
        //    }
        //}

        //void Reset() 
        //{
        //    IgnoreInventory = true;
        //    IgnoreIngredient = true;
        //    IgnoreSales = true;
        //    IgnorePurchases = true;
        //    IgnoreRecipe = true;

        //    SelectedIngredient = null;

        //    MinPrice = 0; MaxPrice = 0;

        //    SelectedUMFamily = null;

        //    SelectedProductionArea = null;
        //}

        //#endregion
    }
}