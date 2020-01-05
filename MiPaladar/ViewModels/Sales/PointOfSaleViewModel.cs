//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Services;
//using MiPaladar.Enums;
//using MiPaladar.Classes;
//using MiPaladar.Entities;

//using System.Collections.ObjectModel;
//using System.Windows.Input;
//using System.ComponentModel;
//using System.Windows.Data;
//using MiPaladar.Views;

//namespace MiPaladar.ViewModels
//{
//    public class PointOfSaleViewModel : ViewModelBase
//    {
//        MainWindowViewModel appvm;
//        IInventoryService inventoryService;
//        //IPasswordAsker passwordAsker;
//        IConfirmator confirmator;

//        //UserManager personnelManager;

//        public PointOfSaleViewModel(MainWindowViewModel appvm)
//        {
//            this.appvm = appvm;
//            this.inventoryService = appvm.InventoryService;
//            //this.passwordAsker = passwordAsker;
//            this.confirmator = appvm.Confirmator;
//            //this.personnelManager = appvm.PersonnelManager;

//            //Initialize();

//            workingDate = DateTime.Today;

//            miniTotals = new TotalsByWaiterViewModel(appvm);
//        }

//         public override string DisplayName
//         {
//             get { return "PUNTO DE VENTA"; }
//         }

//        protected override void OnDispose()
//        {
//            //appvm.PropertyChanged -= appvm_PropertyChanged;

//            //miniInventoryVM.CloseCommand.Execute(null);
//            //miniMenuVM.CloseCommand.Execute(null);

//            if (miniInventoryVM != null) miniInventoryVM.Dispose();
//            if (miniMenuVM != null) miniMenuVM.Dispose();

//            //foreach (var item in valeModels)
//            //{
//            //    item.PropertyChanged -= vale_PropertyChanged;
//            //    //item.RemoveEventHandlers();
//            //}
//        }

//        //void VentasViewModel_RequestClose(object sender, EventArgs e)
//        //{
//        //    //appvm.PropertyChanged -= appvm_PropertyChanged;

//        //    //miniInventoryVM.CloseCommand.Execute(null);
//        //    //miniMenuVM.CloseCommand.Execute(null);

//        //    if (miniInventoryVM != null) miniInventoryVM.CloseCommand.Execute(null);
//        //    if (miniMenuVM != null) miniMenuVM.CloseCommand.Execute(null);

//        //    //foreach (var item in valeModels)
//        //    //{
//        //    //    item.PropertyChanged -= vale_PropertyChanged;
//        //    //    //item.RemoveEventHandlers();
//        //    //}
//        //}

//        public MainWindowViewModel AppVM 
//        {
//            get { return appvm; }
//        }

//        public void ChangeDay(DateTime date)
//        {
//            //WorkingDate = date;

//            inventoryService.CreateInventorySnapshot(date);

//            LoadVales();

//            //UpdateCuadreMessage();

//            UpdateTotals();

//            miniTotals.ShortUpdateTotalsByWaiter(workingDate);
//        }        

//        private void LoadVales()
//        {
//            //unsubscribe events
//            //foreach (var item in valeModels)
//            //{
//            //    item.PropertyChanged -= vale_PropertyChanged;
//            //    //item.RemoveEvents();
//            //}
//            //clear vales
//            valeModels.Clear();

//            //gives error if comparing date directly
//            var queryResult = from o in appvm.Context.Orders.OfType<Sale>()
//                              where o.Date.Day == workingDate.Day &&
//                                    o.Date.Month == workingDate.Month &&
//                                    o.Date.Year == workingDate.Year
//                              //where o.Waiter.Name=="Diango"
//                              select o;

//            foreach (var item in queryResult)
//            {
//                //Action<SaleViewModel> showOrder = x => 
//                //{
//                //    ShowingSale = true;
//                //    //SelectedOrder = x; 
//                //};
//                SaleViewModel sovm = new SaleViewModel(appvm, item, OnRemoved, OnCheckOut, OnClientsChanged, OnWaiterChanged);
//                valeModels.Add(sovm);

//                //sovm.PropertyChanged += new PropertyChangedEventHandler(vale_PropertyChanged);                
//            }

//            //if (icvAvailableTables != null) icvAvailableTables.Refresh();

//            //if (valeModels.Count > 0) SelectedOrder = valeModels[0];
//        }

//        void OnCheckOut() 
//        {
//            miniTotals.ShortUpdateTotalsByWaiter(workingDate);
//            UpdateTotals();
//        }
//        void OnClientsChanged() 
//        {
//            miniTotals.ShortUpdateTotalsByWaiter(workingDate);
//            UpdateTotals();            
//        }
//        void OnWaiterChanged() 
//        {
//            miniTotals.ShortUpdateTotalsByWaiter(workingDate);
//        }

//        void OnRemoved(Sale s)
//        {
//            SaleViewModel svm_to_remove = valeModels.Single(x => x.SalesOrder == s);

//            //svm_to_remove.PropertyChanged -= vale_PropertyChanged;

//            //remove from viewmodel list
//            valeModels.Remove(svm_to_remove);

//            //icvAvailableTables.Refresh();

//            //update totals
//            UpdateTotals();
//            miniTotals.ShortUpdateTotalsByWaiter(workingDate);
//            //UpdateCuadreMessage();
//        }

//        //void vale_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        //{
//        //    SaleViewModel svm = (SaleViewModel)sender;

//        //    //if (e.PropertyName == "TotalPrice")
//        //    //{
//        //    //    if (svm.Paid) UpdateCuadreMessage();
//        //    //}           
//        //    //else 
//        //        if (e.PropertyName == "Paid")
//        //    {
//        //        //make table available
//        //        //icvAvailableTables.Refresh();

//        //        //ShowingSale = false;

//        //        //UpdateCuadreMessage();
//        //        ShortUpdateTotalsByWaiter();
//        //        UpdateTotals();
//        //    }
//        //    else if (e.PropertyName == "Waiter")
//        //    {
//        //        ShortUpdateTotalsByWaiter();
//        //    }
//        //    else if (e.PropertyName == "Persons") 
//        //    {
//        //        ShortUpdateTotalsByWaiter();
//        //        TotalClients = valeModels.Sum(x => x.Persons);
//        //    }
//        //    //else if (e.PropertyName == "Table")
//        //    //{
//        //    //    icvAvailableTables.Refresh();
//        //    //}
//        //}

//        int GenerateId()
//        {
//            int newID = 1;

//            if (valeModels.Count() > 0)
//            {
//                int? maxIDvale = valeModels.Max(x => x.Number);

//                int max_value = maxIDvale.HasValue ? maxIDvale.Value : 0;

//                if (maxIDvale < 100) newID = max_value + 1;
//                else
//                {
//                    //find smallest unused ID
//                    while (valeModels.Where(v => v.Number == newID).Count() > 0) newID++;
//                }
//            }

//            return newID;
//        }

//        public void CreateNewVale() 
//        {
//            CreateNewVale(null, null, 0);
//        }

//        public SaleViewModel CreateNewVale(Table table, Employee waiter, int numberOfPersons)
//        {
//            //find a new ID for the new order

//            int newID = GenerateId();            

//            //create the new order
//            Sale newvale = new Sale();
//            newvale.Number = newID;

//            newvale.DateCreated = DateTime.Now;
//            newvale.Date = workingDate;

//            if (table != null) newvale.Table = table;
//            if (waiter != null) newvale.Employee = waiter;
//            if (numberOfPersons > 0) newvale.Persons = numberOfPersons;

//            appvm.Context.Orders.AddObject(newvale);

//            appvm.SaveChanges();

//            //Action<SaleViewModel> showOrder = x =>
//            //{
//            //    ShowingSale = true;
//            //    //SelectedOrder = x;
//            //};

//            //Action<List<InventoryItem>> updateMessage = x =>
//            //{
//            //    UpdateProductsUnavailableMessage(x);
//            //};

//            //create a view model for the new order
//            SaleViewModel sovm = new SaleViewModel(appvm, newvale, OnRemoved, OnCheckOut, OnClientsChanged, OnWaiterChanged);

//            //sovm.PropertyChanged += new PropertyChangedEventHandler(vale_PropertyChanged);

//            valeModels.Add(sovm);

//            //icvAvailableTables.Refresh();

//            //UpdateCuadreMessage();
//            //TotalOrders += 1;            

//            //select the new order
//            SelectedOrder = sovm;            

//            return sovm;
//        }

//        //public void RemoveVale(SaleViewModel vale)
//        //{
//        //    vale.PropertyChanged -= vale_PropertyChanged;

//        //    //remove from viewmodel list
//        //    valeModels.Remove(vale);

//        //    //icvAvailableTables.Refresh();

//        //    //update totals
//        //    UpdateTotals();
//        //    ShortUpdateTotalsByWaiter();
//        //    UpdateCuadreMessage();

//        //    //empty vale & add products to inventory
//        //    while (vale.OrderItems.Count > 0)
//        //    {
//        //        LineItemViewModel current = vale.OrderItems[0];

//        //        vale.RemoveLineItem(current);
//        //    }            

//        //    //remove from database
//        //    appvm.Context.Orders.DeleteObject(vale.SalesOrder);

//        //    appvm.SaveChanges();                        
//        //}

//        #region Totals

//        void UpdateTotals()
//        {
//            TotalVentas = 0;
//            TotalOrders = 0;
//            TotalClients = 0;
//            TaxTotal = 0;
//            Tips = 0;

//            foreach (SaleViewModel vale in valeModels)
//            {
//                if (vale.Paid)
//                {
//                    TotalOrders++;
//                    TotalClients += vale.Persons;
//                    TotalVentas += vale.TotalPrice;
//                    TaxTotal += vale.TaxToMoney;
//                    Tips += vale.Tips;
//                }
//            }
//        }

//        //void UpdateCuadreMessage() 
//        //{
//        //    int pending_vales = valeModels.Count(x => !x.Paid);

//        //    decimal current_sales = valeModels.Where(x => x.Paid).Sum(x => x.TotalPrice);
//        //    TotalVentas = current_sales;

//        //    CuadreMessage = current_sales.ToString("c") + (pending_vales > 0 ? " + " 
//        //        + pending_vales + " pendiente(s)" : string.Empty);
//        //}

//        //string cuadreMessage;
//        //public string CuadreMessage 
//        //{
//        //    get 
//        //    {
//        //        if (cuadreMessage == null) UpdateCuadreMessage();
//        //        return cuadreMessage;
//        //    }
//        //    set
//        //    {
//        //        cuadreMessage = value;
//        //        OnPropertyChanged("CuadreMessage");
//        //    }
//        //}

//        decimal totalVentas;
//        public decimal TotalVentas
//        {
//            get { return totalVentas; }
//            set
//            {
//                totalVentas = value;
//                OnPropertyChanged("TotalVentas");
//            }
//        }

//        int totalClients;
//        public int TotalClients 
//        {
//            get { return totalClients; }
//            set
//            {
//                totalClients = value;
//                OnPropertyChanged("TotalClients");
//            }
//        }

//        int totalOrders;
//        public int TotalOrders
//        {
//            get { return totalOrders; }
//            set
//            {
//                totalOrders = value;
//                OnPropertyChanged("TotalOrders");
//            }
//        }

        

//        decimal taxTotal;
//        public decimal TaxTotal
//        {
//            get { return taxTotal; }
//            set
//            {
//                taxTotal = value;
//                OnPropertyChanged("TaxTotal");
//            }
//        }

//        decimal tips;
//        public decimal Tips
//        {
//            get { return tips; }
//            set
//            {
//                tips = value;
//                OnPropertyChanged("Tips");
//            }
//        }

//        #endregion

//        public ObservableCollection<TotalByDependiente> ShortTotalsByWaiter
//        {
//            get { return miniTotals.ShortTotalsByWaiter; }
//        }

//        ObservableCollection<SaleViewModel> valeModels;
//        public ObservableCollection<SaleViewModel> Vales
//        {
//            get 
//            {
//                if (valeModels == null)
//                {
//                    valeModels = new ObservableCollection<SaleViewModel>();

//                    WorkingDate = DateTime.Today;
//                }
//                return valeModels; 
//            }
//        }

//        //ICollectionView icvVales;
//        //public ICollectionView IcvVales 
//        //{
//        //    get
//        //    {
//        //        CollectionViewSource cvs = new CollectionViewSource();
//        //        cvs.Source = valeModels;
//        //        icvVales = cvs.View;

//        //        SortDescription sd = new SortDescription("RealDateTime", ListSortDirection.Ascending);
//        //        icvVales.SortDescriptions.Add(sd);

//        //        return icvVales; 
//        //    }
//        //}

//        #region Create New Sale Dialog Members

//        //ICollectionView icvAvailableTables;
//        //public ICollectionView IcvAvailableTables
//        //{
//        //    get
//        //    {
//        //        if (icvAvailableTables == null)
//        //        {
//        //            CollectionViewSource cvs = new CollectionViewSource();
//        //            cvs.Source = appvm.TablesOC;
//        //            icvAvailableTables = cvs.View;

//        //            var sortDesc = new SortDescription("Number", ListSortDirection.Ascending);
//        //            icvAvailableTables.SortDescriptions.Add(sortDesc);

//        //            var groupDesc = new PropertyGroupDescription("PriceList");
//        //            icvAvailableTables.GroupDescriptions.Add(groupDesc);

//        //            icvAvailableTables.Filter = TableIsAvailable;
//        //        }
//        //        return icvAvailableTables;
//        //    }
//        //}

//        //bool TableIsAvailable(object o)
//        //{
//        //    Table table = (Table)o;

//        //    if (table.IsBar) return true;

//        //    foreach (var item in valeModels)
//        //    {
//        //        if (!item.Paid && item.Table == table) return false;
//        //    }

//        //    return true;
//        //}

//        //used when creating a new vale
//        //public ICollectionView Waiters
//        //{
//        //    get { return personnelManager.CanSellEmployees; }
//        //}

//        //Table selectedTable;
//        //public Table SelectedTable
//        //{
//        //    get { return selectedTable; }
//        //    set
//        //    {
//        //        selectedTable = value;
//        //        OnPropertyChanged("SelectedTable");
//        //    }
//        //}

//        //Employee selectedWaiter;
//        //public Employee SelectedWaiter
//        //{
//        //    get { return selectedWaiter; }
//        //    set
//        //    {
//        //        selectedWaiter = value;
//        //        OnPropertyChanged("SelectedWaiter");
//        //    }
//        //}

//        //int numberOfPersons;
//        //public int NumberOfPersons
//        //{
//        //    get { return numberOfPersons; }
//        //    set
//        //    {
//        //        numberOfPersons = value;
//        //        OnPropertyChanged("NumberOfPersons");
//        //    }
//        //}

//        //RelayCommand newSaleCommand;
//        //public ICommand NewSaleCommand
//        //{
//        //    get
//        //    {
//        //        if (newSaleCommand == null)
//        //            newSaleCommand = new RelayCommand(x => this.DoNewSale(), x => this.CanNewSale);

//        //        return newSaleCommand;
//        //    }
//        //}

//        //bool CanNewSale 
//        //{ 
//        //    get 
//        //    {
//        //        return selectedTable != null && selectedWaiter != null && numberOfPersons > 0;
//        //    }
//        //}

//        //void DoNewSale()
//        //{
//        //    CreateNewVale(selectedTable, selectedWaiter, numberOfPersons);

//        //    SelectingTable = false;
//        //    ShowingSale = true;

//        //    SelectedTable = null;
//        //    SelectedWaiter = null;
//        //    NumberOfPersons = 0;
//        //}

//        #endregion

//        //public string ShortWorkingDate
//        //{
//        //    get
//        //    {
//        //        StringBuilder sb = new StringBuilder();
//        //        sb.Append(((Dias)workingDate.DayOfWeek).ToString());
//        //        sb.Append(", " + workingDate.Day);
//        //        sb.Append(" " + ((Meses)workingDate.Month - 1).ToString());
//        //        sb.Append("/" + workingDate.Year);

//        //        return sb.ToString();
//        //    }
//        //}

//        DateTime workingDate;
//        public DateTime WorkingDate
//        {
//            get { return workingDate; }
//            set
//            {
//                workingDate = value;

//                OnPropertyChanged("WorkingDate");

//                ChangeDay(workingDate);
//            }
//        }        

//        SaleViewModel selectedOrder;
//        public SaleViewModel SelectedOrder
//        {
//            get { return selectedOrder; }
//            set
//            {
//                selectedOrder = value;
//                OnPropertyChanged("SelectedOrder");
//            }
//        }

//        //SaleViewModel nextOrder;
//        //public SaleViewModel NextOrder
//        //{
//        //    get { return nextOrder; }
//        //    set
//        //    {
//        //        nextOrder = value;
//        //        OnPropertyChanged("NextOrder");
//        //    }
//        //}

//        //SaleViewModel previousOrder;
//        //public SaleViewModel PreviousOrder
//        //{
//        //    get { return previousOrder; }
//        //    set
//        //    {
//        //        previousOrder = value;
//        //        OnPropertyChanged("PreviousOrder");
//        //    }
//        //}

//        //#region Left Side Dialog

//        //bool showingDialog;
//        //public bool ShowingDialog
//        //{
//        //    get { return showingDialog; }
//        //    set
//        //    {
//        //        showingDialog = value;
//        //        OnPropertyChanged("ShowingDialog");
//        //    }
//        //}

//        //ViewModelBase dialogViewModel;
//        //public ViewModelBase DialogViewModel 
//        //{
//        //    get { return dialogViewModel; }
//        //    set
//        //    {
//        //        dialogViewModel = value;
//        //        OnPropertyChanged("DialogViewModel");
//        //    }
//        //}

//        //#endregion

//        #region Left Pane

//        bool leftPaneVisible;

//        public bool LeftPaneVisible
//        {
//            get { return leftPaneVisible; }
//            set
//            {
//                leftPaneVisible = value;
//                OnPropertyChanged("LeftPaneVisible");
//            }
//        }

//        ViewModelBase leftPaneVM;
//        public ViewModelBase LeftPaneViewModel
//        {
//            get { return leftPaneVM; }
//            set
//            {
//                if (leftPaneVM != value)
//                {
//                    leftPaneVM = value;
//                    OnPropertyChanged("LeftPaneViewModel");
//                }                
//            }
//        }
//        MiniInventoryViewModel miniInventoryVM;
//        MiniMenuViewModel miniMenuVM;
//        TotalsByWaiterViewModel miniTotals;

//        void ShowMiniModel(ViewModelBase miniModel)
//        {
//            //opening left pane
//            if (!leftPaneVisible)
//            {
//                LeftPaneViewModel = miniModel;

//                LeftPaneVisible = true;
//            }
//            //left pane is showing other viewmodel
//            else if (leftPaneVM != miniModel)
//            {
//                LeftPaneViewModel = miniModel;
//            }
//            //close left pane
//            else
//            {
//                LeftPaneVisible = false;
//            }
//        }


//        #endregion        
        

//        #region Show Menu

//        RelayCommand showMiniMenuCommand;
//        public ICommand ShowMiniMenuCommand
//        {
//            get
//            {
//                if (showMiniMenuCommand == null)
//                    showMiniMenuCommand = new RelayCommand(x => ShowMiniMenu());
//                return showMiniMenuCommand;
//            }
//        }

//        void ShowMiniMenu()
//        {
//            if (miniMenuVM == null) miniMenuVM = new MiniMenuViewModel(appvm);

//            ShowMiniModel(miniMenuVM);
//        }

//        #endregion

//        #region Mini Inventory

//        RelayCommand showMiniInventoryCommand;
//        public ICommand ShowMiniInventoryCommand
//        {
//            get
//            {
//                if (showMiniInventoryCommand == null)
//                    showMiniInventoryCommand = new RelayCommand(x => ShowMiniInventory());
//                return showMiniInventoryCommand;
//            }
//        }

//        void ShowMiniInventory()
//        {
//            if (miniInventoryVM == null) miniInventoryVM = new MiniInventoryViewModel(appvm);

//            ShowMiniModel(miniInventoryVM);
//        }

//        #endregion

//        #region Mini Totals

//        RelayCommand showMiniTotalsCommand;
//        public ICommand ShowMiniTotalsCommand
//        {
//            get
//            {
//                if (showMiniTotalsCommand == null) showMiniTotalsCommand = new RelayCommand(x => ShowMiniTotals());
//                return showMiniTotalsCommand;
//            }
//        }

//        void ShowMiniTotals()
//        {
//            if (miniTotals == null) miniTotals = new TotalsByWaiterViewModel(appvm);

//            ShowMiniModel(miniTotals);
//        }

//        #endregion
        

//        #region Expand Sale Command

//        //bool showingSale;
//        //public bool ShowingSale 
//        //{
//        //    get { return showingSale; }
//        //    set
//        //    {
//        //        if (showingSale != value) 
//        //        {
//        //            showingSale = value;
//        //            OnPropertyChanged("ShowingSale");
//        //        }
//        //    }
//        //}

//        RelayCommand toggleSaleVisibilityCommand;
//        public ICommand ToggleSaleVisibilityCommand
//        {
//            get
//            {
//                if (toggleSaleVisibilityCommand == null)
//                    toggleSaleVisibilityCommand = new RelayCommand(x => ToggleSaleVisibility(), x => this.CanToggleSaleVisibility);
//                return toggleSaleVisibilityCommand;
//            }
//        }

//        bool CanToggleSaleVisibility 
//        {
//            get { return selectedOrder != null; }
//        }

//        void ToggleSaleVisibility()
//        {
//            //ShowingSale = !showingSale;

//            ShowSale(selectedOrder);
//        }

//        void ShowSale(SaleViewModel sale_to_show) 
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            if (windowManager.Exists(sale_to_show))
//                windowManager.Activate(sale_to_show);
//            else
//            {
//                windowManager.ShowChildWindow(sale_to_show, appvm);
//            }
//        }

//        #endregion

//        #region New Sale Dialog Command

//        //bool selectingTable;
//        //public bool SelectingTable 
//        //{
//        //    get { return selectingTable; }
//        //    set
//        //    {
//        //        selectingTable = value;
//        //        OnPropertyChanged("SelectingTable");
//        //    }
//        //}

//        RelayCommand newSaleCommand;
//        public ICommand NewSaleCommand 
//        {
//            get 
//            {
//                if (newSaleCommand == null)
//                    newSaleCommand = new RelayCommand(x => ShowNewSaleDialog());
//                return newSaleCommand; 
//            }
//        }

//        void ShowNewSaleDialog() 
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            NewSaleDialogViewModel newSaleDialog = new NewSaleDialogViewModel(appvm);

//            if (windowManager.ShowDialog(newSaleDialog, appvm) == true)
//            {
//                var new_svm = CreateNewVale(newSaleDialog.SelectedTable, newSaleDialog.SelectedWaiter, newSaleDialog.NumberOfPersons);
//                Misc companyInfo = appvm.Context.Miscs.First();
//                new_svm.Tax = companyInfo.DefaultTax;
//                new_svm.TaxInPercent = true;

//                ShowSale(new_svm);

//                //ShowingSale = true;
//            }
//            //SelectingTable = !SelectingTable;
//        }

//        #endregion

//        #region Show Day Report Command

//        RelayCommand showDayReportCommand;
//        public ICommand ShowDayReportCommand
//        {
//            get
//            {
//                if (showDayReportCommand == null)
//                    showDayReportCommand = new RelayCommand(x => this.ShowDayReport());
//                return showDayReportCommand;
//            }
//        }

//        void ShowDayReport()
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            DayReportViewModel dialog = new DayReportViewModel(appvm, workingDate);

//            windowManager.ShowDialog(dialog, appvm);           
//        }

//        #endregion
                
//    }
//}
