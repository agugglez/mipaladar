using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

using System.Collections.ObjectModel;
using MiPaladar.Classes;
using System.Windows.Input;
using MiPaladar.Services;
using System.Windows;

namespace MiPaladar.ViewModels
{
    //public class FaenaViewModel : ViewModelBase, IScreen
    //{
    //    MainWindowViewModel appvm;
    //    Faena faena;

    //    Action<Faena> onCreated;
    //    Action<Faena> onRemoved;
    //    Action<Faena> onAssociationChanged;

    //    bool creating;

    //    //creating a new one
    //    public FaenaViewModel(MainWindowViewModel appvm, Action<Faena> onCreated, Action<Faena> onRemoved, 
    //        Action<Faena> onAssociationChanged) 
    //    {
    //        this.appvm = appvm;
    //        this.onCreated = onCreated;
    //        this.onRemoved = onRemoved;
    //        this.onAssociationChanged = onAssociationChanged;

    //        date = DateTime.Now;
    //        //usedItem = new ProductQuantityViewModel(appvm);

    //        creating = true;

    //        HasPendingChanges = true;
    //    }

    //    public FaenaViewModel(MainWindowViewModel appvm, Faena faena, Action<Faena> onRemoved, 
    //        Action<Faena> onAssociationChanged)
    //    {
    //        this.appvm = appvm;
    //        this.faena = faena;
    //        this.onRemoved = onRemoved;
    //        this.onAssociationChanged = onAssociationChanged;

    //        date = faena.Date;

    //        CopyFrom();

    //        HasPendingChanges = false;
    //    }

    //    #region Copy Methods

    //    void CopyFrom() 
    //    {
    //        Responsible = faena.Responsible;
    //        Inventory = faena.Inventory;
    //        DestinationInventory = faena.DestinationInventory;
    //        Memo = faena.Memo;

    //        ProductToUse = faena.UsedProduct;
    //        QuantityToUse = faena.UsedQuantity;
    //        UnitMeasureToUse = faena.UsedUnitMeasure;
    //        //UsedItem = new ProductQuantityViewModel(appvm, faena.UsedProduct, faena.UsedQuantity, faena.UsedUnitMeasure);

    //        faenaItems.Clear();
    //        foreach (var item in faena.FaenaItems)
    //        {
    //            FaenaItemViewModel pivm = new FaenaItemViewModel(appvm, this, item);
    //            faenaItems.Add(pivm);
    //        }
    //    }

    //    void CopyTo()
    //    {
    //        var inventory_svc = appvm.InventoryService;

    //        if (!creating) 
    //        {
    //            //remove original items
    //            List<FaenaItem> toRemove = new List<FaenaItem>(faena.FaenaItems);
    //            foreach (var item in toRemove)
    //            {
    //                RemoveItem(item, faena.DestinationInventory);
    //            }
    //            //add old used item to inventory
    //            inventory_svc.ExecuteInventoryOperation(faena.Date, faena.Inventory,
    //                faena.UsedProduct, faena.UsedQuantity, faena.UsedUnitMeasure);
    //        }

    //        //the properties, date never changes
    //        if (responsible != faena.Responsible) faena.Responsible = responsible;
    //        if (inventory != faena.Inventory) faena.Inventory = inventory;
    //        if (destinationInventory != faena.DestinationInventory) faena.DestinationInventory = destinationInventory;
    //        if (memo != faena.Memo) faena.Memo = memo;

    //        //replace with the new values
    //        if (faena.UsedQuantity != quantityToUse) faena.UsedQuantity = quantityToUse;
    //        if (faena.UsedUnitMeasure != unitMeasureToUse) faena.UsedUnitMeasure = unitMeasureToUse;
    //        if (faena.UsedProduct != productToUse) faena.UsedProduct = productToUse;

    //        //add current items                
    //        foreach (var item in faenaItems)
    //        {
    //            FaenaItem newPi = new FaenaItem();
    //            newPi.Quantity = item.Quantity;
    //            newPi.UnitMeasure = item.UnitMeasure;
    //            newPi.Product = item.Product;

    //            faena.FaenaItems.Add(newPi);

    //            //remove from inventory
    //            inventory_svc.ExecuteInventoryOperation(Date, destinationInventory, item.Product, item.Quantity, item.UnitMeasure);
    //        }

    //        //remove new used item from inventory
    //        inventory_svc.ExecuteInventoryOperation(Date, inventory, productToUse, -quantityToUse, unitMeasureToUse);

    //        appvm.SaveChanges();
    //    }

    //    #endregion

    //    public Faena WrappedFaena
    //    {
    //        get { return faena; }
    //    }

    //    public ObservableCollection<Employee> Employees
    //    {
    //        get { return appvm.EmployeesOC; }
    //    }

    //    public ObservableCollection<Inventory> Inventories
    //    {
    //        get { return appvm.InventoriesOC; }
    //    }        

    //    #region Header       

    //    DateTime date;
    //    public DateTime Date 
    //    {
    //        get { return date; }
    //    }

    //    Employee responsible;
    //    public Employee Responsible
    //    {
    //        get { return responsible; }
    //        set
    //        {
    //            responsible = value;
    //            OnPropertyChanged("Responsible");
    //            HasPendingChanges = true;
    //        }
    //    }

    //    string memo;
    //    public string Memo
    //    {
    //        get { return memo; }
    //        set
    //        {
    //            memo = value;
    //            OnPropertyChanged("Memo");
    //            HasPendingChanges = true;
    //        }
    //    }

    //    Inventory inventory;
    //    public Inventory Inventory 
    //    {
    //        get { return inventory; }
    //        set
    //        {
    //            inventory = value;
    //            OnPropertyChanged("Inventory");
    //        }
    //    }

    //    Inventory destinationInventory;
    //    public Inventory DestinationInventory
    //    {
    //        get { return destinationInventory; }
    //        set
    //        {
    //            destinationInventory = value;
    //            OnPropertyChanged("DestinationInventory");
    //            HasPendingChanges = true;
    //        }
    //    }

    //    //ProductQuantityViewModel usedItem;
    //    //public ProductQuantityViewModel UsedItem
    //    //{
    //    //    get { return usedItem; }
    //    //    set
    //    //    {
    //    //        usedItem = value;
    //    //        OnPropertyChanged("UsedItem");
    //    //    }
    //    //}

    //    #endregion

    //    ObservableCollection<FaenaItemViewModel> faenaItems = new ObservableCollection<FaenaItemViewModel>();
    //    public ObservableCollection<FaenaItemViewModel> FaenaItems 
    //    {
    //        get { return faenaItems; }
    //    }

    //    bool hasPendingChanges;
    //    public bool HasPendingChanges
    //    {
    //        get { return hasPendingChanges; }
    //        set
    //        {
    //            hasPendingChanges = value;
    //            OnPropertyChanged("HasPendingChanges");
    //        }
    //    }

    //    #region Remove Item Command

    //    RelayCommand removeItemCommand;
    //    public ICommand RemoveItemCommand
    //    {
    //        get
    //        {
    //            if (removeItemCommand == null)
    //                removeItemCommand = new RelayCommand(x => this.RemoveItem(SelectedItem), x => this.CanRemoveItem);
    //            return removeItemCommand;
    //        }
    //    }

    //    public FaenaItemViewModel SelectedItem { get; set; }

    //    public bool CanRemoveItem
    //    {
    //        get { return SelectedItem != null; }
    //    }

    //    public void RemoveItem(FaenaItemViewModel tivm)
    //    {
    //        //delete from lineitemviewmodels
    //        faenaItems.Remove(tivm);

    //        HasPendingChanges = true;
    //    }

    //    #endregion


    //    #region Remove Command

    //    RelayCommand removeCommand;
    //    public ICommand RemoveCommand
    //    {
    //        get
    //        {
    //            if (removeCommand == null)
    //                removeCommand = new RelayCommand(x => this.Remove(), x => this.CanRemove);
    //            return removeCommand;
    //        }
    //    }

    //    bool CanRemove { get { return !creating; } }

    //    void Remove()
    //    {
    //        var msgBox = base.GetService<IMessageBoxService>();

    //        string message = "¿Está seguro que desea eliminar esta faena?";

    //        if (msgBox.ShowYesNoDialog(message) == true)
    //        {
    //            RemoveFaena(faena);

    //            //let parent remove it
    //            if (onRemoved != null) onRemoved(faena);

    //            //close this window
    //            var windowManager = base.GetService<IWindowManager>();

    //            windowManager.Close(this);
    //        }
    //    }

    //    void RemoveFaena(Faena f)
    //    {
    //        var inventory_svc = appvm.InventoryService;
    //        //remove used item, add used quantity to inventory
    //        inventory_svc.ExecuteInventoryOperation(Date, faena.Inventory,
    //            faena.UsedProduct, faena.UsedQuantity, faena.UsedUnitMeasure);

    //        //remove faena items (remove them from inventory)
    //        List<FaenaItem> items_toRemove = new List<FaenaItem>(f.FaenaItems);

    //        foreach (var item in items_toRemove)
    //        {
    //            RemoveItem(item, faena.DestinationInventory);
    //        }

    //        //remove from database
    //        appvm.Context.Faenas.DeleteObject(f);

    //        appvm.SaveChanges();
    //    }

    //    void RemoveItem(FaenaItem pi, Inventory inventory)
    //    {
    //        var inventory_svc = appvm.InventoryService;

    //        //remove from inventory
    //        inventory_svc.ExecuteInventoryOperation(Date, inventory, pi.Product, -pi.Quantity, pi.UnitMeasure);

    //        appvm.Context.FaenaItems.DeleteObject(pi);
    //    }

    //    #endregion

    //    #region Cancel Command

    //    RelayCommand cancelCommand;
    //    public ICommand CancelCommand
    //    {
    //        get
    //        {
    //            if (cancelCommand == null)
    //                cancelCommand = new RelayCommand(x => this.Cancel());
    //            return cancelCommand;
    //        }
    //    }

    //    void Cancel()
    //    {
    //        if (creating)
    //        {
    //            //close this window
    //            var windowManager = base.GetService<IWindowManager>();

    //            windowManager.Close(this);
    //        }
    //        //saving changes
    //        else
    //        {
    //            CopyFrom();
    //            HasPendingChanges = false;
    //        }
    //    }

    //    #endregion

    //    #region Save Command

    //    RelayCommand saveCommand;
    //    public ICommand SaveCommand
    //    {
    //        get
    //        {
    //            if (saveCommand == null)
    //                saveCommand = new RelayCommand(x => this.Save(), x => this.CanSave);
    //            return saveCommand;
    //        }
    //    }

    //    bool CanSave 
    //    {
    //        get { return inventory != null && destinationInventory != null && 
    //            productToUse != null && unitMeasureToUse != null; }
    //    }

    //    void Save()
    //    {
    //        if (creating)
    //        {
    //            faena = new Faena();
    //            faena.Date = date;
    //            appvm.Context.Faenas.AddObject(faena);
    //        }
    //        else onAssociationChanged(faena);
    //        CopyTo();
    //        HasPendingChanges = false;

    //        if (creating)
    //        {
    //            onCreated(faena);
    //            creating = false;
    //        }
    //    }

    //    #endregion

    //    #region Choose Product Command

    //    //ObservableCollection<Product> recipeProducts;
    //    public ObservableCollection<Product> RecipeProducts
    //    {
    //        get { return appvm.ProductManager.RecipeProducts; }
    //    }

    //    //public ObservableCollection<UnitMeasure> UnitMeasures
    //    //{
    //    //    get { return new ObservableCollection<UnitMeasure>(appvm.Context.UnitMeasures); }
    //    //}

    //    float quantityToUse;
    //    public float QuantityToUse
    //    {
    //        get { return quantityToUse; }
    //        set
    //        {
    //            quantityToUse = value;
    //            OnPropertyChanged("QuantityToUse");
    //        }
    //    }

    //    UnitMeasure unitMeasureToUse;
    //    public UnitMeasure UnitMeasureToUse
    //    {
    //        get { return unitMeasureToUse; }
    //        set
    //        {
    //            unitMeasureToUse = value;
    //            OnPropertyChanged("UnitMeasureToUse");
    //        }
    //    }

    //    Product productToUse;
    //    public Product ProductToUse
    //    {
    //        get { return productToUse; }
    //        set
    //        {
    //            productToUse = value;

    //            if (productToUse != null && unitMeasureToUse == null)
    //                UnitMeasureToUse = productToUse.UMFamily.UnitMeasures.First();

    //            OnPropertyChanged("ProductToUse");
    //        }
    //    }

    //    RelayCommand chooseProductCommand;
    //    public ICommand ChooseProductCommand
    //    {
    //        get
    //        {
    //            if (chooseProductCommand == null)
    //                chooseProductCommand = new RelayCommand(x => this.DoChoose(), x => this.CanChoose);
    //            return chooseProductCommand;
    //        }
    //    }

    //    public bool CanChoose
    //    {
    //        get { return /*!usedItem.HasErrors &&*/ quantityToUse != 0 && productToUse != null &&
    //            unitMeasureToUse != null && productToUse.UMFamily == unitMeasureToUse.UMFamily; }
    //    }

    //    void DoChoose()
    //    {
    //        faenaItems.Clear();

    //        float rate = quantityToUse * unitMeasureToUse.ToBaseConversion;
    //            /// (productToUse.RecipeQuantity * productToUse.RecipeUnitMeasure.ToBaseConversion);

    //        foreach (var item in productToUse.Ingredients)
    //        {
    //            FaenaItemViewModel pivm = new FaenaItemViewModel(appvm, this, item.IngredientProduct, item.Quantity * rate, item.UnitMeasure);
    //            faenaItems.Add(pivm);
    //        }

    //        HasPendingChanges = true;
    //    }

    //    #endregion

    //    bool IScreen.TryToClose()
    //    {
    //        if (hasPendingChanges && this.CanSave)
    //        {
    //            var msgBox = base.GetService<IMessageBoxService>();
    //            if (msgBox != null)
    //            {
    //                var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
    //                    "Guardar cambios",
    //                    MessageBoxButton.YesNoCancel,
    //                    MessageBoxImage.Question);

    //                if (result == MessageBoxResult.Cancel)
    //                    return false;

    //                if (result == MessageBoxResult.Yes)
    //                    this.Save();
    //            }
    //        }
    //        return true;
    //    }
    //}
}
