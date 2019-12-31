using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Enums;
using MiPaladar.Views;


namespace MiPaladar.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public OrdersViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            fromDate = DateTime.Today;
            toDate = DateTime.Today;

            //selectedDim = GroupOptions.NoGrouping;

            orders_source = new ObservableCollection<OrdersReportItemViewModel>();
            orders_filtered = new ObservableCollection<OrdersReportItemViewModel>();
            orders_showing = new ObservableCollection<OrdersReportItemViewModel>();

            UpdateColumnsVisibility();

            UpdateSearch();
        }

        string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        
        #region Dates

        //string[] dateChoices = new string[] { "Hoy", "Semana", "Mes", "Específico" };
        //public string[] DateChoices
        //{
        //    get { return dateChoices; }
        //}

        //int selectedDateChoice;
        //public int SelectedDateChoice
        //{
        //    get { return selectedDateChoice; }
        //    set
        //    {
        //        selectedDateChoice = value;
        //        switch (selectedDateChoice)
        //        {
        //            case 0:
        //                fromDate = DateTime.Today;
        //                toDate = DateTime.Today;
        //                break;
        //            case 1:
        //                fromDate = DateTime.Today;
        //                toDate = DateTime.Today;
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

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

        public ObservableCollection<Employee> CanSellEmployees
        {
            get { return appvm.CanSellEmployees; }
        }
        public ObservableCollection<Employee> CanPurchaseEmployees 
        {
            get { return appvm.CanPurchaseEmployees; }
        }
        public ObservableCollection<PriceList> PriceLists
        {
            get { return appvm.PriceListsOC; }
        }
        //public ObservableCollection<Table> Tables
        //{
        //    get { return appvm.TablesOC; }
        //}
        //public ObservableCollection<Product> Products 
        //{
        //    get { return appvm.ProductsOC; }
        //}
        //public ObservableCollection<PurchaseType> PurchaseTypes 
        //{
        //    get { return new ObservableCollection<PurchaseType>(appvm.Context.PurchaseTypes); }
        //}

        //#region Hide Order Command

        //RelayCommand hideOrderCommand;
        //public ICommand HideOrderCommand 
        //{
        //    get 
        //    {
        //        if (hideOrderCommand == null)
        //            hideOrderCommand = new RelayCommand(x => HideOrder());
        //        return hideOrderCommand; 
        //    }
        //}

        //void HideOrder() 
        //{
        //    //if (showingSale) 
        //    //{
        //    //    ShowingSale = false;
        //    //    SelectedSale = null;
        //    //}
        //    if (showingPurchase)
        //    {
        //        ShowingPurchase = false;
        //        //selectedPurchase.RemoveEvents();
        //        SelectedPurchase = null;
        //    }
        //}

        //#endregion

        #region Find Command

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

        bool CanFind { get { return !busy; } }

        BackgroundWorker bWorker;

        //CollectionView auxView;
        ObservableCollection<OrdersReportItemViewModel> orders_source;

        private void UpdateSearch()
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
            //CollectionViewSource cvs = new CollectionViewSource();
            //cvs.Source = query;
            //OrdersView = (CollectionView)cvs.View;            

            //this will call ShowStartingHere
            //Total = query.Count();
            //if (Total > 0) FirstShowing = 1;            

            //ordersView.Filter = new Predicate<object>(OrderFilter);

            //int count = query.Count();
            //int toTake = Math.Min(20, count);

            //foreach (var item in query.Take(toTake)) 
            //{
            //    visibleOrders.Add(item);
            //}

            //SkipCount = toTake;
            //FirstShowing = toTake > 0 ? 1 : 0;
            //LastShowing = toTake;

            //CommandManager.InvalidateRequerySuggested();
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            orders_source.Clear();

            DateTime toDatePlusOne = toDate.AddDays(1);

            var query = from order in appvm.Context.Orders.OfType<Sale>()
                        where order.Date >= fromDate && order.Date < toDatePlusOne
                        orderby order.Date, order.DateCreated
                        select order;

            foreach (var item in query)
            {
                OrdersReportItemViewModel sc = new OrdersReportItemViewModel(item);
                sc.Count = 1;
                orders_source.Add(sc);
            }

            FilterItems();
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CreateItemsShowing();

            UpdateTotals();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        void OnFilterConditionsChanged() 
        {
            FilterItems();

            CreateItemsShowing();

            UpdateTotals();
        }

        void OnGroupingConditionsChanged()
        {
            int count = 0;

            if (noGrouping) count++;
            if (groupingByDate) count++;
            if (groupingByWeek) count++;
            if (groupingByMonth) count++;
            if (groupingByDayOfWeek) count++;
            if (groupingByWaiter) count++;
            if (groupingByArea) count++;

            //give time for the radio button binding
            if (count != 1) return;

            CreateItemsShowing();

            UpdateColumnsVisibility();
        }

        //void RefreshView() 
        //{
        //    UpdateColumnsVisibility();            
            
        //    FilterItems();

        //    //UpdateGroupDescriptions();

        //    //auxView.Refresh();

        //    CreateItemsShowing();
        //}

        #endregion        

        #region Filtering

        ObservableCollection<OrdersReportItemViewModel> orders_filtered;
        public ObservableCollection<OrdersReportItemViewModel> FilteredItems
        {
            get { return orders_filtered; }
        }

        private void FilterItems()
        {
            //List<OrderReportViewModel> toRemove = new List<OrderReportViewModel>();

            ////remove those that don't pass the filter
            //foreach (var item in filteredItems)
            //{
            //    if (!PassesFilter(item)) toRemove.Add(item);
            //}

            //foreach (var item in toRemove)
            //{
            //    filteredItems.Remove(item);
            //}

            orders_filtered.Clear();

            //add those that pass the filter
            foreach (var item in orders_source)
            {
                if (PassesFilter(item))
                {
                    orders_filtered.Add(item);
                }
            }
        }

        bool PassesFilter(OrdersReportItemViewModel order)
        {
            bool cond = true;

            //if (showAllOrders) 
            //    return true;
            //else if (showSaleOrders)
            {
                //if (order.OrderType != OrderType.Sale) return false;

                if (filteringByWaiter)
                {
                    cond = order.Employee == selectedWaiter;
                }

                if (!cond) return false;

                if (filteringByPriceList)
                {
                    cond = order.PriceList == selectedPriceList;
                }

                //if (!cond) return false;

                //if (filteringByTable)
                //{
                //    cond = order.Table == selectedTable;
                //}

                return cond;
            }
            //else
            //{
            //    //if (order.OrderType != OrderType.Purchase) return false;

            //    //if (filteringByResponsible)
            //    //    cond = selectedResponsible == order.Employee;

            //    if (!cond) return false;

            //    //if (filteringByPurchaseType)
            //    //    cond = selectedPurchaseType == order.PurchaseType;

            //    if (!cond) return false;

            //    if (filteringByPurchaseTitle)
            //    {
            //        if (string.IsNullOrWhiteSpace(purchaseTitleSearch)) cond = true;
            //        else
            //        {
            //            if (order.Title == null) cond = false;
            //            else cond = order.Title.IndexOf(purchaseTitleSearch, StringComparison.OrdinalIgnoreCase) >= 0;
            //        }
            //    }

            //    return cond;
            //}

            //if (!cond) return false;

            //if (filteringByProduct) 
            //{
            //    cond = false;

            //    foreach (var item in sale.LineItems)
            //    {
            //        if (item.Product == selectedProduct) 
            //        {
            //            cond = true; break;
            //        }
            //    }
            //}

            //return cond;
        }

        //int count = 0;
        //bool showAllOrders;
        //public bool ShowAllOrders
        //{
        //    get { return showAllOrders; }
        //    set
        //    {
        //        if (showAllOrders != value) 
        //        {
        //            showAllOrders = value;

        //            count++;

        //            if (count % 2 == 0)
        //            {
        //                count = 0;
        //                RefreshView();
        //            }

        //            //OnPropertyChanged("ShowTotalSalesFooter");
        //            //OnPropertyChanged("ShowTotalPurchasesFooter");
        //        }
        //    }
        //}

        //bool showSaleOrders = true;
        //public bool ShowSaleOrders
        //{
        //    get { return showSaleOrders; }
        //    set
        //    {
        //        if (showSaleOrders != value) 
        //        {
        //            showSaleOrders = value;

        //            count++;

        //            if (count % 2 == 0)
        //            {
        //                count = 0;
        //                RefreshView();
        //            }

        //            OnPropertyChanged("ShowSaleOrders");

        //            //OnPropertyChanged("ShowTotalSalesFooter");
        //            //OnPropertyChanged("ShowTotalDiscountFooter");
        //            //OnPropertyChanged("ShowTotalClientsFooter");
        //        }                
        //    }
        //}

        //bool showPurchaseOrders;
        //public bool ShowPurchaseOrders
        //{
        //    get { return showPurchaseOrders; }
        //    set
        //    {
        //        if (showPurchaseOrders != value) 
        //        {
        //            showPurchaseOrders = value;

        //            count++;

        //            if (count % 2 == 0)
        //            {
        //                count = 0;
        //                RefreshView();
        //            }

        //            OnPropertyChanged("ShowPurchaseOrders");

        //            //OnPropertyChanged("ShowTotalPurchasesFooter");
        //        }                                
        //    }
        //}

        bool filteringByWaiter;
        public bool FilteringByWaiter
        {
            get { return filteringByWaiter; }
            set
            {
                filteringByWaiter = value;
                OnPropertyChanged("FilteringByWaiter");

                if (selectedWaiter != null)
                {
                    //auxView.Refresh();
                    OnFilterConditionsChanged();
                }
            }
        }

        Employee selectedWaiter;
        public Employee SelectedWaiter
        {
            get { return selectedWaiter; }
            set
            {
                selectedWaiter = value;
                //OnPropertyChanged("SelectedWaiter");

                //auxView.Refresh();
                OnFilterConditionsChanged();
            }
        }

        bool filteringByPriceList;
        public bool FilteringByPriceList
        {
            get { return filteringByPriceList; }
            set
            {
                filteringByPriceList = value;
                OnPropertyChanged("FilteringByPriceList");

                if (selectedPriceList != null)
                {
                    //auxView.Refresh();
                    OnFilterConditionsChanged();
                }
            }
        }

        PriceList selectedPriceList;
        public PriceList SelectedPriceList
        {
            get { return selectedPriceList; }
            set
            {
                selectedPriceList = value;
                //OnPropertyChanged("SelectedPriceList");

                //auxView.Refresh();
                OnFilterConditionsChanged();
            }
        }

        //bool filteringByTable;
        //public bool FilteringByTable
        //{
        //    get { return filteringByTable; }
        //    set
        //    {
        //        filteringByTable = value;
        //        OnPropertyChanged("FilteringByTable");

        //        if (selectedTable != null)
        //        {
        //            auxView.Refresh();
        //            UpdateItems();
        //        }
        //    }
        //}

        //Table selectedTable;
        //public Table SelectedTable
        //{
        //    get { return selectedTable; }
        //    set
        //    {
        //        selectedTable = value;
        //        //OnPropertyChanged("SelectedTable");

        //        auxView.Refresh();
        //        UpdateItems();
        //    }
        //}

        //bool filteringByResponsible;
        //public bool FilteringByResponsible
        //{
        //    get { return filteringByResponsible; }
        //    set
        //    {
        //        filteringByResponsible = value;
        //        OnPropertyChanged("FilteringByResponsible");

        //        if (selectedResponsible != null)
        //        {
        //            auxView.Refresh();
        //            UpdateItems();
        //        }
        //    }
        //}

        //Employee selectedResponsible;
        //public Employee SelectedResponsible
        //{
        //    get { return selectedResponsible; }
        //    set
        //    {
        //        selectedResponsible = value;
        //        //OnPropertyChanged("SelectedWaiter");

        //        auxView.Refresh();
        //        UpdateItems();
        //    }
        //}

        //bool filteringByPurchaseType;
        //public bool FilteringByPurchaseType 
        //{
        //    get { return filteringByPurchaseType; }
        //    set
        //    {
        //        filteringByPurchaseType = value;

        //        OnPropertyChanged("FilteringByPurchaseType");

        //        if (selectedPurchaseType != null) 
        //        {
        //            auxView.Refresh();
        //            UpdateItems();
        //        }
        //    }
        //}

        //PurchaseType selectedPurchaseType;
        //public PurchaseType SelectedPurchaseType 
        //{
        //    get { return selectedPurchaseType; }
        //    set
        //    {
        //        selectedPurchaseType = value;
        //        //OnPropertyChanged("SelectedPurchaseType");

        //        if (filteringByPurchaseType) 
        //        {
        //            auxView.Refresh();
        //            UpdateItems();
        //        }                
        //    }
        //}

        //bool filteringByPurchaseTitle;
        //public bool FilteringByPurchaseTitle
        //{
        //    get { return filteringByPurchaseTitle; }
        //    set
        //    {
        //        filteringByPurchaseTitle = value;

        //        OnPropertyChanged("FilteringByPurchaseTitle");

        //        auxView.Refresh();
        //        UpdateItems();                
        //    }
        //}

        //string purchaseTitleSearch;

        //public string PurchaseTitleSearch
        //{
        //    get { return purchaseTitleSearch; }
        //    set 
        //    {                
        //        purchaseTitleSearch = value;

        //        if (filteringByPurchaseTitle) 
        //        {
        //            auxView.Refresh();
        //            UpdateItems(); 
        //        }
        //    }
        //}


        #endregion

        #region Grouping

        ObservableCollection<OrdersReportItemViewModel> orders_showing;
        public ObservableCollection<OrdersReportItemViewModel> Orders
        {
            get
            {
                //if (ordersShowing == null)
                //{
                //    sourceList = new ObservableCollection<OrderReportViewModel>();
                //    ordersShowing = new ObservableCollection<OrderReportViewModel>();

                //    CollectionViewSource cvs = new CollectionViewSource();
                //    cvs.Source = filteredItems;
                //    auxView = (CollectionView)cvs.View;

                //    //auxView.Filter = PassesFilter;

                //    UpdateSearch();
                //}
                return orders_showing;
            }
        }

        void CreateItemsShowing()
        {
            orders_showing.Clear();

            //no grouping
            if (noGrouping)
            {
                foreach (var item in orders_filtered)
                {
                    orders_showing.Add(item);
                }
            }
            else if (groupingByDate)
            {
                var groupsByDate = from item in orders_filtered
                                   group item by item.Date;

                foreach (var group in groupsByDate)
                {
                    OrdersReportItemViewModel sc = new OrdersReportItemViewModel();

                    sc.Date = group.Key;

                    MakeSum(group, sc);

                    orders_showing.Add(sc);
                }
            }
            else if (groupingByWeek)
            {
                var groupsByWeek = from item in orders_filtered
                                   group item by item.MondayDate;

                foreach (var group in groupsByWeek)
                {
                    OrdersReportItemViewModel sc = new OrdersReportItemViewModel();

                    sc.MondayDate = group.Key;

                    MakeSum(group, sc);

                    orders_showing.Add(sc);
                }
            }
            else if (groupingByDayOfWeek)
            {
                var groupsByDayOfWeek = from item in orders_filtered
                                        group item by item.DayOfWeek;

                foreach (var group in groupsByDayOfWeek)
                {
                    OrdersReportItemViewModel sc = new OrdersReportItemViewModel();

                    sc.DayOfWeek = group.Key;

                    MakeSum(group, sc);

                    orders_showing.Add(sc);
                }
            }
            else if (groupingByMonth)
            {
                var queryNestedGroups = from item in orders_filtered
                                        group item by item.Date.Year into yearGroup
                                        from monthGroup in
                                            (from item in yearGroup
                                             group item by item.Date.Month)
                                        group monthGroup by yearGroup.Key;

                foreach (var yearGroup in queryNestedGroups)
                {
                    foreach (var monthGroup in yearGroup)
                    {
                        OrdersReportItemViewModel sc = new OrdersReportItemViewModel();

                        //get month from the first sale
                        sc.Date = monthGroup.First().Date;

                        MakeSum(monthGroup, sc);

                        orders_showing.Add(sc);
                    }
                }
            }
            else if (groupingByWaiter)
            {
                var groupsByWaiter = from item in orders_filtered
                                     group item by item.Employee;

                foreach (var group in groupsByWaiter)
                {
                    OrdersReportItemViewModel sc = new OrdersReportItemViewModel();

                    sc.Employee = group.Key;

                    MakeSum(group, sc);

                    orders_showing.Add(sc);
                }
            }
            else if (groupingByArea)
            {
                var groupsByArea = from item in orders_filtered
                                   group item by item.PriceList;

                foreach (var group in groupsByArea)
                {
                    OrdersReportItemViewModel sc = new OrdersReportItemViewModel();

                    sc.PriceList = group.Key;

                    MakeSum(group, sc);

                    orders_showing.Add(sc);
                }
            }
            //else
            //{
            //    foreach (var item in auxView.Groups)
            //    {
            //        CollectionViewGroup group = (CollectionViewGroup)item;

            //        OrderReportViewModel sc = new OrderReportViewModel();

            //        //group by date
            //        if (groupingByDate)
            //            sc.Date = (DateTime)group.Name;
            //        //grouping by week
            //        else if (groupingByWeek)
            //            sc.MondayDate = (DateTime)group.Name;
            //        //group by day of week
            //        else if (groupingByWeekDay)
            //            sc.DayOfWeek = (DayOfWeek)group.Name;
            //        //group by waiter
            //        else if (groupingByWaiter)
            //            sc.Employee = (Employee)group.Name;
            //        //grouping by table
            //        //else if (groupingByTable)
            //        //    sc.Table = (Table)group.Name;
            //        //grouping by pricelist
            //        else if (groupingByArea)
            //            sc.PriceList = (PriceList)group.Name;

            //        MakeSum(group, sc);

            //        ordersShowing.Add(sc);
            //    }
            //}
        }

        private void MakeSum(IEnumerable<OrdersReportItemViewModel> group, OrdersReportItemViewModel sc)
        {
            int count = 0;
            int clients = 0;
            decimal discount = 0;
            decimal tax = 0;
            decimal totalPrice = 0;
            decimal totalTips = 0;
            //decimal sales_total = 0;
            //decimal purchases_total = 0;
            List<DateTime> different_dates = new List<DateTime>();

            //sc.Count = group.ItemCount;
            //sc.Clients = group.Items.Sum(x => ((OrderReportViewModel)x).Clients);
            //sc.Discount = group.Items.Sum(x => ((OrderReportViewModel)x).Discount);

            foreach (OrdersReportItemViewModel item in group)
            {
                count++;
                clients += item.Clients;
                discount += item.Discount;
                tax += item.Tax;
                totalTips += item.Tips;

                totalPrice += item.TotalPrice;

                if (!different_dates.Contains(item.Date.Date)) different_dates.Add(item.Date.Date);
            }

            sc.Count = count;
            sc.Clients = clients;
            sc.Discount = discount;
            sc.Tax = tax;
            sc.Tips = totalTips;
            sc.TotalPrice = totalPrice;            
            //sc.TotalPurchase = purchases_total;
            if (different_dates.Count > 0) sc.DayAverage = totalPrice / different_dates.Count;

            //if (groupingByDate)
            //{
            //    decimal totalSales = 0; decimal totalPurchases = 0;
            //    foreach (var i in group.Items)
            //    {
            //        OrderReportViewModel order = (OrderReportViewModel)i;
            //        if (order.OrderType == OrderType.Sale) totalSales += order.TotalPrice;
            //        else totalPurchases += order.TotalPrice;
            //    }
            //    sc.TotalPrice = totalSales;
            //    sc.TotalPurchase = totalPurchases;
            //}
            //else sc.TotalPrice = group.Items.Sum(x => ((OrderReportViewModel)x).TotalPrice);
        }

        //bool grouping;
        //public bool Grouping
        //{
        //    get { return grouping; }
        //    set
        //    {
        //        grouping = value;
        //        OnPropertyChanged("Grouping");
        //    }
        //}

        //public string[] Dimensions
        //{
        //    get
        //    {
        //        return new string[4] { "Sin Agrupar", "Dependiente", "Mesa", "Centro" };
        //    }
        //}

        bool noGrouping;
        public bool NoGrouping
        {
            get { return noGrouping; }
            set
            {
                if (noGrouping != value)
                {
                    noGrouping = value;

                    OnGroupingConditionsChanged();
                }
            }
        }
        bool groupingByDate;
        public bool GroupingByDate
        {
            get { return groupingByDate; }
            set
            {
                if (groupingByDate != value)
                {
                    groupingByDate = value;

                    OnGroupingConditionsChanged();
                }
            }
        }

        bool groupingByWeek;
        public bool GroupingByWeek
        {
            get { return groupingByWeek; }
            set
            {
                if (groupingByWeek != value)
                {
                    groupingByWeek = value;

                    OnGroupingConditionsChanged();
                }
            }
        }

        bool groupingByMonth;
        public bool GroupingByMonth
        {
            get { return groupingByMonth; }
            set
            {
                if (groupingByMonth != value)
                {
                    groupingByMonth = value;

                    OnGroupingConditionsChanged();
                }
            }
        }

        bool groupingByDayOfWeek;
        public bool GroupingByWeekDay
        {
            get { return groupingByDayOfWeek; }
            set
            {
                if (groupingByDayOfWeek != value)
                {
                    groupingByDayOfWeek = value;

                    OnGroupingConditionsChanged();
                }
            }
        }

        bool groupingByWaiter;
        public bool GroupingByWaiter
        {
            get { return groupingByWaiter; }
            set
            {
                if (groupingByWaiter != value)
                {
                    groupingByWaiter = value;

                    OnGroupingConditionsChanged();
                }
            }
        }
        //bool groupingByTable;
        //public bool GroupingByTable
        //{
        //    get { return groupingByTable; }
        //    set
        //    {
        //        if (groupingByTable != value)
        //        {
        //            groupingByTable = value;

        //            groupingCount++;

        //            if (groupingCount % 2 == 0)
        //            {
        //                RefreshView();
        //            }
        //        }
        //    }
        //}
        bool groupingByArea;
        public bool GroupingByArea
        {
            get { return groupingByArea; }
            set
            {
                if (groupingByArea != value)
                {
                    groupingByArea = value;

                    OnGroupingConditionsChanged();
                }
            }
        }

        #endregion        

        #region Totals Footer

        private void UpdateTotals()
        {
            TotalOrders = orders_showing.Sum(x => x.Count);
            TotalImport = orders_showing./*Where(x => x.OrderType == OrderType.Sale).*/Sum(x => x.TotalPrice);//ordersShowing.Sum(x => x.TotalPrice);
            //TotalPurchases = ordersShowing/*.Where(x => x.OrderType == OrderType.Purchase)*/.Sum(x => x.TotalPurchase);
            TotalClients = orders_showing.Sum(x => x.Clients);
            TotalDiscount = orders_showing.Sum(x => x.Discount);
            TotalTax = orders_showing.Sum(x => x.Tax);
            TotalTips = orders_showing.Sum(x => x.Tips);
            SalesByClientAverage = totalclients == 0 ? 0 : totalSales / totalclients;
        }

        int totalOrders;
        public int TotalOrders
        {
            get { return totalOrders; }
            set
            {
                totalOrders = value;
                OnPropertyChanged("TotalOrders");
            }
        }

        decimal totalSales;
        public decimal TotalImport
        {
            get { return totalSales; }
            set
            {
                totalSales = value;
                OnPropertyChanged("TotalImport");
            }
        }

        //decimal totalPurchases;
        //public decimal TotalPurchases
        //{
        //    get { return totalPurchases; }
        //    set
        //    {
        //        totalPurchases = value;
        //        OnPropertyChanged("TotalPurchases");
        //    }
        //}

        int totalclients;
        public int TotalClients
        {
            get { return totalclients; }
            set
            {
                totalclients = value;
                OnPropertyChanged("TotalClients");
            }
        }

        decimal totalDiscount;
        public decimal TotalDiscount 
        {
            get { return totalDiscount; }
            set 
            {
                totalDiscount = value;
                OnPropertyChanged("TotalDiscount");
            }
        }

        decimal totalTax;
        public decimal TotalTax
        {
            get { return totalTax; }
            set
            {
                totalTax = value;
                OnPropertyChanged("TotalTax");
            }
        }

        decimal totalTips;
        public decimal TotalTips
        {
            get { return totalTips; }
            set
            {
                totalTips = value;
                OnPropertyChanged("TotalTips");
            }
        }

        decimal salesByClientAverage;
        public decimal SalesByClientAverage
        {
            get { return salesByClientAverage; }
            set
            {
                salesByClientAverage = value;
                OnPropertyChanged("SalesByClientAverage");
            }
        }

        #endregion        

        #region Columns Visibility

        private void UpdateColumnsVisibility()
        {
            DateColumnVisible = groupingByDate || noGrouping;
            //WeekDayColumnVisible = groupingByWeekDay;
            WeekStringColumnVisible = groupingByWeek;
            MonthColumnVisible = groupingByMonth;
            //OrderColumnVisible = noGrouping;
            AreaColumnVisible = (noGrouping || groupingByArea);
            //TableColumnVisible = showSaleOrders && (noGrouping || groupingByTable);
            WaiterColumnVisible = (noGrouping || groupingByWaiter);
            //PurchaseTypeColumnVisible = showPurchaseOrders && noGrouping;
            //TitleColumnVisible = showPurchaseOrders && noGrouping;
            //ResponsibleColumnVisible = showPurchaseOrders && noGrouping;
            //OrderCountColumnVisible = !noGrouping;
            //ClientCountColumnVisible = true;
            //SaleTotalColumnVisible = !groupingByDate;
            //DayAverageColumnVisible = groupingByWeekDay;
            //PurchasePriceColumnVisible = !showSaleOrders && groupingByDate;
            //PriceByClientColumnVisible = !noGrouping;
            DiscountColumnVisible = true;
            TaxColumnVisible = true;
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

        bool weekStringColumnVisible;
        public bool WeekStringColumnVisible
        {
            get { return weekStringColumnVisible; }
            set
            {
                weekStringColumnVisible = value;
                OnPropertyChanged("WeekStringColumnVisible");
            }
        }

        bool monthColumnVisible;
        public bool MonthColumnVisible
        {
            get { return monthColumnVisible; }
            set
            {
                monthColumnVisible = value;
                OnPropertyChanged("MonthColumnVisible");
            }
        }

        //bool weekDayColumnVisible;
        //public bool WeekDayColumnVisible
        //{
        //    get { return weekDayColumnVisible; }
        //    set
        //    {
        //        weekDayColumnVisible = value;
        //        OnPropertyChanged("WeekDayColumnVisible");
        //    }
        //}

        //bool weekDayPercentColumnVisible;
        //public bool WeekDayPercentColumnVisible
        //{
        //    get { return weekDayPercentColumnVisible; }
        //    set
        //    {
        //        weekDayPercentColumnVisible = value;
        //        OnPropertyChanged("WeekDayPercentColumnVisible");
        //    }
        //}

        //bool orderColumnVisible;
        //public bool OrderColumnVisible
        //{
        //    get { return orderColumnVisible; }
        //    set
        //    {
        //        orderColumnVisible = value;
        //        OnPropertyChanged("OrderColumnVisible");
        //    }
        //}

        bool areaColumnVisible;
        public bool AreaColumnVisible
        {
            get { return areaColumnVisible; }
            set
            {
                areaColumnVisible = value;
                OnPropertyChanged("AreaColumnVisible");
            }
        }

        //bool tableColumnVisible;
        //public bool TableColumnVisible
        //{
        //    get { return tableColumnVisible; }
        //    set
        //    {
        //        tableColumnVisible = value;
        //        OnPropertyChanged("TableColumnVisible");
        //    }
        //}

        bool waiterColumnVisible;
        public bool WaiterColumnVisible
        {
            get { return waiterColumnVisible; }
            set
            {
                waiterColumnVisible = value;
                OnPropertyChanged("WaiterColumnVisible");
            }
        }

        //bool purchaseTypeColumnVisible;
        //public bool PurchaseTypeColumnVisible
        //{
        //    get { return purchaseTypeColumnVisible; }
        //    set
        //    {
        //        purchaseTypeColumnVisible = value;
        //        OnPropertyChanged("PurchaseTypeColumnVisible");
        //    }
        //}
        //bool titleColumnVisible;
        //public bool TitleColumnVisible
        //{
        //    get { return titleColumnVisible; }
        //    set
        //    {
        //        titleColumnVisible = value;
        //        OnPropertyChanged("TitleColumnVisible");
        //    }
        //}

        //bool responsibleColumnVisible;
        //public bool ResponsibleColumnVisible
        //{
        //    get { return responsibleColumnVisible; }
        //    set 
        //    {
        //        responsibleColumnVisible = value;
        //        OnPropertyChanged("ResponsibleColumnVisible");
        //    }
        //}

        //bool orderCountColumnVisible;
        //public bool OrderCountColumnVisible
        //{
        //    get { return orderCountColumnVisible; }
        //    set
        //    {
        //        orderCountColumnVisible = value;
        //        OnPropertyChanged("OrderCountColumnVisible");
        //    }
        //}

        //bool clientCountColumnVisible;

        //public bool ClientCountColumnVisible
        //{
        //    get { return clientCountColumnVisible; }
        //    set
        //    {
        //        clientCountColumnVisible = value;
        //        OnPropertyChanged("ClientCountColumnVisible");
        //    }
        //}
        //bool saleTotalColumnVisible;
        //public bool SaleTotalColumnVisible
        //{
        //    get { return saleTotalColumnVisible; }
        //    set
        //    {
        //        saleTotalColumnVisible = value;
        //        OnPropertyChanged("SaleTotalColumnVisible");
        //    }
        //}

        //bool dayAverageColumnVisible;
        //public bool DayAverageColumnVisible
        //{
        //    get { return dayAverageColumnVisible; }
        //    set
        //    {
        //        dayAverageColumnVisible = value;
        //        OnPropertyChanged("DayAverageColumnVisible");
        //    }
        //}

        //bool purchasePriceColumnVisible;

        //public bool PurchasePriceColumnVisible
        //{
        //    get { return purchasePriceColumnVisible; }
        //    set
        //    {
        //        purchasePriceColumnVisible = value;
        //        OnPropertyChanged("PurchasePriceColumnVisible");
        //    }
        //}
        //public bool PurchasePriceColumnVisible
        //{
        //    get { return showAllOrders && groupingByDate; }
        //}

        //bool priceByClientColumnVisible;

        //public bool PriceByClientColumnVisible
        //{
        //    get { return priceByClientColumnVisible; }
        //    set
        //    {
        //        priceByClientColumnVisible = value;
        //        OnPropertyChanged("PriceByClientColumnVisible");
        //    }
        //}

        bool discountColumnVisible;

        public bool DiscountColumnVisible
        {
            get { return discountColumnVisible; }
            set
            {
                discountColumnVisible = value;
                OnPropertyChanged("DiscountColumnVisible");
            }
        }

        bool taxColumnVisible;

        public bool TaxColumnVisible
        {
            get { return taxColumnVisible; }
            set
            {
                taxColumnVisible = value;
                OnPropertyChanged("TaxColumnVisible");
            }
        }

        #endregion

        #region Export to Excel

        RelayCommand exportToExcel;
        public ICommand ExportToExcelCommand
        {
            get 
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => Export());
                return exportToExcel;
            }
        }

        private void Export()
        {
            Action<Excel.Range> displayHeader = (cell) =>
            {
                int column = 0;

                if (DateColumnVisible) 
                {
                    cell.Offset[0, column].Value = "Fecha";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                if (WeekStringColumnVisible) 
                {
                    cell.Offset[0, column].Value = "Semana";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                if (MonthColumnVisible)
                {
                    cell.Offset[0, column].Value = "Mes";
                    cell.Offset[0, column++].Font.Bold = "True";
                }                                
                if (AreaColumnVisible) 
                {
                    cell.Offset[0, column].Value = "Area";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                //if (TableColumnVisible) 
                //{
                //    cell.Offset[0, column].Value = "Mesa";
                //    cell.Offset[0, column++].Font.Bold = "True";
                //}
                if (WaiterColumnVisible) 
                {
                    cell.Offset[0, column].Value = "Dependiente";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                //if (PurchaseTypeColumnVisible) 
                //{
                //    cell.Offset[0, column].Value = "Tipo";
                //    cell.Offset[0, column++].Font.Bold = "True";
                //}
                //if (TitleColumnVisible)
                //{
                //    cell.Offset[0, column].Value = "Título";
                //    cell.Offset[0, column++].Font.Bold = "True";
                //}
                //if (ResponsibleColumnVisible)
                //{
                //    cell.Offset[0, column].Value = "Responsable";
                //    cell.Offset[0, column++].Font.Bold = "True";
                //}
                //if (OrderCountColumnVisible)
                {
                    cell.Offset[0, column].Value = "# vales";
                    cell.Offset[0, column++].Font.Bold = "True";
                }

                //if (ClientCountColumnVisible)
                {
                    cell.Offset[0, column].Value = "Clientes";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                if (DiscountColumnVisible)
                {
                    cell.Offset[0, column].Value = "Descuento";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                if (TaxColumnVisible)
                {
                    cell.Offset[0, column].Value = "Recargo";
                    cell.Offset[0, column++].Font.Bold = "True";
                }

                cell.Offset[0, column].Value = "Propina";
                cell.Offset[0, column++].Font.Bold = "True";
                
                //sales total
                    cell.Offset[0, column].Value = "Importe";
                    cell.Offset[0, column++].Font.Bold = "True";               

                //if (PurchasePriceColumnVisible) 
                //{
                //    cell.Offset[0, column].Value = "Compras";
                //    cell.Offset[0, column++].Font.Bold = "True";
                //}
                //if (PriceByClientColumnVisible) 
                {
                    cell.Offset[0, column].Value = "Importe/Cliente";
                    cell.Offset[0, column++].Font.Bold = "True";
                }
                
                //if (OrderColumnVisible)
                //{
                //    cell.Offset[0, column].Value = "Vale";
                //    cell.Offset[0, column++].Font.Bold = "True";
                //}
            };

            Action<OrdersReportItemViewModel, Excel.Range> displayItem = (orderCopy, cell) =>
            {
                int column = 0;

                if (DateColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.Date.ToString("M");
                }
                if (WeekStringColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.WeekString;
                }
                if (MonthColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.Date.ToString("MMM/yy");
                }
                if (AreaColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.PriceList == null ? "" : orderCopy.PriceList.Name;
                }
                //if (TableColumnVisible)
                //{
                //    cell.Offset[0, column++].Value = orderCopy.Table == null ? "" : orderCopy.Table.Number.ToString();
                //}
                if (WaiterColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.Employee == null ? "" : orderCopy.Employee.Name;
                }
                //if (PurchaseTypeColumnVisible)
                //{
                //    cell.Offset[0, column++].Value = orderCopy.PurchaseType == null ? "" : orderCopy.PurchaseType.Name;
                //}
                //if (TitleColumnVisible)
                //{
                //    cell.Offset[0, column++].Value = orderCopy.Title;
                //}
                //if (ResponsibleColumnVisible) 
                //{
                //    cell.Offset[0, column++].Value = orderCopy.Employee == null ? "" : orderCopy.Employee.Name;
                //}
                //if (OrderCountColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.Count;
                }
                //if (ClientCountColumnVisible)
                {
                    cell.Offset[0, column++].Value = orderCopy.Clients;
                }
                if (DiscountColumnVisible)
                {
                    cell.Offset[0, column].Value = orderCopy.Discount;
                    cell.Offset[0, column++].Style = "Currency";
                }
                if (TaxColumnVisible)
                {
                    cell.Offset[0, column].Value = orderCopy.Tax;
                    cell.Offset[0, column++].Style = "Currency";
                }
                //if (TipsColumnVisible)
                {
                    cell.Offset[0, column].Value = orderCopy.Tips;
                    cell.Offset[0, column++].Style = "Currency";
                }
                //if (SaleTotalColumnVisible)
                {
                    cell.Offset[0, column].Value = orderCopy.TotalPrice;
                    cell.Offset[0, column++].Style = "Currency";
                }
                //if (PurchasePriceColumnVisible) 
                //{
                //    cell.Offset[0, column].Value = orderCopy.TotalPurchase;
                //    cell.Offset[0, column++].Style = "Currency";
                //}

                //if (PriceByClientColumnVisible)
                {
                    cell.Offset[0, column].Value = orderCopy.SalesByClient;
                    cell.Offset[0, column++].Style = "Currency";
                }
                
                //if (OrderColumnVisible)
                //{
                //    cell.Offset[0, column++].Value =  "Vale " + orderCopy.Number;
                //}
            };

            int numberOfColumns = CountVisibleColumns();

            //get the view so it keeps the sort descriptions
            ICollectionView view = CollectionViewSource.GetDefaultView(orders_showing);

            var excelExporter = base.GetService<IExcelExporter>();

            excelExporter.ExportToExcel<OrdersReportItemViewModel>(view.Cast<OrdersReportItemViewModel>(),
                displayHeader, displayItem, numberOfColumns);            
        }

        int CountVisibleColumns() 
        {
            int columns = 0;

            if (DateColumnVisible)
            {
                columns++;
            }
            if (WeekStringColumnVisible)
            {
                columns++;
            }
            if (MonthColumnVisible)
            {
                columns++;
            }
            
            if (AreaColumnVisible)
            {
                columns++;
            }
            //if (TableColumnVisible)
            //{
            //    columns++;
            //}
            if (WaiterColumnVisible)
            {
                columns++;
            }
            //if (PurchaseTypeColumnVisible)
            //{
            //    columns++;
            //}
            //if (TitleColumnVisible)
            //{
            //    columns++;
            //}
            //if (ResponsibleColumnVisible)
            //{
            //    columns++;
            //}
            //if (OrderCountColumnVisible)
            {
                columns++;
            }
            //if (ClientCountColumnVisible)
            {
                columns++;
            }
            if (DiscountColumnVisible)
            {
                columns++;
            }
            if (taxColumnVisible)
            {
                columns++;
            }
            //if (tipsColumnVisible)
            {
                columns++;
            }
            //"importe"
            //if (SaleTotalColumnVisible)
            {                
                columns++;
            }
            //if (PurchasePriceColumnVisible)
            //    columns++;

            //if (PriceByClientColumnVisible)
            {
                columns++;
            }
            
            //if (OrderColumnVisible)
            //{
            //    columns++;
            //}

            return columns;
        }

        #endregion        

        //#region Advanced Search Visibility

        //bool advancedSearchVisible;
        //public bool AdvancedSearchVisible
        //{
        //    get { return advancedSearchVisible; }
        //    set
        //    {
        //        advancedSearchVisible = value;
        //        OnPropertyChanged("AdvancedSearchVisible");
        //    }
        //}

        //RelayCommand showAdvancedSearchPopupCommand;
        //public ICommand ShowAdvancedSearchPopupCommand
        //{
        //    get
        //    {
        //        if (showAdvancedSearchPopupCommand == null)
        //            showAdvancedSearchPopupCommand = new RelayCommand(x => ShowProductTemplatePopup());
        //        return showAdvancedSearchPopupCommand;
        //    }
        //}

        //void ShowProductTemplatePopup()
        //{
        //    AdvancedSearchVisible = true;
        //}

        //#endregion

        #region Show Order Command

        //bool showingPurchase;
        //public bool ShowingPurchase
        //{
        //    get { return showingPurchase; }
        //    set
        //    {
        //        if (showingPurchase != value)
        //        {
        //            showingPurchase = value;
        //            OnPropertyChanged("ShowingPurchase");
        //        }
        //    }
        //}

        //PurchaseViewModel selectedPurchase;
        //public PurchaseViewModel SelectedPurchase
        //{
        //    get { return selectedPurchase; }
        //    set
        //    {
        //        selectedPurchase = value;
        //        OnPropertyChanged("SelectedPurchase");
        //    }
        //}

        RelayCommand showOrderCommand;
        public ICommand ShowOrderCommand
        {
            get
            {
                if (showOrderCommand == null)
                    showOrderCommand = new RelayCommand(x => ShowOrder((Order)x));
                return showOrderCommand;
            }
        }

        private void ShowOrder(Order x)
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
                    SaleViewModel viewmodel = new SaleViewModel(appvm, sale, OnRemoved);
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
            //MessageBox.Show("order ID: " + x.Id);
        }

        void OnRemoved(Sale s)
        {
            foreach (var item in orders_showing)
            {
                if (item.Order == s)
                {
                    orders_showing.Remove(item);
                    UpdateTotals();
                    break;
                }
            }
        }

        #endregion
    }
}
