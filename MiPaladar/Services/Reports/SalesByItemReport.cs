using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

using MiPaladar.MVVM;
using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;
using MiPaladar.ViewModels;
using MiPaladar.Repository;

namespace MiPaladar.Services
{
    public class SalesByItemReport : ReportBase, ISalesByItemRS, ISalesByItemEE
    {
        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, false);
        }

        protected override string ReportName
        {
            get { return "Ventas por producto"; }
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return true;
        }

        protected override List<ReportLineViewModel> Transform(List<ReportLineViewModel> filtered, CustomizeReportOptions cro)
        {
            var groupsByProduct = from item in filtered
                                  group item by item.Product;

            List<ReportLineViewModel> transformed = new List<ReportLineViewModel>();

            foreach (var group in groupsByProduct)
            {
                ReportLineViewModel lic = new ReportLineViewModel();

                lic.Product = group.Key;
                lic.ProductId = group.Key.Id;

                MakeSum(group, lic);

                transformed.Add(lic);
            }

            return transformed;
        }

        private void MakeSum(IEnumerable<ReportLineViewModel> group, ReportLineViewModel lic)
        {
            lic.Quantity = group.Sum(x => x.Quantity);
            lic.Amount = group.Sum(x => x.Amount);
            lic.Cost = group.Sum(x => x.Cost);            
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => -x.Amount).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Producto");
            PrintHeader(cell, 0, 1, "Cantidad");
            PrintHeader(cell, 0, 2, "Ventas");
            PrintHeader(cell, 0, 3, "Costo");
            PrintHeader(cell, 0, 4, "Ganancia");
            PrintHeader(cell, 0, 5, "Costo %");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.Product.Name;

            PrintQuantity(cell, 0, 1, line.Quantity, null);

            PrintMoney(cell, 0, 2, line.Amount);

            PrintMoney(cell, 0, 3, line.Cost);            

            PrintMoney(cell, 0, 4, line.Profit);

            cell.Offset[0, 5].Value = line.CostToPriceRatio;
            cell.Offset[0, 5].Style = "Percent";
        }

        protected override int GetColumnsCount()
        {
            return 6;
        }

        #endregion
    }
}
