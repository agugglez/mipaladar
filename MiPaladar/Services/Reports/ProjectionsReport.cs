using MiPaladar.Classes;
using MiPaladar.Entities;
using MiPaladar.MVVM;
using MiPaladar.ViewModels;
using MiPaladar.Repository;

using Excel = Microsoft.Office.Interop.Excel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MiPaladar.Services
{
    public abstract class ProjectionsReport : ReportBase, IProjectionsEE
    {
        protected override List<ReportLineViewModel> Transform(List<ReportLineViewModel> filtered, CustomizeReportOptions cro)
        {
            double projPct = cro.ProjectionPercent;

            List<ReportLineViewModel> transformed = new List<ReportLineViewModel>();

            var queryProductGroups = from opi in filtered
                                     group opi by opi.Product;

            foreach (var productGroup in queryProductGroups)
            {
                var queryDowGroups = from line in productGroup
                                     group line by line.DayOfWeek;

                foreach (var dowGroup in queryDowGroups)
                {
                    //create a line for each day of week
                    ReportLineViewModel rivm = new ReportLineViewModel();

                    rivm.Product = productGroup.Key;

                    //set date
                    DayOfWeek dow = dowGroup.Key;

                    int finalIndex = GetFinalIndex(dow);
                    rivm.Date = DateTime.Today.AddDays(finalIndex);

                    //group by date
                    var queryDateGroups = from line in dowGroup
                                          group line by line.Date.Date;

                    double min = double.MaxValue;
                    double max = 0;

                    double dowTotal = 0;
                    List<double> dateTotals = new List<double>();

                    foreach (var dateGroup in queryDateGroups)
                    {
                        double dateTotal = dateGroup.Sum(x => x.Quantity * x.UnitMeasure.ToBaseConversion);

                        if (dateTotal < min) min = dateTotal;
                        if (dateTotal > max) max = dateTotal;

                        dateTotals.Add(dateTotal);
                        dowTotal += dateTotal;
                    }

                    //MIN
                    rivm.MinimumQuantity = min;
                    //rivm.MinQuantities[finalIndex] = min;
                    //MAX
                    rivm.MaximumQuantity = max;
                    //rivm.MaxQuantities[finalIndex] = max;

                    //AVERAGE
                    //int numberofDays = innerGroup.Distinct(new SameDateComparer()).Count();
                    double averageInBase = dowTotal / queryDateGroups.Count();
                    rivm.AverageQuantity = averageInBase;
                    //rivm.AveQuantities[finalIndex] = averageInBase;//Math.Round(tempQuantity, isQtty ? 0 : 1);

                    //PROJECTED
                    //rivm.ProjectedQuantity = averageInBase * (1 + projPct / 100);
                    //rivm.ProjQuantities[finalIndex] = averageInBase * (1 + projPct / 100);

                    if (rivm.Product.Name.Contains("Cerv.Cristal"))
                        Console.WriteLine();
                    //CHANGE %
                    rivm.ChangePercent = CalculateChangePercent(dateTotals);

                    //add line
                    transformed.Add(rivm);
                }               

            }//outer foreach

            MakePretty(transformed);

            return transformed.OrderBy(x => x.Product.Name).ToList();
        }

        private double CalculateChangePercent(List<double> dateTotals)
        {
            double firstQtty = dateTotals.First();

            double lastQtty = dateTotals.Last();

            return (lastQtty - firstQtty) / firstQtty;//what % of lastQtty is the difference?
        }

        private static int GetFinalIndex(DayOfWeek dow)
        {
            int todayIndex = (int)DateTime.Today.DayOfWeek;

            int dowIndex = (int)dow;

            int finalIndex = dowIndex - todayIndex;

            if (todayIndex > dowIndex)
            {
                finalIndex = 7 - todayIndex + dowIndex;
            }
            return finalIndex;
        }

        private void MakePretty(List<ReportLineViewModel> transformed)
        {
            var invSVC = ServiceContainer.GetService<IInventoryService>();

            foreach (var item in transformed)
            {
                UMFamily umf = item.Product.UMFamily;
                bool isQtty = umf.Id == 1;

                int decimalPlaces = isQtty ? 0 : 2;

                double minQttyInBase = item.MinimumQuantity;
                double maxQttyInBase = item.MaximumQuantity;
                double aveQttyInBase = item.AverageQuantity;
                //double projQttyInBase = item.ProjectedQuantity;
                //double minQttyInBase = item.MinQuantities[i];
                //double maxQttyInBase = item.MaxQuantities[i];
                //double aveQttyInBase = item.AveQuantities[i];
                //double projQttyInBase = item.ProjQuantities[i];

                //if (minQttyInBase != 0)
                {
                    item.MinimumUnitMeasure = invSVC.GetBestUM(umf, minQttyInBase);
                    item.MinimumQuantity = Math.Round(minQttyInBase / item.MinimumUnitMeasure.ToBaseConversion, decimalPlaces);
                }

                //if (maxQttyInBase != 0)
                {
                    item.MaximumUnitMeasure = invSVC.GetBestUM(umf, maxQttyInBase);
                    item.MaximumQuantity = Math.Round(maxQttyInBase / item.MaximumUnitMeasure.ToBaseConversion, decimalPlaces);
                }

                //if (aveQttyInBase != 0)
                {
                    item.AverageUnitMeasure = invSVC.GetBestUM(umf, aveQttyInBase);
                    item.AverageQuantity = Math.Round(aveQttyInBase / item.AverageUnitMeasure.ToBaseConversion, decimalPlaces);
                }

                //if (projQttyInBase != 0)
                //{
                //    item.ProjectedUnitMeasure = invSVC.GetBestUM(umf, projQttyInBase);
                //    item.ProjectedQuantity = Math.Round(projQttyInBase / item.ProjectedUnitMeasure.ToBaseConversion, decimalPlaces);
                //}

                //for (int i = 0; i < 7; i++)
                //{
                //    //item.ProjectedQuantities[i] = Math.Round(projectedQtty, 1);
                //}
            }
        }

        //private static UnitMeasure GetBestUM(UMFamily umFamily, double qttyInBase)
        //{
        //    var invSVC = ServiceContainer.GetService<IInventoryService>();

        //    double resultQtty;
        //    UnitMeasure bestUM;
        //    invSVC.FitQuantity(umFamily, qttyInBase, out resultQtty, out bestUM);

        //    return bestUM;
        //}

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.Date).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            DateTime today = DateTime.Today;

            //project sales for seven days
            for (int i = 0; i < 7; i++)
            {
                Excel.Range fromCell = cell.Offset[0, 1 + i * 4];
                Excel.Range toCell = fromCell.Offset[0, 3];

                PrintHeader(fromCell, 0, 0, today.AddDays(i).ToString("d/MMM"));
                //fromCell.Value = today.AddDays(i).ToString("d/MMM");
                //fromCell.Font.Bold = "True";

                //merge header cells
                excelApp.Range[fromCell, toCell].Merge();
            }

            //change line
            cell.Offset[1, 0].Activate();

            PrintHeader(cell, 1, 0, "Producto");
            //cell.Offset[0, 0].Value = "Producto";
            //cell.Offset[0, 0].Font.Bold = "True";

            //project sales for seven days
            for (int i = 0; i < 7; i++)
            {
                Excel.Range fromCell = cell.Offset[1, 1 + i * 4];

                fromCell.Offset[0, 0].Value = "Min";
                fromCell.Offset[0, 1].Value = "Max";
                fromCell.Offset[0, 2].Value = "Prom";
                fromCell.Offset[0, 3].Value = "Pct";
            }
        }

        protected override void DisplayGridData(IEnumerable<ReportLineViewModel> items,
            Excel.Application excelApp, BackgroundWorker bWorker)
        {            
            int count = 0;

            var query = from line in items
                        group line by line.Product into g
                        orderby g.Key.Name
                        select g;

            int items_count = query.Count();

            foreach (var productGroup in query)
            {
                DisplayGroup(productGroup, excelApp.ActiveCell);

                //move to next row
                excelApp.ActiveCell.Offset[1, 0].Select();

                if (bWorker != null) bWorker.ReportProgress(++count * 100 / items_count);
            }
        }

        private void DisplayGroup(IGrouping<Product, ReportLineViewModel> productGroup, Excel.Range cell)
        {
            //int row = FindProductRow(cell.Application, line.Product);

            //var excelApp = cell.Application;
            //excelApp.Range["A" + row].Select();

            //cell = excelApp.ActiveCell;

            //product name column
            //if (cell.Value == null) cell.Value = line.Product.Name;

            cell.Value = productGroup.Key.Name;

            foreach (var line in productGroup)
            {
                int dowIndex = GetFinalIndex(line.Date.DayOfWeek);

                int pivot_index = dowIndex * 4;

                PrintQuantity(cell, 0, 1 + pivot_index, line.MinimumQuantity, line.MinimumUnitMeasure);

                PrintQuantity(cell, 0, 2 + pivot_index, line.MaximumQuantity, line.MaximumUnitMeasure);

                PrintQuantity(cell, 0, 3 + pivot_index, line.AverageQuantity, line.AverageUnitMeasure);

                PrintPercent(cell, 0, 4 + pivot_index, line.ChangePercent);
                
                //PrintQuantity(cell, 0, 4 + pivot_index, line.ProjectedQuantity, line.ProjectedUnitMeasure);
            }            
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            throw new NotImplementedException();
        }

        //private int FindProductRow(Excel.Application excelApp, Product prod)
        //{
        //    //start from A3 until you find a blank cell
        //    excelApp.Range["A3"].Select();
        //    var cell = excelApp.ActiveCell;

        //    int rowIndex = 0;

        //    while (true)
        //    {
        //        if (cell.Offset[rowIndex, 0].Value == null) break;
        //        if (cell.Offset[rowIndex, 0].Value == prod.Name) break;

        //        rowIndex++;
        //    }

        //    return rowIndex + 3;
        //}

        protected override int GetColumnsCount()
        {
            return 29;
        }

        #endregion

        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            throw new NotImplementedException();
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            throw new NotImplementedException();
        }

        
    }
}
