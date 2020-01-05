using MiPaladar.Classes;
using MiPaladar.Entities;
using MiPaladar.ViewModels;
using MiPaladar.Repository;

using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPaladar.Services
{
    public abstract class ReportBase
    {
        public List<ReportLineViewModel> GenerateReportData(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            //Filter and Transform
            return Load(Transform(Filter(Extract(unitOfWork, cro), cro), cro));
        }

        protected abstract List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro);

        #region Filter

        private List<ReportLineViewModel> Filter(List<ReportLineViewModel> unfiltered, CustomizeReportOptions cro)
        {
            //filter data
            List<ReportLineViewModel> lines_filtered = new List<ReportLineViewModel>();

            foreach (var item in unfiltered)
            {
                if (!OfflineFilter(item)) continue;
                if (!cro.AllGood && !CheckCustomizeReportOptions(item, cro)) continue;

                lines_filtered.Add(item);
            }

            return lines_filtered;
        }

        protected abstract bool OfflineFilter(ReportLineViewModel li);

        private bool CheckCustomizeReportOptions(ReportLineViewModel li, CustomizeReportOptions cro)
        {
            //PRODUCT TYPES
            //if (rType == ReportType.CostByItem || rType == ReportType.CostProjectionsByItem)
            //{
            //    if (li.ProductType != ProductType.RawMaterials && li.ProductType != ProductType.CompraVenta) return false;
            //}
            //else if (rType == ReportType.SalesProjectionsByItem)
            //{
            //    if (li.ProductType != ProductType.FinishedGoods && li.ProductType != ProductType.CompraVenta) return false;
            //}
            //else if (rType == ReportType.WIPByItem || rType == ReportType.WIPProjectionsByItem)
            //{
            //    if (li.ProductType != ProductType.WorkInProcess) return false;
            //}

            //CUSTOMIZE REPORT OPTIONS
            //SHIFTS
            if (!cro.ShiftsAllGood)
            {
                if (li.ShiftId == 0)
                {
                    if (!cro.AdmitsNoShift) return false;
                }
                else
                {
                    if (cro.SelectedShifts.FirstOrDefault(x => x.Id == li.ShiftId) == null) return false;
                }
            }

            //SALESPERSON
            if (!cro.SalesPersonsAllGood)
            {
                if (li.SalesPerson == null)
                {
                    if (!cro.AdmitsNoSalesPerson) return false;
                }
                else
                {
                    if (cro.SelectedSalesPersons.FirstOrDefault(x => x.Id == li.SalesPerson.Id) == null) return false;
                }
            }

            //CATEGORIES
            if (!IgnoreCategories && !cro.CategoriesAllGood)
            {
                if (li.Category == null)
                {
                    if (!cro.AdmitsNoCategory) return false;
                }
                else
                {
                    if (cro.SelectedCategories.FirstOrDefault(x => x.Id == li.Category.Id) == null) return false;
                }
            }

            //TAGS
            if (!IgnoreTags && !cro.TagsAllGood)
            {
                if (li.TagIds.Count == 0)
                {
                    if (!cro.AdmitsNoTag) return false;
                }
                else
                {
                    if (cro.SelectedTags.Select(x => x.Id).Intersect(li.TagIds).Count() == 0) 
                        return false;
                }
            }

            return true;
        }        

        #endregion        

        protected abstract List<ReportLineViewModel> Transform(List<ReportLineViewModel> filtered, CustomizeReportOptions cro);

        protected abstract List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed);

        public bool IgnoreCategories { get; set; }

        public bool IgnoreTags { get; set; }

        #region Excel

        public void ExportToExcel(IEnumerable<ReportLineViewModel> items, BackgroundWorker bWorker)
        {
            var excelApp = InitExcel();

            //DisplayReportName(excelApp);

            ChangeLine(excelApp);

            //DisplayCurrentDate(excelApp);
            
            ChangeLine(excelApp);

            DisplayHeader(excelApp);
            
            ChangeLine(excelApp);

            DisplayGridData(items, excelApp, bWorker);

            AutofitAndMakeVisible(excelApp);

            excelApp.Range["A1"].Select();

            DisplayReportName(excelApp);
            ChangeLine(excelApp);
            DisplayCurrentDate(excelApp);
        }

        private static void ChangeLine(Excel.Application excelApp)
        {
            excelApp.ActiveCell.Offset[1, 0].Select();
        }        

        #region abstract members

        protected abstract string ReportName { get;}         

        protected abstract void DisplayHeader(Excel.Application excelApp);

        protected abstract void DisplayItem(ReportLineViewModel line, Excel.Range cell);

        protected abstract int GetColumnsCount();

        #endregion

        #region Helpers

        private Excel.Application InitExcel()
        {
            var excelApp = new Excel.Application();

            // Add a new Excel workbook.
            excelApp.Workbooks.Add();

            excelApp.Range["A1"].Select();

            return excelApp;
        }

        //{
        //var cell = excelApp.ActiveCell;

        //if (rType == ReportType.GlobalSales)
        //{
        //    PrintHeader(cell, 0, 0, "Intervalo");
        //    PrintHeader(cell, 0, 1, "Ventas");
        //    PrintHeader(cell, 0, 2, "Clientes");
        //    PrintHeader(cell, 0, 3, "$/Cliente");
        //}
        //else
        //    if (rType == ReportType.SalesByItem)
        //{
        //    PrintHeader(cell, 0, 0, "Producto");
        //    PrintHeader(cell, 0, 1, "Cantidad");
        //    PrintHeader(cell, 0, 2, "Ventas");
        //    PrintHeader(cell, 0, 3, "Costo");
        //    PrintHeader(cell, 0, 4, "Costo %");
        //    PrintHeader(cell, 0, 5, "Ganancia");
        //}
        //else 
        //        if (rType == ReportType.SalesByCategory)
        //{
        //    PrintHeader(cell, 0, 0, "Categoría");
        //    PrintHeader(cell, 0, 1, "Ventas");
        //    PrintHeader(cell, 0, 2, "Costo");
        //    PrintHeader(cell, 0, 3, "Costo %");
        //    PrintHeader(cell, 0, 4, "Ganancia");
        //}
        //else 
        //if (rType == ReportType.SalesPerson)
        //{
        //    PrintHeader(cell, 0, 0, "Dependiente");
        //    PrintHeader(cell, 0, 1, "Ventas");
        //    PrintHeader(cell, 0, 2, "Clientes");
        //    PrintHeader(cell, 0, 3, "$/Cliente");
        //}
        //else 
        //    if (rType == ReportType.DayOfWeekSalesAverage)
        //{
        //    PrintHeader(cell, 0, 0, "Día");
        //    PrintHeader(cell, 0, 1, "Ventas");
        //    PrintHeader(cell, 0, 2, "Clientes");
        //    PrintHeader(cell, 0, 3, "$/Cliente");
        //}
        //else 
        //        if (rType == ReportType.ProductClasification)
        //{
        //    PrintHeader(cell, 0, 0, "Producto");
        //    PrintHeader(cell, 0, 1, "% Preferencia");
        //    PrintHeader(cell, 0, 2, "% Contribución");
        //    PrintHeader(cell, 0, 3, "Clase");
        //}
        //else 
        //if (rType == ReportType.WIPByItem)
        //{
        //    PrintHeader(cell, 0, 0, "Producto");
        //    PrintHeader(cell, 0, 1, "Cantidad");
        //}
        //else 
        //if (rType == ReportType.CostByItem)
        //{
        //    PrintHeader(cell, 0, 0, "Producto");
        //    PrintHeader(cell, 0, 1, "Cantidad Estándar");
        //    PrintHeader(cell, 0, 2, "Cantidad Real");
        //    PrintHeader(cell, 0, 3, "Costo Estándar");
        //    PrintHeader(cell, 0, 4, "Costo Real");
        //}
        //else 
        //if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
        //    rType == ReportType.CostProjectionsByItem)
        //{
        //    DateTime today = DateTime.Today;

        //    //project sales for seven days
        //    for (int i = 0; i < 7; i++)
        //    {
        //        Excel.Range fromCell = cell.Offset[0, 1 + i * 4];
        //        Excel.Range toCell = fromCell.Offset[0, 3];

        //        PrintHeader(fromCell, 0, 0, today.AddDays(i).ToString("d/MMM"));
        //        //fromCell.Value = today.AddDays(i).ToString("d/MMM");
        //        //fromCell.Font.Bold = "True";

        //        //merge header cells
        //        excelApp.Range[fromCell, toCell].Merge();
        //    }

        //    //change line
        //    cell.Offset[1, 0].Activate();
        //    cell = excelApp.ActiveCell;

        //    PrintHeader(cell, 0, 0, "Producto");
        //    //cell.Offset[0, 0].Value = "Producto";
        //    //cell.Offset[0, 0].Font.Bold = "True";

        //    //project sales for seven days
        //    for (int i = 0; i < 7; i++)
        //    {
        //        Excel.Range fromCell = cell.Offset[0, 1 + i * 4];

        //        fromCell.Offset[0, 0].Value = "Min";
        //        fromCell.Offset[0, 1].Value = "Max";
        //        fromCell.Offset[0, 2].Value = "Prom";
        //        fromCell.Offset[0, 3].Value = "Proy";
        //    }
        //}
        //}

        private void DisplayReportName(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, ReportName);            
        }

        private void DisplayCurrentDate(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, DateTime.Now.ToString("f"));
        }

        protected virtual void DisplayGridData(IEnumerable<ReportLineViewModel> items,
            Excel.Application excelApp, BackgroundWorker bWorker)
        {
            int items_count = items.Count();
            int count = 0;

            foreach (var item in items)
            {
                DisplayItem(item, excelApp.ActiveCell);
                excelApp.ActiveCell.Offset[1, 0].Select();

                if (bWorker != null) bWorker.ReportProgress(++count * 100 / items_count);
            }
        }

        private void AutofitAndMakeVisible(Excel.Application excelApp)
        {
            int columnCount = GetColumnsCount();

            //if (rType == ReportType.DayOfWeekSalesAverage || rType == ReportType.ProductClasification)
            //{
            //    columnCount = 4;
            //}
            //else if (rType == ReportType.SalesByItem)
            //{
            //    columnCount = 6;
            //}
            //else if (rType == ReportType.SalesByCategory || rType == ReportType.CostByItem)
            //{
            //    columnCount = 5;
            //}
            //else if (rType == ReportType.WIPByItem)
            //{
            //    columnCount = 2;
            //}
            //else if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
            //    rType == ReportType.CostProjectionsByItem) columnCount = 29;

            //autofit columns
            for (int i = 1; i <= columnCount; i++)
            {
                excelApp.Columns[i].AutoFit();
            }

            excelApp.Visible = true;
        }


        //{
        //if (rType == ReportType.GlobalSales)
        //{
        //    cell.Offset[0, 0].Value = line.DateSpanString;

        //    PrintMoney(cell, 0, 1, line.Amount);

        //    cell.Offset[0, 2].Value = line.Clients;

        //    PrintMoney(cell, 0, 3, line.SalesByClient);
        //}
        //else 
        //    if (rType == ReportType.SalesByItem)
        //{
        //    cell.Offset[0, 0].Value = line.Product.Name;

        //    cell.Offset[0, 1].Value = Math.Round(line.Quantity, 2) + " " + line.UnitMeasure.Caption;

        //    PrintMoney(cell, 0, 2, line.Amount);

        //    PrintMoney(cell, 0, 3, line.Cost);

        //    cell.Offset[0, 4].Value = line.CostToPriceRatio;
        //    cell.Offset[0, 4].Style = "Percent";

        //    PrintMoney(cell, 0, 5, line.Profit);
        //}
        //else 
        //if (rType == ReportType.SalesByCategory)
        //{
        //    cell.Offset[0, 0].Value = line.CategoryName;

        //    PrintMoney(cell, 0, 1, line.Amount);

        //    PrintMoney(cell, 0, 2, line.Cost);

        //    cell.Offset[0, 3].Value = line.CostToPriceRatio;
        //    cell.Offset[0, 3].Style = "Percent";

        //    PrintMoney(cell, 0, 4, line.Profit);
        //}
        //else
        //if (rType == ReportType.SalesPerson)
        //{
        //    cell.Offset[0, 0].Value = line.SalesPersonName;

        //    PrintMoney(cell, 0, 1, line.Amount);

        //    cell.Offset[0, 2].Value = line.Clients;

        //    PrintMoney(cell, 0, 3, line.SalesByClient);
        //}
        //else 
        //    if (rType == ReportType.DayOfWeekSalesAverage)
        //{
        //    cell.Offset[0, 0].Value = line.DiaEnEspanol.ToString();

        //    cell.Offset[0, 1].Value = line.Amount;
        //    cell.Offset[0, 1].Style = "Currency";

        //    cell.Offset[0, 2].Value = line.Clients;

        //    PrintMoney(cell, 0, 3, line.SalesByClient);
        //}
        //else
        //        if (rType == ReportType.ProductClasification)
        //{
        //    cell.Offset[0, 0].Value = line.Product.Name;

        //    cell.Offset[0, 1].NumberFormat = "0.00%";
        //    cell.Offset[0, 1].Value = line.Preference;
        //    //cell.Offset[0, 1].Style = "Percent";

        //    cell.Offset[0, 2].Value = line.ProfitMargin;
        //    cell.Offset[0, 2].Style = "Percent";

        //    cell.Offset[0, 3].Value = line.ProductClass.ToString();
        //}
        //else 
        //if (rType == ReportType.WIPByItem)
        //{
        //    cell.Offset[0, 0].Value = line.Product.Name;

        //    cell.Offset[0, 1].Value = Math.Round(line.Quantity, 2) + " " + line.UnitMeasure.Caption;
        //}
        //else 
        //    if (rType == ReportType.CostByItem)
        //{
        //    cell.Offset[0, 0].Value = line.Product.Name;

        //    PrintQuantity(cell, 0, 1, line.BasicQuantity, line.UnitMeasure);
        //    PrintQuantity(cell, 0, 2, line.Quantity, line.UnitMeasure);

        //    PrintMoney(cell, 0, 3, line.BasicCost);
        //    PrintMoney(cell, 0, 4, line.Cost);
        //}
        //else 
        //if (rType == ReportType.SalesProjectionsByItem || rType == ReportType.WIPProjectionsByItem ||
        //    rType == ReportType.CostProjectionsByItem)
        //{
        //    //product column
        //    cell.Offset[0, 0].Value = line.Product.Name;

        //    for (int i = 0; i < 7; i++)
        //    {
        //        int pivot_index = i * 4;

        //        PrintQuantity(cell, 0, 1 + pivot_index, line.MinQuantities[i], line.MinUnitOfMeasures[i]);

        //        PrintQuantity(cell, 0, 2 + pivot_index, line.MaxQuantities[i], line.MaxUnitOfMeasures[i]);

        //        PrintQuantity(cell, 0, 3 + pivot_index, line.AveQuantities[i], line.AverageUnitOfMeasures[i]);

        //        PrintQuantity(cell, 0, 4 + pivot_index, line.ProjQuantities[i], line.ProjUnitOfMeasures[i]);
        //    }
        //}
        //}

        #endregion

        #region Print Methods

        protected void PrintHeader(Excel.Range cell, int row_offset, int column_offset, string value)
        {
            cell.Offset[row_offset, column_offset].Value = value;
            cell.Offset[row_offset, column_offset].Font.Bold = "True";
        }

        protected void PrintMoney(Excel.Range cell, int row_offsett, int column_offset, decimal value)
        {
            cell.Offset[row_offsett, column_offset].Value = value;
            cell.Offset[row_offsett, column_offset].Style = "Currency";
        }

        protected void PrintQuantity(Excel.Range cell, int row_offsett, int column_offset,
            double qtty, UnitMeasure um)
        {
            cell.Offset[row_offsett, column_offset].Value = Math.Round(qtty, 2) + (um == null ? string.Empty : " " + um.Caption);
        }

        protected void PrintPercent(Excel.Range cell, int row_offsett, int column_offset, double qtty)
        {
            cell.Offset[row_offsett, column_offset].Value = qtty;
            cell.Offset[row_offsett, column_offset].Style = "Percent";
        }

        #endregion

        #endregion
    }
}
