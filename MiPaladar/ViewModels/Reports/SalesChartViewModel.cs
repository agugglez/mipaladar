using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Enums;
using System.ComponentModel;
using MiPaladar.Views;
using System.Windows;

namespace MiPaladar.ViewModels
{
    public class SalesChartViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        //IExcelExporter excelExporter;

        public SalesChartViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            //initialize dates
            DateTime today = DateTime.Today;
            DayOfWeek dow = today.DayOfWeek;
            sevenDaysDate = today.Add(TimeSpan.FromDays(-7));
            fourWeeksDate = today.AddDays(-(dow != DayOfWeek.Sunday ? 27 + (int)dow : 34));
            DateTime sixMonthsAgo = today.AddMonths(-6);
            sixMonthsDate = new DateTime(sixMonthsAgo.Year, sixMonthsAgo.Month, 1);

            //selectedDim = GroupOptions.NoGrouping;

            orders_source = new ObservableCollection<SalesChartItemViewModel>();
            //orders_filtered = new ObservableCollection<SalesChartItemViewModel>();
            sevenDaysItems = new ObservableCollection<SalesChartItemViewModel>();
            fourWeeksItems = new ObservableCollection<SalesChartItemViewModel>();
            sixMonthsItems = new ObservableCollection<SalesChartItemViewModel>();
            dayAverageItems = new ObservableCollection<SalesChartItemViewModel>();

            //UpdateColumnsVisibility();

            UpdateSearch();
        }

        public override string DisplayName
        {
            get { return "Vales de Venta"; }
        }
        
        #region Dates

        //DateTime fromDate;
        //public DateTime FromDate
        //{
        //    get { return fromDate; }
        //    set { fromDate = value; }
        //}

        //DateTime toDate;
        //public DateTime ToDate
        //{
        //    get { return toDate; }
        //    set { toDate = value; }
        //}

        DateTime sevenDaysDate;
        DateTime fourWeeksDate;
        DateTime sixMonthsDate;

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
        public ObservableCollection<Table> Tables
        {
            get { return appvm.TablesOC; }
        }
        public ObservableCollection<Product> Products 
        {
            get { return appvm.ProductsOC; }
        }

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
        ObservableCollection<SalesChartItemViewModel> orders_source;

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

            //bWorker_DoWork(null, null);

            //bWorker_RunWorkerCompleted(null, null);
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            orders_source.Clear();

            DateTime toDatePlusOne = DateTime.Today.AddDays(1);

            RestaurantDBEntities context = new RestaurantDBEntities();

            var query = from order in context.Orders.OfType<Sale>()
                        where order.Date >= sixMonthsDate && order.Date < toDatePlusOne
                        select order;

            foreach (var item in query)
            {
                SalesChartItemViewModel sc = new SalesChartItemViewModel(item);
                sc.Count = 1;
                orders_source.Add(sc);
            }

            //var query_p = from order in context.Orders.OfType<Purchase>()
            //              where order.Date >= fromDate && order.Date < toDatePlusOne
            //              select order;

            //foreach (var item in query_p)
            //{
            //    SalesChartItemViewModel sc = new SalesChartItemViewModel(item);
            //    orders_source.Add(sc);
            //}

            //FilterItems();
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CreateItemsShowing();

            //UpdateTotals();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        //void OnFilterConditionsChanged() 
        //{
        //    FilterItems();

        //    CreateItemsShowing();

        //    UpdateTotals();
        //}

        //void OnGroupingConditionsChanged()
        //{
        //    CreateItemsShowing();

        //    UpdateColumnsVisibility();
        //}

        #endregion

        #region Grouping

        ObservableCollection<SalesChartItemViewModel> sevenDaysItems;
        public ObservableCollection<SalesChartItemViewModel> SevenDaysData
        {
            get { return sevenDaysItems; }
        }

        ObservableCollection<SalesChartItemViewModel> fourWeeksItems;
        public ObservableCollection<SalesChartItemViewModel> FourWeeksData
        {
            get { return fourWeeksItems; }
        }

        ObservableCollection<SalesChartItemViewModel> sixMonthsItems;
        public ObservableCollection<SalesChartItemViewModel> SixMonthsData
        {
            get { return sixMonthsItems; }
        }

        ObservableCollection<SalesChartItemViewModel> dayAverageItems;
        public ObservableCollection<SalesChartItemViewModel> DayAverageData
        {
            get { return dayAverageItems; }
        }

        void CreateItemsShowing()
        {
            sevenDaysItems.Clear();
            fourWeeksItems.Clear();
            sixMonthsItems.Clear();
            dayAverageItems.Clear();

            //day  data
            var groupsByDate = from item in orders_source
                               where item.Date >= sevenDaysDate
                               group item by item.Date;

            foreach (var group in groupsByDate)
            {
                SalesChartItemViewModel sc = new SalesChartItemViewModel();

                sc.Date = group.Key;
                sc.X = sc.Date.ToString("ddd d MMM");

                MakeSum(group, sc);

                sc.Y1 = Math.Round(sc.TotalSale);
                sc.Y2 = Math.Round(sc.TotalCost);

                sevenDaysItems.Add(sc);
            }

            //week data
            var groupsByWeek = from item in orders_source
                               where item.Date >= fourWeeksDate
                               let mondayDate = SalesChartItemViewModel.GetWeekMonday(item.Date)
                               group item by mondayDate;

            foreach (var group in groupsByWeek)
            {
                SalesChartItemViewModel sc = new SalesChartItemViewModel();

                sc.Date = group.Key;
                sc.X = sc.WeekString;

                MakeSum(group, sc);

                sc.Y1 = Math.Round(sc.TotalSale);
                sc.Y2 = Math.Round(sc.TotalCost);

                fourWeeksItems.Add(sc);
            }

            //months data
            var queryNestedGroups = from item in orders_source
                                    where item.Date >= sixMonthsDate
                                    group item by item.Date.Year into yearGroup
                                    from monthGroup in
                                        (from item in yearGroup
                                         group item by item.Date.Month)
                                    group monthGroup by yearGroup.Key;

            foreach (var yearGroup in queryNestedGroups)
            {
                foreach (var monthGroup in yearGroup)
                {
                    SalesChartItemViewModel sc = new SalesChartItemViewModel();

                    //get month from the first sale
                    sc.Date = monthGroup.First().Date;
                    sc.X = sc.Date.ToString("MMM/yy");

                    MakeSum(monthGroup, sc);

                    sc.Y1 = Math.Round(sc.TotalSale);
                    sc.Y2 = Math.Round(sc.TotalCost);

                    sixMonthsItems.Add(sc);
                }
            }

            //day averages data
            var groupsByDayOfWeek = from item in orders_source
                                    orderby item.Date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)item.Date.DayOfWeek - 1
                                    group item by item.DayOfWeek;

            foreach (var group in groupsByDayOfWeek)
            {
                SalesChartItemViewModel sc = new SalesChartItemViewModel();

                sc.X = group.First().Date.ToString("dddd");

                sc.DayOfWeek = group.Key;

                MakeSum(group, sc);

                if (sc.NumberOfDays != 0)
                {
                    sc.Y1 = Math.Round(sc.TotalSale / sc.NumberOfDays);
                    sc.Y2 = Math.Round(sc.TotalCost / sc.NumberOfDays);
                }

                dayAverageItems.Add(sc);
            }
        }

        private void MakeSum(IEnumerable<SalesChartItemViewModel> group, SalesChartItemViewModel sc)
        {
            int count = 0;
            int clients = 0;
            decimal discount = 0;
            decimal total_sale = 0;
            //decimal total_purchase = 0;
            decimal total_cost = 0;

            List<DateTime> different_dates = new List<DateTime>();

            foreach (SalesChartItemViewModel item in group)
            {
                //if (item.IsSale)
                {
                    count++;
                    clients += item.Clients;
                    discount += item.Discount;

                    total_sale += item.TotalSale;
                    total_cost += item.TotalCost;

                    if (!different_dates.Contains(item.Date.Date)) different_dates.Add(item.Date.Date);
                }
                
                //else
                //{
                //    total_purchase += item.TotalPurchase;
                //}
            }

            sc.Count = count;
            sc.Clients = clients;
            sc.Discount = discount;
            sc.TotalSale = total_sale;
            //sc.TotalPurchase = total_purchase;
            sc.TotalCost = total_cost;

            sc.NumberOfDays = different_dates.Count;
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
            //Action<Excel.Range> displayHeader = (cell) =>
            //{
            //    int column = 0;

            //    if (DateColumnVisible) 
            //    {
            //        cell.Offset[0, column].Value = "Fecha";
            //        cell.Offset[0, column++].Font.Bold = "True";
            //    }
                                
                
            //    if (OrderCountColumnVisible)
            //    {
            //        cell.Offset[0, column].Value = "# vales";
            //        cell.Offset[0, column++].Font.Bold = "True";
            //    }

            //    if (ClientCountColumnVisible)
            //    {
            //        cell.Offset[0, column].Value = "Clientes";
            //        cell.Offset[0, column++].Font.Bold = "True";
            //    }                
                
            //        cell.Offset[0, column].Value = "Importe";
            //        cell.Offset[0, column++].Font.Bold = "True";               

            //    if (PriceByClientColumnVisible) 
            //    {
            //        cell.Offset[0, column].Value = "Importe/Cliente";
            //        cell.Offset[0, column++].Font.Bold = "True";
            //    }
            //    if (DiscountColumnVisible) 
            //    {
            //        cell.Offset[0, column].Value = "Descuento";
            //        cell.Offset[0, column++].Font.Bold = "True";
            //    }
            //};

            //Action<OrdersReportItemViewModel, Excel.Range> displayItem = (orderCopy, cell) =>
            //{
            //    int column = 0;

            //    if (DateColumnVisible)
            //    {
            //        cell.Offset[0, column++].Value = orderCopy.Date.ToString("M");
            //    }                
                
            //    if (OrderCountColumnVisible)
            //    {
            //        cell.Offset[0, column++].Value = orderCopy.Count;
            //    }
            //    if (ClientCountColumnVisible)
            //    {
            //        cell.Offset[0, column++].Value = orderCopy.Clients;
            //    }
            //    //if (SaleTotalColumnVisible)
            //    {
            //        cell.Offset[0, column].Value = orderCopy.TotalPrice;
            //        cell.Offset[0, column++].Style = "Currency";
            //    }
                

            //    if (PriceByClientColumnVisible)
            //    {
            //        cell.Offset[0, column].Value = orderCopy.SalesByClient;
            //        cell.Offset[0, column++].Style = "Currency";
            //    }
            //    if (DiscountColumnVisible)
            //    {
            //        cell.Offset[0, column].Value = orderCopy.Discount;
            //        cell.Offset[0, column++].Style = "Currency";
            //    }
            //};

            //int numberOfColumns = CountVisibleColumns();

            ////get the view so it keeps the sort descriptions
            //ICollectionView view = CollectionViewSource.GetDefaultView(sevenDaysItems);

            //var excelExporter = base.GetService<IExcelExporter>();

            //excelExporter.ExportToExcel<OrdersReportItemViewModel>(view.Cast<OrdersReportItemViewModel>(),
            //    displayHeader, displayItem, numberOfColumns);            
        }

        int CountVisibleColumns() 
        {
            int columns = 0;

            //if (DateColumnVisible)
            //{
            //    columns++;
            //}
            
            
            //if (OrderCountColumnVisible)
            //{
            //    columns++;
            //}
            //if (ClientCountColumnVisible)
            //{
            //    columns++;
            //}
            ////"importe"
            ////if (SaleTotalColumnVisible)
            //{                
            //    columns++;
            //}   columns++;

            //if (PriceByClientColumnVisible)
            //{
            //    columns++;
            //}
            //if (DiscountColumnVisible)
            //{
            //    columns++;
            //}

            return columns;
        }

        #endregion        
    }
}
