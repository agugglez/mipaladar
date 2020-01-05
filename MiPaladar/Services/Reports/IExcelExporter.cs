using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

using MiPaladar.ViewModels;

namespace MiPaladar.Services
{
    public interface IExcelExporter 
    {
        void ExportToExcel(IEnumerable<ReportLineViewModel> items, BackgroundWorker bWorker);

        //void ExportToExcel(ReportType rType, IEnumerable<ReportLineViewModel> items);

        //void ExportToExcel(IEnumerable<ReportLineViewModel> items, BackgroundWorker bWorker);

        //void ExportDayAveragesReport(IEnumerable<ReportItemViewModel> itemsList, BackgroundWorker bWorker);

        //void Export101ReportInfo(IEnumerable<LineOfWork> linesOfWork, BackgroundWorker bWorker);

        //void CreateAndExportReportToExcel(CustomizeReportOptions cro, BackgroundWorker bWoker);
    }

    //EE -> Excel Exporter
    public interface IGlobalSalesEE : IExcelExporter
    {
    }
    public interface ISalesByItemEE : IExcelExporter
    {
    }
    public interface IConteoEE : IExcelExporter
    {
    }
    public interface ISalesByCategoryEE : IExcelExporter
    {
    }
    public interface ISalesPersonEE : IExcelExporter
    {
    }
    public interface IDayOfWeekSalesEE : IExcelExporter
    {
    }
    public interface IProductClassesEE : IExcelExporter
    {
    }
    public interface IWIPByItemEE : IExcelExporter
    {
    }
    public interface ICostByItemEE : IExcelExporter
    {
    }
    public interface IProjectionsEE : IExcelExporter
    {
    }
    public interface IServiceTimeEE : IExcelExporter
    {
    }
    public interface IDemandByHourEE : IExcelExporter
    {
    }

    //public class ExportToExcelService : IExcelExporter
    //{
    //    //public void ExportToExcel(ReportType rType, IEnumerable<ReportLineViewModel> items)
    //    //{
    //    //    ExportToExcel(rType, items, null);
    //    //}
    //    //public void ExportToExcel(IEnumerable<ReportLineViewModel> items, BackgroundWorker bWorker)
    //    //{
    //    //    var excelApp = InitExcel();

    //    //    //excelApp.Visible = true;

    //    //    DisplayHeader(excelApp);

    //    //    //change line
    //    //    excelApp.ActiveCell.Offset[1, 0].Select();

    //    //    DisplayGridData(items, excelApp, bWorker);

    //    //    AutofitAndMakeVisible(excelApp);
            
    //    //    excelApp.Range["A1"].Select();
    //    //}

    //    //#region Helpers

    //    //private Excel.Application InitExcel()
    //    //{
    //    //    var excelApp = new Microsoft.Office.Interop.Excel.Application();

    //    //    // Add a new Excel workbook.
    //    //    excelApp.Workbooks.Add();

    //    //    return excelApp;
    //    //}

    //    //private void DisplayHeader(ReportType rType, Excel.Application excelApp)
    //    //{
    //    //    excelApp.Range["A1"].Select();

    //    //    var cell = excelApp.ActiveCell;

    //    //    if (rType == ReportType.GlobalSales)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Intervalo");
    //    //        PrintHeader(cell, 0, 1, "Ventas");
    //    //        PrintHeader(cell, 0, 2, "Clientes");
    //    //        PrintHeader(cell, 0, 3, "$/Cliente");
    //    //    }
    //    //    else if (rType == ReportType.SalesByItem)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Producto");
    //    //        PrintHeader(cell, 0, 1, "Cantidad");
    //    //        PrintHeader(cell, 0, 2, "Ventas");
    //    //        PrintHeader(cell, 0, 3, "Costo");
    //    //        PrintHeader(cell, 0, 4, "Costo %");
    //    //        PrintHeader(cell, 0, 5, "Ganancia");
    //    //    }
    //    //    else if (rType == ReportType.SalesByCategory)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Categoría");
    //    //        PrintHeader(cell, 0, 1, "Ventas");
    //    //        PrintHeader(cell, 0, 2, "Costo");
    //    //        PrintHeader(cell, 0, 3, "Costo %");
    //    //        PrintHeader(cell, 0, 4, "Ganancia");
    //    //    }
    //    //    else if (rType == ReportType.SalesPerson)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Dependiente");
    //    //        PrintHeader(cell, 0, 1, "Ventas");
    //    //        PrintHeader(cell, 0, 2, "Clientes");
    //    //        PrintHeader(cell, 0, 3, "$/Cliente");
    //    //    }
    //    //    else if (rType == ReportType.DayOfWeekSalesAverage)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Día");
    //    //        PrintHeader(cell, 0, 1, "Ventas");
    //    //        PrintHeader(cell, 0, 2, "Clientes");
    //    //        PrintHeader(cell, 0, 3, "$/Cliente");
    //    //    }
    //    //    else if (rType == ReportType.ProductClasification)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Producto");
    //    //        PrintHeader(cell, 0, 1, "% Preferencia");
    //    //        PrintHeader(cell, 0, 2, "% Contribución");
    //    //        PrintHeader(cell, 0, 3, "Clase");
    //    //    }
    //    //    else if (rType == ReportType.WIPByItem)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Producto");
    //    //        PrintHeader(cell, 0, 1, "Cantidad");
    //    //    }
    //    //    else if (rType == ReportType.CostByItem)
    //    //    {
    //    //        PrintHeader(cell, 0, 0, "Producto");
    //    //        PrintHeader(cell, 0, 1, "Cantidad Estándar");
    //    //        PrintHeader(cell, 0, 2, "Cantidad Real");
    //    //        PrintHeader(cell, 0, 3, "Costo Estándar");
    //    //        PrintHeader(cell, 0, 4, "Costo Real");
    //    //    }
    //    //    else if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
    //    //        rType == ReportType.CostProjectionsByItem)
    //    //    {
    //    //        DateTime today = DateTime.Today;

    //    //        //project sales for seven days
    //    //        for (int i = 0; i < 7; i++)
    //    //        {
    //    //            Excel.Range fromCell = cell.Offset[0, 1 + i * 4];
    //    //            Excel.Range toCell = fromCell.Offset[0, 3];

    //    //            PrintHeader(fromCell, 0, 0, today.AddDays(i).ToString("d/MMM"));
    //    //            //fromCell.Value = today.AddDays(i).ToString("d/MMM");
    //    //            //fromCell.Font.Bold = "True";

    //    //            //merge header cells
    //    //            excelApp.Range[fromCell, toCell].Merge();
    //    //        }

    //    //        //change line
    //    //        cell.Offset[1, 0].Activate();
    //    //        cell = excelApp.ActiveCell;

    //    //        PrintHeader(cell, 0, 0, "Producto");
    //    //        //cell.Offset[0, 0].Value = "Producto";
    //    //        //cell.Offset[0, 0].Font.Bold = "True";

    //    //        //project sales for seven days
    //    //        for (int i = 0; i < 7; i++)
    //    //        {
    //    //            Excel.Range fromCell = cell.Offset[0, 1 + i * 4];

    //    //            fromCell.Offset[0, 0].Value = "Min";
    //    //            fromCell.Offset[0, 1].Value = "Max";
    //    //            fromCell.Offset[0, 2].Value = "Prom";
    //    //            fromCell.Offset[0, 3].Value = "Proy";
    //    //        }
    //    //    }
    //    //}

    //    //private void PrintHeader(Excel.Range cell, int row_offset, int column_offset, string value)
    //    //{
    //    //    cell.Offset[row_offset, column_offset].Value = value;
    //    //    cell.Offset[row_offset, column_offset].Font.Bold = "True";
    //    //}

    //    //private void DisplayGridData(ReportType rType, IEnumerable<ReportLineViewModel> items, 
    //    //    Excel.Application excelApp, BackgroundWorker bWorker)
    //    //{
    //    //    int items_count = items.Count();
    //    //    int count = 0;

    //    //    foreach (var item in items)
    //    //    {
    //    //        DisplayItem(rType, item, excelApp.ActiveCell);
    //    //        excelApp.ActiveCell.Offset[1, 0].Select();

    //    //        if (bWorker != null) bWorker.ReportProgress(++count * 100 / items_count);
    //    //    }
    //    //}

    //    //private void DisplayItem(ReportType rType, ReportLineViewModel line, Excel.Range cell)
    //    //{
    //    //    if (rType == ReportType.GlobalSales)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.DateSpanString;

    //    //        PrintMoney(cell, 0, 1, line.Amount);

    //    //        cell.Offset[0, 2].Value = line.Clients;

    //    //        PrintMoney(cell, 0, 3, line.SalesByClient);
    //    //    }
    //    //    else if (rType == ReportType.SalesByItem)
    //    //    {                
    //    //        cell.Offset[0, 0].Value = line.Product.Name;

    //    //        cell.Offset[0, 1].Value = Math.Round(line.Quantity, 2) + " " + line.UnitMeasure.Caption;

    //    //        PrintMoney(cell, 0, 2, line.Amount);

    //    //        PrintMoney(cell, 0, 3, line.Cost);

    //    //        cell.Offset[0, 4].Value = line.CostToPriceRatio;
    //    //        cell.Offset[0, 4].Style = "Percent";

    //    //        PrintMoney(cell, 0, 5, line.Profit);
    //    //    }
    //    //    else if (rType == ReportType.SalesByCategory)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.CategoryName;

    //    //        PrintMoney(cell, 0, 1, line.Amount);

    //    //        PrintMoney(cell, 0, 2, line.Cost);

    //    //        cell.Offset[0, 3].Value = line.CostToPriceRatio;
    //    //        cell.Offset[0, 3].Style = "Percent";

    //    //        PrintMoney(cell, 0, 4, line.Profit);
    //    //    }
    //    //    else if (rType == ReportType.SalesPerson)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.SalesPersonName;

    //    //        PrintMoney(cell, 0, 1, line.Amount);

    //    //        cell.Offset[0, 2].Value = line.Clients;

    //    //        PrintMoney(cell, 0, 3, line.SalesByClient);
    //    //    }
    //    //    else if (rType == ReportType.DayOfWeekSalesAverage)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.DiaEnEspanol.ToString();

    //    //        cell.Offset[0, 1].Value = line.Amount;
    //    //        cell.Offset[0, 1].Style = "Currency";

    //    //        cell.Offset[0, 2].Value = line.Clients;

    //    //        PrintMoney(cell, 0, 3, line.SalesByClient);                
    //    //    }
    //    //    else if (rType == ReportType.ProductClasification)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.Product.Name;

    //    //        cell.Offset[0, 1].NumberFormat = "0.00%";
    //    //        cell.Offset[0, 1].Value = line.Preference;
    //    //        //cell.Offset[0, 1].Style = "Percent";

    //    //        cell.Offset[0, 2].Value = line.ProfitMargin;
    //    //        cell.Offset[0, 2].Style = "Percent";

    //    //        cell.Offset[0, 3].Value = line.ProductClass.ToString();
    //    //    }
    //    //    else if (rType == ReportType.WIPByItem)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.Product.Name;

    //    //        cell.Offset[0, 1].Value = Math.Round(line.Quantity, 2) + " " + line.UnitMeasure.Caption;
    //    //    }
    //    //    else if (rType == ReportType.CostByItem)
    //    //    {
    //    //        cell.Offset[0, 0].Value = line.Product.Name;

    //    //        PrintQuantity(cell, 0, 1, line.BasicQuantity, line.UnitMeasure);
    //    //        PrintQuantity(cell, 0, 2, line.Quantity, line.UnitMeasure);

    //    //        PrintMoney(cell, 0, 3, line.BasicCost);
    //    //        PrintMoney(cell, 0, 4, line.Cost);
    //    //    }
    //    //    else if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
    //    //        rType == ReportType.CostProjectionsByItem)
    //    //    {
    //    //        //product column
    //    //        cell.Offset[0, 0].Value = line.Product.Name;

    //    //        for (int i = 0; i < 7; i++)
    //    //        {
    //    //            int pivot_index = i * 4;

    //    //            PrintQuantity(cell, 0, 1 + pivot_index, line.MinQuantities[i], line.MinUnitOfMeasures[i]);

    //    //            PrintQuantity(cell, 0, 2 + pivot_index, line.MaxQuantities[i], line.MaxUnitOfMeasures[i]);

    //    //            PrintQuantity(cell, 0, 3 + pivot_index, line.AveQuantities[i], line.AverageUnitOfMeasures[i]);

    //    //            PrintQuantity(cell, 0, 4 + pivot_index, line.ProjQuantities[i], line.ProjUnitOfMeasures[i]);
    //    //        }
    //    //    }
    //    //}

    //    //private void PrintMoney(Excel.Range cell, int row_offsett, int column_offset, decimal value)
    //    //{
    //    //    cell.Offset[row_offsett, column_offset].Value = value;
    //    //    cell.Offset[row_offsett, column_offset].Style = "Currency";            
    //    //}
        
    //    //private static void PrintQuantity(Excel.Range cell, int row_offsett, int column_offset,
    //    //    double qtty, UnitMeasure um)
    //    //{
    //    //    cell.Offset[row_offsett, column_offset].Value = Math.Round(qtty, 2) + (um == null ? string.Empty : " " + um.Caption);
    //    //}

    //    //private static void AutofitAndMakeVisible(ReportType rType, Excel.Application excelApp)
    //    //{
    //    //    int columnCount = 0;

    //    //    if (rType == ReportType.GlobalSales || rType == ReportType.SalesPerson ||
    //    //        rType == ReportType.DayOfWeekSalesAverage || rType == ReportType.ProductClasification)
    //    //    {
    //    //        columnCount = 4;
    //    //    }
    //    //    else if (rType == ReportType.SalesByItem)
    //    //    {
    //    //        columnCount = 6;
    //    //    }
    //    //    else if (rType == ReportType.SalesByCategory || rType == ReportType.CostByItem)
    //    //    {
    //    //        columnCount = 5;
    //    //    }
    //    //    else if (rType == ReportType.WIPByItem)
    //    //    {
    //    //        columnCount = 2;
    //    //    }
    //    //    else if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
    //    //        rType == ReportType.CostProjectionsByItem) columnCount = 29;

    //    //    //autofit columns
    //    //    for (int i = 1; i <= columnCount; i++)
    //    //    {
    //    //        excelApp.Columns[i].AutoFit();
    //    //    }

    //    //    excelApp.Visible = true;
    //    //}

    //    //#endregion

    //    //#region Create and Export to Excel

    //    //public void CreateAndExportReportToExcel(CustomizeReportOptions cro, BackgroundWorker bWorker)
    //    //{
    //    //    var reportGenerator = ServiceContainer.GetService<IExcelExporter>();

    //    //    ReportLineViewModel[] lines_toshow = null;

    //    //    if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
    //    //        rType == ReportType.CostProjectionsByItem)
    //    //    {
    //    //        lines_toshow = reportGenerator.GenerateProjectionsReport(rType, cro);
    //    //    }
    //    //    //else if (rType == ReportType.GlobalSales)
    //    //    //{
    //    //    //    lines_toshow = reportGenerator.GenerateGlobalSalesData(cro);
    //    //    //}

    //    //    ExportToExcel(rType, lines_toshow, bWorker);
    //    //}

    //    //#endregion        

    //    //public void ExportDayAveragesReport(IEnumerable<ReportItemViewModel> itemsList, BackgroundWorker bWorker)
    //    //{
    //    //    Action<Excel.Range> displayHeader = (cell) =>
    //    //    {
    //    //        cell.Offset[0, 0].Value = "Producto";
    //    //        cell.Offset[0, 0].Font.Bold = "True";

    //    //        cell.Offset[0, 1].Value = "Lunes";
    //    //        cell.Offset[0, 1].Font.Bold = "True";

    //    //        cell.Offset[0, 2].Value = "Martes";
    //    //        cell.Offset[0, 2].Font.Bold = "True";

    //    //        cell.Offset[0, 3].Value = "Miércoles";
    //    //        cell.Offset[0, 3].Font.Bold = "True";

    //    //        cell.Offset[0, 4].Value = "Jueves";
    //    //        cell.Offset[0, 4].Font.Bold = "True";

    //    //        cell.Offset[0, 5].Value = "Viernes";
    //    //        cell.Offset[0, 5].Font.Bold = "True";

    //    //        cell.Offset[0, 6].Value = "Sábado";
    //    //        cell.Offset[0, 6].Font.Bold = "True";

    //    //        cell.Offset[0, 7].Value = "Domingo";
    //    //        cell.Offset[0, 7].Font.Bold = "True";

    //    //        cell.Offset[0, 8].Value = "Lunes - Jueves";
    //    //        cell.Offset[0, 8].Font.Bold = "True";

    //    //        cell.Offset[0, 9].Value = "Viernes - Domingo";
    //    //        cell.Offset[0, 9].Font.Bold = "True";
    //    //    };

    //    //    Action<ReportItemViewModel, Excel.Range> displayItem = (lineitem, cell) =>
    //    //    {
    //    //        cell.Offset[0, 0].Value = lineitem.Product.Name;

    //    //        cell.Offset[0, 1].Value = lineitem.Quantities[1];

    //    //        cell.Offset[0, 2].Value = lineitem.Quantities[2];

    //    //        cell.Offset[0, 3].Value = lineitem.Quantities[3];

    //    //        cell.Offset[0, 4].Value = lineitem.Quantities[4];

    //    //        cell.Offset[0, 5].Value = lineitem.Quantities[5];

    //    //        cell.Offset[0, 6].Value = lineitem.Quantities[6];

    //    //        cell.Offset[0, 7].Value = lineitem.Quantities[0];

    //    //        cell.Offset[0, 8].Value = lineitem.MondayToThursday;

    //    //        cell.Offset[0, 9].Value = lineitem.FridayToSunday;
    //    //    };

    //    //    var sorted = from item in itemsList
    //    //                 orderby item.Product.Name
    //    //                 select item;

    //    //    ExportToExcel<ReportItemViewModel>(sorted, displayHeader, displayItem, 10, bWorker);
    //    //}

    //    //public void Export101ReportInfo(IEnumerable<LineOfWork> linesOfWork, BackgroundWorker bWorker)
    //    //{
    //    //    Action<Excel.Range> displayHeader = (cell) =>
    //    //    {
    //    //        cell.Offset[0, 0].Value = "# Factura";
    //    //        cell.Offset[0, 0].Font.Bold = "True";

    //    //        cell.Offset[0, 1].Value = "Inicio";
    //    //        cell.Offset[0, 1].Font.Bold = "True";

    //    //        cell.Offset[0, 2].Value = "Recibo";
    //    //        cell.Offset[0, 2].Font.Bold = "True";

    //    //        cell.Offset[0, 3].Value = "Factura";
    //    //        cell.Offset[0, 3].Font.Bold = "True";

    //    //        cell.Offset[0, 4].Value = "Servicio (mins)";
    //    //        cell.Offset[0, 4].Font.Bold = "True";

    //    //        cell.Offset[0, 5].Value = "Importe";
    //    //        cell.Offset[0, 5].Font.Bold = "True";
    //    //    };

    //    //    Action<LineOfWork, Excel.Range> displayItem = (low, cell) =>
    //    //    {
    //    //        cell.Offset[0, 0].Value = low.FacturaNumber;

    //    //        cell.Offset[0, 1].Value2 = string.Format("'{0:d/MMM h:mm tt}", low.StartDate);

    //    //        if (low.PrintTicketDate.HasValue) 
    //    //        {
    //    //            DateTime pd = low.PrintTicketDate.Value;
    //    //            cell.Offset[0, 2].Value = pd.Year == 1 ? "-" : string.Format("'{0:d/MMM h:mm tt}", pd);
    //    //        }

    //    //        if (low.FacturaDate.HasValue)
    //    //        {
    //    //            DateTime fd = low.FacturaDate.Value;
    //    //            cell.Offset[0, 3].Value = fd.Year == 1 ? "-" : string.Format("'{0:d/MMM h:mm tt}", fd);
    //    //        }

    //    //        cell.Offset[0, 4].Value = low.ServiceTime;

    //    //        cell.Offset[0, 5].Value = low.Total;
    //    //        cell.Offset[0, 5].Style = "Currency";
    //    //    };

    //    //    //var sorted = from item in itemsList
    //    //    //             orderby item.Product.Name
    //    //    //             select item;

    //    //    ExportToExcel<LineOfWork>(ReportType.IncomeVsExpense, linesOfWork, bWorker);
    //    //}
    //}
}
