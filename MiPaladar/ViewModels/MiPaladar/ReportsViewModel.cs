using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;

using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public ReportsViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "REPORTES"; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        #region Totals Commands

        RelayCommand showTotalsCommand;
        public ICommand TotalsCommand
        {
            get
            {
                if (showTotalsCommand == null)
                {
                    showTotalsCommand = new RelayCommand(x => this.ShowTotals());
                }
                return showTotalsCommand;
            }
        }
        void ShowTotals()
        {
            var windowManager = base.GetService<IWindowManager>();
            if (windowManager.Exists<TotalesViewModel>())
                windowManager.Activate<TotalesViewModel>();
            else
            {
                TotalesViewModel tvm = new TotalesViewModel(appvm);
                windowManager.Show(tvm);
            }
        }

        #endregion

        //#region Compare Products Commands

        //RelayCommand compareProductsCommand;
        //public ICommand CompareProductsCommand
        //{
        //    get
        //    {
        //        if (compareProductsCommand == null)
        //        {
        //            compareProductsCommand = new RelayCommand(x => this.ShowCompareProducts());
        //        }
        //        return compareProductsCommand;
        //    }
        //}
        //private void ShowCompareProducts()
        //{
        //    var windowManager = base.GetService<IWindowManager>();
        //    if (windowManager.Exists<CompareProductsViewModel>())
        //        windowManager.Activate<CompareProductsViewModel>();
        //    else
        //    {
        //        CompareProductsViewModel ovm = new CompareProductsViewModel(this);
        //        windowManager.Show(ovm);
        //    }
        //}

        //#endregion

        #region Orders Command

        RelayCommand ordersCommand;
        public ICommand OrdersCommand
        {
            get
            {
                if (ordersCommand == null)
                {
                    ordersCommand = new RelayCommand(x => this.ShowOrders());
                }
                return ordersCommand;
            }
        }

        private void ShowOrders()
        {
            var windowManager = base.GetService<IWindowManager>();
            //if (windowManager.Exists<OrdersViewModel>())
            //    windowManager.Activate<OrdersViewModel>();
            //else
            //{
                
            //}

            OrdersViewModel ovm = new OrdersViewModel(appvm);
            windowManager.Show(ovm);
        }

        #endregion

        #region Daily Sales Command

        RelayCommand dailySalesCommand;
        public ICommand DailySalesCommand
        {
            get
            {
                if (dailySalesCommand == null)
                {
                    dailySalesCommand = new RelayCommand(x => this.ShowDailySalesReport());
                }
                return dailySalesCommand;
            }
        }

        private void ShowDailySalesReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (
                windowManager.Exists(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByDate)
                )
                windowManager.Activate(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByDate);
            else
            {
                OrdersViewModel ovm = new OrdersViewModel(appvm);
                ovm.Title = "Ventas Diarias";

                ovm.FromDate = DateTime.Today.AddDays(-6);
                ovm.ToDate = DateTime.Today;

                ovm.GroupingByDate = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Weekly Sales Command

        RelayCommand weeklySalesCommand;
        public ICommand WeeklySalesCommand
        {
            get
            {
                if (weeklySalesCommand == null)
                {
                    weeklySalesCommand = new RelayCommand(x => this.ShowWeeklySalesReport());
                }
                return weeklySalesCommand;
            }
        }

        private void ShowWeeklySalesReport()
        {
            var windowManager = base.GetService<IWindowManager>();
            if (
                windowManager.Exists(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByWeek)
                )
                windowManager.Activate(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByWeek);
            else
            {
                OrdersViewModel ovm = new OrdersViewModel(appvm);
                ovm.Title = "Ventas Semanales";

                //set date to 4 weeks ago
                DayOfWeek dow = DateTime.Today.DayOfWeek;
                int days = dow != DayOfWeek.Sunday ? 20 + (int)dow : 27;

                ovm.FromDate = DateTime.Today.AddDays(-days);
                ovm.ToDate = DateTime.Today;

                ovm.GroupingByWeek = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Monthly Sales Command

        RelayCommand monthlySalesCommand;
        public ICommand MonthlySalesCommand
        {
            get
            {
                if (monthlySalesCommand == null)
                {
                    monthlySalesCommand = new RelayCommand(x => this.ShowMonthlySalesReport());
                }
                return monthlySalesCommand;
            }
        }

        private void ShowMonthlySalesReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (
                windowManager.Exists(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByMonth)
                )
                windowManager.Activate(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByMonth);
            else
            {
                OrdersViewModel ovm = new OrdersViewModel(appvm);
                ovm.Title = "Ventas Mensuales";

                //first day of the month
                ovm.FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                ovm.ToDate = DateTime.Today;

                ovm.GroupingByMonth = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Sales by Employee Command

        RelayCommand salesByEmployeeCommand;
        public ICommand SalesByEmployeeCommand
        {
            get
            {
                if (salesByEmployeeCommand == null)
                {
                    salesByEmployeeCommand = new RelayCommand(x => this.ShowSalesByEmployeeReport());
                }
                return salesByEmployeeCommand;
            }
        }

        private void ShowSalesByEmployeeReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (
                windowManager.Exists(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByWaiter)
                )
                windowManager.Activate(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByWaiter);
            else
            {
                OrdersViewModel ovm = new OrdersViewModel(appvm);
                ovm.Title = "Ventas x Dependientes";

                ovm.GroupingByWaiter = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Sales by Area Command

        RelayCommand salesByAreaCommand;
        public ICommand SalesByAreaCommand
        {
            get
            {
                if (salesByAreaCommand == null)
                {
                    salesByAreaCommand = new RelayCommand(x => this.ShowSalesByAreaReport());
                }
                return salesByAreaCommand;
            }
        }

        private void ShowSalesByAreaReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (
                windowManager.Exists(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByArea)
                )
                windowManager.Activate(x => x is OrdersViewModel && ((OrdersViewModel)x).GroupingByArea);
            else
            {
                OrdersViewModel ovm = new OrdersViewModel(appvm);
                ovm.Title = "Ventas x Area";

                ovm.GroupingByArea = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Sales Report Command

        RelayCommand salesReportCommand;
        public ICommand SalesReportCommand
        {
            get
            {
                if (salesReportCommand == null)
                {
                    salesReportCommand = new RelayCommand(x => this.ShowSalesReport());
                }
                return salesReportCommand;
            }
        }

        private void ShowSalesReport()
        {
            var windowManager = base.GetService<IWindowManager>();
            if (windowManager.Exists<SalesChartViewModel>())
                windowManager.Activate<SalesChartViewModel>();
            else
            {
                SalesChartViewModel ovm = new SalesChartViewModel(appvm);
                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Operations Command

        RelayCommand operationsCommand;
        public ICommand OperationsCommand
        {
            get
            {
                if (operationsCommand == null)
                {
                    operationsCommand = new RelayCommand(x => this.Showoperations());
                }
                return operationsCommand;
            }
        }
        private void Showoperations()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (
                windowManager.Exists(x => x is SalesByItemViewModel && ((SalesByItemViewModel)x).GroupingByProduct)
                )
                windowManager.Activate(x => x is SalesByItemViewModel && ((SalesByItemViewModel)x).GroupingByProduct);
            else
            {
                SalesByItemViewModel ovm = new SalesByItemViewModel(appvm);
                ovm.Title = "Ventas x Productos";

                ovm.GroupingByProduct = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Sales by Category Command

        RelayCommand salesByCategoryCommand;
        public ICommand SalesByCategoryCommand
        {
            get
            {
                if (salesByCategoryCommand == null)
                {
                    salesByCategoryCommand = new RelayCommand(x => this.ShowSalesByCategoryReport());
                }
                return salesByCategoryCommand;
            }
        }
        private void ShowSalesByCategoryReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (
                windowManager.Exists(x => x is SalesByItemViewModel && ((SalesByItemViewModel)x).GroupingByCategory)
                )
                windowManager.Activate(x => x is SalesByItemViewModel && ((SalesByItemViewModel)x).GroupingByCategory);
            else
            {
                SalesByItemViewModel ovm = new SalesByItemViewModel(appvm);
                ovm.Title = "Ventas x Categoría";

                ovm.GroupingByCategory = true;

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Sold Products by Employee Command

        RelayCommand soldProductsByEmployeeCommand;
        public ICommand SoldProductsByEmployeeCommand
        {
            get
            {
                if (soldProductsByEmployeeCommand == null)
                {
                    soldProductsByEmployeeCommand = new RelayCommand(x => this.ShowSoldProductsByEmployeeReport());
                }
                return soldProductsByEmployeeCommand;
            }
        }
        private void ShowSoldProductsByEmployeeReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (windowManager.Exists<SoldProductsByEmployeeViewModel>())
                windowManager.Activate<SoldProductsByEmployeeViewModel>();
            else
            {
                SoldProductsByEmployeeViewModel ovm = new SoldProductsByEmployeeViewModel(appvm);

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Sales By Item By Shift Command

        RelayCommand salesByItemByShiftCommand;
        public ICommand SalesByItemByShiftCommand
        {
            get
            {
                if (salesByItemByShiftCommand == null)
                {
                    salesByItemByShiftCommand = new RelayCommand(x => this.ShowSalesByItemByShiftReport());
                }
                return salesByItemByShiftCommand;
            }
        }
        private void ShowSalesByItemByShiftReport()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (windowManager.Exists<SalesByItemByShiftViewModel>())
                windowManager.Activate<SalesByItemByShiftViewModel>();
            else
            {
                SalesByItemByShiftViewModel ovm = new SalesByItemByShiftViewModel(appvm);

                windowManager.Show(ovm);
            }
        }

        #endregion

        #region FollowProductCommand

        RelayCommand followProductCommand;
        public ICommand FollowProductCommand
        {
            get
            {
                if (followProductCommand == null)
                {
                    followProductCommand = new RelayCommand(x => this.FollowProduct());
                }
                return followProductCommand;
            }
        }
        void FollowProduct()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (windowManager.Exists<FollowProductViewModel>())
                windowManager.Activate<FollowProductViewModel>();
            else
            {
                FollowProductViewModel ovm = new FollowProductViewModel(appvm);
                windowManager.Show(ovm);
            }
        }

        #endregion

        #region Day Averages Command

        RelayCommand dayAveragesReportCommand;
        public ICommand DayAveragesReportCommand
        {
            get
            {
                if (dayAveragesReportCommand == null)
                {
                    dayAveragesReportCommand = new RelayCommand(x => this.ShowDayAveragesReportWindow());
                }
                return dayAveragesReportCommand;
            }
        }
        void ShowDayAveragesReportWindow()
        {
            var windowManager = base.GetService<IWindowManager>();

            if (windowManager.Exists<DayAveragesReportViewModel>())
                windowManager.Activate<DayAveragesReportViewModel>();
            else
            {
                DayAveragesReportViewModel ovm = new DayAveragesReportViewModel(appvm);
                windowManager.Show(ovm);
            }
        }

        #endregion 
    }
}
