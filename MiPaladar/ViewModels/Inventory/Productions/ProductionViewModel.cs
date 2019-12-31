using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace MiPaladar.ViewModels
{
    public class ProductionViewModel : ViewModelBase, IScreen
    {
        MainWindowViewModel appvm;
        Production production;

        Action<Production> onCreated;
        Action<Production> onRemoved;
        Action<Production> onAssociationChanged;
        
        bool creating;
        //creating a new one
        public ProductionViewModel(MainWindowViewModel appvm, Action<Production> onCreated,
            Action<Production> onRemoved, Action<Production> onAssociationChanged) 
        {
            this.appvm = appvm;
            this.onCreated = onCreated;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = DateTime.Now;
            date = DateTime.Today;
            //producedItem = new ProductQuantityViewModel(appvm);

            creating = true;
            HasPendingChanges = true;
        }

        public ProductionViewModel(MainWindowViewModel appvm, Production production,
            Action<Production> onRemoved, Action<Production> onAssociationChanged)
        {
            this.appvm = appvm;
            this.production = production;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = production.DateCreated;

            CopyFrom();

            //SearchText = production.ProducedProduct.Name;

            HasPendingChanges = false;
        }

        #region Copy Methods

        void CopyFrom() 
        {
            Date = production.Date;
            Responsible = production.Employee;
            Inventory = production.Inventory;
            //DestinationInventory = production.DestinationInventory;
            Memo = production.Memo;

            //QuantityToProduce = production.ProducedQuantity;
            //UnitMeasureToProduce = production.ProducedUnitMeasure;
            //ProductToProduce = production.ProducedProduct;

            //ProducedItem = new ProductQuantityViewModel(appvm, production.ProducedProduct, 
            //    production.ProducedQuantity, production.ProducedUnitMeasure);

            productionItems.Clear();
            foreach (ProductionItem item in production.LineItems)
            {
                ProductionItemViewModel pivm = new ProductionItemViewModel(this, item);
                productionItems.Add(pivm);
            }
        }

        void CopyTo()
        {
            var ts = base.GetService<ITransactionService>();
            var inventory_svc = appvm.InventoryService;

            if (!creating) 
            {
                //remove original items
                List<ProductionItem> toRemove = new List<ProductionItem>();
                foreach (ProductionItem item in production.LineItems)
                {
                    toRemove.Add(item);
                }
                foreach (ProductionItem item in toRemove)
                {
                    ts.UndoProduce(item.Product, item.Quantity, item.UnitMeasure, item.Cost, production.Date, production.Inventory);

                    appvm.Context.LineItems.DeleteObject(item);
                    //RemoveItem(item);
                }
                //remove old produced item from inventory
                //inventory_svc.ExecuteInventoryOperation(production.Date, production.DestinationInventory,
                //    productToProduce, -quantityToProduce, unitMeasureToProduce);
            }

            //the properties
            if (date != production.Date) production.Date = date;
            if (responsible != production.Employee) production.Employee = responsible;
            if (inventory != production.Inventory) production.Inventory = inventory;
            //if (destinationInventory != production.DestinationInventory) production.DestinationInventory = destinationInventory;
            if (memo != production.Memo) production.Memo = memo;

            //replace new values
            //if (production.ProducedQuantity != quantityToProduce) production.ProducedQuantity = quantityToProduce;
            //if (production.ProducedUnitMeasure != unitMeasureToProduce) production.ProducedUnitMeasure = unitMeasureToProduce;
            //if (production.ProducedProduct != productToProduce) production.ProducedProduct = productToProduce;
            
            //add current items                
            foreach (var item in productionItems)
            {
                ProductionItem newPi = new ProductionItem();
                newPi.Quantity = item.Quantity;
                newPi.UnitMeasure = item.UnitMeasure;
                newPi.Product = item.Product;

                production.LineItems.Add(newPi);

                decimal cost = ts.Produce(item.Product, item.Quantity, item.UnitMeasure, date, inventory);

                newPi.Cost = cost;
                
                //ExecuteProduceOperation(item.Product, item.Quantity, item.UnitMeasure);
            }

            //add new produced item to inventory
            //inventory_svc.ExecuteInventoryOperation(Date, destinationInventory,
            //    productToProduce, quantityToProduce, unitMeasureToProduce);

            appvm.SaveChanges();
        }

        #endregion

        public Production WrappedProduction
        {
            get { return production; }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return appvm.EmployeesOC; }
        }

        public ObservableCollection<Inventory> Inventories
        {
            get { return appvm.InventoryAreasOC; }
        }

        public ObservableCollection<Product> InventoryProducts 
        {
            get { return appvm.ProductManager.InventoryProducts; }
        }

        #region Header

        DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
                HasPendingChanges = true;
            }
        }

        DateTime dateCreated;
        public DateTime DateCreated 
        {
            get { return dateCreated; }
        }

        Employee responsible;
        public Employee Responsible
        {
            get { return responsible; }
            set
            {
                responsible = value;
                OnPropertyChanged("Responsible");
                HasPendingChanges = true;
            }
        }

        string memo;
        public string Memo
        {
            get { return memo; }
            set
            {
                memo = value;
                OnPropertyChanged("Memo");
                HasPendingChanges = true;
            }
        }

        Inventory inventory;
        public Inventory Inventory
        {
            get { return inventory; }
            set
            {
                inventory = value;
                OnPropertyChanged("Inventory");
                HasPendingChanges = true;
            }
        }

        //Inventory destinationInventory;
        //public Inventory DestinationInventory
        //{
        //    get { return destinationInventory; }
        //    set
        //    {
        //        destinationInventory = value;
        //        OnPropertyChanged("DestinationInventory");
        //        HasPendingChanges = true;
        //    }
        //}
        //ProductQuantityViewModel producedItem;
        //public ProductQuantityViewModel ProducedItem 
        //{
        //    get { return producedItem; }
        //    set 
        //    {
        //        producedItem = value;
        //        OnPropertyChanged("ProducedItem");
        //    }
        //}

        //float producedQuantity;
        //public float ProducedQuantity
        //{
        //    get { return producedQuantity; }
        //    set
        //    {
        //        producedQuantity = value;
        //        OnPropertyChanged("ProducedQuantity");
        //    }
        //}

        //UnitMeasure producedUnitMeasure;
        //public UnitMeasure ProducedUnitMeasure
        //{
        //    get { return producedUnitMeasure; }
        //    set
        //    {
        //        producedUnitMeasure = value;
        //        OnPropertyChanged("ProducedUnitMeasure");
        //    }
        //}

        //Product producedProduct;
        //public Product ProducedProduct
        //{
        //    get { return producedProduct; }
        //    set
        //    {
        //        producedProduct = value;
        //        OnPropertyChanged("ProducedProduct");
        //    }
        //}

        #endregion        

        ObservableCollection<ProductionItemViewModel> productionItems = new ObservableCollection<ProductionItemViewModel>();
        public ObservableCollection<ProductionItemViewModel> ProductionItems 
        {
            get { return productionItems; }
        }

        bool hasPendingChanges;
        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                OnPropertyChanged("HasPendingChanges");
            }
        }

        #region New Item Command

        double quantityToAdd;
        public double QuantityToAdd
        {
            get { return quantityToAdd; }
            set
            {
                quantityToAdd = value;
                OnPropertyChanged("QuantityToAdd");
            }
        }

        Product productToAdd;
        public Product ProductToAdd
        {
            get { return productToAdd; }
            set
            {
                productToAdd = value;

                OnPropertyChanged("ProductToAdd");
            }
        }

        RelayCommand newItemCommand;
        public ICommand NewItemCommand
        {
            get
            {
                if (newItemCommand == null)
                    newItemCommand = new RelayCommand(x => this.DoNewItem(), x => this.CanNewItem);
                return newItemCommand;
            }
        }

        public bool CanNewItem
        {
            get { return /*!itemToAdd.HasErrors &&*/ quantityToAdd > 0 && productToAdd != null; }
        }

        void DoNewItem()
        {
            var baseUM = ProductToAdd.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            ProductionItemViewModel newItemViewModel =
                new ProductionItemViewModel(this, productToAdd, quantityToAdd, baseUM);

            productionItems.Add(newItemViewModel);

            HasPendingChanges = true;

            //clear input fields
            QuantityToAdd = 0;
            ProductToAdd = null;
            //SearchText = string.Empty;

            OnNewItemAdded();
        }

        public event EventHandler NewItemAdded;

        protected void OnNewItemAdded()
        {
            EventHandler handler = this.NewItemAdded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Remove Item Command

        RelayCommand removeItemCommand;
        public ICommand RemoveItemCommand
        {
            get
            {
                if (removeItemCommand == null)
                    removeItemCommand = new RelayCommand(x => this.RemoveItem(SelectedItem), x => this.CanRemoveItem);
                return removeItemCommand;
            }
        }

        public ProductionItemViewModel SelectedItem { get; set; }

        public bool CanRemoveItem
        {
            get { return SelectedItem != null; }
        }

        public void RemoveItem(ProductionItemViewModel tivm)
        {
            //delete from lineitemviewmodels
            productionItems.Remove(tivm);

            HasPendingChanges = true;
        }

        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove(), x => this.CanRemove);
                return removeCommand;
            }
        }

        bool CanRemove { get { return !creating; } }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar esta producción?";

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                //let parent remove it
                if (onRemoved != null) onRemoved(production);

                var ts = base.GetService<ITransactionService>();
                ts.RemoveProduction(production, true);

                appvm.SaveChanges();

                //close this window
                var windowManager = base.GetService<IWindowManager>();

                windowManager.Close(this);                                
            }
        }

        //void RemoveProduction(Production p)
        //{
        //    //var inventory_svc = appvm.InventoryService;
        //    ////remove produced item, remove produced quantity from inventory
        //    //inventory_svc.ExecuteInventoryOperation(Date, production.DestinationInventory,
        //    //    production.ProducedProduct, -production.ProducedQuantity, production.ProducedUnitMeasure);

        //    //remove production items (add them back to inventory)
        //    List<ProductionItem> items_toRemove = new List<ProductionItem>();

        //    foreach (ProductionItem item in production.LineItems)
        //    {
        //        items_toRemove.Add(item);
        //    }

        //    foreach (var item in items_toRemove)
        //    {
        //        //RemoveItem(item);

        //        var ts = base.GetService<ITransactionService>();

        //        ts.UndoProduce(item.Product, item.Quantity, item.UnitMeasure, item.Cost, p.Date, p.Inventory);

        //        appvm.Context.LineItems.DeleteObject(item);
        //    }

        //    //remove from database
        //    appvm.Context.Orders.DeleteObject(p);            
        //}

        //void RemoveItem(ProductionItem pi)
        //{
        //    ExecuteProduceOperation(pi.Product, -pi.Quantity, pi.UnitMeasure);

            
        //}        

        #endregion

        #region Cancel Command

        RelayCommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(x => this.Cancel());
                return cancelCommand;
            }
        }

        void Cancel()
        {
            if (creating)
            {
                //close this window
                var windowManager = base.GetService<IWindowManager>();

                windowManager.Close(this);
            }
            //saving changes
            else
            {
                CopyFrom();
                HasPendingChanges = false;
            }
        }

        #endregion

        #region Save Command

        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => this.Save(), x => this.CanSave);
                return saveCommand;
            }
        }

        bool CanSave 
        {
            get { return inventory != null; }
            //&& destinationInventory != null;// && quantityToProduce > 0 && productToProduce != null && unitMeasureToProduce != null;
            
        }

        void Save()
        {
            if (creating)
            {
                production = new Production();
                production.Date = date;
                production.DateCreated = dateCreated;
                appvm.Context.Orders.AddObject(production);
            }
            
            CopyTo();

            HasPendingChanges = false;

            if (creating)
            {
                onCreated(production);
                creating = false;
            }
            else if (onAssociationChanged != null) onAssociationChanged(production);
        }

        #endregion

        //void ExecuteProduceOperation(Product p, double qtty, UnitMeasure um)
        //{
        //    var inventory_svc = appvm.InventoryService;

        //    //add to inventory
        //    inventory_svc.ExecuteInventoryOperation(production.Date, inventory, p, qtty, um);
            
        //    //if (p.IsRecipe) 
        //    //{
        //    //    //substract ingredients from inventory
        //    //    foreach (var item in p.Ingredients)
        //    //    {
        //    //        inventory_svc.ExecuteInventoryOperation(Date, inventory, 
        //    //            item.IngredientProduct, -qtty * item.Quantity, item.UnitMeasure);
        //    //    }
        //    //}
        //}

        //#region Choose Product Command
                
        //public ObservableCollection<Product> RecipeProducts
        //{
        //    get
        //    {
        //        return appvm.ProductManager.RecipeProducts;
        //    }
        //}

        //RelayCommand chooseProductCommand;
        //public ICommand ChooseProductCommand
        //{
        //    get
        //    {
        //        if (chooseProductCommand == null)
        //            chooseProductCommand = new RelayCommand(x => this.DoChoose(), x => this.CanChoose);
        //        return chooseProductCommand;
        //    }
        //}

        //public bool CanChoose
        //{
        //    get
        //    {
        //        return /*!producedItem.HasErrors &&*/ quantityToProduce != 0 && productToProduce != null &&
        //            unitMeasureToProduce != null && productToProduce.UMFamily == unitMeasureToProduce.UMFamily;
        //    }
        //}

        //void DoChoose()
        //{
        //    productionItems.Clear();

        //    float rate = (quantityToProduce * unitMeasureToProduce.ToBaseConversion);
        //        /// (productToProduce.RecipeQuantity * productToProduce.RecipeUnitMeasure.ToBaseConversion);

        //    foreach (var item in productToProduce.Ingredients)
        //    {
        //        ProductionItemViewModel pivm = new ProductionItemViewModel(appvm, this, item.IngredientProduct, item.Quantity * rate, item.UnitMeasure);
        //        productionItems.Add(pivm);
        //    }

        //    HasPendingChanges = true;
        //}

        //#endregion

        bool IScreen.TryToClose()
        {
            if (hasPendingChanges && this.CanSave)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                {
                    var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
                        "Guardar cambios",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                        return false;

                    if (result == MessageBoxResult.Yes)
                        this.Save();
                }
            }
            return true;
        }

    }
}
