using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Entities;
using MiPaladar.Classes;

using System.ComponentModel;
using System.Windows.Data;
using System.Data.Objects.DataClasses;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows;

namespace MiPaladar.ViewModels
{
    public class PurchaseViewModel : ViewModelBase, IScreen
    {
        MainWindowViewModel appvm;
        Purchase purchase;

        //DateTime workingDate;
        //IInventoryService inventoryService;
        //ProductManager productManager;
        //UserManager personnelManager;

        Action<Purchase> onRemoved;
        Action<Purchase> onCreated;
        Action<Purchase> onAssociationChanged;

        bool creating;

        //public const int Warehouse_Destination_Code = 0;
        //public const int Floor_Destination_Code = 1;

        public PurchaseViewModel(MainWindowViewModel appvm, Action<Purchase> onCreated,
            Action<Purchase> onRemoved, Action<Purchase> onAssociationChanged)
        {
            this.appvm = appvm;
            //this.workingDate = workingDate;
            //this.inventoryService = appvm.InventoryService;
            //this.productManager = appvm.ProductManager;
            //this.personnelManager = appvm.PersonnelManager;
            this.onCreated = onCreated;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = DateTime.Now;
            date = DateTime.Today;

            creating = true;

            HasPendingChanges = true;
        }

        public PurchaseViewModel(MainWindowViewModel appvm, Purchase purchase,
            Action<Purchase> onRemoved = null, Action<Purchase> onAssociationChanged = null)
        {
            this.appvm = appvm;
            this.purchase = purchase;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = purchase.DateCreated;

            CopyFrom();

            //SearchText = production.ProducedProduct.Name;

            HasPendingChanges = false;
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        #region Copy Methods

        void CopyFrom() 
        {
            Date = purchase.Date;
            Responsible = purchase.Employee;
            Destination = purchase.Inventory;
            Title = purchase.Memo;
            TotalPrice = purchase.Total;

            lineitems.Clear();
            foreach (PurchaseLineItem item in purchase.LineItems)
            {
                PurchaseLineItemViewModel pivm = new PurchaseLineItemViewModel(this, item);
                lineitems.Add(pivm);
            }

            //ProductCount = lineitems.Sum(x => x.Quantity);
        }

        void CopyTo()
        {
            var ts = base.GetService<ITransactionService>();
            var inventory_svc = appvm.InventoryService;

            if (!creating)
            {
                //remove original items
                List<PurchaseLineItem> toRemove = new List<PurchaseLineItem>();
                foreach (PurchaseLineItem item in purchase.LineItems)
                {
                    toRemove.Add(item);
                }
                foreach (PurchaseLineItem item in toRemove)
                {
                    ts.UndoPurchase(item.Product, item.Quantity, item.UnitMeasure, item.Amount, purchase.Date, purchase.Inventory);

                    appvm.Context.LineItems.DeleteObject(item);
                    //RemoveItem(item);
                }
                //remove old produced item from inventory
                //inventory_svc.ExecuteInventoryOperation(production.Date, production.DestinationInventory,
                //    productToProduce, -quantityToProduce, unitMeasureToProduce);
            }

            //the properties
            if (date != purchase.Date) purchase.Date = date;
            if (responsible != purchase.Employee) purchase.Employee = responsible;
            if (destination != purchase.Inventory) purchase.Inventory = destination;
            //if (destinationInventory != production.DestinationInventory) production.DestinationInventory = destinationInventory;
            if (memo != purchase.Memo) purchase.Memo = memo;
            if (totalPrice != purchase.Total) purchase.Total = totalPrice; 

            //replace new values
            //if (production.ProducedQuantity != quantityToProduce) production.ProducedQuantity = quantityToProduce;
            //if (production.ProducedUnitMeasure != unitMeasureToProduce) production.ProducedUnitMeasure = unitMeasureToProduce;
            //if (production.ProducedProduct != productToProduce) production.ProducedProduct = productToProduce;

            //add current items                
            foreach (var item in lineitems)
            {
                PurchaseLineItem newPi = new PurchaseLineItem();
                newPi.Quantity = item.Quantity;
                newPi.UnitMeasure = item.UnitMeasure;
                newPi.Product = item.Product;
                newPi.Amount = item.Price;

                purchase.LineItems.Add(newPi);

                ts.Purchase(item.Product, item.Quantity, item.UnitMeasure, item.Price, date, destination);
            }

            //add new produced item to inventory
            //inventory_svc.ExecuteInventoryOperation(Date, destinationInventory,
            //    productToProduce, quantityToProduce, unitMeasureToProduce);

            
        }

        #endregion

        public Purchase WrappedPurchase
        {
            get { return purchase; }
        }

        public ObservableCollection<Inventory> InventoryAreas
        {
            get { return appvm.InventoryAreasOC; }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return appvm.CanPurchaseEmployees; }
        }

        public ObservableCollection<Product> PurchasableProducts
        {
            get { return appvm.ProductsOC; }
        }

        //public override string DisplayName
        //{
        //    get { return "Compra: " + Title; }
        //}

        //public void RemoveEvents()
        //{
        //    //if (loadevents)
        //    //{
        //    //v.PropertyChanging += new PropertyChangingEventHandler(v_PropertyChanging);
        //    //v.PropertyChanged -= v_PropertyChanged;

        //    //if (lineitems != null)
        //    //{
        //        //foreach (var orderitem in purchase.LineItems)
        //        //{
        //        //    RemovePurchaseItemEvents(orderitem);
        //        //}

        //        //lineitems.CollectionChanged -= lineitems_CollectionChanged;
        //        //purchase.LineItems.AssociationChanged -= PurchaseItems_AssociationChanged;
        //    //}

        //    foreach (var item in appvm.ProductsOC)
        //    {
        //        item.PropertyChanged -= product_PropertyChanged;
        //    }

        //    appvm.ProductsOC.CollectionChanged -= products_CollectionChanged;
        //}

        
        //#region Purchasable Products

        //ICollectionView icvPurchasableProducts;
                

        //void product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsPurchasable") icvPurchasableProducts.Refresh();
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

        //#endregion

        

        #region Properties

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

        //public int Number 
        //{
        //    get { return purchase.TheNumber; }
        //    set 
        //    {
        //        purchase.TheNumber = value;
        //        appvm.SaveChanges();
        //        OnPropertyChanged("Number");
        //    }
        //}

        string memo;
        public string Title 
        {
            get { return memo; }
            set
            {
                memo = value;
                OnPropertyChanged("Title");

                HasPendingChanges = true;
            }
        }

        //public string Notes       
        //{
        //    get { return purchase.Memo; }
        //    set 
        //    {
        //        purchase.Memo = value;
        //        appvm.SaveChanges();

        //        OnPropertyChanged("HasNotes");
        //    }
        //}

        //public PurchaseType PurchaseType
        //{
        //    get { return purchase.PurchaseType; }
        //    set
        //    {
        //        purchase.PurchaseType = value;
        //        appvm.SaveChanges();
        //        OnPropertyChanged("PurchaseType");
        //    }
        //}

        decimal totalPrice;
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        //double productCount;
        //public double ProductCount 
        //{
        //    get { return productCount; }
        //    set 
        //    {
        //        productCount = value;
        //        OnPropertyChanged("ProductCount");
        //    }
        //}

        //public bool Closed 
        //{
        //    get { return purchase.IsClosed; }
        //    set
        //    {
        //        purchase.IsClosed = value;
        //        OnPropertyChanged("Closed");
        //        OnPropertyChanged("Opened");
        //        OnPropertyChanged("OpenOrCloseContent");                
        //    }
        //}

        //public bool Opened 
        //{
        //    get { return !Closed; }
        //}

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

        //public IEnumerable<PurchaseTagItem> PurchaseTagItems 
        //{
        //    get { return purchase.PurchaseTagItems; }
        //}

        //int destination_code;
        //public int DestinationCode 
        //{
        //    get { return destination_code; }
        //}

        Inventory destination;
        public Inventory Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }

        #endregion

        //bool orderItemsFirstTime = true;
        ObservableCollection<PurchaseLineItemViewModel> lineitems = new ObservableCollection<PurchaseLineItemViewModel>();

        public ObservableCollection<PurchaseLineItemViewModel> LineItems
        {
            get
            {
                //if (lineitems == null)
                //{
                //    //orderItemsFirstTime = false;

                //    lineitems = new ObservableCollection<PurchaseLineItemViewModel>();

                //    foreach (PurchaseLineItem lineitem in purchase.LineItems)
                //    {
                //        PurchaseLineItemViewModel pvm =
                //            new PurchaseLineItemViewModel(appvm, lineitem, this);
                //        //AddPurchaseItemEvents(lineitem);
                //        lineitems.Add(pvm);
                //    }

                //    //lineitems.CollectionChanged += new NotifyCollectionChangedEventHandler(lineitems_CollectionChanged);
                //    //purchase.LineItems.AssociationChanged +=
                //    //    new CollectionChangeEventHandler(PurchaseItems_AssociationChanged);
                //}
                return lineitems;
            }
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

        #region New LineItem Command

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

        //UnitMeasure unitMeasureToAdd;
        //public UnitMeasure UnitMeasureToAdd
        //{
        //    get { return unitMeasureToAdd; }
        //    set
        //    {
        //        unitMeasureToAdd = value;
        //        OnPropertyChanged("UnitMeasureToAdd");
        //    }
        //}

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

        //ProductQuantityViewModel itemToAdd;
        //public ProductQuantityViewModel ItemToAdd
        //{
        //    get
        //    {
        //        if (itemToAdd == null) itemToAdd = new ProductQuantityViewModel(appvm);
        //        return itemToAdd;
        //    }
        //    set { itemToAdd = value; }
        //}

        //string searchText;
        //public string SearchText
        //{
        //    get { return searchText; }
        //    set
        //    {
        //        searchText = value;
        //        OnPropertyChanged("SearchText");

        //        //Typing = !string.IsNullOrEmpty(searchText);

        //        //icvVentaItems.Refresh();
        //    }
        //}

        RelayCommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                if (newCommand == null)
                    newCommand = new RelayCommand(x => this.DoNewLineItem(), x => this.CanNew);
                return newCommand;
            }
        }

        public bool CanNew
        {
            get { return /*!itemToAdd.HasErrors &&*/ quantityToAdd > 0 && productToAdd != null; }
        }

        void DoNewLineItem()
        {
            //var baseUM = ProductToAdd.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            //NewLineItem(QuantityToAdd, baseUM, ProductToAdd);

            //PurchaseLineItem newLineItem = new PurchaseLineItem();
            //newLineItem.Quantity = quantityToAdd;
            //newLineItem.UnitMeasure = baseUM;
            //newLineItem.Product = productToAdd;
            UnitMeasure um = productToAdd.CostUnitMeasure;
            decimal amount = ProductManager.GetCurrentCost(productToAdd, quantityToAdd, um);
            ////decimal amount = (decimal)quantity * product.PurchasePrice;
            //newLineItem.Amount = amount;

            //purchase.LineItems.Add(newLineItem);

            PurchaseLineItemViewModel newLineItemViewModel = 
                new PurchaseLineItemViewModel(this, productToAdd, quantityToAdd, um, amount);

            lineitems.Add(newLineItemViewModel);

            //var ts = base.GetService<ITransactionService>();
            //ts.Purchase(productToAdd, quantityToAdd, baseUM, amount, Date, Destination);

            ////ExecuteOperation(product, quantity, um);

            //appvm.SaveChanges();

            RefreshTotal();

            HasPendingChanges = true;

            //clear input fields
            QuantityToAdd = 0;
            ProductToAdd = null;
            //SearchText = string.Empty;

            OnLineItemAdded();
        }

        public event EventHandler LineItemAdded;

        protected void OnLineItemAdded()
        {
            EventHandler handler = this.LineItemAdded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Remove LineItem Command

        RelayCommand removeLineItemCommand;
        public ICommand RemoveLineItemCommand
        {
            get
            {
                if (removeLineItemCommand == null)
                    removeLineItemCommand = new RelayCommand(x => this.RemoveSelectedItem(), x => this.CanRemoveLineItem);
                return removeLineItemCommand;
            }
        }

        public PurchaseLineItemViewModel SelectedLineItem { get; set; }

        public bool CanRemoveLineItem
        {
            get { return SelectedLineItem != null; }
        }

        public void RemoveSelectedItem()
        {
            //RemoveLineItem(SelectedLineItem);

            lineitems.Remove(SelectedLineItem);

            //appvm.SaveChanges();

            RefreshTotal();

            HasPendingChanges = true;
        }

        //void RemoveLineItem(PurchaseLineItemViewModel livm)
        //{
        //    var ts = base.GetService<ITransactionService>();
        //    ts.UndoPurchase(livm.Product, livm.Quantity, livm.UnitMeasure, livm.Price, Date, Destination);

        //    //remove from inventory
        //    //ExecuteOperation(li.Product, -li.Quantity, li.UnitMeasure);

        //    LineItem li = livm.LineItem;

        //    //delete from lineitemviewmodels
        //    lineitems.Remove(livm);

        //    //remove from database
        //    appvm.Context.LineItems.DeleteObject(li);
        //}

        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(x => this.RemovePurchase(), x=> CanRemove);
                }
                return removeCommand;
            }
        }

        bool CanRemove { get { return !creating; } }

        public void RemovePurchase()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            if (msgBox.ShowYesNoDialog("Está seguro que desea eliminar la compra?") == true) 
            {
                var windowManager = base.GetService<IWindowManager>();

                windowManager.Close(this);

                //let parent remove it
                if (onRemoved != null) onRemoved(purchase);

                List<PurchaseLineItem> toRemove = new List<PurchaseLineItem>();

                foreach (PurchaseLineItem item in purchase.LineItems)
                {
                    toRemove.Add(item);
                }

                foreach (var item in toRemove)
                {
                    var ts = base.GetService<ITransactionService>();

                    ts.UndoPurchase(item.Product, item.Quantity, item.UnitMeasure, item.Amount, purchase.Date, purchase.Inventory);

                    appvm.Context.LineItems.DeleteObject(item);
                }

                appvm.Context.Orders.DeleteObject(purchase);

                appvm.SaveChanges();

                //if (onRemoved != null) onRemoved(purchase);
                //compras.Remove(selectedPurchase);
            }            
        }

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

        bool CanSave { get { return destination != null; } }

        void Save()
        {
            if (creating)
            {
                purchase = new Purchase();
                purchase.Date = date;
                purchase.DateCreated = dateCreated;
                appvm.Context.Orders.AddObject(purchase);
            }

            CopyTo();

            appvm.SaveChanges();           

            if (creating)
            {                
                creating = false;

                if (onCreated != null) onCreated(purchase);
            }
            else if (onAssociationChanged != null) onAssociationChanged(purchase);

            HasPendingChanges = false;
        }

        #endregion

        public void RefreshTotal()
        {
            decimal total = 0;
            //double prodCount = 0;
            //refresh total price
            foreach (PurchaseLineItemViewModel vi in lineitems)
            {
                total += vi.Price;
                //prodCount += vi.Quantity;
            }

            TotalPrice = total;
            //ProductCount = prodCount;

            //appvm.SaveChanges();
        }

        bool IScreen.TryToClose()
        {
            if (hasPendingChanges)
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

        //#region ToggleClose Command

        //public string OpenOrCloseContent
        //{
        //    get { return Closed ? "Abrir" : "Cerrar"; }
        //}

        //RelayCommand closeCommand;
        //public ICommand ToggleCloseCommand
        //{
        //    get
        //    {
        //        if (closeCommand == null)
        //            closeCommand = new RelayCommand(x => this.Close());
        //        return closeCommand;
        //    }
        //}

        //void Close()
        //{
        //    Closed = !Closed;

        //    if (Closed)             
        //    {
        //        int index = 0;

        //        while (index < lineitems.Count)
        //        {
        //            var li = lineitems[index];

        //            if (li.Quantity == 0) RemoveLineItem(li);
        //            else index++;
        //        }
        //    }            

        //    //if (purchase.DateClosed == null)
        //    //{
        //    //    purchase.DateClosed = DateTime.Now;
        //    //    appvm.SaveChanges();
        //    //}
        //}

        //#endregion

        //#region Show Notes Command

        //public bool HasNotes 
        //{
        //    get { return !string.IsNullOrWhiteSpace(Notes); }
        //}

        //bool showingNotes;
        //public bool ShowingNotes 
        //{
        //    get { return showingNotes; }
        //    set
        //    {
        //        showingNotes = value;
        //        OnPropertyChanged("ShowingNotes");
        //    }
        //}

        //RelayCommand showNotesCommand;
        //public ICommand ShowNotesCommand 
        //{
        //    get
        //    {
        //        if (showNotesCommand == null)
        //            showNotesCommand = new RelayCommand(x => ShowNotes());
        //        return showNotesCommand;
        //    }
        //}

        //void ShowNotes() 
        //{
        //    ShowingNotes = true;
        //}

        //#endregion        

        //#region Show Product Template Popup
        
        //bool showingProductTemplatePopup;
        //public bool ShowingProductTemplatePopup
        //{
        //    get { return showingProductTemplatePopup; }
        //    set
        //    {
        //        showingProductTemplatePopup = value;
        //        OnPropertyChanged("ShowingProductTemplatePopup");
        //    }
        //}

        //RelayCommand showProductTemplatePopupCommand;
        //public ICommand ShowProductTemplatePopupCommand
        //{
        //    get
        //    {
        //        if (showProductTemplatePopupCommand == null)
        //            showProductTemplatePopupCommand = new RelayCommand(x => ShowProductTemplatePopup());
        //        return showProductTemplatePopupCommand;
        //    }
        //}

        //void ShowProductTemplatePopup()
        //{
        //    ShowingProductTemplatePopup = true;
        //}        

        //#endregion

        //#region Add Product Template Command

        //ProductTemplate selectedTemplate;
        //public ProductTemplate SelectedTemplate 
        //{
        //    get { return selectedTemplate; }
        //    set 
        //    {
        //        selectedTemplate = value;
        //        OnPropertyChanged("SelectedTemplate");
        //    }
        //}

        //public ObservableCollection<ProductTemplate> ProductTemplates
        //{
        //    get { return appvm.ProductTemplatesOC; }
        //}

        //RelayCommand addProductTemplateCommand;
        //public ICommand AddProductTemplateCommand 
        //{
        //    get 
        //    {
        //        if (addProductTemplateCommand == null)
        //            addProductTemplateCommand = new RelayCommand(x => AddProductTemplate(), x => CanAdd);
        //        return addProductTemplateCommand;
        //    }
        //}

        //bool CanAdd { get { return selectedTemplate != null; } }

        //void AddProductTemplate() 
        //{
        //    foreach (var item in SelectedTemplate.Products)
        //    {
        //        UnitMeasure umFamilyBase = item.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
        //        NewLineItem(0, umFamilyBase, item.Product);
        //    }

        //    ShowingProductTemplatePopup = false;
        //    SelectedTemplate = null;
        //}

        //#endregion
        
        //#region Add PurchaseTag Command

        //PurchaseTag purchaseTagToAdd;
        //public PurchaseTag PurchaseTagToAdd 
        //{
        //    get { return purchaseTagToAdd; }
        //    set 
        //    {
        //        purchaseTagToAdd = value;
        //        OnPropertyChanged("PurchaseTagToAdd");
        //    }
        //}

        //RelayCommand addPurchaseTagCommand;
        //public ICommand AddPurchaseTagCommand 
        //{
        //    get 
        //    {
        //        if (addPurchaseTagCommand == null)
        //            addPurchaseTagCommand = new RelayCommand(x => AddPurchaseTag(), x => CanAddPurchaseTag);
        //        return addPurchaseTagCommand; 
        //    }
        //}

        //bool CanAddPurchaseTag
        //{
        //    get { return purchaseTagToAdd != null; } 
        //}

        //void AddPurchaseTag() 
        //{
        //    PurchaseTagItem pti = new PurchaseTagItem();
        //    pti.Orders_Purchase = purchase;
        //    pti.PurchaseTag = purchaseTagToAdd;

        //    appvm.Context.PurchaseTagItems.AddObject(pti);

        //    appvm.SaveChanges();
        //}

        //#endregion

        //void ExecuteOperation(Product prod, double quantity, UnitMeasure um)
        //{
        //    if (Destination == null) return;
        //    inventoryService.ExecuteInventoryOperation(Date, Destination, prod, quantity, um);
        //    //if (destination_code == Warehouse_Destination_Code)
        //    //{
                

        //    //    //temporary solution to update vale's total price
        //    //    //vale.RefreshTotal();

        //    //    //UpdateTotalVentas();
        //    //}
        //    //else if (destination_code == Floor_Destination_Code)
        //    //{
        //    //    inventoryService.ExecuteFloorOperation(prod, quantity, um);
        //    //}
        //    //else throw new Exception("Destination code not recognized");
        //}        

        
        //public IEnumerable<PurchaseType> PurchaseTypes 
        //{
        //    get { return new ObservableCollection<PurchaseType>(appvm.Context.PurchaseTypes); }
        //}

        //public ObservableCollection<PurchaseTag> PurchaseTags 
        //{
        //    get { return appvm.PurchaseTagsOC; }
        //}
    }
}
