//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.Services;

//using System.Windows.Input;
//using System.Collections.ObjectModel;
//using MiPaladar.Classes;
//using System.Windows;

//namespace MiPaladar.ViewModels
//{
//    public class AdjustmentViewModel : ViewModelBase, IScreen
//    {
//        MainWindowViewModel appvm;
//        Adjustment adjustment;

//        Action<Adjustment> onRemoved;
//        Action<Adjustment> onAssociationChanged;

//        public AdjustmentViewModel(MainWindowViewModel appvm, Adjustment adjustment,
//            Action<Adjustment> onRemoved, Action<Adjustment> onAssociationChanged)
//        {
//            this.appvm = appvm;
//            this.adjustment = adjustment;
//            this.onRemoved = onRemoved;
//            this.onAssociationChanged = onAssociationChanged;

//            CopyFrom();

//            HasPendingChanges = false;
//        }

//        #region Copy Methods

//        void CopyFrom()        
//        {
//            Date = adjustment.Date;            
//            Inventory = adjustment.Inventory;
//            Responsible = adjustment.Employee;
//            Memo = adjustment.Memo;            

//            adjustmentItems.Clear();
//            foreach (AdjustmentItem item in adjustment.LineItems)
//            {

//                AdjustmentItemViewModel pivm = new AdjustmentItemViewModel(this, item);
//                adjustmentItems.Add(pivm);
//            }
//        }

//        void CopyTo()
//        {
//            var inventory_svc = appvm.InventoryService;
//            var ts = base.GetService<ITransactionService>();

//            //remove original items
//            List<AdjustmentItem> toRemove = new List<AdjustmentItem>();
//            foreach (AdjustmentItem item in adjustment.LineItems)
//            {
//                    toRemove.Add(item);
//            }
//            foreach (var item in toRemove)
//            {
//                //RemoveItem(item);
//                //inventory_svc.ExecuteInventoryOperation(adjustment.Inventory, item.Product, -item.Quantity, item.UnitMeasure);

//                ts.UndoAdjust(item.Product, item.Quantity, item.UnitMeasure, item.Cost, adjustment.Date, adjustment.Inventory);

//                appvm.Context.LineItems.DeleteObject(item);
//            }

//            //add current items                
//            foreach (var item in adjustmentItems)
//            {
//                AdjustmentItem newAi = new AdjustmentItem();
//                newAi.Quantity = item.Quantity;
//                newAi.UnitMeasure = item.UnitMeasure;
//                newAi.Product = item.Product;

//                adjustment.LineItems.Add(newAi);

//                decimal cost = ts.Adjust(item.Product, item.Quantity, item.UnitMeasure, date, inventory);

//                newAi.Cost = cost;

//                //inventory_svc.ExecuteInventoryOperation(Date, inventory, item.Product, item.Quantity, item.UnitMeasure);                
//            }

//            //the rest of the properties
//            if (date != adjustment.Date) adjustment.Date = date;
//            if (inventory != adjustment.Inventory) adjustment.Inventory = inventory;
//            if (responsible != adjustment.Employee) adjustment.Employee = responsible;
//            if (memo != adjustment.Memo) adjustment.Memo = memo;

//            appvm.SaveChanges();
//        }

//        #endregion

//        public Adjustment WrappedAdjustment
//        {
//            get { return adjustment; }
//        }

//        public ObservableCollection<Employee> Employees 
//        {
//            get { return appvm.EmployeesOC; }
//        }

//        public ObservableCollection<Inventory> Inventories 
//        {
//            get { return appvm.InventoryAreasOC; }
//        }

//        #region Header

//        DateTime date;
//        public DateTime Date
//        {
//            get { return date; }
//            set 
//            {
//                date = value;
//                OnPropertyChanged("Date");
//                HasPendingChanges = true;
//            }
//        }

//        public DateTime DateCreated
//        {
//            get { return adjustment.DateCreated; }
//        }

//        Inventory inventory;
//        public Inventory Inventory
//        {
//            get { return inventory; }
//            set
//            {
//                inventory = value;
//                OnPropertyChanged("Inventory");
//                HasPendingChanges = true;
//            }
//        }

//        Employee responsible;
//        public Employee Responsible 
//        {
//            get { return responsible; }
//            set
//            {
//                responsible = value;
//                OnPropertyChanged("Responsible");
//                HasPendingChanges = true;
//            }
//        }

//        string memo;
//        public string Memo
//        {
//            get { return memo; }
//            set 
//            {
//                memo = value;
//                OnPropertyChanged("Memo");
//                HasPendingChanges = true;
//            }
//        }

//        #endregion        

//        ObservableCollection<AdjustmentItemViewModel> adjustmentItems = new ObservableCollection<AdjustmentItemViewModel>();
//        public IEnumerable<AdjustmentItemViewModel> AdjustmentItems 
//        {
//            get { return adjustmentItems; }
//        }

//        bool hasPendingChanges;
//        public bool HasPendingChanges 
//        {
//            get { return hasPendingChanges; }
//            set
//            {
//                hasPendingChanges = value;
//                OnPropertyChanged("HasPendingChanges");
//            }
//        }

//        #region Remove Command

//        RelayCommand removeCommand;
//        public ICommand RemoveCommand
//        {
//            get
//            {
//                if (removeCommand == null)
//                    removeCommand = new RelayCommand(x => this.Remove());
//                return removeCommand;
//            }
//        }

//        void Remove()
//        {
//            var msgBox = base.GetService<IMessageBoxService>();

//            string message = "¿Está seguro que desea eliminar este ajuste?";

//            if (msgBox.ShowYesNoDialog(message) == true)
//            {
//                //let parent remove it
//                if (onRemoved != null) onRemoved(adjustment);

//                var ts = base.GetService<ITransactionService>();
//                ts.RemoveAdjustment(adjustment, true);

//                appvm.SaveChanges();

//                //close this window
//                var windowManager = base.GetService<IWindowManager>();

//                windowManager.Close(this);
//            }
//        }
        

//        //void RemoveItem(AdjustmentItem ai)
//        //{
//        //    var inventory_svc = appvm.InventoryService;

//        //    //note -quantity
//        //    inventory_svc.ExecuteInventoryOperation(Date, inventory, ai.Product, -ai.Quantity, ai.UnitMeasure);

//        //    appvm.Context.LineItems.DeleteObject(ai);
//        //}

//        #endregion        

//        #region Cancel Command

//        RelayCommand cancelCommand;
//        public ICommand CancelCommand 
//        {
//            get
//            {
//                if (cancelCommand == null)
//                    cancelCommand = new RelayCommand(x => this.Cancel());
//                return cancelCommand;
//            }
//        }

//        void Cancel() 
//        {
//            CopyFrom();
//            HasPendingChanges = false;
//        }

//        #endregion

//        #region Save Command

//        RelayCommand saveCommand;
//        public ICommand SaveCommand
//        {
//            get
//            {
//                if (saveCommand == null)
//                    saveCommand = new RelayCommand(x => this.Save());
//                return saveCommand;
//            }
//        }

//        bool CanSave { get { return inventory != null; } }

//        void Save()
//        {            
//            CopyTo();            
//            HasPendingChanges = false;
//            if (onAssociationChanged != null) onAssociationChanged(adjustment);
//        }

//        #endregion

//        #region New Item Command

//        public ObservableCollection<Product> InventoryProducts 
//        {
//            get { return appvm.ProductManager.InventoryProducts; }
//        }
        
//        //ProductQuantityViewModel itemToAdd;
//        //public ProductQuantityViewModel ItemToAdd
//        //{
//        //    get
//        //    {
//        //        if (itemToAdd == null) itemToAdd = new ProductQuantityViewModel(appvm);
//        //        return itemToAdd;
//        //    }
//        //    set { itemToAdd = value; }
//        //}

//        //string searchText;
//        //public string SearchText
//        //{
//        //    get { return searchText; }
//        //    set
//        //    {
//        //        searchText = value;
//        //        OnPropertyChanged("SearchText");
//        //    }
//        //}

//        double quantityToAdd;
//        public double QuantityToAdd
//        {
//            get { return quantityToAdd; }
//            set
//            {
//                quantityToAdd = value;
//                OnPropertyChanged("QuantityToAdd");
//            }
//        }

//        Product productToAdd;
//        public Product ProductToAdd
//        {
//            get { return productToAdd; }
//            set
//            {
//                productToAdd = value;

//                OnPropertyChanged("ProductToAdd");
//            }
//        }

//        RelayCommand newItemCommand;
//        public ICommand NewItemCommand
//        {
//            get
//            {
//                if (newItemCommand == null)
//                    newItemCommand = new RelayCommand(x => this.DoNewItem(), x => this.CanNewItem);
//                return newItemCommand;
//            }
//        }

//        public bool CanNewItem
//        {
//            get
//            {
//                return /*!itemToAdd.HasErrors &&*/ quantityToAdd != 0 && productToAdd != null;
//            }
//        }

//        void DoNewItem()
//        {
//            var baseUM = ProductToAdd.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
//            AdjustmentItemViewModel newItemViewModel =
//                new AdjustmentItemViewModel(this, productToAdd, quantityToAdd, baseUM);

//            adjustmentItems.Add(newItemViewModel);

//            HasPendingChanges = true;

//            //clear input fields
//            QuantityToAdd = 0;
//            ProductToAdd = null;
//            //SearchText = string.Empty;

//            OnNewItemAdded();
//        }

//        public event EventHandler NewItemAdded;

//        protected void OnNewItemAdded()
//        {
//            EventHandler handler = this.NewItemAdded;
//            if (handler != null)
//            {
//                handler(this, EventArgs.Empty);
//            }
//        }

//        #endregion

//        #region Remove Item Command

//        RelayCommand removeItemCommand;
//        public ICommand RemoveItemCommand
//        {
//            get
//            {
//                if (removeItemCommand == null)
//                    removeItemCommand = new RelayCommand(x => this.RemoveItem(SelectedItem), x => this.CanRemoveItem);
//                return removeItemCommand;
//            }
//        }

//        public AdjustmentItemViewModel SelectedItem { get; set; }

//        public bool CanRemoveItem
//        {
//            get { return SelectedItem != null; }
//        }

//        public void RemoveItem(AdjustmentItemViewModel aivm)
//        {
//            //delete from lineitemviewmodels
//            adjustmentItems.Remove(aivm);

//            HasPendingChanges = true;
//        }

//        #endregion

//        bool IScreen.TryToClose()
//        {
//            if (hasPendingChanges && this.CanSave)
//            {
//                var msgBox = base.GetService<IMessageBoxService>();
//                if (msgBox != null)
//                {
//                    var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
//                        "Guardar cambios",
//                        MessageBoxButton.YesNoCancel,
//                        MessageBoxImage.Question);

//                    if (result == MessageBoxResult.Cancel)
//                        return false;

//                    if (result == MessageBoxResult.Yes)
//                        this.Save();
//                }
//            }
//            return true;
//        }

//        //var inventory_svc = appvm.InventoryService;

//        ////check for items removed
//        //List<AdjustmentItem> toRemove = new List<AdjustmentItem>();
//        //foreach (var item in adjustment.AdjustmentItems)
//        //{
//        //    if (adjustmentItems.Where(x => x.AdjustmentItem == item).Count() == 0)
//        //        toRemove.Add(item);
//        //}
//        //foreach (var item in toRemove)
//        //{
//        //    appvm.Context.AdjustmentItems.DeleteObject(item);

//        //    inventory_svc.ExecuteInventoryOperation(inventory, item.Product, -item.Quantity, item.UnitMeasure);
//        //}
//        ////check for new items or modified                        
//        //foreach (var item in adjustmentItems)
//        //{
//        //    //it's new
//        //    if (item.AdjustmentItem == null) 
//        //    {
//        //        AdjustmentItem newAi = new AdjustmentItem();
//        //        newAi.Quantity = item.Quantity;
//        //        newAi.UnitMeasure = item.UnitMeasure;
//        //        newAi.Product = item.Product;

//        //        adjustment.AdjustmentItems.Add(newAi);

//        //        inventory_svc.ExecuteInventoryOperation(inventory, item.Product, item.Quantity, item.UnitMeasure);
//        //    }
//        //        //not new
//        //    else
//        //    {
//        //        //check for modifications
//        //        AdjustmentItem currentAi = item.AdjustmentItem;
//        //        float difference = item.Quantity * item.UnitMeasure.ToBaseConversion - currentAi.Quantity * currentAi.UnitMeasure.ToBaseConversion;
//        //        if (difference != 0) 
//        //        {
//        //            currentAi.Quantity = item.Quantity;
//        //            currentAi.UnitMeasure = item.UnitMeasure;

//        //            UnitMeasure family_base = item.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
//        //            inventory_svc.ExecuteInventoryOperation(inventory, item.Product, difference, family_base);
//        //        }
//        //    }
//        //}            
//    }
//}
