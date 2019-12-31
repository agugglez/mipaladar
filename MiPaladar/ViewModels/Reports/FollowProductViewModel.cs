using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Enums;
using MiPaladar.Services;
using MiPaladar.Views;


namespace MiPaladar.ViewModels
{
    public class FollowProductViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        Inventory piso_inventory;

        public FollowProductViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            fromDate = DateTime.Today;
            toDate = DateTime.Today;


            piso_inventory = appvm.InventoryAreasOC.Where(x => x.IsFloor).FirstOrDefault();

            SelectedInventoryArea = piso_inventory;

            groupingByDate = true;

            //UpdateSearch();
        }

        public override string DisplayName
        {
            get { return "Operaciones"; }
        }
        
        #region Dates

        DateTime fromDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        DateTime toDate;
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        #endregion                

        public ObservableCollection<Product> Products 
        {
            get { return appvm.ProductManager.InventoryProducts; }
        }

        public ObservableCollection<Inventory> InventoryAreas
        {
            get { return appvm.InventoryAreasOC; }
        }

        ObservableCollection<FollowProductItemViewModel> itemsShowing= new ObservableCollection<FollowProductItemViewModel>();
        public ObservableCollection<FollowProductItemViewModel> ItemsShowing
        {
            get 
            {
                //if (itemsShowing == null) 
                //{
                //    itemsShowing = new ObservableCollection<FollowProductItemViewModel>();

                //    //CollectionViewSource cvs = new CollectionViewSource();
                //    //cvs.Source = fp_items;
                //    //auxView = (CollectionView)cvs.View;

                //    //auxView.Filter = FilterProduct;

                //    //UpdateSearch();
                //}
                return itemsShowing; 
            }
        }

        #region Show Order Command

        RelayCommand showOrderCommand;
        public ICommand ShowOrderCommand
        {
            get
            {
                if (showOrderCommand == null)
                    showOrderCommand = new RelayCommand(x => ShowOrder(x));
                return showOrderCommand;
            }
        }

        private void ShowOrder(object x)
        {
            var windowManager = base.GetService<IWindowManager>();

            if (x is Sale)
            {
                Sale sale = (Sale)x;

                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
                {
                    if (!(wsvm is SaleViewModel)) return false;

                    SaleViewModel svm = (SaleViewModel)wsvm;

                    return svm.SalesOrder == sale;
                };

                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
                else
                {
                    SaleViewModel viewmodel = new SaleViewModel(appvm, sale, null);
                    windowManager.ShowChildWindow(viewmodel, this);
                }
            }
            else if (x is Purchase)
            {
                Purchase purchase = (Purchase)x;

                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
                {
                    if (!(wsvm is PurchaseViewModel)) return false;

                    PurchaseViewModel pvm = (PurchaseViewModel)wsvm;

                    return pvm.WrappedPurchase == purchase;
                };

                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
                else
                {
                    PurchaseViewModel viewmodel = new PurchaseViewModel(appvm, purchase);
                    windowManager.ShowChildWindow(viewmodel, this);
                }
            }
            else if (x is Adjustment) 
            {
                Adjustment adjustment = (Adjustment)x;

                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
                {
                    if (!(wsvm is AdjustmentViewModel)) return false;

                    AdjustmentViewModel pvm = (AdjustmentViewModel)wsvm;

                    return pvm.WrappedAdjustment == adjustment;
                };

                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
                else
                {
                    AdjustmentViewModel viewmodel = new AdjustmentViewModel(appvm, adjustment, null, null);
                    windowManager.ShowChildWindow(viewmodel, this);
                }
            }
            else if (x is Transfer)
            {
                Transfer transfer = (Transfer)x;

                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
                {
                    if (!(wsvm is TransferViewModel)) return false;

                    TransferViewModel pvm = (TransferViewModel)wsvm;

                    return pvm.WrappedTransfer == transfer;
                };

                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
                else
                {
                    TransferViewModel viewmodel = new TransferViewModel(appvm, transfer, null, null);
                    windowManager.ShowChildWindow(viewmodel, this);
                }
            }
            else if (x is Production)
            {
                Production production = (Production)x;

                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
                {
                    if (!(wsvm is ProductionViewModel)) return false;

                    ProductionViewModel pvm = (ProductionViewModel)wsvm;

                    return pvm.WrappedProduction == production;
                };

                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
                else
                {
                    ProductionViewModel viewmodel = new ProductionViewModel(appvm, production, null, null);
                    windowManager.ShowChildWindow(viewmodel, this);
                }
            }
        }

        //void OnRemoved(object order)
        //{
        //    foreach (var item in fp_items)
        //    {
        //        if (item.Order == order)
        //        {
        //            fp_items.Remove(item);
        //            break;
        //        }
        //    }
        //}

        //void OnAssociationChanged(object o)
        //{
        //    int index = -1;
        //    FollowProductItemViewModel removed_fp=null;
        //    for (int i = 0; i < fp_items.Count; i++)
        //    {
        //        if (fp_items[i].Order == o) 
        //        {
        //            index = i; removed_fp = fp_items[i];
        //            break; 
        //        }
        //    }
        //    if (index >= 0) 
        //    {
        //        fp_items.RemoveAt(index);
        //        fp_items.Insert(index, removed_fp);
        //    }
        //}        

        #endregion
        
        #region Find Command

        public Product SelectedProduct { get; set; }

        public Inventory SelectedInventoryArea { get; set; }

        bool busy;
        public bool Busy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged("Busy");
            }
        }

        RelayCommand findCommand;
        public ICommand FindCommand
        {
            get
            {
                if (findCommand == null)
                {
                    findCommand = new RelayCommand(x => this.UpdateSearch(), x => this.CanFind);
                }
                return findCommand;
            }
        }

        public bool CanFind
        {
            get { return !busy && SelectedProduct != null && SelectedInventoryArea != null; }
        }

        BackgroundWorker bWorker;

        //CollectionView auxView;
        ObservableCollection<FollowProductItemViewModel> fp_items = new ObservableCollection<FollowProductItemViewModel>();        

        public void UpdateSearch()
        {
            if (bWorker == null)
            {
                bWorker = new BackgroundWorker();

                bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
                bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);
            }

            Busy = true;
            CommandManager.InvalidateRequerySuggested();

            bWorker.RunWorkerAsync();
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CreateFPItems();
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CreateItemsShowing();

            UpdateColumnsVisibility();

            UpdateTotals();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        #region Create FP_Items

        void CreateFPItems()
        {
            fp_items.Clear();

            DateTime toDatePlusOne = toDate.AddDays(1);

            //get data
            var lineitems_query =
                from lineitem in appvm.Context.LineItems
                where lineitem.Order.Date >= fromDate && lineitem.Order.Date < toDatePlusOne
                select lineitem;

            //
            foreach (var item in lineitems_query)
            {
                if (item.Order is Sale)
                {
                    if (SelectedInventoryArea != piso_inventory) continue;

                    //consider ingredients of sold products
                    if (item.Product.IsRecipe)
                    {
                        AddIngredientsFromRecipe(item);
                    }
                }
                else if (item.Order is Purchase)
                {
                    if (((Purchase)item.Order).Inventory != SelectedInventoryArea) continue;
                }
                else if (item.Order is Adjustment)
                {
                    if (((Adjustment)item.Order).Inventory != SelectedInventoryArea) continue;
                }
                else if (item.Order is Transfer)
                {
                    if (((Transfer)item.Order).InventoryFrom != SelectedInventoryArea &&
                        ((Transfer)item.Order).InventoryTo != SelectedInventoryArea) continue;
                }
                else if (item.Order is Production)
                {
                    if (((Production)item.Order).Inventory != SelectedInventoryArea) continue;
                }

                if (item.Product == SelectedProduct) fp_items.Add(CreateFPFromLineItem(item));
            }

            //inventory traces
            var inventorytrace_query =
                from iTrace in appvm.Context.InventoryTraces
                where iTrace.Date >= fromDate && iTrace.Date <= toDate && iTrace.Product.Id == SelectedProduct.Id
                select iTrace;

            foreach (var item in inventorytrace_query)
            {
                if (item.Inventory != SelectedInventoryArea) continue;
                //FollowProductItemViewModel copy = new FollowProductItemViewModel(item);

                fp_items.Add(CreateFPFromITrace(item));
            }
        }

        FollowProductItemViewModel CreateFPFromLineItem(LineItem li) 
        {
            FollowProductItemViewModel new_fp = new FollowProductItemViewModel();

            if (li.Order is Sale)
            {
                if (li.Quantity >= 0) new_fp.OperationType = InventoryOperationType.Salida;
                else new_fp.OperationType = InventoryOperationType.Entrada;

                //new_fp.InventoryArea = piso_inventory;
            }
            else if (li.Order is Transfer) 
            {
                Transfer t = (Transfer)li.Order;
                if (t.InventoryTo == SelectedInventoryArea) new_fp.OperationType = InventoryOperationType.Entrada;
                else new_fp.OperationType = InventoryOperationType.Salida;
            }
            else if (li.Order is Purchase || li.Order is Adjustment || li.Order is Production) 
            {
                if (li.Quantity >= 0) new_fp.OperationType = InventoryOperationType.Entrada;
                else new_fp.OperationType = InventoryOperationType.Salida;

                //new_fp.InventoryArea = ((Purchase)li.Order).Inventory;
            }

            new_fp.Quantity = Math.Abs(li.Quantity);
            new_fp.UnitMeasure = li.UnitMeasure;
            new_fp.Product = li.Product;

            //get this from Order
            new_fp.Date = li.Order.Date;

            new_fp.Order = li.Order;

            return new_fp;
        }

        void AddIngredientsFromRecipe(LineItem recipe_li)
        {
            foreach (var ingredient in recipe_li.Product.Ingredients)
            {
                if (ingredient.IngredientProduct != SelectedProduct) continue;

                fp_items.Add(CreateFPFromIngredient(ingredient, recipe_li));
            }
        }

        FollowProductItemViewModel CreateFPFromIngredient(Ingredient ingredient, LineItem recipe_li) 
        {
            FollowProductItemViewModel ing_copy = new FollowProductItemViewModel();

            ing_copy.OperationType = InventoryOperationType.Salida;

            ing_copy.Product = ingredient.IngredientProduct;

            double total_qtty = recipe_li.Quantity /*/ recipe_li.Product.RecipeQuantity)*/ * ingredient.Quantity;
            ing_copy.Quantity = total_qtty;
            ing_copy.UnitMeasure = ingredient.UnitMeasure;            

            //ing_copy.InventoryArea = piso_inventory;

            ing_copy.Date = recipe_li.Order.DateCreated;

            ing_copy.Order = recipe_li.Order;

            return ing_copy;
        }

        FollowProductItemViewModel CreateFPFromITrace(InventoryTrace iTrace) 
        {
            FollowProductItemViewModel new_fp = new FollowProductItemViewModel();

            new_fp.OperationType = InventoryOperationType.Inicio;

            new_fp.Date = iTrace.Date;
            //new_fp.InventoryArea = iTrace.Inventory;
            new_fp.Product = iTrace.Product;
            new_fp.Quantity = iTrace.Quantity;
            new_fp.UnitMeasure = iTrace.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

            return new_fp;
        }

        #endregion

        #endregion

        #region Grouping
        
        //int groupingCount;

        //bool noGrouping;
        //public bool NoGrouping 
        //{
        //    get { return noGrouping; }
        //    set
        //    {
        //        if (noGrouping != value) 
        //        {
        //            noGrouping = value;

        //            groupingCount++;

        //            if (groupingCount % 2 == 0)
        //            {
        //                RefreshView();
        //            }
        //        }                
        //    }
        //}

        bool groupingByDate;
        public bool GroupingByDate 
        {
            get { return groupingByDate; }
            set
            {
                if (groupingByDate != value) 
                {
                    groupingByDate = value;

                    CreateItemsShowing();

                    UpdateColumnsVisibility();
                }                
            }
        }

        void CreateItemsShowing() 
        {
            itemsShowing.Clear();

            if (fp_items.Count == 0) return;

            UnitMeasure baseUM = SelectedProduct.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

            //if not totalizing simply copy from fp_items
            if (!groupingByDate)
            {
                foreach (var item in fp_items)
                {
                    itemsShowing.Add(item);
                }
            }
            else
            {
                var groupsByDate = from fp in fp_items
                                    group fp by fp.Date.Date;

                foreach (var group in groupsByDate)
                {
                    FollowProductItemViewModel lic = new FollowProductItemViewModel();

                    lic.Product = SelectedProduct;
                    lic.UnitMeasure = baseUM;

                    lic.Date = group.Key;

                    //totalize all items in group                    
                    MakeSum(group, lic);

                    itemsShowing.Add(lic);
                }                                      
            }
        }

        private void MakeSum(IEnumerable<FollowProductItemViewModel> group, FollowProductItemViewModel lic)
        {
            //float tempQuantity = 0;
            double initialQuantity = 0;
            double inQuantity = 0;
            double outQuantity = 0;

            foreach (var item in group)
            {
                FollowProductItemViewModel x = (FollowProductItemViewModel)item;

                if (x.OperationType == InventoryOperationType.Salida)
                    outQuantity += x.Quantity * x.UnitMeasure.ToBaseConversion;
                else if (x.OperationType == InventoryOperationType.Entrada)
                    inQuantity += x.Quantity * x.UnitMeasure.ToBaseConversion;
                else if (x.OperationType == InventoryOperationType.Inicio)
                    initialQuantity = x.Quantity;

                //tempQuantity += x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion;
            }
            //lic.Quantity = group.Items.Sum(x => ((LineItemCopy)x).Quantity);
            //lic.Price = group.Items.Sum(x => ((LineItemCopy)x).Price);

            UnitMeasure costUM = lic.Product.CostUnitMeasure;
            double rate = costUM.ToBaseConversion;

            bool divide = rate != 1 && (Math.Abs(initialQuantity) >= rate || Math.Abs(inQuantity) >= rate ||Math.Abs(outQuantity) >= rate);

            lic.InitialQuantity = divide ? initialQuantity / rate : initialQuantity;
            lic.InQuantity = divide ? inQuantity / rate : inQuantity;
            lic.OutQuantity = divide ? outQuantity / rate : outQuantity;

            if (divide) lic.UnitMeasure = costUM;
        }

        #endregion               

        #region Totals

        void UpdateTotals()
        {
            totalIn = 0;
            totalOut = 0;

            foreach (var item in itemsShowing)
            {
                totalIn += item.InQuantity;
                totalOut += item.OutQuantity;
            }

            TotalIn = totalIn;
            TotalOut = totalOut;
        }

        double totalIn;
        public double TotalIn
        {
            get { return totalIn; }
            set
            {
                totalIn = value;
                OnPropertyChanged("TotalIn");
            }
        }

        double totalOut;
        public double TotalOut
        {
            get { return totalOut; }
            set
            {
                totalOut = value;
                OnPropertyChanged("TotalOut");
            }
        }

        #endregion

        #region Columns Visibility

        void UpdateColumnsVisibility() 
        {
            OperationTypeColumnVisible = !groupingByDate;
            DateColumnVisible = !groupingByDate || groupingByDate;
            QuantityColumnVisible = !groupingByDate;
            InitialQuantityColumnVisible = groupingByDate;
            InQuantityColumnVisible = groupingByDate;
            OutQuantityColumnVisible = groupingByDate;
            ProductColumnVisible = true;
            OrderColumnVisible = !groupingByDate;
        }        

        bool dateColumnVisible;
        public bool DateColumnVisible
        {
            get { return dateColumnVisible; }
            set 
            {
                dateColumnVisible = value;
                OnPropertyChanged("DateColumnVisible");
            }
        }

        bool operationTypeColumnVisible;
        public bool OperationTypeColumnVisible
        {
            get { return operationTypeColumnVisible; }
            set
            {
                operationTypeColumnVisible = value;
                OnPropertyChanged("OperationTypeColumnVisible");
            }
        }

        bool quantityColumnVisible;
        public bool QuantityColumnVisible
        {
            get { return quantityColumnVisible; }
            set 
            {
                quantityColumnVisible = value;
                OnPropertyChanged("QuantityColumnVisible");
            }
        }

        bool initialQuantityColumnVisible;
        public bool InitialQuantityColumnVisible
        {
            get { return initialQuantityColumnVisible; }
            set
            {
                initialQuantityColumnVisible = value;
                OnPropertyChanged("InitialQuantityColumnVisible");
            }
        }

        bool inQuantityColumnVisible;
        public bool InQuantityColumnVisible
        {
            get { return inQuantityColumnVisible; }
            set
            {
                inQuantityColumnVisible = value;
                OnPropertyChanged("InQuantityColumnVisible");
            }
        }

        bool outQuantityColumnVisible;
        public bool OutQuantityColumnVisible
        {
            get { return outQuantityColumnVisible; }
            set
            {
                outQuantityColumnVisible = value;
                OnPropertyChanged("OutQuantityColumnVisible");
            }
        }

        bool productColumnVisible;
        public bool ProductColumnVisible
        {
            get { return productColumnVisible; }
            set
            {
                productColumnVisible = value;
                OnPropertyChanged("ProductColumnVisible");
            }
        }        

        bool orderColumnVisible;
        public bool OrderColumnVisible
        {
            get { return orderColumnVisible; }
            set
            {
                orderColumnVisible = value;
                OnPropertyChanged("OrderColumnVisible");
            }
        }

        #endregion
    }
}
