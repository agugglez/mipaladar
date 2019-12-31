using System;
using System.Collections.Generic;
using System.Linq;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public class ProductRowViewModel : ViewModelBase
    {
        //List<InventoryItem> inventoryItems;
        MainWindowViewModel appvm;

        //Action onCostChanged;

        #region Constructors

        //public ProductRowViewModel(MainWindowViewModel appvm, Product prod)
        //{
        //    this.appvm = appvm;
        //    //inventoryItems = new List<InventoryItem>();
        //    //this.onCostChanged = onCostChanged;            

        //    this.product = prod;

        //    //if (product.IsStorable) 
        //    //{
        //    //    quantity = 0;
        //    //    um = prod.CostUnitMeasure;
        //    //}

        //    product.PropertyChanged += new PropertyChangedEventHandler(product_PropertyChanged);
        //}        

        public ProductRowViewModel(MainWindowViewModel appvm, Product prod, List<InventoryItem> inventoryItems=null)
        {
            this.appvm = appvm;
            this.product = prod;
            //this.inventoryItems = inventoryItems;
            //this.onCostChanged = onCostChanged;            
            this.um = product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

            product.PropertyChanged += new PropertyChangedEventHandler(product_PropertyChanged);

            inventoryItemsDict = new PassthruDictionary<int, InventoryItemViewModel>();

            if (inventoryItems != null) 
            {
                //UpdateQuantity();

                //minimum_quantity = inventoryItems.Sum(x => x.MinimumQuantity);
                //total_cost = inventoryItems.Sum(x => x.Cost);  

                foreach (var item in inventoryItems)
                {
                    inventoryItemsDict.Add(item.InventoryId, new InventoryItemViewModel(appvm, item, OnInventoryItemQuantityChanged));
                    //item.PropertyChanged += new PropertyChangedEventHandler(inventoryItem_PropertyChanged);
                }

                UpdateRedNumbers();
            }            
        }

        #endregion        

        #region Events

        protected override void OnDispose()
        {
            product.PropertyChanged -= product_PropertyChanged;

            foreach (var item in inventoryItemsDict.Values)
            {
                //item.PropertyChanged -= inventoryItem_PropertyChanged;
                item.Dispose();
            }
        }

        void product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsStorable")
            {
                //remove inventory items when when a product is unmarked as "storable"
                if (!product.IsStorable)
                {
                    foreach (var item in inventoryItemsDict.Values)
                    {
                        //item.PropertyChanged -= inventoryItem_PropertyChanged;
                        item.Dispose();
                    }

                    inventoryItemsDict.Clear();

                    UpdateQuantity();

                    //Quantity = null;
                    //UnitMeasure = null;
                    //TotalCost = null;
                }
                else if (product.IsStorable)
                {
                    UpdateQuantity();
                    //UnitMeasure = product.CostUnitMeasure;
                    //UpdateCost();
                }
            }
            else if (e.PropertyName == "MinimumStock")
            {
                UpdateRedNumbers();
            }
        }

        void OnInventoryItemQuantityChanged()
        {
            UpdateRedNumbers();
        }

        void UpdateRedNumbers()
        {
            double total_qtty = 0;

            foreach (var item in inventoryItemsDict.Values)
            {
                total_qtty += item.Quantity * item.UnitMeasure.ToBaseConversion;
            }

            RedNumbers = total_qtty < product.MinimumStock * product.MinimumStockUM.ToBaseConversion;
        }

        //void inventoryItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Quantity")
        //    {
        //        UpdateQuantity();
        //    }
        //    //else if (e.PropertyName == "MinimumQuantity")
        //    //{
        //    //    MinimumQuantity = inventoryItems.Sum(x => x.MinimumQuantity);
        //    //}
        //    //else if (e.PropertyName == "Cost")
        //    //{
        //    //    UpdateCost();
        //    //    //TotalCost = inventoryItems.Sum(x => x.Cost);
        //    //}
        //}

        #endregion

        void UpdateQuantity()
        {
            OnPropertyChanged("InventoryItems");
            //double temp_qtty = inventoryItems.Sum(x => x.Quantity);

            //express quantity in cost UM
            //Quantity = temp_qtty / product.CostUnitMeasure.ToBaseConversion;
        }

        //void UpdateCost() 
        //{
        //    TotalCost = inventoryItems.Sum(x => x.Cost);
        //}

        public void AddNewItem(InventoryItem newItem)
        {
            //check product
            if (newItem.Product != product) throw new ArgumentException("the product is not right");

            //check inventory area
            foreach (var item in inventoryItemsDict)
            {
                if (item.Key == newItem.InventoryId) throw new ArgumentException("the inventory area already exists");
            }

            inventoryItemsDict.Add(newItem.InventoryId, new InventoryItemViewModel(appvm, newItem, OnInventoryItemQuantityChanged));

            UpdateQuantity();
            //UpdateCost();
            //MinimumQuantity += newItem.MinimumQuantity;

            //newItem.PropertyChanged += new PropertyChangedEventHandler(inventoryItem_PropertyChanged);
        }

        public void RemoveItem(InventoryItem oldItem) 
        {
            if (oldItem.Product != product) throw new ArgumentException("the product is not right");

            //oldItem.PropertyChanged -= inventoryItem_PropertyChanged;

            //Quantity -= oldItem.Quantity;
            //TotalCost -= oldItem.Cost;
            //MinimumQuantity -= oldItem.MinimumQuantity;

            inventoryItemsDict[oldItem.InventoryId].Dispose();

            inventoryItemsDict.Remove(oldItem.InventoryId);

            UpdateQuantity();
            //UpdateCost();
        }

        Product product;
        public Product Product
        {
            get { return product; }
        }

        Category category;
        bool mainCategoryFound;
        public Category Category
        {
            get
            {
                if (!mainCategoryFound)
                {
                    foreach (var item in product.RelatedCategories)
                    {
                        if (item.IsMain) category = item.Category;
                    }

                    mainCategoryFound = true;
                }
                return category;
            }
        }

        PassthruDictionary<int, InventoryItemViewModel> inventoryItemsDict;

        public PassthruDictionary<int, InventoryItemViewModel> InventoryItems
        {
            get { return inventoryItemsDict; }
        }

        //double? quantity;
        //public double? Quantity
        //{
        //    get { return quantity; }
        //    set
        //    {
        //        if (quantity != value) 
        //        {
        //            quantity = value;

        //            OnPropertyChanged("Quantity");
        //            //OnPropertyChanged("Difference");
        //            //OnPropertyChanged("RedNumbers");
        //        }
        //    }
        //}

        UnitMeasure um;
        public UnitMeasure UnitMeasure
        {
            get
            {
                return um;
                //return product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            }
            set
            {
                if (um != value)
                {
                    um = value;
                    OnPropertyChanged("UnitMeasure");
                }
            }
        }

        //decimal? total_cost;
        //public decimal? TotalCost 
        //{
        //    get { return total_cost; }
        //    set
        //    {
        //        if (total_cost != value) 
        //        {
        //            total_cost = value;                    
        //            OnPropertyChanged("TotalCost");

        //            if (onCostChanged != null) onCostChanged();
        //        }
        //    }
        //}

        //Category category;
        //bool mainCategoryFound;
        //public Category Category
        //{
        //    get
        //    {
        //        if (!mainCategoryFound)
        //        {
        //            foreach (var item in product.RelatedCategories)
        //            {
        //                if (item.IsMain) category = item.Category;
        //            }

        //            mainCategoryFound = true;
        //        }
        //        return category;
        //    }
        //}

        //double minimum_quantity;
        //public double MinimumQuantity
        //{
        //    get { return minimum_quantity; }
        //    set
        //    {
        //        minimum_quantity = value;

        //        OnPropertyChanged("MinimumQuantity");
        //        OnPropertyChanged("Difference");
        //        OnPropertyChanged("RedNumbers");
        //    }
        //}

        //public double Difference
        //{
        //    get { return Quantity - MinimumQuantity; }
        //}

        bool redNumbers;
        public bool RedNumbers
        {
            get { return redNumbers; }
            set
            {
                redNumbers = value;
                OnPropertyChanged("RedNumbers");
            }
        }
    }

    public class PassthruDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        // ... other stuff

        public new TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }
                else
                {
                    return default(TValue);
                }
            }
        }
    }
}
