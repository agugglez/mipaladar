using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;
using MiPaladar.MVVM;
using MiPaladar.Classes;

using System.Windows.Input;
using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public class ReportsCenterViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public ReportsCenterViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "REPORTES"; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }


        #region Show Form Report Command

        RelayCommand showFormReportCmd;
        public RelayCommand ShowFormReportCommand
        {
            get
            {
                if (showFormReportCmd == null)
                {
                    showFormReportCmd = new RelayCommand(x => ShowFormReport((string)x));
                }
                return showFormReportCmd;
            }
        }

        private void ShowFormReport(string param)
        {
            var windowManager = base.GetService<IWindowManager>();

            ReportsWindowViewModel vm = null;

            //income vs expense
            if (param == "GlobalSales")
            {
                vm = new GlobalSalesReportViewModel(appvm);
                //vm = new ReportsWindowViewModel(appvm, "Ventas Globales", ReportType.GlobalSales);
            }
            else if (param == "SalesByItem")
            {
                vm = new SalesByItemReportViewModel(appvm);
            }
            else if (param == "SalesByCategory")
            {
                vm = new SalesByCategoryReportViewModel(appvm);
            }
            else if (param == "SalesPerson")
            {
                vm = new SalesPersonReportViewModel(appvm);
            }
            else if (param == "DayOfWeekSales")
            {
                vm = new DayOfWeekSalesReportViewModel(appvm);
            }
            else if (param == "ProductClasses")
            {
                vm = new ProductClassesReportViewModel(appvm);
            }
            else if (param == "WIPByItem")
            {
                vm = new WIPByItemReportViewModel(appvm);
            }
            else if (param == "CostByItem")
            {
                vm = new CostByItemReportViewModel(appvm);
            }
            //sales projections
            else if (param == "SalesProjections")
            {
                vm = new SalesProjectionsReportViewModel(appvm);
            }
            else if (param == "WIPProjections")
            {
                vm = new WIPProjectionsReportViewModel(appvm);
            }
            else if (param == "CostProjections")
            {
                vm = new CostProjectionsReportViewModel(appvm);
            }
            else if (param == "ServiceTime")
            {
                vm = new ServiceTimeReportViewModel(appvm);
            }
            else if (param == "DemandByHour")
            {
                vm = new DemandByHourReportViewModel(appvm);
            }

            windowManager.Show(vm);
        }

        #endregion   
    
        //#region Show Excel Report Command

        //BackgroundWorker excelWorker;
        //ProgressDialogViewModel excelProgressDialog;

        //RelayCommand showExcelReportCmd;
        //public RelayCommand ShowExcelReportCommand
        //{
        //    get
        //    {
        //        if (showExcelReportCmd == null)
        //        {
        //            showExcelReportCmd = new RelayCommand(x => ShowExcelReport((string)x));
        //        }
        //        return showExcelReportCmd;
        //    }
        //}

        //private void ShowExcelReport(string param)
        //{
        //    if (param == "sp") rType = ReportType.SalesProjectionsByItem;
        //    else if (param == "wp") rType = ReportType.WIPProjectionsByItem;
        //    else if (param == "cp") rType = ReportType.CostProjectionsByItem;
        //    else if (param == "ive") rType = ReportType.GlobalSales;

        //    CustomizeReportOptions cro = new CustomizeReportOptions();
        //    if (!ShowCustomizeReportDialog(cro)) return;

        //    ExportToExcel(cro);
        //}

        //bool ShowCustomizeReportDialog(CustomizeReportOptions cro)
        //{
        //    CustomizeReportViewModel dialog = new CustomizeReportViewModel(appvm, cro, showCategories, showTags);

        //    var windowManager = base.GetService<IWindowManager>();

        //    if (windowManager.ShowDialog(dialog, appvm) == true)
        //    {
        //        dialog.SyncCustomizeOptions(cro);
        //        return true;
        //    }

        //    return false;
        //}

        //private void ExportToExcel(CustomizeReportOptions cro)
        //{
        //    //init background worker
        //    excelWorker = new BackgroundWorker();

        //    excelWorker.DoWork += new DoWorkEventHandler(excelWorker_DoWork);
        //    excelWorker.WorkerReportsProgress = true;
        //    excelWorker.ProgressChanged += new ProgressChangedEventHandler(excelWorker_ProgressChanged);
        //    excelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(excelWorker_RunWorkerCompleted);

        //    //init progress dialog
        //    excelProgressDialog = new ProgressDialogViewModel();
        //    excelProgressDialog.Message = "Exportando a Excel...";
        //    excelProgressDialog.IsBusy = true;

        //    var windowManager = base.GetService<IWindowManager>();

        //    //run worker
        //    excelWorker.RunWorkerAsync(new object[] { rType, cro });

        //    //show progress dialog
        //    windowManager.ShowDialog(excelProgressDialog, appvm);
        //}

        //void excelWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    object[] args = (object[])e.Argument;
        //    CustomizeReportOptions cro = (CustomizeReportOptions)args[1];

        //    var excelExporter = base.GetService<IExcelExporter>();

        //    excelExporter.CreateAndExportReportToExcel(cro, excelWorker);
        //}

        //void excelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    excelProgressDialog.Progress = e.ProgressPercentage;
        //}

        //void excelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    excelWorker.DoWork -= excelWorker_DoWork;
        //    excelWorker.ProgressChanged -= excelWorker_ProgressChanged;
        //    excelWorker.RunWorkerCompleted -= excelWorker_RunWorkerCompleted;

        //    excelWorker = null;

        //    excelProgressDialog.IsBusy = false;

        //    //close progress dialog
        //    var windowManager = base.GetService<IWindowManager>();
        //    windowManager.Close(excelProgressDialog);
        //}

        //#endregion 

        //#region Test Command

        //RelayCommand tstCmd;
        //public RelayCommand TestReportCommand
        //{
        //    get
        //    {
        //        if (tstCmd == null)
        //        {
        //            tstCmd = new RelayCommand(x => ShowTestReport());
        //        }
        //        return tstCmd;
        //    }
        //}

        //void ShowTestReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportViewModel vm = new ReportViewModel();

        //    windowManager.Show(vm);
        //}

        //#endregion        

        //COST REPORTS

        //#region Daily Cost Average By Item

        //RelayCommand dowCostAvgByItemCmd;
        //public RelayCommand DailyCostAverageByItemCommand
        //{
        //    get
        //    {
        //        if (dowCostAvgByItemCmd == null)
        //        {
        //            dowCostAvgByItemCmd = new RelayCommand(x => this.ShowDailyCostAvgByItem());
        //        }
        //        return dowCostAvgByItemCmd;
        //    }
        //}

        //void ShowDailyCostAvgByItem()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel vm = new ReportsWindowViewModel(appvm, "Promedios Diarios (Costos)", ReportType.DayOfWeekCostAverageByItem);

        //    windowManager.Show(vm);
        //}

        //#endregion

        //#region Costs Report Commands



        //RelayCommand cbiCommand;
        //public ICommand CostsByItemCommand
        //{
        //    get
        //    {
        //        if (cbiCommand == null)
        //        {
        //            cbiCommand = new RelayCommand(x => this.ShowCosts());
        //        }
        //        return cbiCommand;
        //    }
        //}
        //void ShowCosts()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Costos por Productos", ReportType.CostByItem);

        //    windowManager.Show(ovm);
        //}

        //#endregion

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

        //#region Orders Command

        //RelayCommand ordersCommand;
        //public ICommand OrdersCommand
        //{
        //    get
        //    {
        //        if (ordersCommand == null)
        //        {
        //            ordersCommand = new RelayCommand(x => this.ShowOrders());
        //        }
        //        return ordersCommand;
        //    }
        //}

        //private void ShowOrders()
        //{
        //    var windowManager = base.GetService<IWindowManager>();
        //    //if (windowManager.Exists<OrdersViewModel>())
        //    //    windowManager.Activate<OrdersViewModel>();
        //    //else
        //    //{

        //    //}

        //    OrdersViewModel ovm = new OrdersViewModel(appvm);
        //    windowManager.Show(ovm);
        //}

        //#endregion

        //SALES REPORTS

        //#region Sales by Employee Command

        //RelayCommand salesByEmployeeCommand;
        //public ICommand SalesByEmployeeCommand
        //{
        //    get
        //    {
        //        if (salesByEmployeeCommand == null)
        //        {
        //            salesByEmployeeCommand = new RelayCommand(x => this.ShowSalesByEmployeeReport());
        //        }
        //        return salesByEmployeeCommand;
        //    }
        //}

        //private void ShowSalesByEmployeeReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Ventas por Dependientes", ReportType.SalesPerson);

        //    windowManager.Show(ovm);
        //}

        //#endregion

        //#region Sales by Shift Command

        //RelayCommand salesByShiftCommand;
        //public ICommand SalesByShiftCommand
        //{
        //    get
        //    {
        //        if (salesByShiftCommand == null)
        //        {
        //            salesByShiftCommand = new RelayCommand(x => this.ShowSalesByShiftReport());
        //        }
        //        return salesByShiftCommand;
        //    }
        //}

        //private void ShowSalesByShiftReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Ventas por Turnos", ReportType.Shift);

        //    windowManager.Show(ovm);
        //}

        //#endregion


        //#region Sales Report Command

        //RelayCommand salesReportCommand;
        //public ICommand SalesReportCommand
        //{
        //    get
        //    {
        //        if (salesReportCommand == null)
        //        {
        //            salesReportCommand = new RelayCommand(x => this.ShowSalesReport());
        //        }
        //        return salesReportCommand;
        //    }
        //}

        //private void ShowSalesReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();
        //    if (windowManager.Exists<SalesChartViewModel>())
        //        windowManager.Activate<SalesChartViewModel>();
        //    else
        //    {
        //        SalesChartViewModel ovm = new SalesChartViewModel(appvm);
        //        windowManager.Show(ovm);
        //    }
        //}

        //#endregion

        //#region Global Sales

        //RelayCommand incVsExpCmd;
        //public ICommand IncomeVsExpenseCommand
        //{
        //    get
        //    {
        //        if (incVsExpCmd == null)
        //        {
        //            incVsExpCmd = new RelayCommand(x => this.ShowIncomeVsExpenseReport());
        //        }
        //        return incVsExpCmd;
        //    }
        //}
        //private void ShowIncomeVsExpenseReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Ventas vs Costos", ReportType.IncomeVsExpense);

        //    windowManager.Show(ovm);
        //}

        //#endregion

        //#region Sales By Item Command

        //RelayCommand salesByItemCmd;
        //public ICommand SalesByItemCommand
        //{
        //    get
        //    {
        //        if (salesByItemCmd == null)
        //        {
        //            salesByItemCmd = new RelayCommand(x => this.ShowSalesByItemReport());
        //        }
        //        return salesByItemCmd;
        //    }
        //}
        //private void ShowSalesByItemReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Ventas por Productos", ReportType.SalesByItem);
        //    //ovm.Title = "Ventas por Productos";

        //    //ovm.GroupingByProduct = true;

        //    windowManager.Show(ovm);
        //}

        //#endregion

        //#region Sales by Category Command

        //RelayCommand salesByCategoryCommand;
        //public ICommand SalesByCategoryCommand
        //{
        //    get
        //    {
        //        if (salesByCategoryCommand == null)
        //        {
        //            salesByCategoryCommand = new RelayCommand(x => this.ShowSalesByCategoryReport());
        //        }
        //        return salesByCategoryCommand;
        //    }
        //}
        //private void ShowSalesByCategoryReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Ventas por Categoría", ReportType.SalesByCategory);
        //    //ovm.Title = "Ventas por Categoría";

        //    //ovm.GroupingByCategory = true;

        //    windowManager.Show(ovm);
        //}

        //#endregion

        //#region Sold Products by Employee Command

        //RelayCommand soldProductsByEmployeeCommand;
        //public ICommand SoldProductsByEmployeeCommand
        //{
        //    get
        //    {
        //        if (soldProductsByEmployeeCommand == null)
        //        {
        //            soldProductsByEmployeeCommand = new RelayCommand(x => this.ShowSoldProductsByEmployeeReport());
        //        }
        //        return soldProductsByEmployeeCommand;
        //    }
        //}
        //private void ShowSoldProductsByEmployeeReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    if (windowManager.Exists<SoldProductsByEmployeeViewModel>())
        //        windowManager.Activate<SoldProductsByEmployeeViewModel>();
        //    else
        //    {
        //        SoldProductsByEmployeeViewModel ovm = new SoldProductsByEmployeeViewModel(appvm);

        //        windowManager.Show(ovm);
        //    }
        //}

        //#endregion

        //#region Sales By Item By Shift Command

        //RelayCommand salesByItemByShiftCommand;
        //public ICommand SalesByItemByShiftCommand
        //{
        //    get
        //    {
        //        if (salesByItemByShiftCommand == null)
        //        {
        //            salesByItemByShiftCommand = new RelayCommand(x => this.ShowSalesByItemByShiftReport());
        //        }
        //        return salesByItemByShiftCommand;
        //    }
        //}
        //private void ShowSalesByItemByShiftReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    if (windowManager.Exists<SalesByItemByShiftViewModel>())
        //        windowManager.Activate<SalesByItemByShiftViewModel>();
        //    else
        //    {
        //        SalesByItemByShiftViewModel ovm = new SalesByItemByShiftViewModel(appvm);

        //        windowManager.Show(ovm);
        //    }
        //}

        //#endregion

        //#region FollowProductCommand

        //RelayCommand followProductCommand;
        //public ICommand FollowProductCommand
        //{
        //    get
        //    {
        //        if (followProductCommand == null)
        //        {
        //            followProductCommand = new RelayCommand(x => this.FollowProduct());
        //        }
        //        return followProductCommand;
        //    }
        //}
        //void FollowProduct()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    if (windowManager.Exists<FollowProductViewModel>())
        //        windowManager.Activate<FollowProductViewModel>();
        //    else
        //    {
        //        FollowProductViewModel ovm = new FollowProductViewModel(appvm);
        //        windowManager.Show(ovm);
        //    }
        //}

        //#endregion

        //#region Daily Sales Average Command

        //RelayCommand dailySaleAvgReportCommand;
        //public ICommand WeekAverageReportCommand
        //{
        //    get
        //    {
        //        if (dailySaleAvgReportCommand == null)
        //        {
        //            dailySaleAvgReportCommand = new RelayCommand(x => this.ShowDayAveragesReportWindow());
        //        }
        //        return dailySaleAvgReportCommand;
        //    }
        //}
        //void ShowDayAveragesReportWindow()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Ventas por Día de la Semana", ReportType.DayOfWeekSalesAverage);

        //    windowManager.Show(ovm);
        //}

        //#endregion 

        //#region Day of Week Sales Average By Item

        //RelayCommand dowSalesAvgByItemCmd;
        //public RelayCommand DayOfWeekSalesAverageByItemCommand
        //{
        //    get
        //    {
        //        if (dowSalesAvgByItemCmd == null)
        //        {
        //            dowSalesAvgByItemCmd = new RelayCommand(x => this.ShowDailyAvgByItemReport());
        //        }
        //        return dowSalesAvgByItemCmd;
        //    }
        //}

        //void ShowDailyAvgByItemReport() 
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel vm = new ReportsWindowViewModel(appvm, "Promedios Diarios(Ventas)", ReportType.DayOfWeekSalesAverageByItem);

        //    windowManager.Show(vm);
        //}

        //#endregion

        //#region ProductClassificationCommand

        //RelayCommand prodClassCmd;
        //public RelayCommand ProductClassificationCommand
        //{
        //    get
        //    {
        //        if (prodClassCmd == null)
        //        {
        //            prodClassCmd = new RelayCommand(x => this.ShowProdClassReport());
        //        }
        //        return prodClassCmd;
        //    }
        //}

        //void ShowProdClassReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel vm = new ReportsWindowViewModel(appvm, "Classificación de Productos", ReportType.ProductClasification);

        //    windowManager.Show(vm);
        //}

        //#endregion

        //#region Daily WIP Average By Item

        //RelayCommand dowWIPAvgByItemCmd;
        //public RelayCommand DailyWIPAverageByItemCommand
        //{
        //    get
        //    {
        //        if (dowWIPAvgByItemCmd == null)
        //        {
        //            dowWIPAvgByItemCmd = new RelayCommand(x => this.ShowDailyWIPAvgByItem());
        //        }
        //        return dowWIPAvgByItemCmd;
        //    }
        //}

        //void ShowDailyWIPAvgByItem()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel vm = new ReportsWindowViewModel(appvm, "Promedios Diarios (Elaboraciones)", ReportType.DayOfWeekWIPAverageByItem);

        //    windowManager.Show(vm);
        //}

        //#endregion

        //#region WIP By Item Command

        //RelayCommand wipByItemCmd;
        //public ICommand WIPByItemCommand
        //{
        //    get
        //    {
        //        if (wipByItemCmd == null)
        //        {
        //            wipByItemCmd = new RelayCommand(x => this.ShowWIPByItemReport());
        //        }
        //        return wipByItemCmd;
        //    }
        //}
        //private void ShowWIPByItemReport()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Elaboraciones", ReportType.WIPByItem);

        //    windowManager.Show(ovm);
        //}

        //#endregion

        //#region Sales Projections Command

        //RelayCommand salesProjCmd;
        //public RelayCommand SalesProjectionsCommand
        //{
        //    get
        //    {
        //        if (salesProjCmd == null)
        //        {
        //            salesProjCmd = new RelayCommand(x => this.ShowSalesProjections());
        //        }
        //        return salesProjCmd;
        //    }
        //}

        //private void ShowSalesProjections()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Proyecciones de Venta", ReportType.SalesProjectionsByItem);

        //    windowManager.Show(ovm);
        //}
        //#endregion

        //#region WIP Projections Command

        //RelayCommand wipProjCmd;
        //public RelayCommand WIPProjectionsCommand
        //{
        //    get
        //    {
        //        if (wipProjCmd == null)
        //        {
        //            wipProjCmd = new RelayCommand(x => this.ShowWIPProjections());
        //        }
        //        return wipProjCmd;
        //    }
        //}

        //private void ShowWIPProjections()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Proyecciones de Elaboración", ReportType.WIPProjectionsByItem);

        //    windowManager.Show(ovm);
        //}
        //#endregion

        //#region Cost Projections Command

        //RelayCommand costProjCmd;
        //public RelayCommand CostProjectionsCommand
        //{
        //    get
        //    {
        //        if (costProjCmd == null)
        //        {
        //            costProjCmd = new RelayCommand(x => this.ShowCostProjections());
        //        }
        //        return costProjCmd;
        //    }
        //}

        //private void ShowCostProjections()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ReportsWindowViewModel ovm = new ReportsWindowViewModel(appvm, "Proyecciones de Costos", ReportType.CostProjectionsByItem);

        //    windowManager.Show(ovm);
        //}
        //#endregion
    }
}
