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

namespace MiPaladar.Services
{
    public class SalesByCategoryReport : ReportBase, ISalesByCategoryRS, ISalesByCategoryEE
    {
        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, false);
        }

        protected override string ReportName
        {
            get { return "Ventas por categoría"; }
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return true;
        }

        protected override List<ReportLineViewModel> Transform(List<ReportLineViewModel> filtered, CustomizeReportOptions cro)
        {
            var groupsByCategory = from item in filtered
                                   orderby item.CategoryName
                                   group item by item.Category;

            List<ReportLineViewModel> result = new List<ReportLineViewModel>();

            foreach (var group in groupsByCategory)
            {
                ReportLineViewModel line = new ReportLineViewModel();

                MakeSum(group, line);

                if (group.Key == null)
                {
                    //UNCATEGORIZED
                    line.CategoryName = "Sin Categoría";
                    //noCategoryLine.IsRootCategory = true;                    

                    line.ChildrenProductsTotalSale = line.Amount;

                    result.Add(line);
                }
                else
                {
                    AddToCategoryBranch(line, group.Key, result, false);

                    //lineitems_showing.Add(lic);
                }                
            }

            return result;
        }

        private void AddToCategoryBranch(ReportLineViewModel lic, Category targetCategory, 
            List<ReportLineViewModel> tempList, bool isRecursiveCall)
        {
            ReportLineViewModel targetLine = tempList.FirstOrDefault(x => x.Category == targetCategory);

            //Category targetCategory = appvm.CategoriesOC.Single(x => x.Id == targetCategoryId);

            if (targetLine == null)
            {
                targetLine = new ReportLineViewModel();
                targetLine.Category = targetCategory;

                var invSvc = ServiceContainer.GetService<IInventoryService>();
                targetLine.CategoryName = invSvc.GetFullCategoryName(targetCategory);

                tempList.Add(targetLine);
            }

            targetLine.Amount += lic.Amount;
            if (!isRecursiveCall) targetLine.ChildrenProductsTotalSale += lic.Amount;
            targetLine.Cost += lic.Cost;

            if (targetCategory.ParentCategory != null)
            {
                AddToCategoryBranch(lic, targetCategory.ParentCategory, tempList, true);
            }
            //else targetLine.IsRootCategory = true;
        }

        private void MakeSum(IEnumerable<ReportLineViewModel> group, ReportLineViewModel lic)
        {
            lic.Amount = group.Sum(x => x.Amount);
            lic.Cost = group.Sum(x => x.Cost);
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.CategoryName).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Categoría");
            PrintHeader(cell, 0, 1, "Ventas");
            PrintHeader(cell, 0, 2, "Costo");            
            PrintHeader(cell, 0, 4, "Ganancia");
            PrintHeader(cell, 0, 3, "Costo %");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.CategoryName;

            PrintMoney(cell, 0, 1, line.Amount);

            PrintMoney(cell, 0, 2, line.Cost);            

            PrintMoney(cell, 0, 4, line.Profit);

            cell.Offset[0, 3].Value = line.CostToPriceRatio;
            cell.Offset[0, 3].Style = "Percent";
        }

        protected override int GetColumnsCount()
        {
            return 5;
        }

        #endregion
    }
}
