using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

using System.Collections.ObjectModel;
using System.Windows.Input;
using MiPaladar.Services;
using MiPaladar.Classes;
using System.Windows;

namespace MiPaladar.ViewModels
{
    public class TransferViewModel : ViewModelBase, IScreen
    {
        MainWindowViewModel appvm;
        Transfer transfer;
        Action<Transfer> onRemoved;
        Action<Transfer> onCreated;
        Action<Transfer> onAssociationChanged;

        bool creating;

        //creating a new one
        public TransferViewModel(MainWindowViewModel appvm, Action<Transfer> onCreated, 
            Action<Transfer> onRemoved, Action<Transfer> onAssociationChanged) 
        {
            this.appvm = appvm;
            this.onCreated = onCreated;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            date = DateTime.Today;
            dateCreated = DateTime.Now;

            creating = true;

            HasPendingChanges = true;
        }

        public TransferViewModel(MainWindowViewModel appvm, Transfer transfer,
            Action<Transfer> onRemoved, Action<Transfer> onAssociationChanged)
        {
            this.appvm = appvm;
            this.transfer = transfer;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = transfer.DateCreated;

            CopyFrom();

            HasPendingChanges = false;
        }

        #region Copy Methods

        void CopyFrom() 
        {
            Date = transfer.Date;
            Responsible = transfer.Employee;
            Memo = transfer.Memo;
            InventoryFrom = transfer.InventoryFrom;
            InventoryTo = transfer.InventoryTo;

            transferItems.Clear();
            foreach (TransferItem item in transfer.LineItems)
            {
                TransferItemViewModel pivm = new TransferItemViewModel(this, item);
                transferItems.Add(pivm);
            }
        }

        void CopyTo()
        {
            var ts = base.GetService<ITransactionService>();
            var inventory_svc = appvm.InventoryService;

            //remove original items
            if (!creating) 
            {                
                List<TransferItem> toRemove = new List<TransferItem>();
                foreach (TransferItem item in transfer.LineItems)
                {
                    toRemove.Add(item);
                }
                foreach (var item in toRemove)
                {
                    //RemoveItem(item);
                    ts.UndoTransfer(item.Product, item.Quantity, item.UnitMeasure, item.Cost, transfer.Date, transfer.InventoryFrom, transfer.InventoryTo);

                    appvm.Context.LineItems.DeleteObject(item);
                }
            }            

            //add current items
            foreach (var item in transferItems)
            {
                TransferItem newTi = new TransferItem();
                newTi.Quantity = item.Quantity;
                newTi.UnitMeasure = item.UnitMeasure;
                newTi.Product = item.Product;

                transfer.LineItems.Add(newTi);

                ts.Transfer(item.Product, item.Quantity, item.UnitMeasure, date, inventoryFrom, inventoryTo);
            }

            //the rest of the properties
            if (date != transfer.Date) transfer.Date = date;
            if (inventoryFrom != transfer.InventoryFrom) transfer.InventoryFrom = inventoryFrom;
            if (inventoryTo != transfer.InventoryTo) transfer.InventoryTo = inventoryTo;
            if (responsible != transfer.Employee) transfer.Employee = responsible;
            if (memo != transfer.Memo) transfer.Memo = memo;

            appvm.SaveChanges();
        }

        #endregion

        public Transfer WrappedTransfer
        {
            get { return transfer; }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return appvm.EmployeesOC; }
        }

        public ObservableCollection<Inventory> Inventories
        {
            get { return appvm.InventoryAreasOC; }
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

        Inventory inventoryFrom;
        public Inventory InventoryFrom 
        {
            get { return inventoryFrom; }
            set
            {
                inventoryFrom = value;
                OnPropertyChanged("InventoryFrom");
                HasPendingChanges = true;
            }
        }

        Inventory inventoryTo;
        public Inventory InventoryTo
        {
            get { return inventoryTo; }
            set
            {
                inventoryTo = value;
                OnPropertyChanged("InventoryTo");
                HasPendingChanges = true;                
            }
        }

        #endregion

        ObservableCollection<TransferItemViewModel> transferItems = new ObservableCollection<TransferItemViewModel>();
        public ObservableCollection<TransferItemViewModel> TransferItems 
        {
            get { return transferItems; }
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

            string message = "¿Está seguro que desea eliminar esta transferencia?";

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                //let parent remove it
                if (onRemoved != null) onRemoved(transfer);

                var ts = base.GetService<ITransactionService>();
                ts.RemoveTransfer(transfer, true);

                appvm.SaveChanges();

                //close this window
                var windowManager = base.GetService<IWindowManager>();

                windowManager.Close(this);
            }
        }

        //void RemoveTransfer(Transfer t)
        //{
        //    var ts = base.GetService<ITransactionService>();

        //    List<TransferItem> items_toRemove = new List<TransferItem>();

        //    foreach (TransferItem item in t.LineItems)
        //    {
        //        items_toRemove.Add(item);
        //    }

        //    foreach (var item in items_toRemove)
        //    {
        //        ts.UndoTransfer(item.Product, item.Quantity, item.UnitMeasure, transfer.Date, transfer.InventoryFrom, transfer.InventoryTo);

        //        appvm.Context.LineItems.DeleteObject(item);
        //    }

        //    //remove from database
        //    appvm.Context.Orders.DeleteObject(t);

        //    appvm.SaveChanges();
        //}

        //void RemoveItem(TransferItem ti)
        //{
        //    var inventory_svc = appvm.InventoryService;

        //    //remove from destination inventory
        //    inventory_svc.ExecuteInventoryOperation(transfer.Date, transfer.InventoryTo, ti.Product, -ti.Quantity, ti.UnitMeasure);

        //    //add to origin inventory
        //    inventory_svc.ExecuteInventoryOperation(transfer.Date, transfer.InventoryFrom, ti.Product, ti.Quantity, ti.UnitMeasure);

        //    appvm.Context.LineItems.DeleteObject(ti);
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

        bool CanSave { get { return inventoryFrom != null & inventoryTo != null; } }

        void Save()
        {
            if (creating)
            {
                transfer = new Transfer();
                transfer.Date = date;
                transfer.DateCreated = dateCreated;
                appvm.Context.Orders.AddObject(transfer);
            }
            
            CopyTo();
            HasPendingChanges = false;

            if (creating) 
            {
                onCreated(transfer);
                creating = false;
            }
            else if (onAssociationChanged != null) onAssociationChanged(transfer);
        }

        #endregion

        #region New Item Command

        public ObservableCollection<Product> InventoryProducts
        {
            get { return appvm.ProductManager.InventoryProducts; }
        }

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
        //    }
        //}

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
            TransferItemViewModel newItemViewModel = new TransferItemViewModel(this, productToAdd, quantityToAdd, baseUM);

            transferItems.Add(newItemViewModel);

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

        public TransferItemViewModel SelectedItem { get; set; }

        public bool CanRemoveItem
        {
            get { return SelectedItem != null; }
        }

        public void RemoveItem(TransferItemViewModel tivm)
        {
            //delete from lineitemviewmodels
            transferItems.Remove(tivm);

            HasPendingChanges = true;
        }

        #endregion

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
