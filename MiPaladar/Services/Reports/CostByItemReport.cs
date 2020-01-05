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
    public class CostByItemReport : ReportBase, ICostByItemRS, ICostByItemEE
    {
        protected override string ReportName
        {
            get { return "Costos por producto"; }
        }

        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, true);
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return li.Product.ProductType == ProductType.RawMaterials || 
                li.Product.ProductType == ProductType.CompraVenta;
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
            //qtty in base unit of measure
            double qttySum = group.Sum(x => x.Quantity * x.UnitMeasure.ToBaseConversion);

            //UMFamily umFamily = appvm.ProductsOC.Single(x => x.Id == lic.Product.Id).UMFamily;
            var invSVC = ServiceContainer.GetService<IInventoryService>();

            //quantity
            UnitMeasure bestUM = invSVC.GetBestUM(lic.Product.UMFamily, qttySum);

            lic.UnitMeasure = bestUM;
            lic.Quantity = qttySum / bestUM.ToBaseConversion;

            //basic quantity
            double ep = lic.Product.EdiblePart;
            UnitMeasure realUM = invSVC.GetBestUM(lic.Product.UMFamily, qttySum / ep);

            lic.RealUnitMeasure = realUM;
            lic.RealQuantity = qttySum / ep / realUM.ToBaseConversion;

            //cost
            lic.Cost = group.Sum(x => x.Cost);
            lic.RealCost = lic.Cost / (decimal)ep;
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => -x.Cost).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Producto");
            PrintHeader(cell, 0, 1, "Cantidad Estándar");
            PrintHeader(cell, 0, 2, "Cantidad Real");
            PrintHeader(cell, 0, 3, "Costo Estándar");
            PrintHeader(cell, 0, 4, "Costo Real");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.Product.Name;

            PrintQuantity(cell, 0, 1, line.Quantity, line.UnitMeasure);
            PrintQuantity(cell, 0, 2, line.RealQuantity, line.RealUnitMeasure);

            PrintMoney(cell, 0, 3, line.Cost);
            PrintMoney(cell, 0, 4, line.RealCost);
        }

        protected override int GetColumnsCount()
        {
            return 5;
        }

        #endregion
    }
}
