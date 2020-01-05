//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Collections.ObjectModel;
//using System.Windows.Data;
//using System.ComponentModel;
//using System.Collections.Specialized;
//using System.Windows.Input;
//using Excel = Microsoft.Office.Interop.Excel;
//using System.Windows;

//using MiPaladar.Classes;
//using MiPaladar.Services;
//using MiPaladar.Entities;
//using MiPaladar.Converters;
//using System.Data.Objects;

//namespace MiPaladar.ViewModels
//{
//    public class AdjustInventoryViewModel : ViewModelBase, IScreen
//    {
//        MainWindowViewModel appvm;
//        Action<Adjustment> onAdjustmentCreated;

//        public AdjustInventoryViewModel(MainWindowViewModel appvm, Action<Adjustment> onAdjustmentCreated)
//        {
//            this.appvm = appvm;
//            this.onAdjustmentCreated = onAdjustmentCreated;

//            appvm.InventoryItemsOC.CollectionChanged +=
//                new NotifyCollectionChangedEventHandler(InventoryItemsOC_CollectionChanged);

//            //base.RequestClose += new EventHandler(AlmacenViewModel_RequestClose);
//        }

//        protected override void OnDispose()
//        {
//            appvm.InventoryItemsOC.CollectionChanged -= InventoryItemsOC_CollectionChanged;

//            foreach (var item in inventoryItems)
//            {
//                item.Dispose();
//            }
//        }

//        public ObservableCollection<Inventory> Inventories 
//        {
//            get { return appvm.InventoryAreasOC; }
//        }

//        Inventory targetInventory;
//        public Inventory TargetInventory 
//        {
//            get { return targetInventory; }
//            set 
//            {
//                targetInventory = value;
//                cvTotals.Refresh();
//            }
//        }

//        #region Inventory Items Events

//        void InventoryItemsOC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.Action == NotifyCollectionChangedAction.Add)
//            {
//                InventoryItem ce = e.NewItems[0] as InventoryItem;

//                AdjustInventoryItemViewModel newitem = new AdjustInventoryItemViewModel(appvm, ce);

//                inventoryItems.Add(newitem);

//                //ce.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
//            }
//            else if (e.Action == NotifyCollectionChangedAction.Remove)
//            {
//                InventoryItem olditem = e.OldItems[0] as InventoryItem;
                                
//                //can remove directly because it's only 1 time
//                foreach (var item in inventoryItems)
//                {
//                    if (item.WrappedInventoryItem == olditem)
//                    {
//                        item.Dispose();
//                        inventoryItems.Remove(item);
//                        break;
//                    }
//                }                                
//            }
//        }

//        #endregion        

//        //public ObservableCollection<Category> Categories 
//        //{
//        //    get { return appvm.CategoriesOC; } 
//        //}

//        ObservableCollection<AdjustInventoryItemViewModel> inventoryItems = new ObservableCollection<AdjustInventoryItemViewModel>();

//        CollectionView cvTotals;
//        public CollectionView TotalsAlmacen
//        {
//            get
//            {
//                if (cvTotals == null)                 
//                {
//                    foreach (var item in appvm.InventoryItemsOC)
//                    {
//                        //round
//                        //double rounded_value = Math.Round(item.Quantity, 2);
//                        //if (rounded_value != item.Quantity)
//                        //{
//                        //    item.Quantity = rounded_value;
//                        //}
//                        AdjustInventoryItemViewModel cevm = new AdjustInventoryItemViewModel(appvm, item);
//                        inventoryItems.Add(cevm);
//                    }

//                    //save rounded values
//                    appvm.SaveChanges();

//                    CollectionViewSource myCVS = new CollectionViewSource();
//                    myCVS.Source = inventoryItems;
//                    cvTotals = (CollectionView)myCVS.View;

//                    //filter
//                    cvTotals.Filter = new Predicate<object>(FilterPredicate);

//                    //sort
//                    cvTotals.SortDescriptions.Add(new SortDescription("Product.Name", ListSortDirection.Ascending));
//                }
                
//                return cvTotals;
//            }
//        }        

//        #region Filtering

//        public bool FilterPredicate(object b)
//        {
//            AdjustInventoryItemViewModel inventoryItem = b as AdjustInventoryItemViewModel;

//            //if (!inventoryItem.Product.IsStorable) return false;

//            if (inventoryItem.Inventory != TargetInventory) return false;

//            //text
//            bool cond = false;
//            if (string.IsNullOrWhiteSpace(findText)) cond = true;
//            else
//            {
//                string prefix = FindText.Trim();

//                if (inventoryItem.Product != null && inventoryItem.Product.Name != null)
//                    cond = inventoryItem.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;
//            }
            
//            //if (!cond) return false;

//            ////product lists
//            //if (filteringByCategory)
//            //{  

//            //    cond = false;

//            //    foreach (ProductIndex item in inventoryItem.Product.RelatedCategories)
//            //    {
//            //        if (item.Category == selectedCategory)
//            //        {
//            //            cond = true;
//            //            break;
//            //        }
//            //    }
//            //}

//            if (!cond) return false;

//            if (filterOnlyModified) 
//            {
//                cond = inventoryItem.Difference != 0;
//            }

//            return cond;
//        }

//        string findText;
//        public string FindText
//        {
//            get { return findText; }
//            set
//            {
//                findText = value;
//                if (cvTotals != null) cvTotals.Refresh();
//            }
//        }

//        //bool filteringByCategory;
//        //public bool FilteringByCategory
//        //{
//        //    get { return filteringByCategory; }
//        //    set
//        //    {
//        //        filteringByCategory = value;

//        //        cvTotals.Refresh();
//        //    }
//        //}

//        //Category selectedCategory;
//        //public Category SelectedCategory
//        //{
//        //    get { return selectedCategory; }
//        //    set 
//        //    {
//        //        selectedCategory = value; 
//        //        if (FilteringByCategory) cvTotals.Refresh(); 
//        //    }
//        //}

//        bool filterOnlyModified;
//        public bool FilterOnlyModified
//        {
//            get { return filterOnlyModified; }
//            set
//            {
//                filterOnlyModified = value;

//                cvTotals.Refresh();
//            }
//        }

//        #endregion  
      
//        #region Cancel Command

//        RelayCommand cancelCommand;
//        public ICommand CancelCommand
//        {
//            get
//            {
//                if (cancelCommand == null) cancelCommand = new RelayCommand(x => this.DoCancel());

//                return cancelCommand;
//            }
//        }

//        void DoCancel()
//        {
//            var msgBox = base.GetService<IMessageBoxService>();

//            if (msgBox.ShowYesNoDialog("¿Está seguro que desea cancelar? Perderá todos los cambios.") == true) 
//            {
//                var windowManager = base.GetService<IWindowManager>();
//                SelfClosing = true;
//                windowManager.Close(this);
//            }
//        }

//        #endregion

//        #region Save Command

//        RelayCommand saveCommand;
//        public ICommand SaveCommand 
//        {
//            get
//            {
//                if (saveCommand == null) saveCommand = new RelayCommand(x => this.Save(), x => this.CanSave);

//                return saveCommand; 
//            }

//        }

//        bool CanSave 
//        {
//            get { return TargetInventory != null; }
//        }

//        void Save()
//        {
//            Adjustment newAdj = new Adjustment();

//            newAdj.Date = DateTime.Today;
//            newAdj.DateCreated = DateTime.Now;
//            newAdj.Inventory = TargetInventory;

//            appvm.Context.Orders.AddObject(newAdj);

//            foreach (var item in inventoryItems)
//            {
//                if (item.Inventory == targetInventory && item.Difference != 0)
//                {
//                    AdjustmentItem newItem = new AdjustmentItem();
//                    newItem.Product = item.Product;
//                    newItem.UnitMeasure = item.UnitMeasure;
//                    newItem.Quantity = item.Difference;

//                    newItem.Order = newAdj;

//                    var ts = base.GetService<ITransactionService>();
//                    decimal cost = ts.Adjust(item.Product, item.Difference, item.UnitMeasure, newAdj.DateCreated, targetInventory);

//                    newItem.Cost = item.Difference > 0 ? cost : -cost;

//                    //ExecuteInventoryOperation(item.Product, item.Difference, item.UnitMeasure);
//                }
//            }

//            appvm.SaveChanges();

//            if (onAdjustmentCreated != null)
//            {
//                onAdjustmentCreated(newAdj);
//            }

//            var windowManager = base.GetService<IWindowManager>();
//            SelfClosing = true;
//            windowManager.Close(this);
//        }

//        public bool SelfClosing { get; set; }

//        //private void ExecuteInventoryOperation(Product product, double qtty, UnitMeasure unitMeasure)
//        //{
//        //    var inventorySvc = appvm.InventoryService;

//        //    inventorySvc.ExecuteInventoryOperation(DateTime.Now, targetInventory, product, qtty, unitMeasure);
//        //}

//        #endregion

//        bool IScreen.TryToClose()
//        {
//            if (this.CanSave)
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
//    }
//}
