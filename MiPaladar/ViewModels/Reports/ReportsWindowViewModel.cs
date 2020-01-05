using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Enums;
using MiPaladar.Services;
using MiPaladar.MVVM;
using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    //public enum ReportType
    //{
    //    GlobalSales,
    //    SalesByItem,
    //    SalesByCategory, 
    //    SalesPerson, 
    //    Shift, 
    //    DayOfWeekSalesAverage,
    //    //DayOfWeekSalesAverageByItem,
    //    //DayOfWeekWIPAverageByItem,
    //    //DayOfWeekCostAverageByItem,
    //    WIPByItem,        
    //    CostByItem,
    //    Conteo,
    //    ProductClasification,
    //    SalesProjectionsByItem,
    //    WIPProjectionsByItem,
    //    CostProjectionsByItem
    //}

    public abstract class ReportsWindowViewModel : ViewModelBase
    {
        protected MainWindowViewModel appvm;
        //IExcelExporter excelExporter;
        // passwordAsker;

        public ReportsWindowViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            //this.title = title;

            //this.rType = ro;

            //switch (ro)
            //{
            //    case ReportType.SalesByItem:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.SalesByCategory:
            //        groupingByCategory = true;
            //        break;
            //    case ReportType.SalesPerson:
            //        groupingBySalesPerson = true;
            //        break;
            //    case ReportType.Shift:
            //        //groupingByShift = true;
            //        break;
            //    case ReportType.DayOfWeekSalesAverage:
            //        groupingByDayOfWeek = true;
            //        break;
            //    //case ReportType.DayOfWeekSalesAverageByItem:
            //    //    groupingByProduct = true;
            //    //    break;
            //    //case ReportType.DayOfWeekWIPAverageByItem:
            //    //    groupingByProduct = true;
            //    //    break;
            //    //case ReportType.DayOfWeekCostAverageByItem:
            //    //    groupingByProduct = true;
            //    //    break;
            //    case ReportType.WIPByItem:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.CostByItem:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.Conteo:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.ProductClasification:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.SalesProjectionsByItem:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.WIPProjectionsByItem:
            //        groupingByProduct = true;
            //        break;
            //    case ReportType.CostProjectionsByItem:
            //        groupingByProduct = true;
            //        break;
            //    default:
            //        break;
            //}                       

            //BuildChart();
            //BuildTotals();
            //BuildColumns();            

            //UpdateSearch();

            //lineitems_source = new ObservableCollection<SaleLineItemReportViewModel>();
            //lineitems_filtered = new ObservableCollection<SaleLineItemReportViewModel>();
            //lineitems_showing = new ObservableCollection<SaleLineItemReportViewModel>();

            //UpdateColumnsVisibility();

            fromDate = toDate = DateTime.Today;

            cro = new CustomizeReportOptions(base.GetNewUnitOfWork());

            //CreateDefaultReportOptions();

            //use last to week of sales to project
            //if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem)
            //{
            //    var querySvc = base.GetService<IQueryService>();
            //    toDate = querySvc.GetLastSaleDate();
            //    fromDate = toDate.AddDays(-13);//last two weeks
            //}
        }

        //
        //public void CreateDefaultReportOptions()
        //{
        //    cro = new CustomizeReportOptions();

        //    cro.FromDate = DateTime.Today;
        //    cro.ToDate = DateTime.Today;

        //    foreach (var item in appvm.ShiftsOC)
        //    {
        //        cro.SelectedShifts.Add(item);
        //    }
        //    cro.ShiftsAllGood = true;

        //    foreach (var item in appvm.EmployeesOC)
        //    {
        //        cro.SelectedSalesPersons.Add(item);
        //    }
        //    cro.SalesPersonsAllGood = true;

        //    foreach (var item in appvm.CategoriesOC)
        //    {
        //        cro.SelectedCategories.Add(item);
        //    }
        //    cro.CategoriesAllGood = true;

        //    foreach (var item in appvm.TagsOC)
        //    {
        //        cro.SelectedTags.Add(item);
        //    }
        //    cro.TagsAllGood = true;
        //}

        protected abstract IReportingService GetReportService();

        protected abstract IExcelExporter GetExcelService();

        private List<ReportLineViewModel> UpdateReport()
        {
            cro.FromDate = fromDate; cro.ToDate = toDate;
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                return GetReportService().GenerateReportData(unitOfWork, cro);
            }            
        }

        public virtual void ExportToExcel(BackgroundWorker backWorker)
        {
            GetExcelService().ExportToExcel(ItemsShowing, backWorker);
        }

        public bool ShowCategoriesInCustomizeReportOptions
        {
            get { return !GetReportService().IgnoreCategories; }
        }

        public bool ShowTagsInCustomizeReportOptions
        {
            get { return !GetReportService().IgnoreTags; }
        }

        //#region Columns, Totals, Chart

        //void BuildColumns()
        //{
        //    headers = new ObservableCollection<ReportColumnViewModel>();

        //    //if (rType == ReportType.GlobalSales)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Intervalo";
        //    //    hvm.BindingString = "DateSpanString";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ventas";
        //    //    hvm.BindingString = "Amount";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Clientes";
        //    //    hvm.BindingString = "Clients";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "$/Cliente";
        //    //    hvm.BindingString = "SalesByClient";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    //hvm = new ReportColumnViewModel();
        //    //    //hvm.Header = "Costos";
        //    //    //hvm.BindingString = "Cost";
        //    //    //hvm.FormatString = "c";

        //    //    //headers.Add(hvm);
        //    //}
        //    //if (rType == ReportType.SalesByItem)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Producto";
        //    //    hvm.BindingString = "Product.Name";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Cantidad";
        //    //    hvm.MultiBinding = new string[] { "Quantity", "UnitMeasure.Caption" };
        //    //    hvm.FormatString = "{0:0.##} {1}";
        //    //    hvm.SortPathString = "Quantity";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ventas";
        //    //    hvm.BindingString = "Amount";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Costo";
        //    //    hvm.BindingString = "Cost";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Costo %";
        //    //    hvm.BindingString = "CostToPriceRatio";
        //    //    hvm.FormatString = "p";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ganancia";
        //    //    hvm.BindingString = "Profit";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    //hvm = new ReportColumnViewModel();
        //    //    //hvm.Header = "Ganancia %";
        //    //    //hvm.BindingString = "ProfitMargin";
        //    //    //hvm.FormatString = "p";

        //    //    //headers.Add(hvm);
        //    //}
        //    //if (rType == ReportType.SalesByCategory)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Categoría";
        //    //    hvm.BindingString = "CategoryName";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ventas";
        //    //    hvm.BindingString = "Amount";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Costo";
        //    //    hvm.BindingString = "Cost";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Costo %";
        //    //    hvm.BindingString = "CostToPriceRatio";
        //    //    hvm.FormatString = "p";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ganancia";
        //    //    hvm.BindingString = "Profit";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);
        //    //}
        //    //if (rType == ReportType.SalesPerson)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Dependiente";
        //    //    hvm.BindingString = "SalesPersonName";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ventas";
        //    //    hvm.BindingString = "Amount";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Clientes";
        //    //    hvm.BindingString = "Clients";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "$/Cliente";
        //    //    hvm.BindingString = "SalesByClient";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);
        //    //}
        //    //if (rType == ReportType.DayOfWeekSalesAverage)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Día";
        //    //    hvm.BindingString = "DiaEnEspanol";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Ventas";
        //    //    hvm.BindingString = "Amount";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Clientes";
        //    //    hvm.BindingString = "Clients";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "$/Cliente";
        //    //    hvm.BindingString = "SalesByClient";
        //    //    hvm.FormatString = "c";

        //    //    headers.Add(hvm);
        //    //}
        //    //else if (rType == ReportType.DayOfWeekSalesAverageByItem || rType == ReportType.DayOfWeekWIPAverageByItem || rType == ReportType.DayOfWeekCostAverageByItem)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Producto";
        //    //    hvm.BindingString = "Product.Name";

        //    //    headers.Add(hvm);

        //    //    //days of week
        //    //    for (int i = 1; i <= 7; i++)
        //    //    {
        //    //        hvm = new ReportColumnViewModel();
        //    //        hvm.FormatString = "{0:0.##} {1}";
        //    //        if (i < 7)
        //    //        {
        //    //            hvm.Header = ((Dias)i).ToString();
        //    //            hvm.MultiBinding = new string[] { "Quantities[" + i + "]", "UnitOfMeasures[" + i + "].Caption" };
        //    //            hvm.SortPathString = "Quantities[" + i + "]";
        //    //        }
        //    //        else
        //    //        {
        //    //            hvm.Header = Dias.Domingo.ToString();
        //    //            hvm.MultiBinding = new string[] { "Quantities[0]", "UnitOfMeasures[0].Caption" };
        //    //            hvm.SortPathString = "Quantities[0]";
        //    //        }                    

        //    //        headers.Add(hvm);
        //    //    }

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Lun-Jue";
        //    //    hvm.MultiBinding = new string[] { "WeekdayQtty", "WeekdayUM.Caption" };
        //    //    hvm.FormatString = "{0:0.##} {1}";
        //    //    hvm.SortPathString = "WeekdayQtty";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Vie-Dom";
        //    //    hvm.MultiBinding = new string[] { "WeekendQtty", "WeekendUM.Caption" };
        //    //    hvm.FormatString = "{0:0.##} {1}";
        //    //    hvm.SortPathString = "WeekendQtty";

        //    //    headers.Add(hvm);
        //    //}
        //    //if (rType == ReportType.ProductClasification)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Producto";
        //    //    hvm.BindingString = "Product.Name";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "% Preferencia";
        //    //    hvm.BindingString = "Preference";
        //    //    hvm.FormatString = "p";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "% Contribución";
        //    //    hvm.BindingString = "ProfitMargin";
        //    //    hvm.FormatString = "p";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Clase";
        //    //    hvm.BindingString = "ProductClass";

        //    //    headers.Add(hvm);
        //    //}
        //    //if (rType == ReportType.WIPByItem)
        //    //{
        //    //    ReportColumnViewModel hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Producto";
        //    //    hvm.BindingString = "Product.Name";

        //    //    headers.Add(hvm);

        //    //    hvm = new ReportColumnViewModel();
        //    //    hvm.Header = "Cantidad";
        //    //    hvm.MultiBinding = new string[] { "Quantity", "UnitMeasure.Caption" };
        //    //    hvm.FormatString = "{0:0.##} {1}";
        //    //    hvm.SortPathString = "Quantity";

        //    //    headers.Add(hvm);
        //    //}
        //    if (rType == ReportType.CostByItem)
        //    {
        //        ReportColumnViewModel hvm = new ReportColumnViewModel();
        //        hvm.Header = "Producto";
        //        hvm.BindingString = "Product.Name";

        //        headers.Add(hvm);

        //        hvm = new ReportColumnViewModel();
        //        hvm.Header = "Cantidad Estándar";
        //        hvm.MultiBinding = new string[] { "BasicQuantity", "UnitMeasure.Caption" };
        //        hvm.FormatString = "{0:0.##} {1}";
        //        hvm.SortPathString = "BasicQuantity";

        //        headers.Add(hvm);

        //        hvm = new ReportColumnViewModel();
        //        hvm.Header = "Cantidad Real";
        //        hvm.MultiBinding = new string[] { "Quantity", "UnitMeasure.Caption" };
        //        hvm.FormatString = "{0:0.##} {1}";
        //        hvm.SortPathString = "Quantity";

        //        headers.Add(hvm);

        //        hvm = new ReportColumnViewModel();
        //        hvm.Header = "Costo Estándar";
        //        hvm.BindingString = "BasicCost";
        //        hvm.FormatString = "c";

        //        headers.Add(hvm);

        //        hvm = new ReportColumnViewModel();
        //        hvm.Header = "Costo Real";
        //        hvm.BindingString = "Cost";
        //        hvm.FormatString = "c";

        //        headers.Add(hvm);
        //    }
        //    else if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem)
        //    {
        //        ReportColumnViewModel hvm = new ReportColumnViewModel();
        //        hvm.Header = "Producto";
        //        hvm.BindingString = "Product.Name";

        //        headers.Add(hvm);

        //        DateTime today = DateTime.Today;

        //        //project sales for seven days
        //        for (int i = 0; i < 7; i++)
        //        {
        //            hvm = new ReportColumnViewModel();
        //            hvm.Header = today.AddDays(i).ToString("d/MMM");
        //            hvm.MultiBinding = new string[] { "Quantities[" + i + "]", "UnitOfMeasures[" + i + "].Caption" };
        //            hvm.FormatString = "{0:0.##} {1}";
        //            hvm.SortPathString = "Quantities[" + i + "]";

        //            headers.Add(hvm);
        //        }
        //    }
        //    else if (rType == ReportType.Conteo)
        //    {
        //        ReportColumnViewModel hvm = new ReportColumnViewModel();
        //        hvm.Header = "Producto";
        //        hvm.BindingString = "Product.Name";

        //        headers.Add(hvm);

        //        hvm = new ReportColumnViewModel();
        //        hvm.Header = "Cantidad";
        //        hvm.MultiBinding = new string[] { "Quantity", "UnitMeasure.Caption" };
        //        hvm.FormatString = "{0:0.##} {1}";
        //        hvm.SortPathString = "Quantity";

        //        headers.Add(hvm);
        //    }

        //}

        //void BuildTotals()
        //{
        //    tItems = new ObservableCollection<TotalItem>();

        //    if (rType == ReportType.SalesByItem || rType == ReportType.SalesByCategory)
        //    {
        //        TotalItem tItem = new TotalItem();
        //        tItem.Header = "Ventas";
        //        tItem.BindingString = "TotalSales";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "Costo";
        //        tItem.BindingString = "TotalCost";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "Costo %";
        //        tItem.BindingString = "TotalCostPercent";
        //        tItem.FormatString = "p";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "Ganancia";
        //        tItem.BindingString = "TotalProfit";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);
        //    }
        //    else if (rType == ReportType.SalesPerson || rType == ReportType.DayOfWeekSalesAverage)
        //    {
        //        TotalItem tItem = new TotalItem();
        //        tItem.Header = "Ventas";
        //        tItem.BindingString = "TotalSales";
        //        tItem.FormatString = "c0";

        //        tItems.Add(tItem);

        //        //tItem = new TotalItem();
        //        //tItem.Header = "Costos";
        //        //tItem.BindingString = "TotalCost";
        //        //tItem.FormatString = "c0";

        //        //tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "Clientes";
        //        tItem.BindingString = "TotalClients";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "$/Cliente";
        //        tItem.BindingString = "SpendingByClient";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);
        //    }
        //    else if (rType == ReportType.GlobalSales)
        //    {
        //        TotalItem tItem = new TotalItem();
        //        tItem.Header = "Ventas";
        //        tItem.BindingString = "TotalSales";
        //        tItem.FormatString = "c0";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "Clientes";
        //        tItem.BindingString = "TotalClients";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "$/Cliente";
        //        tItem.BindingString = "SpendingByClient";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);
        //    }
        //    //else if (rType == ReportType.DayOfWeekSalesAverageByItem || rType == ReportType.DayOfWeekWIPAverageByItem || rType == ReportType.DayOfWeekCostAverageByItem)
        //    //{
                
        //    //}
        //    else if (rType == ReportType.ProductClasification)
        //    {
        //        TotalItem tItem = new TotalItem();
        //        tItem.Header = "% Preferencia";
        //        tItem.BindingString = "AveragePreference";
        //        tItem.FormatString = "p";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "% Contribución";
        //        tItem.BindingString = "AverageProfitMargin";
        //        tItem.FormatString = "p";

        //        tItems.Add(tItem);
        //    }
        //    else if (rType == ReportType.WIPByItem)
        //    {
                
        //    }
        //    else if (rType == ReportType.CostByItem)
        //    {
        //        TotalItem tItem = new TotalItem();
        //        tItem.Header = "Costo Estándar";
        //        tItem.BindingString = "TotalBasicCost";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);

        //        tItem = new TotalItem();
        //        tItem.Header = "Costo Real";
        //        tItem.BindingString = "TotalCost";
        //        tItem.FormatString = "c";

        //        tItems.Add(tItem);
        //    }
        //    else if (rType == ReportType.SalesProjectionsByItem)
        //    {
                
        //    }
        //}

        //void BuildChart()
        //{
        //    ChartDefinition chart = new ChartDefinition();
        //    this.ChartDef = chart;

        //    if (rType == ReportType.SalesByItem || rType == ReportType.SalesPerson ||
        //        rType == ReportType.DayOfWeekSalesAverage )
        //    {
        //        ChartSeries columnSeries = new ChartSeries();
        //        columnSeries.ChartType = ChartSeriesType.Column;
        //        columnSeries.Title = "Ventas";
        //        columnSeries.XBinding = "X";
        //        columnSeries.YBinding = "TotalSale";

        //        chart.ChartSeries.Add(columnSeries);
        //    }
        //    else if (rType == ReportType.GlobalSales)
        //    {
        //        ChartSeries columnSeries = new ChartSeries();
        //        columnSeries.ChartType = ChartSeriesType.Column;
        //        columnSeries.Title = "Ventas";
        //        columnSeries.XBinding = "X";
        //        columnSeries.YBinding = "TotalSale";

        //        chart.ChartSeries.Add(columnSeries);

        //        //columnSeries = new ChartSeries();
        //        //columnSeries.ChartType = ChartSeriesType.Column;
        //        //columnSeries.Title = "Costos";
        //        //columnSeries.XBinding = "X";
        //        //columnSeries.YBinding = "TotalCost";

        //        //chart.ChartSeries.Add(columnSeries);
        //    }
        //    else if (rType == ReportType.SalesByCategory)
        //    {
        //        ChartSeries pieSeries = new ChartSeries();
        //        pieSeries.ChartType = ChartSeriesType.Pie;
        //        pieSeries.Title = "Ventas";
        //        pieSeries.XBinding = "X";
        //        pieSeries.YBinding = "TotalSale";

        //        chart.ChartSeries.Add(pieSeries);
        //    }
        //    //else if (rType == ReportType.DayOfWeekSalesAverageByItem || rType == ReportType.DayOfWeekWIPAverageByItem || rType == ReportType.DayOfWeekCostAverageByItem)
        //    //{

        //    //}
        //    else if (rType == ReportType.ProductClasification)
        //    {
        //        ChartSeries columnSeries = new ChartSeries();
        //        columnSeries.ChartType = ChartSeriesType.Column;
        //        columnSeries.Title = "Cantidad";
        //        columnSeries.XBinding = "X";
        //        columnSeries.YBinding = "Count";

        //        chart.ChartSeries.Add(columnSeries);
        //    }
        //    else if (rType == ReportType.WIPByItem)
        //    {

        //    }
        //    else if (rType == ReportType.CostByItem)
        //    {
        //        ChartSeries columnSeries = new ChartSeries();
        //        columnSeries.ChartType = ChartSeriesType.Column;
        //        columnSeries.Title = "Costos";
        //        columnSeries.XBinding = "X";
        //        columnSeries.YBinding = "TotalCost";

        //        chart.ChartSeries.Add(columnSeries);
        //    }
        //    else if (rType == ReportType.SalesProjectionsByItem)
        //    {

        //    }
        //}

        //#endregion       

        //protected override void OnDispose()
        //{
        //    if (bWorker != null)
        //    {
        //        bWorker.DoWork -= bWorker_DoWork;
        //        bWorker.RunWorkerCompleted -= bWorker_RunWorkerCompleted;
        //    }
        //}

        //string title;
        //public string Title
        //{
        //    get { return title; }
        //}
        
        #region Dates

        DateTime fromDate, toDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        //string[] dateOptions = new string[] { "Hoy", "Ayer", "Específico" };

        //public string[] DateOptions
        //{
        //    get { return dateOptions; }
        //}

        protected DateOption sltdDateOption;
        public DateOption SelectedDateOption
        {
            get { return sltdDateOption; }
            set
            {
                sltdDateOption = value;
                OnPropertyChanged("SelectedDateOption");
            }
        }

        public void SetDateOption(DateOption option)
        {
            switch (option)
            {
                case DateOption.Hoy:
                    FromDate = ToDate = DateTime.Today;
                    UpdateSearchAsync();
                    break;
                case DateOption.Ayer:
                    FromDate = ToDate = DateTime.Today.AddDays(-1);
                    UpdateSearchAsync();
                    break;
                case DateOption.Específico:
                    if (ShowCustomDateRangeDialog())
                    {
                        UpdateSearchAsync();
                    }
                    //for some reason this works
                    //dateOption = null;
                    break;
                default:
                    break;
            }
        }

        bool ShowCustomDateRangeDialog()
        {
            var windowManager = base.GetService<IWindowManager>();

            CustomDatesDialogViewModel dvm = new CustomDatesDialogViewModel(fromDate, toDate);

            if (windowManager.ShowDialog(dvm, this) == true)
            {
                FromDate = dvm.FromDate;
                ToDate = dvm.ToDate;

                return true;
            }

            return false;
        }

        protected void SetCustomDates(DateTime fDate, DateTime tDate) 
        {
            FromDate = fDate;
            ToDate = tDate;

            sltdDateOption = DateOption.Específico;
            OnPropertyChanged("SelectedDateOption");
        }

        #endregion                

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
        public RelayCommand FindCommand
        {
            get
            {
                if (findCommand == null)
                {
                    findCommand = new RelayCommand(x => this.UpdateSearchAsync(), x => this.CanFind);
                }
                return findCommand;
            }
        }

        bool CanFind { get { return !busy; } }

        //CollectionView auxView;
        BackgroundWorker bWorker;

        //List<ReportLineViewModel> lineitems_source;        

        private void UpdateSearchAsync()
        {
            //worker is running
            if (bWorker != null && bWorker.IsBusy) return;

            bWorker = new BackgroundWorker();

            bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
            bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);

            Busy = true;
            //CommandManager.InvalidateRequerySuggested();

            bWorker.RunWorkerAsync();
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = UpdateReport();
        }

        /// <summary>
        /// updates report (abstract)
        /// </summary>
        /// <returns>updated report</returns>
        //protected abstract List<ReportLineViewModel> UpdateReport();

        //abstract void ExtractData()
        //{
        //    var queryService = base.GetService<IQueryService>();

        //    if (groupingByProduct || groupingByCategory)
        //    {
        //        bool searchIngredients = rType == ReportType.Conteo ||
        //            rType == ReportType.CostByItem ||
        //            //rType == ReportType.DayOfWeekSalesAverageByItem ||
        //            //rType == ReportType.DayOfWeekWIPAverageByItem ||
        //            //rType == ReportType.DayOfWeekCostAverageByItem ||
        //            rType == ReportType.WIPByItem ||
        //            rType == ReportType.SalesProjectionsByItem ||
        //            rType == ReportType.WIPProjectionsByItem ||
        //            rType == ReportType.CostProjectionsByItem;

        //        lineitems_source = queryService.GetSalesByItemData(appvm.Context, fromDate, toDate, searchIngredients);
        //    }
        //    //else if (rType == ReportType.IncomeVsExpense)
        //    //{
        //    //    e.Result = queryService.GetSalesDataWithCost(fromDate, toDate);
        //    //}
        //    else
        //    {
        //        lineitems_source = queryService.GetSalesData(appvm.Context, fromDate, toDate);
        //    }
        //}

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            linesBeforeLiveFilter = (List<ReportLineViewModel>)e.Result;

            UpdateLiveFilter();            

            UpdateTotals();

            Busy = false;
            //CommandManager.InvalidateRequerySuggested();

            //unload events
            bWorker.DoWork -= bWorker_DoWork;
            bWorker.RunWorkerCompleted -= bWorker_RunWorkerCompleted;

            UpdateGraphDataAsync();
        }

        //void FilterAndUpdate()
        //{
        //    //FilterItems();

        //    UpdateItems();

        //    UpdateGraphData();

        //    UpdateTotals();

        //    //FireReportUpdated();
        //}

        //#region Report Updated Event

        //public event EventHandler ReportUpdated;

        //private void FireReportUpdated()
        //{
        //    EventHandler handler = this.ReportUpdated;
        //    if (handler != null)
        //    {
        //        handler(this, null);
        //    }
        //}

        //#endregion

        //void OnGroupingConditionsChanged()
        //{
        //    int count = 0;

        //    if (noGrouping) count++;
        //    if (groupingByProduct) count++;
        //    if (groupingByCategory) count++;
        //    //if (groupingByDate) count++;
        //    //if (groupingByWeek) count++;
        //    //if (groupingByDayOfWeek) count++;
        //    if (groupingBySalesPerson) count++;

        //    //give time for the radio button binding
        //    if (count != 1) return;

        //    UpdateItems();

        //    UpdateColumnsVisibility();
        //}

        
        //{
        //    graphData.Clear();

            //if (rType == ReportType.DayOfWeekSalesAverage)
            //{
            //    foreach (var item in lines_showing.OrderBy(x => x.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)x.DayOfWeek))
            //    {
            //        SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //        scc.X = ((Dias)item.DayOfWeek).ToString();

            //        scc.TotalSale = Math.Round(item.Amount);

            //        graphData.Add(scc);                    
            //    }
            //}
            //else 
            //    if (rType == ReportType.SalesByCategory)
            //{
            //    List<SalesChartColumnViewModel> tempList = new List<SalesChartColumnViewModel>();

            //    foreach (var item in lines_showing.Where(x => x.ChildrenProductsTotalSale > 0))
            //    {
            //        SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //        scc.X = item.CategoryName;

            //        scc.TotalSale = Math.Round(item.ChildrenProductsTotalSale);

            //        tempList.Add(scc);
            //    }

            //    foreach (var item in tempList.OrderBy(x => -x.TotalSale))
            //    {
            //        graphData.Add(item);
            //    }
            //}
            //else 
            //        if (rType == ReportType.SalesPerson)
            //{
            //    List<SalesChartColumnViewModel> tempList = new List<SalesChartColumnViewModel>();

            //    foreach (var item in lines_showing)
            //    {
            //        SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //        scc.X = item.SalesPersonName;

            //        scc.TotalSale = item.Amount;

            //        tempList.Add(scc);
            //    }

            //    foreach (var item in tempList.OrderBy(x => -x.TotalSale))
            //    {
            //        graphData.Add(item);
            //    }
            //}
            //else 
            //            if (rType == ReportType.ProductClasification)
            //{
            //    foreach (var group in lines_showing.GroupBy(x => x.ProductClass).OrderBy(x => x.Key))
            //    {
            //        SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //        scc.X = group.Key.ToString();

            //        scc.Count = group.Count();

            //        graphData.Add(scc);
            //    }
            //}
            //else 
            //                if (rType == ReportType.Conteo)
            ////rType == ReportType.DayOfWeekSalesAverageByItem ||
            ////rType == ReportType.DayOfWeekWIPAverageByItem ||
            ////rType == ReportType.DayOfWeekCostAverageByItem                
            //{
            //    //do nothing
            //}
            //else if (rType == ReportType.GlobalSales)
            //{
            //    foreach (var item in lines_showing)
            //    {
            //        SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //        //get month from the first sale
            //        scc.Date = item.Date;
            //        scc.X = item.DateSpanString;

            //        scc.TotalSale = Math.Round(item.Amount);
            //        //scc.TotalCost = Math.Round(item.Cost);

            //        graphData.Add(scc);
            //    }
            //}
            //else
            //{
            //    int daysCount = lines_showing.Distinct(new SameDateComparer()).Count();
            //    int monthCount = lines_showing.Distinct(new SameMonthComparer()).Count();

            //    //group by month
            //    if (monthCount >= 3)
            //    {
            //        //months data
            //        var queryNestedGroups = from item in lines_showing
            //                                group item by item.Date.Year into yearGroup
            //                                from monthGroup in
            //                                    (from item in yearGroup
            //                                     orderby item.Date.Year, item.Date.Month
            //                                     group item by item.Date.Month)
            //                                group monthGroup by yearGroup.Key;

            //        foreach (var yearGroup in queryNestedGroups)
            //        {
            //            foreach (var monthGroup in yearGroup)
            //            {
            //                SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //                //get month from the first sale
            //                scc.Date = monthGroup.First().Date;
            //                scc.X = scc.Date.ToString("MMM/yy");

            //                if (rType == ReportType.CostByItem) scc.TotalCost = Math.Round(monthGroup.Sum(x => x.Cost));
            //                else scc.TotalSale = Math.Round(monthGroup.Sum(x => x.Amount));

            //                graphData.Add(scc);
            //            }
            //        }
            //    }
            //    //group by week
            //    else if (daysCount >= 21)
            //    {
            //        var query = from item in lines_showing
            //                    group item by item.MondayDate;

            //        foreach (var weekGroup in query)
            //        {
            //            SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //            scc.X = weekGroup.First().WeekString;

            //            if (rType == ReportType.CostByItem) scc.TotalCost = Math.Round(weekGroup.Sum(x => x.Cost));
            //            else scc.TotalSale = Math.Round(weekGroup.Sum(x => x.Amount));

            //            graphData.Add(scc);
            //        }
            //    }
            //    //group by date
            //    else
            //    {
            //        var query = from item in lines_showing
            //                    group item by item.Date;

            //        foreach (var dateGroup in query)
            //        {
            //            SalesChartColumnViewModel scc = new SalesChartColumnViewModel();

            //            //scc.Date = dateGroup.Key;
            //            scc.X = dateGroup.Key.ToString("d/MMM");

            //            if (rType == ReportType.CostByItem) scc.TotalCost = Math.Round(dateGroup.Sum(x => x.Cost));
            //            else scc.TotalSale = Math.Round(dateGroup.Sum(x => x.Amount));

            //            graphData.Add(scc);
            //        }
            //    }
            //}
        //}

        //private void UpdateItems()
        //{
            //lines_showing.Clear();
            //salesByItemLines.Clear();

            //if (rType == ReportType.GlobalSales)
            //{
            //    CreateGlobalSalesLines();
            //}
            //else
            //if (groupingByProduct)
            //{
                //group by day of week
                //if (rType == ReportType.DayOfWeekSalesAverageByItem ||
                //    rType == ReportType.DayOfWeekWIPAverageByItem ||
                //    rType == ReportType.DayOfWeekCostAverageByItem)
                //{
                //    CalculateDailyAvgByItem();
                //}
                //else 
                //    if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem)
                //{
                //    CalculateSalesProjectionsByItem();
                //}
                //else
                //{
                //    var groupsByProduct = from item in lines_filtered
                //                          group item by item.Product;

                //    List<ReportLineViewModel> tmpList = new List<ReportLineViewModel>();

                //    foreach (var group in groupsByProduct)
                //    {
                //        ReportLineViewModel lic = new ReportLineViewModel();

                //        //SalesByItemLineViewModel sbiLine = new SalesByItemLineViewModel();

                //        //sbiLine.ProductId = group.Key.Id;
                //        //sbiLine.Product = group.Key;
                //        //sbiLine.Amount = group.Sum(x => x.Amount);
                //        //sbiLine.Quantity = group.Sum(x => x.Quantity);

                //        lic.Product = group.Key;
                //        lic.ProductId = group.Key.Id;
                //        //lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

                //        MakeSum(group, lic);

                //        tmpList.Add(lic);
                //        //salesByItemLines.Add(sbiLine);
                //    }

                //    //PRODUCT CLASSIFICATION
                //    if (rType == ReportType.ProductClasification && tmpList.Count > 0)
                //    {
                //        double totalQtty = tmpList.Sum(x => x.Quantity);

                //        foreach (var item in tmpList)
                //        {
                //            item.Preference = item.Quantity / totalQtty;
                //        }

                //        AveragePreference = tmpList.Average(x => x.Preference);
                //        AverageProfitMargin = tmpList.Average(x => x.ProfitMargin);

                //        foreach (var item in tmpList)
                //        {
                //            if (item.Preference >= AveragePreference && item.ProfitMargin >= AverageProfitMargin)
                //            {
                //                item.ProductClass = ProductClass.A;
                //            }
                //            else if (item.Preference >= AveragePreference && item.ProfitMargin < AverageProfitMargin)
                //            {
                //                item.ProductClass = ProductClass.B;
                //            }
                //            else if (item.Preference < AveragePreference && item.ProfitMargin >= AverageProfitMargin)
                //            {
                //                item.ProductClass = ProductClass.C;
                //            }
                //            else //if (item.Preference < averagePreference && item.ProfitMargin < averageProfitMargin)
                //            {
                //                item.ProductClass = ProductClass.D;
                //            }
                //        }
                //    }

                //    foreach (var item in tmpList.OrderBy(x => x.Product.Name))
                //    {
                //        lines_showing.Add(item);
                //    }
                //}

            //}
            //else if (rType == ReportType.SalesByCategory)
            //{
            //    var groupsByCategory = from item in lines_filtered
            //                           orderby item.CategoryName
            //                           where item.CategoryId != 0
            //                           group item by item.CategoryId;

            //    List<ReportLineViewModel> tempList = new List<ReportLineViewModel>();

            //    foreach (var group in groupsByCategory)
            //    {
            //        ReportLineViewModel lic = new ReportLineViewModel();

            //        MakeSum(group, lic);

            //        AddToCategoryBranch(lic, group.First().CategoryId, tempList, false);

            //        //lineitems_showing.Add(lic);
            //    }

            //    foreach (var item in tempList.OrderBy(x => x.CategoryName))
            //    {
            //        lines_showing.Add(item);
            //    }

            //    if (lines_filtered.Where(x => x.CategoryId == 0).Count() > 0)
            //    {
            //        //UNCATEGORIZED
            //        ReportLineViewModel noCategoryLine = new ReportLineViewModel();
            //        noCategoryLine.CategoryName = "Sin Categoría";
            //        noCategoryLine.IsRootCategory = true;

            //        MakeSum(lines_filtered.Where(x => x.CategoryId == 0), noCategoryLine);

            //        noCategoryLine.ChildrenProductsTotalSale = noCategoryLine.Amount;

            //        lines_showing.Add(noCategoryLine);
            //    }
            //}
            //else if (groupingByDate)
            //{
            //    var queryNestedGroups = from item in lineitems_filtered
            //                            group item by item.Product into productGroup
            //                            from dateGroup in
            //                                (from item in productGroup
            //                                 group item by item.Date)
            //                            group dateGroup by productGroup.Key;

            //    foreach (var productGroup in queryNestedGroups)
            //    {
            //        foreach (var dateGroup in productGroup)
            //        {
            //            SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

            //            lic.Product = productGroup.Key;
            //            lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            //            lic.Date = dateGroup.Key;

            //            MakeSum(dateGroup, lic);

            //            lineitems_showing.Add(lic);
            //        }                    
            //    }
            //}
            //else if (groupingByWeek)
            //{
            //    var queryNestedGroups = from item in lineitems_filtered
            //                            group item by item.Product into productGroup
            //                            from weekGroup in
            //                                (from item in productGroup
            //                                 group item by item.MondayDate)
            //                            group weekGroup by productGroup.Key;

            //    foreach (var productGroup in queryNestedGroups)
            //    {
            //        foreach (var weekGroup in productGroup)
            //        {
            //            SaleLineItemReportViewModel lic = new SaleLineItemReportViewModel();

            //            lic.Product = productGroup.Key;
            //            lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
            //            lic.MondayDate = weekGroup.Key;

            //            MakeSum(weekGroup, lic);

            //            lineitems_showing.Add(lic);
            //        }                    
            //    }
            //}
            //else if (rType == ReportType.DayOfWeekSalesAverage)
            //{
            //    var query = from item in lines_filtered
            //                orderby item.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)item.DayOfWeek
            //                group item by item.DayOfWeek;

            //    foreach (var dowGroup in query)
            //    {
            //        ReportLineViewModel lic = new ReportLineViewModel();

            //        lic.DayOfWeek = dowGroup.Key;

            //        //MakeSum(dowGroup, lic);

            //        int days = dowGroup.Distinct(new SameDateComparer()).Count();

            //        lic.Amount = dowGroup.Sum(x => x.Amount) / days;
            //        lic.Clients = dowGroup.Sum(x => x.Clients) / days;

            //        lines_showing.Add(lic);
            //    }
            //}
            //else if (rType == ReportType.SalesPerson)
            //{
            //    var query = from sale in lines_filtered
            //                where sale.SalesPersonId != 0
            //                orderby sale.SalesPersonName
            //                group sale by sale.SalesPersonId;

            //    foreach (var salesPersonGroup in query)
            //    {
            //        ReportLineViewModel lic = new ReportLineViewModel();

            //        lic.SalesPersonId = salesPersonGroup.Key;
            //        lic.SalesPersonName = appvm.EmployeesOC.Single(x => x.Id == salesPersonGroup.Key).Name;

            //        MakeSum(salesPersonGroup, lic);

            //        lines_showing.Add(lic);
            //    }

            //    if (lines_filtered.Where(x => x.SalesPersonId == 0).Count() > 0)
            //    {
            //        //NO  SALESPERSON
            //        ReportLineViewModel noSPLine = new ReportLineViewModel();
            //        noSPLine.SalesPersonName = "Sin Dependiente";

            //        MakeSum(lines_filtered.Where(x => x.SalesPersonId == 0), noSPLine);

            //        lines_showing.Add(noSPLine);
            //    }
            //}
            //else if (rType == ReportType.Shift)
            //{
            //    var query = from sale in lines_filtered
            //                where sale.ShiftId != 0
            //                orderby sale.ShiftName
            //                group sale by sale.ShiftId;

            //    foreach (var shiftGroup in query)
            //    {
            //        ReportLineViewModel lic = new ReportLineViewModel();

            //        lic.ShiftId = shiftGroup.Key;
            //        lic.ShiftName = appvm.ShiftsOC.Single(x => x.Id == shiftGroup.Key).Name;

            //        MakeSum(shiftGroup, lic);

            //        lines_showing.Add(lic);
            //    }

            //    if (lines_filtered.Where(x => x.ShiftId == 0).Count() > 0)
            //    {
            //        //NO  SHIFT
            //        ReportLineViewModel noShiftLine = new ReportLineViewModel();
            //        noShiftLine.ShiftName = "Sin Turno";

            //        MakeSum(lines_filtered.Where(x => x.ShiftId == 0), noShiftLine);

            //        lines_showing.Add(noShiftLine);
            //    }
            //}
        //}

        //private void CreateGlobalSalesLines()
        //{
        //    int daysCount = lines_filtered.Distinct(new SameDateComparer()).Count();
        //    int monthCount = lines_filtered.Distinct(new SameMonthComparer()).Count();

        //    //group by month
        //    if (monthCount >= 3)
        //    {
        //        //months data
        //        var queryNestedGroups = from item in lines_filtered
        //                                group item by item.Date.Year into yearGroup
        //                                from monthGroup in
        //                                    (from item in yearGroup
        //                                     orderby item.Date.Year, item.Date.Month
        //                                     group item by item.Date.Month)
        //                                group monthGroup by yearGroup.Key;

        //        foreach (var yearGroup in queryNestedGroups)
        //        {
        //            foreach (var monthGroup in yearGroup)
        //            {
        //                ReportLineViewModel rLine = new ReportLineViewModel();
        //                rLine.DateSpanString = monthGroup.First().Date.ToString("MMM/yyy");

        //                MakeSum(monthGroup, rLine);

        //                lines_showing.Add(rLine);
        //            }
        //        }
        //    }
        //    //group by week
        //    else if (daysCount >= 21)
        //    {
        //        var query = from item in lines_filtered
        //                    group item by item.MondayDate;

        //        foreach (var weekGroup in query)
        //        {
        //            ReportLineViewModel rLine = new ReportLineViewModel();
        //            rLine.DateSpanString = weekGroup.First().WeekString;

        //            MakeSum(weekGroup, rLine);

        //            lines_showing.Add(rLine);
        //        }
        //    }
        //    //group by date
        //    else
        //    {
        //        var query = from item in lines_filtered
        //                    group item by item.Date;

        //        foreach (var dateGroup in query)
        //        {
        //            ReportLineViewModel rLine = new ReportLineViewModel();
        //            rLine.DateSpanString = dateGroup.Key.ToString("d/MMM");

        //            MakeSum(dateGroup, rLine);

        //            lines_showing.Add(rLine);
        //        }
        //    }
        //}

        //private void CalculateDailyAvgByItem()
        //{
        //    var queryNestedGroups = from opi in lines_filtered
        //                            group opi by opi.Product into newGroup1
        //                            from newGroup2 in
        //                                (from opi in newGroup1
        //                                 group opi by opi.DayOfWeek)
        //                            group newGroup2 by newGroup1.Key;

        //    foreach (var outerGroup in queryNestedGroups.OrderBy(x => x.Key.Name))
        //    {
        //        ReportLineViewModel rivm = new ReportLineViewModel();

        //        rivm.Product = outerGroup.Key;

        //        UMFamily umFamily = appvm.ProductsOC.Single(x => x.Id == rivm.Product.Id).UMFamily;
        //        bool isQtty = umFamily == appvm.UnitMeasureManager.Quantity;

        //        double weekdayQtty = 0;
        //        double weekendQtty = 0;

        //        foreach (var innerGroup in outerGroup)
        //        {
        //            DayOfWeek dow = innerGroup.Key;

        //            int numberofDays = innerGroup.Distinct(new SameDateComparer()).Count();

        //            //total qtty in base UM
        //            double tempQuantity = innerGroup.Sum(x => x.Quantity * x.UnitMeasure.ToBaseConversion);
        //            //daily average
        //            tempQuantity /= numberofDays;

        //            if (dow >= DayOfWeek.Monday && dow <= DayOfWeek.Thursday)
        //            {
        //                weekdayQtty += tempQuantity;
        //            }
        //            else weekendQtty += tempQuantity;

        //            //find biggest UM
        //            if (!isQtty)
        //            {
        //                var invSVC = base.GetService<IInventoryService>();
        //                UnitMeasure bestUM = invSVC.GetBestFittingUM(umFamily, tempQuantity);

        //                tempQuantity /= bestUM.ToBaseConversion;
        //                rivm.UnitOfMeasures[(int)dow] = bestUM;
        //            }
        //            else rivm.UnitOfMeasures[(int)dow] = appvm.UnitMeasureManager.Unit;

        //            rivm.Quantities[(int)dow] = Math.Round(tempQuantity, isQtty ? 0 : 1);
        //        }

        //        //find better ums for weekday and weekend qtties
        //        if (!isQtty)
        //        {
        //            //Weekday
        //            var invSVC = base.GetService<IInventoryService>();
        //            UnitMeasure bestUM = invSVC.GetBestFittingUM(umFamily, weekdayQtty);

        //            weekdayQtty /= bestUM.ToBaseConversion;
        //            rivm.WeekdayUM = bestUM;

        //            //weekend
        //            bestUM = invSVC.GetBestFittingUM(umFamily, weekendQtty);

        //            weekendQtty /= bestUM.ToBaseConversion;
        //            rivm.WeekendUM = bestUM;
        //        }
        //        else
        //        {
        //            rivm.WeekdayUM = rivm.WeekendUM = appvm.UnitMeasureManager.Unit;
        //        }

        //        rivm.WeekdayQtty = Math.Round(weekdayQtty, isQtty ? 0 : 1);
        //        rivm.WeekendQtty = Math.Round(weekendQtty, isQtty ? 0 : 1);

        //        lines_showing.Add(rivm);

        //    }//outer foreach
        //}

        //private void CalculateSalesProjectionsByItem()
        //{
        //    var queryNestedGroups = from opi in lines_filtered
        //                            group opi by opi.Product into newGroup1
        //                            from newGroup2 in
        //                                (from opi in newGroup1
        //                                 group opi by opi.DayOfWeek)
        //                            group newGroup2 by newGroup1.Key;

        //    foreach (var outerGroup in queryNestedGroups.OrderBy(x => x.Key.Name))
        //    {
        //        ReportLineViewModel rivm = new ReportLineViewModel();

        //        rivm.Product = outerGroup.Key;

        //        //UMFamily umFamily = appvm.ProductsOC.Single(x => x.Id == rivm.Product.Id).UMFamily;
        //        //bool isQtty = umFamily == appvm.UnitMeasureManager.Quantity;

        //        foreach (var innerGroup in outerGroup)
        //        {
        //            DayOfWeek dow = innerGroup.Key;

        //            int numberofDays = innerGroup.Distinct(new SameDateComparer()).Count();

        //            //total qtty in base UM
        //            double tempQuantity = innerGroup.Sum(x => x.Quantity * x.UnitMeasure.ToBaseConversion);
        //            //daily average
        //            tempQuantity /= numberofDays;

        //            //find biggest UM
        //            //if (!isQtty)
        //            //{
        //            //    var invSVC = base.GetService<IInventoryService>();
        //            //    UnitMeasure bestUM = invSVC.GetBestFittingUM(umFamily, tempQuantity);

        //            //    tempQuantity /= bestUM.ToBaseConversion;
        //            //    rivm.UnitOfMeasures[(int)dow] = bestUM;
        //            //}
        //            //else rivm.UnitOfMeasures[(int)dow] = appvm.UnitMeasureManager.Unit;                    

        //            int todayIndex = (int)DateTime.Today.DayOfWeek;

        //            int dowIndex = (int)dow;

        //            int finalIndex = dowIndex - todayIndex;

        //            if (todayIndex > dowIndex)
        //            {
        //                finalIndex = 7 - todayIndex + dowIndex;
        //            }

        //            rivm.AveQuantities[finalIndex] = tempQuantity;//Math.Round(tempQuantity, isQtty ? 0 : 1);
        //        }

        //        lines_showing.Add(rivm);

        //    }//outer foreach

        //    MultiplyByProjectionPercent();
        //}

        //private void MultiplyByProjectionPercent()
        //{
        //    foreach (var item in lines_showing)
        //    {
        //        UMFamily umFamily = appvm.ProductsOC.Single(x => x.Id == item.Product.Id).UMFamily;
        //        bool isQtty = umFamily == appvm.UnitMeasureManager.Quantity;

        //        for (int i = 0; i < 7; i++)
        //        {
        //            double projectedQtty = item.AveQuantities[i] * (1 + projPct / 100);

        //            if (projectedQtty == 0) continue;

        //            //find biggest UM
        //            if (!isQtty)
        //            {
        //                var invSVC = base.GetService<IInventoryService>();
        //                UnitMeasure bestUM = invSVC.FitQuantity(umFamily, projectedQtty);

        //                projectedQtty /= bestUM.ToBaseConversion;
        //                item.AverageUnitOfMeasures[i] = bestUM;
        //            }
        //            else item.AverageUnitOfMeasures[i] = appvm.UnitMeasureManager.Unit;

        //            item.ProjQuantities[i] = Math.Round(projectedQtty, isQtty ? 0 : 1);
        //        }

        //        //item.ReportQuantitiesChanged();
        //    }
        //}

        //private void AddToCategoryBranch(ReportLineViewModel lic, int targetCategoryId, List<ReportLineViewModel> tempList, bool recursive)
        //{
        //    ReportLineViewModel targetLine = tempList.FirstOrDefault(x => x.CategoryId == targetCategoryId);

        //    Category targetCategory = appvm.CategoriesOC.Single(x => x.Id == targetCategoryId);

        //    if (targetLine == null)
        //    {
        //        targetLine = new ReportLineViewModel();
        //        targetLine.CategoryId = targetCategoryId;

        //        var invSvc = base.GetService<IInventoryService>();
        //        targetLine.CategoryName = invSvc.GetFullCategoryName(targetCategory);

        //        tempList.Add(targetLine);
        //    }                       

        //    targetLine.Amount += lic.Amount;
        //    if (!recursive) targetLine.ChildrenProductsTotalSale += lic.Amount;
        //    targetLine.Cost += lic.Cost;

        //    if (targetCategory.ParentCategory != null)
        //    {
        //        AddToCategoryBranch(lic, targetCategory.ParentCategory.Id, tempList, true);
        //    }
        //    else targetLine.IsRootCategory = true;
        //}

        //private void MakeSum(IEnumerable<ReportLineViewModel> group, ReportLineViewModel lic)
        //{
        //    //decimal tempProfit = 0;

        //    //List<DateTime> different_dates = new List<DateTime>();

        //    //foreach (ReportLineViewModel x in group)
        //    //{
        //    //    //if (showSaleOrders || showPurchaseOrders)
        //    //    {
        //    //        tempQuantity += x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion;
        //    //        tempPrice += x.Amount;

        //    //        tempCost += x.Cost;
        //    //        //tempProfit += x.Profit;
        //    //    }

        //    //    //if (groupingByDayOfWeek && !different_dates.Contains(x.Order.Date.Date)) different_dates.Add(x.Order.Date.Date);
        //    //}

        //    if (groupingByProduct)
        //    {
        //        //qtty in base unit of measure
        //        double tempQuantity = group.Sum(x => x.Quantity * x.UnitMeasure.ToBaseConversion);
        //        double tempBasicQuantity = group.Sum(x => x.BasicQuantity * x.UnitMeasure.ToBaseConversion);

        //        UMFamily umFamily = appvm.ProductsOC.Single(x => x.Id == lic.Product.Id).UMFamily;
        //        var invSVC = base.GetService<IInventoryService>();
        //        UnitMeasure bestUM = invSVC.FitQuantity(umFamily, tempQuantity);
        //        lic.UnitMeasure = bestUM;
                
        //        lic.Quantity = tempQuantity / bestUM.ToBaseConversion;
        //        lic.BasicQuantity = tempBasicQuantity / bestUM.ToBaseConversion;
        //    }
        //    else if (rType == ReportType.SalesPerson || rType == ReportType.Shift ||
        //        rType == ReportType.DayOfWeekSalesAverage || rType == ReportType.GlobalSales)
        //    {
        //        lic.Clients = group.Sum(x => x.Clients);
        //    }

        //    lic.Amount = group.Sum(x => x.Amount);
        //    lic.Cost = group.Sum(x => x.Cost);
        //    lic.BasicCost = group.Sum(x => x.BasicCost);

        //    //if (groupingByDayOfWeek && different_dates.Count > 0) lic.Quantity = tempQuantity / different_dates.Count;

        //    //product might be null if it's grouping by category
        //    //if (lic.Product != null) 
        //    //{
        //    //    UnitMeasure costUM = lic.Product.CostUnitMeasure;
        //    //    double rate = costUM.ToBaseConversion;
        //    //    bool divide = rate != 1 && Math.Abs(tempQuantity) > rate;

        //    //    if (divide)
        //    //    {
        //    //        lic.Quantity = lic.Quantity / rate;
        //    //        lic.DayAverage = lic.DayAverage / rate;

        //    //        lic.UnitMeasure = costUM;
        //    //    }
        //    //}
        //}

        #endregion

        #region Update Graph Data

        protected void UpdateGraphDataAsync()
        {
            //worker is running
            if (bWorker != null && bWorker.IsBusy) return;

            bWorker = new BackgroundWorker();

            bWorker.DoWork += new DoWorkEventHandler(graphDataWorker_DoWork);
            bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(graphDataWorker_RunWorkerCompleted);

            Busy = true;
            //CommandManager.InvalidateRequerySuggested();

            bWorker.RunWorkerAsync();
        }

        void graphDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GetGraphData();
        }

        void graphDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                var result = (List<ChartItemViewModel>)e.Result;

                GraphData.Clear();
                foreach (var item in result)
                {
                    GraphData.Add(item);
                }
            }            

            Busy = false;
            CommandManager.InvalidateRequerySuggested();

            //unload events
            bWorker.DoWork -= graphDataWorker_DoWork;
            bWorker.RunWorkerCompleted -= graphDataWorker_RunWorkerCompleted;
        }

        protected abstract List<ChartItemViewModel> GetGraphData();

        #endregion

        #region Filtering

        protected List<ReportLineViewModel> linesBeforeLiveFilter = new List<ReportLineViewModel>();
        //public ObservableCollection<ReportLineViewModel> FilteredItems
        //{
        //    get { return lines_filtered; }
        //}

        protected void UpdateLiveFilter()
        {
            ItemsShowing.Clear();

            foreach (var item in linesBeforeLiveFilter)
            {
                if (LiveFilter(item))
                {
                    ItemsShowing.Add(item);
                }
            }
        }

        protected abstract bool LiveFilter(ReportLineViewModel item);

        //abstract bool PassesFilter(ReportLineViewModel li)
        //{
        //    //TEXT SEARCH
        //    if (groupingByProduct)
        //    {
        //        if (!TextCondition(li.Product.Name)) return false;
        //    }            
        //    if (rType == ReportType.SalesByCategory)
        //    {
        //        if (!TextCondition(li.CategoryName)) return false;
        //    }
        //    else if (rType == ReportType.SalesPerson)
        //    {
        //        if (!TextCondition(li.SalesPersonName)) return false;
        //    }
            
        //    //PRODUCT TYPES
        //    if (rType == ReportType.CostByItem || rType == ReportType.CostProjectionsByItem)
        //    {
        //        if (li.ProductType != ProductType.RawMaterials && li.ProductType != ProductType.CompraVenta) return false;
        //    }
        //    else if (rType == ReportType.SalesProjectionsByItem)
        //    {
        //        if (li.ProductType != ProductType.FinishedGoods && li.ProductType != ProductType.CompraVenta) return false;
        //    }
        //    else if (rType == ReportType.WIPByItem || rType == ReportType.WIPProjectionsByItem)
        //    {
        //        if (li.ProductType != ProductType.WorkInProcess) return false;
        //    }

        //    //CUSTOMIZE REPORT OPTIONS
        //    if (!allShiftsChecked)
        //    {
        //        if (!checkedShiftsIds.Contains(li.ShiftId)) return false;
        //    }

        //    if (!allSalesPersonsChecked)
        //    {
        //        if (!checkedSalesPersonsIds.Contains(li.SalesPersonId)) return false;
        //    }

        //    if (!allCategoriesChecked)
        //    {
        //        if (!checkedCategoriesIds.Contains(li.CategoryId)) return false;
        //    }

        //    if (!allTagsChecked)
        //    {
        //        if (checkedTagsIds.Intersect(li.TagIds).Count() == 0) return false;
        //    }

        //    return true;
        //}

        protected bool TextCondition(string searchWhere)
        {
            //SEARCH TEXT
            if (!string.IsNullOrWhiteSpace(searchWhat))
            {
                string trimed = searchWhat.Trim();

                if (string.IsNullOrWhiteSpace(searchWhere)) return false;

                if (searchWhere.IndexOf(trimed, StringComparison.OrdinalIgnoreCase) < 0) return false;
            }

            return true;
        }

        string searchWhat;
        public string SearchText
        {
            get { return searchWhat; }
            set
            {
                if (searchWhat != value)
                {
                    searchWhat = value;

                    UpdateLiveFilter();
                    UpdateTotals();
                    UpdateGraphDataAsync();
                }
            }
        }        

        #region Customize Command

        protected CustomizeReportOptions cro;

        //bool allShiftsChecked = true;
        //List<int> checkedShiftsIds;

        //bool allSalesPersonsChecked = true;
        //List<int> checkedSalesPersonsIds;

        //bool allCategoriesChecked = true;
        //List<int> checkedCategoriesIds;

        //bool allTagsChecked = true;
        //List<int> checkedTagsIds;

        RelayCommand customizeCmd;
        public RelayCommand CustomizeCommand
        {
            get
            {
                if (customizeCmd == null)
                {
                    customizeCmd = new RelayCommand(x => ShowCustomizeReportDialog());
                }
                return customizeCmd;
            }
        }

        //protected bool ShowCategoriesInCustomizeReportOptions { get; set; }
        //protected bool ShowTagsInCustomizeReportOptions { get; set; }

        void ShowCustomizeReportDialog()
        {
            CustomizeReportViewModel dialog = new CustomizeReportViewModel(cro, 
                ShowCategoriesInCustomizeReportOptions, ShowTagsInCustomizeReportOptions);

            var windowManager = base.GetService<IWindowManager>();

            if (windowManager.ShowDialog(dialog, this) == true)
            {
                dialog.SyncCustomizeOptions(cro);

                ////COPY SHIFT SELECTION
                //if (dialog.Shifts.Where(x => x.IsChecked).Count() - 1 == appvm.ShiftsOC.Count)
                //{
                //    allShiftsChecked = true;
                //}
                //else
                //{
                //    allShiftsChecked = false;
                //    checkedShiftsIds = dialog.Shifts.Where(x => x.IsChecked).Select(x => x.Id).ToList();
                //}
                ////COPY SALESPERSON SELECTION
                //if (dialog.SalesPersons.Where(x => x.IsChecked).Count() - 1 == appvm.EmployeesOC.Count)
                //{
                //    allSalesPersonsChecked = true;
                //}
                //else
                //{
                //    allSalesPersonsChecked = false;
                //    checkedSalesPersonsIds = dialog.SalesPersons.Where(x => x.IsChecked).Select(x => x.Id).ToList();
                //}

                ////COPY CATEGORY SELECTION
                //if (dialog.Categories.Where(x => x.IsChecked).Count() - 1 == appvm.CategoriesOC.Count)
                //{
                //    allCategoriesChecked = true;
                //}
                //else
                //{
                //    allCategoriesChecked = false;
                //    checkedCategoriesIds = dialog.Categories.Where(x => x.IsChecked).Select(x => x.Id).ToList();
                //}

                ////COPY TAG SELECTION
                //if (dialog.Tags.Where(x => x.IsChecked).Count() - 1 == appvm.TagsOC.Count)
                //{
                //    allTagsChecked = true;
                //}
                //else
                //{
                //    allTagsChecked = false;
                //    checkedTagsIds = dialog.Tags.Where(x => x.IsChecked).Select(x => x.Id).ToList();
                //}

                //if (fromDate != dialog.FromDate || toDate != dialog.ToDate)
                //{
                //    FromDate = dialog.FromDate;
                //    ToDate = dialog.ToDate;
                //    //UpdateSearch();
                //}

                FromDate = cro.FromDate;
                ToDate = cro.ToDate;

                UpdateSearchAsync();

                //else
                //{
                //    FilterAndUpdate();
                //}
            }
        }
        
        #endregion

        #endregion

        #region Grouping

        //ReportType rType;
        //public ReportType ReportType
        //{
        //    get { return rType; }
        //}

        ObservableCollection<ChartItemViewModel> graphData = new ObservableCollection<ChartItemViewModel>();
        public ObservableCollection<ChartItemViewModel> GraphData
        {
            get
            {
                if (lines_showing == null)
                {
                    lines_showing = new ObservableCollection<ReportLineViewModel>();

                    UpdateSearchAsync();
                }
                return graphData;
            }
        }

        //ObservableCollection<ReportColumnViewModel> headers;
        //public ObservableCollection<ReportColumnViewModel> Columns
        //{
        //    get { return headers; }
        //}

        //ObservableCollection<TotalItem> tItems;
        //public ObservableCollection<TotalItem> TotalItems
        //{
        //    get { return tItems; }
        //}

        //public ChartDefinition ChartDef { get; private set; }

        ObservableCollection<ReportLineViewModel> lines_showing;
        public ObservableCollection<ReportLineViewModel> ItemsShowing
        {
            get
            {
                if (lines_showing == null)
                {
                    lines_showing = new ObservableCollection<ReportLineViewModel>();

                    UpdateSearchAsync();
                }
                return lines_showing;
            }
        }

        //ObservableCollection<SalesByItemLineViewModel> salesByItemLines = new ObservableCollection<SalesByItemLineViewModel>();
        //public ObservableCollection<SalesByItemLineViewModel> SalesByItemLines
        //{
        //    get 
        //    {
        //        //if (salesByItemLines == null)
        //        //{
        //        //    lines_showing = new ObservableCollection<ReportLineViewModel>();
        //        //    salesByItemLines = new ObservableCollection<SalesByItemLineViewModel>();

        //        //    UpdateSearch();
        //        //}
        //        return salesByItemLines;
        //    }
        //}

        //bool noGrouping;
        //public bool NoGrouping 
        //{
        //    get { return noGrouping; }
        //    set
        //    {
        //        if (noGrouping != value) 
        //        {
        //            noGrouping = value;

        //            OnGroupingConditionsChanged();
        //        }                
        //    }
        //}
        //bool groupingByProduct;
        
        //SIMPLER CONTEO REPORT
        //bool isConteo;
        //public bool IsConteo { get { return isConteo; } }
        //public bool GroupingByProduct         
        //{
        //    get { return groupingByProduct; }
        //    set
        //    {
        //        if (groupingByProduct != value) 
        //        {
        //            groupingByProduct = value;

        //            OnGroupingConditionsChanged();
        //        }                
        //    }
        //}

        //bool groupingByCategory;
        //public bool GroupingByCategory
        //{
        //    get { return groupingByCategory; }
        //    set
        //    {
        //        if (groupingByCategory != value)
        //        {
        //            groupingByCategory = value;

        //            OnGroupingConditionsChanged();
        //        }
        //    }
        //}

        //bool groupingByDate;
        //public bool GroupingByDate 
        //{
        //    get { return groupingByDate; }
        //    set
        //    {
        //        if (groupingByDate != value) 
        //        {
        //            groupingByDate = value;

        //            OnGroupingConditionsChanged();
        //        }                
        //    }
        //}

        //bool groupingByWeek;
        //public bool GroupingByWeek
        //{
        //    get { return groupingByWeek; }
        //    set
        //    {
        //        if (groupingByWeek != value)
        //        {
        //            groupingByWeek = value;

        //            OnGroupingConditionsChanged();
        //        }
        //    }
        //}

        //bool groupingByDayOfWeek;
        //public bool GroupingByDayOfWeek
        //{
        //    get { return groupingByDayOfWeek; }
        //    set
        //    {
        //        if (groupingByDayOfWeek != value)
        //        {
        //            groupingByDayOfWeek = value;
        //        }
        //    }
        //}

        //bool groupingBySalesPerson;
        //public bool GroupingByEmployee
        //{
        //    get { return groupingByEmployee; }
        //    set
        //    {
        //        if (groupingByEmployee != value)
        //        {
        //            groupingByEmployee = value;

        //            OnGroupingConditionsChanged();
        //        }
        //    }
        //}

        //bool groupingByShift;

        #endregion

        #region Totals

        protected abstract void UpdateTotals();
        //{
        //    totalClients = 0;
        //    totalSales = 0;
        //    totalCost = 0;
        //    totalBasicCost = 0;
        //    totalActualCost = 0;
        //    totalProfit = 0;

        //    int count = 0;

        //    foreach (var item in lines_showing)
        //    {
        //        count++;

        //        //sum only root categories
        //        if (groupingByCategory && !item.IsRootCategory) continue;

        //        totalClients += item.Clients;
        //        totalSales += item.Amount;

        //        totalCost += item.Cost;
        //        totalBasicCost += item.BasicCost;
        //        totalActualCost += item.Cost;
        //        totalProfit += item.Profit;
        //    }

        //    TotalClients = totalClients;
        //    TotalSales = totalSales;
        //    TotalCost = totalCost;
        //    TotalBasicCost = totalBasicCost;
        //    TotalActualCost = totalActualCost;
        //    TotalProfit = totalProfit;
        //    SpendingByClient = totalClients == 0 ? 0 : totalSales / totalClients;
        //    TotalCostPercent = totalSales == 0 ? 0 : totalCost / totalSales;

        //}

        //double total;
        //public double Total
        //{
        //    get { return total; }
        //    set
        //    {
        //        total = value;
        //        OnPropertyChanged("Total");
        //    }
        //}

        //public bool TotalsVisible
        //{
        //    get { return true; }
        //}

        //int totalClients;
        //public int TotalClients
        //{
        //    get { return totalClients; }
        //    set
        //    {
        //        totalClients = value;
        //        OnPropertyChanged("TotalClients");
        //    }
        //}

        //decimal totalCost;
        //public decimal TotalCost
        //{
        //    get { return totalCost; }
        //    set
        //    {
        //        totalCost = value;
        //        OnPropertyChanged("TotalCost");
        //    }
        //}

        //decimal totalBasicCost;
        //public decimal TotalBasicCost
        //{
        //    get { return totalBasicCost; }
        //    set
        //    {
        //        totalBasicCost = value;
        //        OnPropertyChanged("TotalBasicCost");
        //    }
        //}

        //decimal totalActualCost;
        //public decimal TotalActualCost
        //{
        //    get { return totalActualCost; }
        //    set
        //    {
        //        totalActualCost = value;
        //        OnPropertyChanged("TotalActualCost");
        //    }
        //}

        //decimal totalProfit;
        //public decimal TotalProfit
        //{
        //    get { return totalProfit; }
        //    set
        //    {
        //        totalProfit = value;
        //        OnPropertyChanged("TotalProfit");
        //    }
        //}

        //decimal totalCostPercent;
        //public decimal TotalCostPercent
        //{
        //    get { return totalCostPercent; }
        //    set
        //    {
        //        totalCostPercent = value;
        //        OnPropertyChanged("TotalCostPercent");
        //    }
        //}

        //decimal totalSales;
        //public decimal TotalSales
        //{
        //    get { return totalSales; }
        //    set
        //    {
        //        totalSales = value;
        //        OnPropertyChanged("TotalSales");
        //    }
        //}

        //decimal spendByClient;
        //public decimal SpendingByClient
        //{
        //    get { return spendByClient; }
        //    set
        //    {
        //        spendByClient = value;
        //        OnPropertyChanged("SpendingByClient");
        //    }
        //}

        //double avgPref;
        //public double AveragePreference
        //{
        //    get { return avgPref; }
        //    set
        //    {
        //        avgPref = value;
        //        OnPropertyChanged("AveragePreference");
        //    }
        //}

        //double avgProfitMargin;
        //public double AverageProfitMargin
        //{
        //    get { return avgProfitMargin; }
        //    set
        //    {
        //        avgProfitMargin = value;
        //        OnPropertyChanged("AverageProfitMargin");
        //    }
        //}

        //decimal totalSalesPrice;
        //public decimal TotalSalesPrice
        //{
        //    get { return totalSalesPrice; }
        //    set
        //    {
        //        totalSalesPrice = value;
        //        OnPropertyChanged("TotalSalesPrice");
        //    }
        //}

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

        #region Visibility

        //void UpdateColumnsVisibility()
        //{
        //    //DateColumnVisible = noGrouping || groupingByDate;
        //    QuantityColumnVisible = rType == ReportType.SalesByItem || rType == ReportType.WIPByItem || rType == ReportType.CostByItem || rType == ReportType.Conteo;
        //    BasicQuantityColumnVisible = rType == ReportType.CostByItem;
        //    //WeekDayColumnVisible = groupingByDayOfWeek;
        //    //WeekStringColumnVisible = groupingByWeek;
        //    ProductColumnVisible = groupingByProduct;
        //    CategoryColumnVisible = groupingByCategory;
        //    SalesColumnVisible = rType == ReportType.SalesByItem || rType == ReportType.SalesByCategory || rType == ReportType.SalesPerson || 
        //        rType == ReportType.Shift|| rType == ReportType.DayOfWeekSalesAverage;

        //    CostColumnVisible = rType == ReportType.SalesByItem || rType == ReportType.SalesByCategory || rType == ReportType.CostByItem;
        //    BasicCostColumnVisible = rType == ReportType.CostByItem;

        //    ProfitColumnVisible = rType == ReportType.SalesByItem || rType == ReportType.SalesByCategory;
        //    CostPercentColumnVisible = rType == ReportType.SalesByItem || rType == ReportType.SalesByCategory;
        //    //ProfitColumnVisible = (groupingByProduct || groupingByCategory) && !isConteo;
        //    //CostPercentColumnVisible = (groupingByProduct || groupingByCategory) && !isConteo;

        //    //DayAverageColumnVisible = groupingByDayOfWeek;

        //    SalesPersonColumnVisible = groupingBySalesPerson;
        //    //ShiftColumnVisible = rType == ReportType.Shift;
        //    //ResponsibleColumnVisible = showPurchaseOrders && noGrouping;
        //    //PurchaseTypeColumnVisible = showPurchaseOrders && noGrouping;
        //    //AreaColumnVisible = noGrouping;
        //    //OrderColumnVisible = noGrouping;

        //    ClientsColumnVisible = SalesByClientColumnVisible = groupingBySalesPerson || rType == ReportType.Shift || groupingByDayOfWeek;
        //    DayOfWeekColumnVisible = groupingByDayOfWeek;

        //    //WeekdaysColumnsVisible = rType == ReportType.DayOfWeekSalesAverageByItem || rType == ReportType.DayOfWeekWIPAverageByItem || rType == ReportType.DayOfWeekCostAverageByItem;

        //    PreferenceColumnVisible = ProfitMarginColumnVisible = ProductClassColumnVisible = rType == ReportType.ProductClasification;
        //}

        //bool dateColumnVisible;
        //public bool DateColumnVisible
        //{
        //    get { return dateColumnVisible; }
        //    set
        //    {
        //        dateColumnVisible = value;
        //        OnPropertyChanged("DateColumnVisible");
        //    }
        //}

        //bool weekStringColumnVisible;
        //public bool WeekStringColumnVisible
        //{
        //    get { return weekStringColumnVisible; }
        //    set
        //    {
        //        weekStringColumnVisible = value;
        //        OnPropertyChanged("WeekStringColumnVisible");
        //    }
        //}

        //bool dowColumnVisible;
        //public bool DayOfWeekColumnVisible
        //{
        //    get { return dowColumnVisible; }
        //    set
        //    {
        //        dowColumnVisible = value;
        //        OnPropertyChanged("DayOfWeekColumnVisible");
        //    }
        //}

        //bool dayAvgColumnVisible;
        //public bool DayAverageColumnVisible
        //{
        //    get { return dayAvgColumnVisible; }
        //    set
        //    {
        //        dayAvgColumnVisible = value;
        //        OnPropertyChanged("DayAverageColumnVisible");
        //    }
        //}

        //bool quantityColumnVisible;
        //public bool QuantityColumnVisible
        //{
        //    get { return quantityColumnVisible; }
        //    set
        //    {
        //        quantityColumnVisible = value;
        //        OnPropertyChanged("QuantityColumnVisible");
        //    }
        //}

        //bool basicQttyColumnVisible;
        //public bool BasicQuantityColumnVisible
        //{
        //    get { return basicQttyColumnVisible; }
        //    set
        //    {
        //        basicQttyColumnVisible = value;
        //        OnPropertyChanged("BasicQuantityColumnVisible");
        //    }
        //}

        //bool productColumnVisible;
        //public bool ProductColumnVisible
        //{
        //    get { return productColumnVisible; }
        //    set
        //    {
        //        productColumnVisible = value;
        //        OnPropertyChanged("ProductColumnVisible");
        //    }
        //}

        //bool categoryColumnVisible;
        //public bool CategoryColumnVisible
        //{
        //    get { return categoryColumnVisible; }
        //    set
        //    {
        //        categoryColumnVisible = value;
        //        OnPropertyChanged("CategoryColumnVisible");
        //    }
        //}

        //bool salesColumnVisible;
        //public bool SalesColumnVisible
        //{
        //    get { return salesColumnVisible; }
        //    set
        //    {
        //        salesColumnVisible = value;
        //        OnPropertyChanged("SalesColumnVisible");
        //    }
        //}

        //bool costColumnVisible;
        //public bool CostColumnVisible
        //{
        //    get { return costColumnVisible; }
        //    set
        //    {
        //        costColumnVisible = value;
        //        OnPropertyChanged("CostColumnVisible");
        //    }
        //}

        //bool basicCostColumnVisible;
        //public bool BasicCostColumnVisible
        //{
        //    get { return basicCostColumnVisible; }
        //    set
        //    {
        //        basicCostColumnVisible = value;
        //        OnPropertyChanged("BasicCostColumnVisible");
        //    }
        //}       

        //bool profitColumnVisible;
        //public bool ProfitColumnVisible
        //{
        //    get { return profitColumnVisible; }
        //    set
        //    {
        //        profitColumnVisible = value;
        //        OnPropertyChanged("ProfitColumnVisible");
        //    }
        //}

        //bool costPercentColumnVisible;
        //public bool CostPercentColumnVisible
        //{
        //    get { return costPercentColumnVisible; }
        //    set
        //    {
        //        costPercentColumnVisible = value;
        //        OnPropertyChanged("CostPercentColumnVisible");
        //    }
        //}

        

        //bool spColumnVisible;
        //public bool SalesPersonColumnVisible
        //{
        //    get { return spColumnVisible; }
        //    set
        //    {
        //        spColumnVisible = value;
        //        OnPropertyChanged("SalesPersonColumnVisible");
        //    }
        //}

        //bool shiftColumnVisible;
        //public bool ShiftColumnVisible
        //{
        //    get { return shiftColumnVisible; }
        //    set
        //    {
        //        shiftColumnVisible = value;
        //        OnPropertyChanged("ShiftColumnVisible");
        //    }
        //}

        //bool areaColumnVisible;
        //public bool AreaColumnVisible
        //{
        //    get { return areaColumnVisible; }
        //    set
        //    {
        //        areaColumnVisible = value;
        //        OnPropertyChanged("AreaColumnVisible");
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

        //bool clientsColumnVisible;

        //public bool ClientsColumnVisible
        //{
        //    get { return clientsColumnVisible; }
        //    set
        //    {
        //        clientsColumnVisible = value;
        //        OnPropertyChanged("ClientsColumnVisible");
        //    }
        //}

        //bool sbcColumnVisible;

        //public bool SalesByClientColumnVisible
        //{
        //    get { return sbcColumnVisible; }
        //    set
        //    {
        //        sbcColumnVisible = value;
        //        OnPropertyChanged("SalesByClientColumnVisible");
        //    }
        //}

        //bool wdColumnsVisible;
        //public bool WeekdaysColumnsVisible
        //{
        //    get { return wdColumnsVisible; }
        //    set
        //    {
        //        wdColumnsVisible = value;
        //        OnPropertyChanged("WeekdaysColumnsVisible");
        //    }
        //}

        //bool prefClmnVisible;
        //public bool PreferenceColumnVisible
        //{
        //    get { return prefClmnVisible; }
        //    set
        //    {
        //        prefClmnVisible = value;
        //        OnPropertyChanged("PreferenceColumnVisible");
        //    }
        //}

        //bool profitClmnVisible;
        //public bool ProfitMarginColumnVisible
        //{
        //    get { return profitClmnVisible; }
        //    set
        //    {
        //        profitClmnVisible = value;
        //        OnPropertyChanged("ProfitMarginColumnVisible");
        //    }
        //}

        //bool prodClassClmnVisible;
        //public bool ProductClassColumnVisible
        //{
        //    get { return prodClassClmnVisible; }
        //    set
        //    {
        //        prodClassClmnVisible = value;
        //        OnPropertyChanged("ProductClassColumnVisible");
        //    }
        //} 

        //public bool CanUserFilterByShift
        //{
        //    get { return rType != ReportType.Shift; }
        //}

        //public bool CanUserFilterByCategory
        //{
        //    get { return groupingByProduct; }
        //}

        //public bool CanUserFilterByTag
        //{
        //    get { return groupingByProduct; }
        //}

        //public bool ShowPie
        //{
        //    get { return groupingByCategory && !(rType == ReportType.Conteo); }
        //}

        //public bool ShowColumns
        //{
        //    get { return !(groupingByCategory || rType == ReportType.Conteo); }
        //}

        //public bool ShowProjectionPercent
        //{
        //    get { return rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem; }
        //}

        #endregion

        //double projPct = 5;
        //public double ProjectionPercent
        //{
        //    get { return projPct; }
        //    set
        //    {
        //        projPct = value;

        //        MultiplyByProjectionPercent();

        //        OnPropertyChanged("ProjectionPercent");
        //    }
        //}
        
        //#region Export to Excel Command

        //RelayCommand exportToExcelCmd;
        //public ICommand ExportToExcelCommand
        //{
        //    get
        //    {
        //        if (exportToExcelCmd == null)
        //            exportToExcelCmd = new RelayCommand(x => ExportToExcel(false));
        //        return exportToExcelCmd;
        //    }
        //}

        //RelayCommand exportByCateogry;
        //public ICommand ExportByCateogry
        //{
        //    get
        //    {
        //        if (exportByCateogry == null)
        //            exportByCateogry = new RelayCommand(x => Export(true), x => CanExportByCategory);
        //        return exportByCateogry;
        //    }
        //}

        //bool CanExportByCategory
        //{
        //    get { return !groupingByCategory; }
        //}

        //private void ExportToExcel(bool groupByCategory)
        //{
        //    //Action<CollectionViewGroup, Excel.Range> displayGroup = (group, cell) =>
        //    //{
        //    //    if (group.Name != DependencyProperty.UnsetValue)
        //    //    {
        //    //        //write the name of the category
        //    //        cell.Value = group.Name;
        //    //        cell.Font.Bold = true;
        //    //    }
        //    //    cell.Offset[1, 0].Select();

        //    //    int row = 1;
        //    //    foreach (var item in group.Items)
        //    //    {
        //    //        DisplayItem((ReportLineViewModel)item, cell.Offset[row, 0]);
        //    //        row++;
        //    //        cell.Offset[row, 0].Select();
        //    //    }
        //    //};

        //    int numberOfColumns = CountVisibleColumns();

        //    ICollectionView view = CollectionViewSource.GetDefaultView(lines_showing);

        //    var excelExporter = base.GetService<IExcelExporter>();

        //    //if (groupByCategory)
        //    //{
        //    //    CollectionViewSource cvs = new CollectionViewSource();
        //    //    cvs.Source = lines_showing;
        //    //    ICollectionView groupedView = cvs.View;

        //    //    PropertyGroupDescription pgd = new PropertyGroupDescription("Category.Name");
        //    //    groupedView.GroupDescriptions.Add(pgd);

        //    //    foreach (var item in view.SortDescriptions)
        //    //    {
        //    //        groupedView.SortDescriptions.Add(item);
        //    //    }

        //    //    excelExporter.ExportToExcel<CollectionViewGroup>(groupedView.Groups.Cast<CollectionViewGroup>(),
        //    //        DisplayHeader, displayGroup, numberOfColumns);

        //    //    //DisplayInGroups(cvTotals.Groups.Cast<CollectionViewGroup>(), displayGroup);
        //    //}
        //    //else 
        //    {
        //        excelExporter.ExportToExcel<ReportLineViewModel>(view.Cast<ReportLineViewModel>(),
        //        DisplayHeader, DisplayItem, numberOfColumns);
        //    }            
        //}
        
        //#endregion

        #region Export to Excel Command

        RelayCommand exportToExcelCmd;
        public RelayCommand ExportToExcelCommand
        {
            get
            {
                if (exportToExcelCmd == null)
                    exportToExcelCmd = new RelayCommand(x => ExportToExcelCmd());
                return exportToExcelCmd;
            }
        }

        BackgroundWorker excelWorker;
        ProgressDialogViewModel excelProgressDialog;

        private void ExportToExcelCmd()
        {
            if (excelWorker == null)
            {
                excelWorker = new BackgroundWorker();

                excelWorker.DoWork += new DoWorkEventHandler(excelWorker_DoWork);

                excelWorker.WorkerReportsProgress = true;
                excelWorker.ProgressChanged += new ProgressChangedEventHandler(excelWorker_ProgressChanged);

                excelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(excelWorker_RunWorkerCompleted);
            }

            excelProgressDialog = new ProgressDialogViewModel();
            excelProgressDialog.Message = "Exportando a Excel...";
            excelProgressDialog.IsBusy = true;

            var windowManager = base.GetService<IWindowManager>();

            excelWorker.RunWorkerAsync();

            windowManager.ShowDialog(excelProgressDialog, this);
        }

        void excelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //int numberOfColumns = CountVisibleColumns();

            //ICollectionView view = CollectionViewSource.GetDefaultView(lines_showing);

            ExportToExcel(excelWorker);
        }

        void excelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            excelProgressDialog.Progress = e.ProgressPercentage;
        }

        void excelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            excelProgressDialog.IsBusy = false;

            //close progress dialog
            var windowManager = base.GetService<IWindowManager>();            
            windowManager.Close(excelProgressDialog);
        }

        

        //void DisplayHeader(Excel.Range cell)
        //{
        //    int column = 0;

        //    //foreach (var h in headers)
        //    //{
        //    //    var fieldInfo = typeof(ReportLineViewModel).GetField(h.BindingString);
        //    //    fieldInfo.GetValue(
        //    //}

        //    if (ProductColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Producto";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (BasicQuantityColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Cantidad Estándar";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        //cell.Offset[0, column].Value = "UM";
        //        //cell.Offset[0, column++].Font.Bold = "True";
        //    }
        //    if (QuantityColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = basicQttyColumnVisible ? "Cantidad Real" : "Cantidad";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        //cell.Offset[0, column].Value = "UM";
        //        //cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (PreferenceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "% Preferencia";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (CategoryColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Categoría";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (SalesPersonColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Dependiente";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (DayOfWeekColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Día";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (SalesColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Ventas";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (BasicCostColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Costo Estándar";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }
        //    if (CostColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = basicCostColumnVisible ? "Costo Real" : "Costo";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }            

        //    if (CostPercentColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Costo %";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (ProfitColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Ganancia";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (ProfitMarginColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "% Contribución";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (ProductClassColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Clase";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    //if (DayAverageColumnVisible)
        //    //{
        //    //    cell.Offset[0, column].Value = "Ventas (Promedio)";
        //    //    cell.Offset[0, column++].Font.Bold = "True";
        //    //}

        //    if (ClientsColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "Clientes";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (SalesByClientColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = "$/Cliente";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (WeekdaysColumnsVisible)
        //    {
        //        cell.Offset[0, column].Value = "Lunes";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Martes";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Miércoles";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Jueves";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Viernes";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Sábado";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Domingo";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Lunes-Jueves";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        cell.Offset[0, column].Value = "Viernes-Domingo";
        //        cell.Offset[0, column++].Font.Bold = "True";
        //    }

        //    if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem)
        //    {
        //        foreach (var item in headers.Skip(1))
        //        {
        //            cell.Offset[0, column].Value = item.Header;
        //            cell.Offset[0, column++].Font.Bold = "True";
        //        }
        //    }
        //}

        //void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        //{
        //    int column = 0;

        //    //product column
        //    if (ProductColumnVisible)
        //        cell.Offset[0, column++].Value = line.Product.Name;

        //    if (BasicQuantityColumnVisible)
        //    {
        //        cell.Offset[0, column++].Value = Math.Round(line.BasicQuantity, 2) + " " + line.UnitMeasure.Caption;
        //        //cell.Offset[0, column++].Value = line.UnitMeasure.Name;
        //    }
        //    if (QuantityColumnVisible)
        //    {
        //        cell.Offset[0, column++].Value = Math.Round(line.Quantity, 2) + " " + line.UnitMeasure.Caption;
        //        //cell.Offset[0, column++].Value = line.UnitMeasure.Name;
        //    }

        //    if (PreferenceColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.Preference;
        //        cell.Offset[0, column++].Style = "Percent";
        //    }

        //    if (CategoryColumnVisible)
        //        cell.Offset[0, column++].Value = line.CategoryName;

        //    if (SalesPersonColumnVisible)
        //        cell.Offset[0, column++].Value = line.SalesPersonName;

        //    if (DayOfWeekColumnVisible)
        //        cell.Offset[0, column++].Value = line.DiaEnEspanol.ToString();

        //    if (SalesColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.Amount;
        //        cell.Offset[0, column++].Style = "Currency";
        //    }

        //    if (BasicCostColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.BasicCost;
        //        cell.Offset[0, column++].Style = "Currency";
        //    }
        //    if (CostColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.Cost;
        //        cell.Offset[0, column++].Style = "Currency";
        //    }            

        //    if (CostPercentColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.CostToPriceRatio;
        //        cell.Offset[0, column++].Style = "Percent";
        //    }

        //    if (ProfitColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.Profit;
        //        cell.Offset[0, column++].Style = "Currency";
        //    }

        //    if (ProfitMarginColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.ProfitMargin;                
        //        cell.Offset[0, column++].Style = "Percent";
        //    }

        //    if (ProductClassColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.ProductClass.ToString();
        //    }
        //    //if (DayAverageColumnVisible)
        //    //{
        //    //    cell.Offset[0, column].Value = lineitem.DayAverage;
        //    //    cell.Offset[0, column++].Style = "Currency";
        //    //}

        //    if (ClientsColumnVisible)
        //        cell.Offset[0, column++].Value = line.Clients;

        //    if (SalesByClientColumnVisible)
        //    {
        //        cell.Offset[0, column].Value = line.SalesByClient;
        //        cell.Offset[0, column++].Style = "Currency";
        //    }

        //    if (WeekdaysColumnsVisible)
        //    {
        //        for (int i = 0; i < line.ProjQuantities.Length; i++)
        //        {
        //            double qtty = line.ProjQuantities[i];
        //            UnitMeasure um = line.ProjUnitOfMeasures[i];
        //            cell.Offset[0, column++].Value = qtty + (um == null ? string.Empty : " " + um.Caption);
        //        }

        //        //sunday
        //        double sundayQtty = line.ProjQuantities[0];
        //        UnitMeasure sundayUM = line.ProjUnitOfMeasures[0];
        //        cell.Offset[0, column++].Value = sundayQtty + (sundayUM == null ? string.Empty : " " + sundayUM.Caption);

        //        cell.Offset[0, column++].Value = line.WeekdayQtty + line.WeekdayUM.Caption;
        //        cell.Offset[0, column++].Value = line.WeekendQtty + line.WeekendUM.Caption;
        //    }

        //    if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem)
        //    {
        //        for (int i = 0; i < line.ProjQuantities.Length; i++)
        //        {
        //            double qtty = line.ProjQuantities[i];
        //            UnitMeasure um = line.ProjUnitOfMeasures[i];
        //            cell.Offset[0, column++].Value = qtty + (um == null ? string.Empty : " " + um.Caption);
        //        }
        //    }
        //}

        //private int CountVisibleColumns()
        //{
        //    int numberOfColumns = 0;

        //    if (BasicQuantityColumnVisible) numberOfColumns++;

        //    if (QuantityColumnVisible) numberOfColumns++;//qtty and um

        //    if (PreferenceColumnVisible) numberOfColumns++;

        //    if (ProductColumnVisible) numberOfColumns++;

        //    if (CategoryColumnVisible) numberOfColumns++;

        //    if (SalesPersonColumnVisible) numberOfColumns++;

        //    if (DayOfWeekColumnVisible) numberOfColumns++;

        //    if (SalesColumnVisible) numberOfColumns++;

        //    if (BasicCostColumnVisible) numberOfColumns++;

        //    if (CostColumnVisible) numberOfColumns++;

        //    if (ProfitColumnVisible) numberOfColumns++;

        //    if (ProfitMarginColumnVisible) numberOfColumns++;

        //    if (ProductClassColumnVisible) numberOfColumns++;

        //    if (CostPercentColumnVisible) numberOfColumns++;

        //    //if (DayAverageColumnVisible) numberOfColumns++;

        //    if (ClientsColumnVisible) numberOfColumns++;

        //    if (SalesByClientColumnVisible) numberOfColumns++;

        //    if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem || rType == ReportType.CostProjectionsByItem)
        //        numberOfColumns += 7;

        //    return numberOfColumns;
        //}        

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

        //#region Show Order Command

        //bool showingSale;
        //public bool ShowingSale
        //{
        //    get { return showingSale; }
        //    set
        //    {
        //        if (showingSale != value)
        //        {
        //            showingSale = value;
        //            OnPropertyChanged("ShowingSale");
        //        }
        //    }
        //}
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

        //OfflineSaleViewModel selectedSale;
        //public OfflineSaleViewModel SelectedSale
        //{
        //    get { return selectedSale; }
        //    set
        //    {
        //        selectedSale = value;
        //        OnPropertyChanged("SelectedSale");
        //    }
        //}

        ////PurchaseViewModel selectedPurchase;
        ////public PurchaseViewModel SelectedPurchase
        ////{
        ////    get { return selectedPurchase; }
        ////    set
        ////    {
        ////        selectedPurchase = value;
        ////        OnPropertyChanged("SelectedPurchase");
        ////    }
        ////}

        //RelayCommand showOrderCommand;
        //public ICommand ShowOrderCommand
        //{
        //    get
        //    {
        //        if (showOrderCommand == null)
        //            showOrderCommand = new RelayCommand(x => ShowOrder((Order)x));
        //        return showOrderCommand;
        //    }
        //}

        //private void ShowOrder(Order x)
        //{
        //    //if (x is Sale)
        //    //{
        //    //    Sale sale = (Sale)x;
        //    //    SaleViewModel viewmodel = new SaleViewModel(appvm, sale);

        //    //    SelectedSale = viewmodel;
        //    //    ShowingSale = true;
        //    //}
        //    //else if (x is Purchase)
        //    //{
        //    //    Purchase purchase = (Purchase)x;
        //    //    PurchaseViewModel viewmodel = new PurchaseViewModel(appvm, purchase, inventoryService);

        //    //    SelectedPurchase = viewmodel;
        //    //    ShowingPurchase = true;
        //    //}
        //    var windowManager = base.GetService<IWindowManager>();

        //    if (x is Sale)
        //    {
        //        Sale sale = (Sale)x;

        //        Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
        //        {
        //            if (!(wsvm is OfflineSaleViewModel)) return false;

        //            OfflineSaleViewModel svm = (OfflineSaleViewModel)wsvm;

        //            return svm.WrappedSale == sale;
        //        };

        //        if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
        //        else
        //        {
        //            OfflineSaleViewModel viewmodel = new OfflineSaleViewModel(appvm, sale, OnRemoved, null);
        //            windowManager.ShowChildWindow(viewmodel, this);
        //        }
        //    }
        //    //else if (x is Purchase)
        //    //{
        //    //    Purchase purchase = (Purchase)x;

        //    //    Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
        //    //    {
        //    //        if (!(wsvm is PurchaseViewModel)) return false;

        //    //        PurchaseViewModel pvm = (PurchaseViewModel)wsvm;

        //    //        return pvm.WrappedPurchase == purchase;
        //    //    };

        //    //    if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
        //    //    else
        //    //    {
        //    //        PurchaseViewModel viewmodel = new PurchaseViewModel(appvm, purchase);
        //    //        windowManager.ShowChildWindow(viewmodel, this);
        //    //    }
        //    //}
        //}

        //void OnRemoved(Sale s)
        //{
        //    foreach (var item in lines_showing)
        //    {
        //        if (item.Sale == s)
        //        {
        //            lines_showing.Remove(item);
        //            UpdateTotals();
        //            break;
        //        }
        //    }
        //}

        //#endregion

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

    //public class SameDateComparer : IEqualityComparer<ReportLineViewModel>
    //{
    //    public bool Equals(ReportLineViewModel x, ReportLineViewModel y)
    //    {
    //        return x.Date.Date == y.Date.Date;
    //    }

    //    public int GetHashCode(ReportLineViewModel obj)
    //    {
    //        return obj.Date.DayOfYear;
    //    }
    //}

    //public class SameMonthComparer : IEqualityComparer<ReportLineViewModel>
    //{
    //    public bool Equals(ReportLineViewModel x, ReportLineViewModel y)
    //    {
    //        return x.Date.Month == y.Date.Month;
    //    }

    //    public int GetHashCode(ReportLineViewModel obj)
    //    {
    //        return obj.Date.Month;
    //    }
    //}

}
