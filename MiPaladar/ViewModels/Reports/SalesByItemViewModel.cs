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
    public class SalesByItemViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        //IExcelExporter excelExporter;
        // passwordAsker;

        public SalesByItemViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            fromDate = DateTime.Today;
            toDate = DateTime.Today;

            //UpdateSearch();

            lineitems_source = new ObservableCollection<SaleLineItemReportViewModel>();
            lineitems_filtered = new ObservableCollection<SaleLineItemReportViewModel>();
            //lineitems_showing = new ObservableCollection<SaleLineItemReportViewModel>();

            UpdateColumnsVisibility();
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
        //            case 1:
        //                fromDate = DateTime.Today;
        //                toDate = DateTime.Today;
        //                break;
        //            case 2:
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

        public ObservableCollection<Category> Categories 
        {
            get { return appvm.CategoriesOC; }
        }

        public ObservableCollection<PriceList> PriceLists 
        {
            get { return appvm.PriceListsOC; }
        }

        //public ObservableCollection<Employee> Waiters         
        //{
        //    get { return appvm.EmployeesOC; }
        //}

        public ObservableCollection<Employee> CanSellEmployees
        {
            get { return appvm.CanSellEmployees; }
        }

        public ObservableCollection<Employee> CanPurchaseEmployees
        {
            get { return appvm.CanPurchaseEmployees; }
        }

        //public ObservableCollection<PurchaseType> PurchaseTypes
        //{
        //    get { return new ObservableCollection<PurchaseType>(appvm.Context.PurchaseTypes); }
        //}

        //ICollectionView icvIngredients;
        //public ICollectionView Ingredients
        //{
        //    get
        //    {
        //        if (icvIngredients == null)
        //        {
        //            //create the view
        //            CollectionViewSource myCVS = new CollectionViewSource();
        //            myCVS.Source = appvm.ProductsOC;
        //            icvIngredients = myCVS.View;

        //            //sort by name
        //            icvIngredients.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        //            //filter by category
        //            icvIngredients.Filter = new Predicate<object>(x => (x as Product).IsIngredient);
        //        }
        //        return icvIngredients;
        //    }
        //}
        
        
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

        bool searchIngredients;
        public bool SearchInIngredients 
        {
            get { return searchIngredients; }
            set 
            {
                searchIngredients = value;
                OnPropertyChanged("SearchInIngredients");
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

        //CollectionView auxView;
        BackgroundWorker bWorker;

        ObservableCollection<SaleLineItemReportViewModel> lineitems_source;        

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

            //UpdateGroupDescriptions();

            //if (filteringByIngredient) CalculateIngredientQuantities();

            //UpdateItems();

            //convertedToIngredients = false;
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lineitems_source.Clear();

            DateTime toDatePlusOne = toDate.AddDays(1);

            //get data
            var query =
                from lineitem in appvm.Context.LineItems.OfType<SaleLineItem>()
                where lineitem.Order.Date >= fromDate && lineitem.Order.Date < toDatePlusOne
                //orderby lineitem.Order.Date, lineitem.Order.DateCreated
                select lineitem;

            foreach (var item in query)
            {
                SaleLineItemReportViewModel copy = new SaleLineItemReportViewModel(item);

                lineitems_source.Add(copy);

                if (searchIngredients && item.Product.IsRecipe)
                {
                    foreach (var ingredient in item.Product.Ingredients)
                    {
                        SaleLineItemReportViewModel ing_op = new SaleLineItemReportViewModel();

                        ing_op.Product = ingredient.IngredientProduct;
                        ing_op.Quantity = item.Quantity * ingredient.Quantity;
                        ing_op.UnitMeasure = ingredient.UnitMeasure;
                        ing_op.Date = item.Order.Date;
                        ing_op.DateCreated = item.Order.DateCreated;
                        ing_op.DayOfWeek = item.Order.Date.DayOfWeek;
                        ing_op.MondayDate = SaleLineItemReportViewModel.GetWeekMonday(item.Order.Date);
                        ing_op.Employee = item.Order.Employee;
                        ing_op.Order = (Sale)item.Order;

                        lineitems_source.Add(ing_op);
                    }
                }
            }

            FilterItems();
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateItems();

            UpdateTotals();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        void OnFilterConditionsChanged()
        {
            FilterItems();

            UpdateItems();

            UpdateTotals();
        }

        void OnGroupingConditionsChanged()
        {
            int count = 0;

            if (noGrouping) count++;
            if (groupingByProduct) count++;
            if (groupingByCategory) count++;
            if (groupingByDate) count++;
            if (groupingByWeek) count++;
            if (groupingByDayOfWeek) count++;
            if (groupingByEmployee) count++;

            //give time for the radio button binding
            if (count != 1) return;

            UpdateItems();

            UpdateColumnsVisibility();
        }

        void UpdateItems()
        {
            if (lineitems_showing == null) return;

            lineitems_showing.Clear();

            if (noGrouping) 
            {
                foreach (var item in lineitems_filtered)
                {
                    lineitems_showing.Add(item);
                }                
            }
            else if (groupingByProduct)
            {
                var groupsByProduct = from item in lineitems_filtered
                                      group item by item.Product;

                foreach (var group in groupsByProduct)
                {
                    SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

                    lic.Product = group.Key;
                    lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

                    MakeSum(group, lic);

                    lineitems_showing.Add(lic);
                }
            }
            else if (groupingByCategory)
            {
                var groupsByCategory = from item in lineitems_filtered
                                       group item by item.Category;

                foreach (var group in groupsByCategory)
                {
                    SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

                    lic.Category = group.Key;

                    MakeSum(group, lic);

                    lineitems_showing.Add(lic);
                }
            }
            else if (groupingByDate)
            {
                var queryNestedGroups = from item in lineitems_filtered
                                        group item by item.Product into productGroup
                                        from dateGroup in
                                            (from item in productGroup
                                             group item by item.Date)
                                        group dateGroup by productGroup.Key;

                foreach (var productGroup in queryNestedGroups)
                {
                    foreach (var dateGroup in productGroup)
                    {
                        SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

                        lic.Product = productGroup.Key;
                        lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                        lic.Date = dateGroup.Key;

                        MakeSum(dateGroup, lic);

                        lineitems_showing.Add(lic);
                    }                    
                }
            }
            else if (groupingByWeek)
            {
                var queryNestedGroups = from item in lineitems_filtered
                                        group item by item.Product into productGroup
                                        from weekGroup in
                                            (from item in productGroup
                                             group item by item.MondayDate)
                                        group weekGroup by productGroup.Key;

                foreach (var productGroup in queryNestedGroups)
                {
                    foreach (var weekGroup in productGroup)
                    {
                        SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

                        lic.Product = productGroup.Key;
                        lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                        lic.MondayDate = weekGroup.Key;

                        MakeSum(weekGroup, lic);

                        lineitems_showing.Add(lic);
                    }                    
                }
            }
            else if (groupingByDayOfWeek)
            {
                var queryNestedGroups = from item in lineitems_filtered
                                        group item by item.Product into productGroup
                                        from dowGroup in
                                            (from item in productGroup
                                             group item by item.DayOfWeek)
                                        group dowGroup by productGroup.Key;

                foreach (var productGroup in queryNestedGroups)
                {
                    foreach (var dowGroup in productGroup)
                    {
                        SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

                        lic.Product = productGroup.Key;
                        lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                        lic.DayOfWeek = dowGroup.Key;

                        MakeSum(dowGroup, lic);

                        lineitems_showing.Add(lic);
                    }
                }
            }
            else if (groupingByEmployee)
            {
                var queryNestedGroups = from item in lineitems_filtered
                                        group item by item.Product into productGroup
                                        from employeeGroup in
                                            (from item in productGroup
                                             group item by item.Employee)
                                        group employeeGroup by productGroup.Key;

                foreach (var productGroup in queryNestedGroups)
                {
                    foreach (var employeeGroup in productGroup)
                    {
                        SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

                        lic.Product = productGroup.Key;
                        lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                        lic.Employee = employeeGroup.Key;

                        MakeSum(employeeGroup, lic);

                        lineitems_showing.Add(lic);
                    }
                }
            }
        }

        private void MakeSum(IEnumerable<SaleLineItemReportViewModel> group, SaleLineItemReportViewModel lic)
        {
            double tempQuantity = 0;
            decimal tempPrice = 0;

            decimal tempCost = 0;
            decimal tempProfit = 0;

            List<DateTime> different_dates = new List<DateTime>();

            foreach (SaleLineItemReportViewModel x in group)
            {
                //if (showSaleOrders || showPurchaseOrders)
                {
                    tempQuantity += x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion;
                    tempPrice += x.Price;

                    tempCost += x.Cost;
                    tempProfit += x.Profit;
                }

                if (groupingByDayOfWeek && !different_dates.Contains(x.Order.Date.Date)) different_dates.Add(x.Order.Date.Date);
            }
            
            lic.Quantity = tempQuantity;
            lic.Price = tempPrice;

            if (groupingByDayOfWeek && different_dates.Count > 0) lic.Quantity = tempQuantity / different_dates.Count;

            //product might be null if it's grouping by category
            if (lic.Product != null) 
            {
                UnitMeasure costUM = lic.Product.CostUnitMeasure;
                double rate = costUM.ToBaseConversion;
                bool divide = rate != 1 && Math.Abs(tempQuantity) > rate;

                if (divide)
                {
                    lic.Quantity = lic.Quantity / rate;
                    lic.DayAverage = lic.DayAverage / rate;

                    lic.UnitMeasure = costUM;
                }
            }

            lic.Cost = tempCost;
            lic.Profit = tempProfit;
        }

        #endregion        

        #region Filtering

        ObservableCollection<SaleLineItemReportViewModel> lineitems_filtered;
        public ObservableCollection<SaleLineItemReportViewModel> FilteredItems
        {
            get { return lineitems_filtered; }
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

            lineitems_filtered.Clear();

            //add those that pass the filter
            foreach (var item in lineitems_source)
            {
                if (PassesFilter(item))
                {
                    lineitems_filtered.Add(item);
                }
            }
        }

        bool PassesFilter(SaleLineItemReportViewModel li)
        {
            bool cond = true;            

            //if (showSaleOrders)
            {
                //if (li.OrderType != OrderType.Sale) return false;

                Sale sale = (Sale)li.Order;

                if (filteringByPriceList)
                {
                    if (sale.Table == null) cond = false;
                    else cond = sale.Table.PriceList == selectedPriceList;
                }

                if (!cond) return false;

                if (filteringByWaiter)
                {
                    cond = sale.Employee == selectedWaiter;
                }

                if (!cond) return false;

                //ingrediente condition
                //if (filteringByIngredient)
                //{
                //    if (li.Product == null) cond = false;

                //    else if (li.Product != selectedIngredient)
                //    {
                //        cond = false;
                //        foreach (var item in li.Product.Ingredients)
                //        {
                //            if (item.IngredientProduct == selectedIngredient)
                //            {
                //                cond = true; break;
                //            }
                //        }
                //    }
                //}

                if (!cond) return false;
            }
            //else if (showPurchaseOrders)
            //{
            //    if (li.OrderType != OrderType.Purchase) return false;

            //    Purchase purchase = (Purchase)li.Order;

            //    if (filteringByResponsible)
            //        if (selectedResponsible != purchase.Employee) return false;

            //    //if (filteringByPurchaseType)
            //    //    if (selectedPurchaseType != purchase.PurchaseType) return false;
            //}

            if (!cond) return false;

            //name condition
            if (string.IsNullOrWhiteSpace(searchText)) cond = true;
            else cond = li.Product == null ? false : li.Product.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;            

            if (!cond) return false;                        

            //category condition
            if (filteringByCategory )
            {
                cond = false;

                if (li.Product != null)
                {
                    foreach (var item in li.Product.RelatedCategories)
                    {
                        if (item.Category == selectedCategory)
                        {
                            cond = true;
                            break;
                        }
                    }
                }
            }                            

            return cond;
        }

        string searchText;
        public string SearchText 
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;

                    OnFilterConditionsChanged();
                }                
            }
        }

        bool filteringByCategory;
        public bool FilteringByCategory
        {
            get { return filteringByCategory; }
            set
            {
                if (filteringByCategory != value)
                {
                    filteringByCategory = value;
                    OnPropertyChanged("FilteringByCategory");

                    OnFilterConditionsChanged();
                }                
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;

                if (filteringByCategory) 
                {
                    OnFilterConditionsChanged();
                }
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

                OnFilterConditionsChanged();
            }
        }

        PriceList selectedPriceList;
        public PriceList SelectedPriceList
        {
            get { return selectedPriceList; }
            set
            {
                selectedPriceList = value;

                if (filteringByPriceList)
                {
                    OnFilterConditionsChanged();
                }
            }
        }

        bool filteringByWaiter;
        public bool FilteringByWaiter
        {
            get { return filteringByWaiter; }
            set
            {
                if (filteringByWaiter != value)
                {
                    filteringByWaiter = value;
                    OnPropertyChanged("FilteringByWaiter");

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

                if (filteringByWaiter)
                {
                    OnFilterConditionsChanged();
                }
            }
        }

        //bool filteringByIngredient;
        //public bool FilteringByIngredient
        //{
        //    get { return filteringByIngredient; }
        //    set
        //    {
        //        filteringByIngredient = value;
        //        OnPropertyChanged("FilteringByIngredient");                

        //        //if (selectedIngredient != null) 
        //        {
        //            //auxView.Refresh(); 

        //            RefreshView();

        //            if (filteringByIngredient) CalculateIngredientQuantities();

        //            UpdateTotals();                    

        //            //UpdateItems();
        //        }
        //    }
        //}        

        //Product selectedIngredient;
        //public Product SelectedIngredient
        //{
        //    get { return selectedIngredient; }
        //    set
        //    {
        //        selectedIngredient = value;
        //        OnPropertyChanged("SelectedIngredient");

        //        if (filteringByIngredient) 
        //        {
        //            auxView.Refresh();

        //            CalculateIngredientQuantities();

        //            UpdateItems();
        //        }
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

        //        if (filteringByResponsible)
        //        {

        //            auxView.Refresh();
        //            UpdateItems();
        //        }
        //    }
        //}

        #endregion

        #region Grouping

        ObservableCollection<SaleLineItemReportViewModel> lineitems_showing;
        public ObservableCollection<SaleLineItemReportViewModel> ItemsShowing
        {
            get
            {
                if (lineitems_showing == null)
                {
                    lineitems_showing = new ObservableCollection<SaleLineItemReportViewModel>();

                    UpdateSearch();
                }
                return lineitems_showing;
            }
        }

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
        bool groupingByProduct;
        public bool GroupingByProduct         
        {
            get { return groupingByProduct; }
            set
            {
                if (groupingByProduct != value) 
                {
                    groupingByProduct = value;

                    OnGroupingConditionsChanged();
                }                
            }
        }

        bool groupingByCategory;
        public bool GroupingByCategory
        {
            get { return groupingByCategory; }
            set
            {
                if (groupingByCategory != value)
                {
                    groupingByCategory = value;

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

        bool groupingByEmployee;
        public bool GroupingByEmployee
        {
            get { return groupingByEmployee; }
            set
            {
                if (groupingByEmployee != value)
                {
                    groupingByEmployee = value;

                    OnGroupingConditionsChanged();
                }
            }
        }

        #endregion

        #region Totals

        void UpdateTotals()
        {
            total = 0;
            totalprice = 0;
            totalCost = 0;
            totalProfit = 0;
            totalSales = 0;
            totalSalesPrice = 0;
            //totalPurchases = 0;
            //totalPurchasesPrice = 0;

            //double totalingredient = 0;

            int count = 0;

            foreach (var item in lineitems_showing)
            {
                count++;

                total += item.Quantity;
                totalprice += item.Price;

                totalSales += item.Quantity;
                totalSalesPrice += item.Price;
                totalCost += item.Cost;
                totalProfit += item.Profit;

                //if (noGrouping) 
                //{
                //    //if (item.OrderType == OrderType.Sale)
                //    {
                        
                //    }
                //    //else 
                //    //{
                //    //    totalPurchases += item.Quantity;
                //    //    totalPurchasesPrice += item.Price;
                //    //}
                //}
                //else 
                //{
                    
                //    //totalSales += item.QuantitySold;
                //    //totalSalesPrice += item.SalePrice;
                //    //totalPurchases += item.QuantityPurchased;
                //    //totalPurchasesPrice += item.PurchasePrice;
                //}                

                //totalingredient += item.IngredientQuantity;
            }

            Total = total;
            TotalPrice = totalprice;
            TotalCost = totalCost;
            TotalProfit = totalProfit;
            if (totalprice > 0) CostPercentAverage = totalCost / totalprice;
            TotalSales = totalSales;
            TotalSalesPrice = totalSalesPrice;
            //TotalPurchases = totalPurchases;
            //TotalPurchasesPrice = totalPurchasesPrice;
            //TotalIngredient = totalingredient;
        }

        double total;
        public double Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged("Total");
            }
        }

        decimal totalprice;
        public decimal TotalPrice
        {
            get { return totalprice; }
            set
            {
                totalprice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        decimal totalCost;
        public decimal TotalCost
        {
            get { return totalCost; }
            set
            {
                totalCost = value;
                OnPropertyChanged("TotalCost");
            }
        }

        decimal totalProfit;
        public decimal TotalProfit
        {
            get { return totalProfit; }
            set
            {
                totalProfit = value;
                OnPropertyChanged("TotalProfit");
            }
        }

        decimal costPercentAverage;
        public decimal CostPercentAverage
        {
            get { return costPercentAverage; }
            set
            {
                costPercentAverage = value;
                OnPropertyChanged("CostPercentAverage");
            }
        }

        double totalSales;
        public double TotalSales
        {
            get { return totalSales; }
            set
            {
                totalSales = value;
                OnPropertyChanged("TotalSales");
            }
        }

        decimal totalSalesPrice;
        public decimal TotalSalesPrice
        {
            get { return totalSalesPrice; }
            set
            {
                totalSalesPrice = value;
                OnPropertyChanged("TotalSalesPrice");
            }
        }

        //double totalPurchases;
        //public double TotalPurchases
        //{
        //    get { return totalPurchases; }
        //    set
        //    {
        //        totalPurchases = value;
        //        OnPropertyChanged("TotalPurchases");
        //    }
        //}

        //decimal totalPurchasesPrice;
        //public decimal TotalPurchasesPrice
        //{
        //    get { return totalPurchasesPrice; }
        //    set
        //    {
        //        totalPurchasesPrice = value;
        //        OnPropertyChanged("TotalPurchasesPrice");
        //    }
        //}

        //double totalingredient;
        //public double TotalIngredient
        //{
        //    get { return totalingredient; }
        //    set
        //    {
        //        totalingredient = value;
        //        OnPropertyChanged("TotalIngredient");
        //    }
        //}

        #endregion

        #region Columns Visibility

        void UpdateColumnsVisibility()
        {
            DateColumnVisible = noGrouping || groupingByDate;
            QuantityColumnVisible = !groupingByCategory;
            WeekDayColumnVisible = groupingByDayOfWeek;
            WeekStringColumnVisible = groupingByWeek;
            ProductColumnVisible = !(groupingByCategory);
            CategoryColumnVisible = groupingByCategory;
            PriceColumnVisible = true;
            CostColumnVisible = (groupingByProduct || groupingByCategory);
            ProfitColumnVisible = (groupingByProduct || groupingByCategory);
            CostPercentColumnVisible = (groupingByProduct || groupingByCategory);
            //SaleQuantityColumnVisible = showAllOrders && !noGrouping;
            //SalePriceColumnVisible = !convertedToIngredients && !filteringByIngredient && showAllOrders && !noGrouping;
            DayAverageColumnVisible = groupingByDayOfWeek;
            //PurchaseQuantityColumnVisible = showAllOrders && !noGrouping;
            //PurchasePriceColumnVisible = !convertedToIngredients && !filteringByIngredient && showAllOrders && !noGrouping;
            //IngredientQuantityColumnVisible = showSaleOrders && filteringByIngredient;
            //IngredientColumnVisible = showSaleOrders && filteringByIngredient;
            WaiterColumnVisible = (noGrouping || groupingByEmployee);
            //ResponsibleColumnVisible = showPurchaseOrders && noGrouping;
            //PurchaseTypeColumnVisible = showPurchaseOrders && noGrouping;
            AreaColumnVisible = noGrouping;
            OrderColumnVisible = noGrouping;
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

        bool weekDayColumnVisible;
        public bool WeekDayColumnVisible
        {
            get { return weekDayColumnVisible; }
            set
            {
                weekDayColumnVisible = value;
                OnPropertyChanged("WeekDayColumnVisible");
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

        bool categoryColumnVisible;
        public bool CategoryColumnVisible
        {
            get { return categoryColumnVisible; }
            set
            {
                categoryColumnVisible = value;
                OnPropertyChanged("CategoryColumnVisible");
            }
        }

        bool priceColumnVisible;
        public bool PriceColumnVisible
        {
            get { return priceColumnVisible; }
            set
            {
                priceColumnVisible = value;
                OnPropertyChanged("PriceColumnVisible");
            }
        }

        bool costColumnVisible;
        public bool CostColumnVisible
        {
            get { return costColumnVisible; }
            set
            {
                costColumnVisible = value;
                OnPropertyChanged("CostColumnVisible");
            }
        }

        bool profitColumnVisible;
        public bool ProfitColumnVisible
        {
            get { return profitColumnVisible; }
            set
            {
                profitColumnVisible = value;
                OnPropertyChanged("ProfitColumnVisible");
            }
        }

        bool costPercentColumnVisible;
        public bool CostPercentColumnVisible
        {
            get { return costPercentColumnVisible; }
            set
            {
                costPercentColumnVisible = value;
                OnPropertyChanged("CostPercentColumnVisible");
            }
        }

        //bool saleQuantityColumnVisible;
        //public bool SaleQuantityColumnVisible
        //{
        //    get { return saleQuantityColumnVisible; }
        //    set 
        //    {
        //        saleQuantityColumnVisible = value;
        //        OnPropertyChanged("SaleQuantityColumnVisible");
        //    }
        //}

        //bool salePriceColumnVisible;
        //public bool SalePriceColumnVisible
        //{
        //    get { return salePriceColumnVisible; }
        //    set 
        //    {
        //        salePriceColumnVisible = value;
        //        OnPropertyChanged("SalePriceColumnVisible");
        //    }
        //}

        bool dayAverageColumnVisible;
        public bool DayAverageColumnVisible
        {
            get { return dayAverageColumnVisible; }
            set
            {
                dayAverageColumnVisible = value;
                OnPropertyChanged("DayAverageColumnVisible");
            }
        }

        //bool purchaseQuantityColumnVisible;
        //public bool PurchaseQuantityColumnVisible
        //{
        //    get { return purchaseQuantityColumnVisible; }
        //    set
        //    {
        //        purchaseQuantityColumnVisible = value;
        //        OnPropertyChanged("PurchaseQuantityColumnVisible");
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

        //bool ingredientQuantityColumnVisible;

        //public bool IngredientQuantityColumnVisible
        //{
        //    get { return ingredientQuantityColumnVisible; }
        //    set
        //    {
        //        ingredientQuantityColumnVisible = value;
        //        OnPropertyChanged("IngredientQuantityColumnVisible");
        //    }
        //}

        //bool ingredientColumnVisible;
        //public bool IngredientColumnVisible
        //{
        //    get { return ingredientColumnVisible; }
        //    set 
        //    {
        //        ingredientColumnVisible = value;
        //        OnPropertyChanged("IngredientColumnVisible");
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

        //#region Convert To Ingredientes

        //RelayCommand convertToIngredientCommand;
        //public ICommand ConvertToIngredientCommand 
        //{
        //    get 
        //    {
        //        if (convertToIngredientCommand == null)
        //            convertToIngredientCommand = new RelayCommand(x => ConvertToIngredients(), x => CanConvert);
        //        return convertToIngredientCommand;
        //    }
        //}

        //bool convertedToIngredients;
        //bool CanConvert { get { return !convertedToIngredients; } }

        
        //void ConvertToIngredients() 
        //{
        //    ObservableCollection<OperationItemViewModel> ingredientItems = new ObservableCollection<OperationItemViewModel>();

        //    foreach (var item in auxView)
        //    {
        //        OperationItemViewModel lic = (OperationItemViewModel)item;
        //        ConvertRecipeToIngredients(lic, ingredientItems);
        //    }

        //    ResetFilterConditions();

        //    //copy ingredient elements
        //    lineitems.Clear();
        //    foreach (var item in ingredientItems)
        //    {
        //        lineitems.Add(item);
        //    }            

        //    convertedToIngredients = true;

        //    RefreshView();
        //}

        //private void ResetFilterConditions()
        //{
        //    searchText = string.Empty;

        //    filteringByCategory = false;
        //    filteringByWaiter = false;
        //    filteringByPriceList = false;
        //    //filteringByIngredient = false;

        //    OnPropertyChanged("SearchText");

        //    OnPropertyChanged("FilteringByCategory");
        //    OnPropertyChanged("FilteringByWaiter");
        //    OnPropertyChanged("FilteringByPriceList");
        //    OnPropertyChanged("FilteringByIngredient");
        //}        
        
        //bool showingIngredients;
        //public bool ShowingIngredients
        //{
        //    get { return showingIngredients; }
        //    set
        //    {
        //        showingIngredients = value;

        //        if (showingIngredients)
        //        {
        //            ingredientItems = new ObservableCollection<LineItemCopy>();

        //            foreach (var item in auxView)
        //            {
        //                LineItemCopy lic = (LineItemCopy)item;

        //                ConvertRecipeToIngredients(lic, ingredientItems);
        //            }

        //            //update the view
        //            CollectionViewSource cvs = new CollectionViewSource();
        //            cvs.Source = ingredientItems;
        //            auxView = (CollectionView)cvs.View;

        //            auxView.Filter = FilterProduct;

        //            auxView.Refresh();
        //            UpdateItems();
        //        }
        //        else { }
        //    }
        //}

        //private void ConvertRecipeToIngredients(OperationItemViewModel lineitem, ObservableCollection<OperationItemViewModel> ingredientList)
        //{
        //    Product product = lineitem.Product;
        //    double quantity = lineitem.Quantity;

        //    if (product == null) return;

        //    //if it's recipe add its ingredients
        //    if (product.IsRecipe)
        //    {
        //        foreach (var item in product.Ingredients)
        //        {
        //            OperationItemViewModel lic = new OperationItemViewModel();

        //            lic.Quantity = quantity * item.Quantity;
        //            lic.UnitMeasure = item.UnitMeasure;
        //            lic.Product = item.IngredientProduct;                    
        //            lic.Order = lineitem.Order;
        //            //lic.OrderType = lineitem.OrderType;
        //            lic.Date = lineitem.Date;
        //            lic.DayOfWeek = lineitem.DayOfWeek;

        //            ConvertRecipeToIngredients(lic, ingredientList);
        //        }
        //    }
        //    else //if(product.IsIngredient)
        //    {
        //        //lineitem.Price = 0;
        //        ingredientList.Add(lineitem);
        //    }
        //}

        //#endregion        

        #region Export to Excel Command

        RelayCommand exportToExcel;
        public ICommand ExportToExcelCommand
        {
            get
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => Export(false));
                return exportToExcel;
            }
        }

        RelayCommand exportByCateogry;
        public ICommand ExportByCateogry
        {
            get
            {
                if (exportByCateogry == null)
                    exportByCateogry = new RelayCommand(x => Export(true), x => CanExportByCategory);
                return exportByCateogry;
            }
        }

        bool CanExportByCategory
        {
            get { return !groupingByCategory; }
        }

        private void Export(bool groupByCategory)
        {
            Action<CollectionViewGroup, Excel.Range> displayGroup = (group, cell) =>
            {
                if (group.Name != DependencyProperty.UnsetValue)
                {
                    //write the name of the category
                    cell.Value = group.Name;
                    cell.Font.Bold = true;
                }
                cell.Offset[1, 0].Select();

                int row = 1;
                foreach (var item in group.Items)
                {
                    DisplayItem((SaleLineItemReportViewModel)item, cell.Offset[row, 0]);
                    row++;
                    cell.Offset[row, 0].Select();
                }
            };

            int numberOfColumns = CalculateNumberOfColumns();

            ICollectionView view = CollectionViewSource.GetDefaultView(lineitems_showing);

            var excelExporter = base.GetService<IExcelExporter>();

            if (groupByCategory)
            {
                CollectionViewSource cvs = new CollectionViewSource();
                cvs.Source = lineitems_showing;
                ICollectionView groupedView = cvs.View;

                PropertyGroupDescription pgd = new PropertyGroupDescription("Category.Name");
                groupedView.GroupDescriptions.Add(pgd);

                foreach (var item in view.SortDescriptions)
                {
                    groupedView.SortDescriptions.Add(item);
                }

                excelExporter.ExportToExcel<CollectionViewGroup>(groupedView.Groups.Cast<CollectionViewGroup>(),
                    DisplayHeader, displayGroup, numberOfColumns);

                //DisplayInGroups(cvTotals.Groups.Cast<CollectionViewGroup>(), displayGroup);
            }
            else 
            {
                excelExporter.ExportToExcel<SaleLineItemReportViewModel>(view.Cast<SaleLineItemReportViewModel>(),
                DisplayHeader, DisplayItem, numberOfColumns);
            }            
        }

        void DisplayHeader(Excel.Range cell)
        {
            int column = 0;

            if (DateColumnVisible)
            {
                cell.Offset[0, column].Value = "Fecha";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            if (QuantityColumnVisible)
            {
                cell.Offset[0, column].Value = "Cantidad";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            //product column
            if (ProductColumnVisible)
            {
                cell.Offset[0, column].Value = "Producto";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            if (CategoryColumnVisible)
            {
                cell.Offset[0, column].Value = "Categoría";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            if (PriceColumnVisible)
            {
                cell.Offset[0, column].Value = "Precio";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            if (CostColumnVisible)
            {
                cell.Offset[0, column].Value = "Costo";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            if (ProfitColumnVisible)
            {
                cell.Offset[0, column].Value = "Ganacia";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            if (CostPercentColumnVisible)
            {
                cell.Offset[0, column].Value = "Costo %";
                cell.Offset[0, column++].Font.Bold = "True";
            }

            //if (SaleQuantityColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Venta (Cant.)";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}
            //if (SalePriceColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Venta ($)";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}

            //if (PurchaseQuantityColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Compra  (Cant.)";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}
            //if (PurchasePriceColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Compra ($)";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}

            //if (IngredientQuantityColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Cant. Ing.";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}

            //if (IngredientColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Ingrediente";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}

            //if (PurchaseTypeColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Tipo";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}

            //if (ResponsibleColumnVisible)
            //{
            //    cell.Offset[0, column].Value = "Responsable";
            //    cell.Offset[0, column++].Font.Bold = "True";
            //}

            if (WaiterColumnVisible)
            {
                cell.Offset[0, column].Value = "Dependiente";
                cell.Offset[0, column++].Font.Bold = "True";
            } 

            if (AreaColumnVisible)
            {
                cell.Offset[0, column].Value = "Area";
                cell.Offset[0, column++].Font.Bold = "True";
            }
            if (OrderColumnVisible)
            {
                cell.Offset[0, column].Value = "Vale";
                cell.Offset[0, column++].Font.Bold = "True";
            }
        }

        void DisplayItem(SaleLineItemReportViewModel lineitem, Excel.Range cell) 
        {
            int column = 0;

            if (DateColumnVisible)
            {
                cell.Offset[0, column++].Value = lineitem.Date.ToString("m"); ;
            }

            if (QuantityColumnVisible) 
                cell.Offset[0, column++].Value = lineitem.Quantity;

            //product column
            if(ProductColumnVisible) 
                cell.Offset[0, column++].Value = lineitem.Product != null ? lineitem.Product.Name : string.Empty;

            if (CategoryColumnVisible)
                cell.Offset[0, column++].Value = lineitem.Category != null ? lineitem.Category.Name : "Sin categoría";

            if (PriceColumnVisible)
            {
                cell.Offset[0, column].Value = lineitem.Price;
                cell.Offset[0, column++].Style = "Currency";
            }

            if (CostColumnVisible)
            {
                cell.Offset[0, column].Value = lineitem.Cost;
                cell.Offset[0, column++].Style = "Currency";
            }

            if (ProfitColumnVisible)
            {
                cell.Offset[0, column].Value = lineitem.Profit;
                cell.Offset[0, column++].Style = "Currency";
            }

            if (CostPercentColumnVisible)
            {
                cell.Offset[0, column].Value = lineitem.CostToPriceRatio;
                cell.Offset[0, column++].Style = "Percent";
            }

            //if (SaleQuantityColumnVisible)
            //{
            //    cell.Offset[0, column++].Value = lineitem.QuantitySold;
            //}
            //if (SalePriceColumnVisible)
            //{
            //    cell.Offset[0, column].Value = lineitem.SalePrice;
            //    cell.Offset[0, column++].Style = "Currency";
            //}

            //if (PurchaseQuantityColumnVisible)
            //{
            //    cell.Offset[0, column++].Value = lineitem.QuantityPurchased;
            //}
            //if (PurchasePriceColumnVisible)
            //{
            //    cell.Offset[0, column].Value = lineitem.PurchasePrice;
            //    cell.Offset[0, column++].Style = "Currency";
            //}

            //if (IngredientQuantityColumnVisible)
            //{
            //    cell.Offset[0, column++].Value = lineitem.IngredientQuantity;
            //}

            //if (IngredientColumnVisible)
            //{
            //    cell.Offset[0, column++].Value = selectedIngredient.Name;
            //}

            if (WaiterColumnVisible)
            {
                Sale sale = lineitem.Order;
                cell.Offset[0, column++].Value = sale.Employee != null ? sale.Employee.Name : "";
            }

            //if (PurchaseTypeColumnVisible) 
            //{
            //    Purchase purchase = (Purchase)lineitem.Order;
            //    cell.Offset[0, column++].Value = purchase.PurchaseType != null ? purchase.PurchaseType.Name : "";
            //}

            //if (ResponsibleColumnVisible)
            //{
            //    Purchase purchase = (Purchase)lineitem.Order;
            //    cell.Offset[0, column++].Value = purchase.Employee != null ? purchase.Employee.Name : "";
            //}

            if (AreaColumnVisible)
            {
                Sale sale = (Sale)lineitem.Order;
                cell.Offset[0, column++].Value = sale.Table.PriceList.Name;
            }

            if (OrderColumnVisible)
            {
                string orderstring = "Vale " + lineitem.Order.Number;

                cell.Offset[0, column++].Value = orderstring;
            }
        }

        private int CalculateNumberOfColumns()
        {
            int numberOfColumns = 0;

            if (DateColumnVisible) numberOfColumns++;

            if (QuantityColumnVisible) numberOfColumns++;            

            //product column
            if(ProductColumnVisible) numberOfColumns++;

            if (CategoryColumnVisible) numberOfColumns++;

            if (PriceColumnVisible) numberOfColumns++;

            if (CostColumnVisible) numberOfColumns++;

            if (ProfitColumnVisible) numberOfColumns++;

            if (CostPercentColumnVisible) numberOfColumns++;

            //if (SaleQuantityColumnVisible) numberOfColumns++;

            //if (SalePriceColumnVisible) numberOfColumns++;

            //if (PurchaseQuantityColumnVisible) numberOfColumns++;

            //if (PurchasePriceColumnVisible) numberOfColumns++;

            //if (IngredientQuantityColumnVisible) numberOfColumns++;

            //if (IngredientColumnVisible) numberOfColumns++;

            if (WaiterColumnVisible) numberOfColumns++;

            //if (ResponsibleColumnVisible) numberOfColumns++;

            //if (PurchaseTypeColumnVisible) numberOfColumns++;

            if (OrderColumnVisible) numberOfColumns++;

            if (AreaColumnVisible) numberOfColumns++;

            return numberOfColumns;
        }

        

        //Action<Excel.Range> displayHeader = (cell) =>
        //{
        //    int column = 0;

        //    if (DateColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Fecha";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (TypeColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Tipo";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (QuantityColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Cantidad";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    //product column
        //    cell.Offset[0, column].Value = "Producto";
        //    cell.Offset[0, column++].Font.Bold = "True";

        //    if (PriceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Importe";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (SaleQuantityColumnVisible) 
        //    {
        //        cell.Offset[0, column].Value = "Venta";
        //        cell.Offset[0, column++].Font.Bold = "True";                    
        //    }
        //    if (SalePriceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Precio Venta";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (PurchaseQuantityColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Compra";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }
        //    if (PurchasePriceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Precio Compra";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (IngredienteQuantityColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Cant. Ing.";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (IngredienteColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Ingrediente";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }                                

        //    if (WaiterColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Dependiente";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }
        //    if (OrderColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Vale";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }
        //    if (AreaColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Area";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }
        //};

        //Action<LineItemCopy, Excel.Range> displayItem = (lineitem, cell) =>
        //{
        //    int column = 0;

        //    if (DateColumnVisible)
        //    {
        //        cell.Offset[0, column++].Value = lineitem.Date.ToString("m"); ;
        //    }

        //    if (TypeColumnVisible) cell.Offset[0, column++].Value = lineitem.OrderType.ToString();

        //    if (QuantityColumnVisible) cell.Offset[0, column++].Value = lineitem.Quantity;

        //    //product column
        //    cell.Offset[0, column++].Value = lineitem.Product != null ? lineitem.Product.Name : "";

        //    if (PriceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = lineitem.Price;
        //        cell.Offset[0, column++].Style = "Currency";
        //    } 

        //    if (SaleQuantityColumnVisible)
        //    {                    
        //        cell.Offset[0, column++].Value = lineitem.QuantitySold;
        //    }
        //    if (SalePriceColumnVisible)
        //    {
        //        cell.Offset[0, column++].Value = lineitem.SalePrice;
        //    }

        //    if (PurchaseQuantityColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = lineitem.QuantityPurchased;
        //    }
        //    if (PurchasePriceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = lineitem.PurchasePrice;
        //    }

        //    if (IngredienteQuantityColumnVisible)
        //    {
        //        cell.Offset[0, column++].Value = lineitem.IngredientQuantity;
        //    }

        //    if (IngredienteColumnVisible)
        //    {
        //        cell.Offset[0, column++].Value = selectedIngredient.Name;
        //    }                

        //    if (WaiterColumnVisible)
        //    {
        //        Sale sale = (Sale)lineitem.Order;
        //        cell.Offset[0, column++].Value = sale.Waiter != null ? sale.Waiter.Name : "";
        //    }

        //    if (OrderColumnVisible)
        //    {
        //        Sale sale = (Sale)lineitem.Order;
        //        cell.Offset[0, column++].Value = "Vale " + sale.Number;
        //    }

        //    if (AreaColumnVisible)
        //    {
        //        Sale sale = (Sale)lineitem.Order;
        //        cell.Offset[0, column++].Value = sale.PriceList.Name;
        //    }
        //};

        #endregion        

        #region Advanced Search Visibility

        bool advancedSearchVisible;
        public bool AdvancedSearchVisible
        {
            get { return advancedSearchVisible; }
            set
            {
                advancedSearchVisible = value;
                OnPropertyChanged("AdvancedSearchVisible");
            }
        }

        RelayCommand showAdvancedSearchPopupCommand;
        public ICommand ShowAdvancedSearchPopupCommand
        {
            get
            {
                if (showAdvancedSearchPopupCommand == null)
                    showAdvancedSearchPopupCommand = new RelayCommand(x => ShowProductTemplatePopup());
                return showAdvancedSearchPopupCommand;
            }
        }

        void ShowProductTemplatePopup()
        {
            AdvancedSearchVisible = true;
        }

        #endregion

        #region Show Order Command

        bool showingSale;
        public bool ShowingSale
        {
            get { return showingSale; }
            set
            {
                if (showingSale != value)
                {
                    showingSale = value;
                    OnPropertyChanged("ShowingSale");
                }
            }
        }
        bool showingPurchase;
        public bool ShowingPurchase
        {
            get { return showingPurchase; }
            set
            {
                if (showingPurchase != value)
                {
                    showingPurchase = value;
                    OnPropertyChanged("ShowingPurchase");
                }
            }
        }

        SaleViewModel selectedSale;
        public SaleViewModel SelectedSale
        {
            get { return selectedSale; }
            set
            {
                selectedSale = value;
                OnPropertyChanged("SelectedSale");
            }
        }

        PurchaseViewModel selectedPurchase;
        public PurchaseViewModel SelectedPurchase
        {
            get { return selectedPurchase; }
            set
            {
                selectedPurchase = value;
                OnPropertyChanged("SelectedPurchase");
            }
        }

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
            //if (x is Sale)
            //{
            //    Sale sale = (Sale)x;
            //    SaleViewModel viewmodel = new SaleViewModel(appvm, sale);

            //    SelectedSale = viewmodel;
            //    ShowingSale = true;
            //}
            //else if (x is Purchase)
            //{
            //    Purchase purchase = (Purchase)x;
            //    PurchaseViewModel viewmodel = new PurchaseViewModel(appvm, purchase, inventoryService);

            //    SelectedPurchase = viewmodel;
            //    ShowingPurchase = true;
            //}
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
        }

        void OnRemoved(Sale s)
        {
            foreach (var item in lineitems_showing)
            {
                if (item.Order == s)
                {
                    lineitems_showing.Remove(item);
                    UpdateTotals();
                    break;
                }
            }
        }

        #endregion

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
        //    if (showingSale)
        //    {
        //        ShowingSale = false;
        //        SelectedSale = null;
        //    }
        //    if (showingPurchase)
        //    {
        //        ShowingPurchase = false;
        //        //selectedPurchase.RemoveEvents();
        //        SelectedPurchase = null;
        //    }
        //}

        //#endregion
    }
}
