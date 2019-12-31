using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.ViewModels;

using System.Collections.ObjectModel;

namespace MiPaladar.Services
{
    public class ProductManager
    {

        public ProductManager(MainWindowViewModel appvm)
        {
            foreach (var item in appvm.Context.Products)
            {
                //if (item.IsPurchasable) purchasableProducts.Add(item);
                if (!item.NotInMenu && item.UMFamily == appvm.UnitMeasureManager.Quantity) ventaItems.Add(item);
                if (item.IsStorable) inventoryProducts.Add(item);
                if (item.IsRecipe) recipeProducts.Add(item);
                //if (item.IsIngredient) ingredientProducts.Add(item);
                //item.PropertyChanged += new PropertyChangedEventHandler(product_PropertyChanged);
            }

            //appvm.ProductsOC.CollectionChanged += new NotifyCollectionChangedEventHandler(products_CollectionChanged);
        }

        //ObservableCollection<Product> purchasableProducts = new ObservableCollection<Product>();
        //public ObservableCollection<Product> PurchasableProducts
        //{
        //    get { return purchasableProducts; }
        //}

        ObservableCollection<Product> ventaItems = new ObservableCollection<Product>();
        public ObservableCollection<Product> VentaItems
        {
            get { return ventaItems; }
        }

        ObservableCollection<Product> inventoryProducts = new ObservableCollection<Product>();
        public ObservableCollection<Product> InventoryProducts
        {
            get { return inventoryProducts; }
        }

        ObservableCollection<Product> recipeProducts = new ObservableCollection<Product>();
        public ObservableCollection<Product> RecipeProducts
        {
            get { return recipeProducts; }
        }

        //ObservableCollection<Product> ingredientProducts = new ObservableCollection<Product>();
        //public ObservableCollection<Product> IngredientProducts
        //{
        //    get { return ingredientProducts; }
        //}

        public void ClearAll() 
        {
            //purchasableProducts.Clear();
            ventaItems.Clear();
            inventoryProducts.Clear();
            recipeProducts.Clear();
            //ingredientProducts.Clear();
        }

        public static decimal GetCurrentCost(Product product, double quantity, UnitMeasure um)
        {
            return (decimal)(quantity * um.ToBaseConversion) * product.CostPrice / (decimal)(product.CostQuantity * product.CostUnitMeasure.ToBaseConversion);
        }

        //void product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsPurchasable" && purchasableProducts != null) purchasableProducts.Refresh();
        //    else if (e.PropertyName == "NotInMenu" && ventaItems != null) ventaItems.Refresh();
        //}

        //void products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        Product item = (Product)e.NewItems[0];

        //        item.PropertyChanged += new PropertyChangedEventHandler(product_PropertyChanged);                
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        Product item = (Product)e.OldItems[0];

        //        item.PropertyChanged -= product_PropertyChanged;
        //    }
        //}
    }
}
