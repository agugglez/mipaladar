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
    public class WIPByItemReport : ReportBase, IWIPByItemRS, IWIPByItemEE
    {
        protected override string ReportName
        {
            get { return "Elaboraciones por producto"; }
        }

        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, true);
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return li.Product.ProductType == ProductType.WorkInProcess;
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
            double tempQuantity = group.Sum(x => x.Quantity * x.UnitMeasure.ToBaseConversion);

            //UMFamily umFamily = appvm.ProductsOC.Single(x => x.Id == lic.Product.Id).UMFamily;
            var invSVC = ServiceContainer.GetService<IInventoryService>();
            
            UnitMeasure bestUM =invSVC.GetBestUM(lic.Product.UMFamily, tempQuantity);

            lic.UnitMeasure = bestUM;
            lic.Quantity = tempQuantity / bestUM.ToBaseConversion;
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.Product.Name).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Producto");
            PrintHeader(cell, 0, 1, "Cantidad");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.Product.Name;

            PrintQuantity(cell, 0, 1, line.Quantity, line.UnitMeasure);
            //cell.Offset[0, 1].Value = Math.Round(line.Quantity, 2) + " " + line.UnitMeasure.Caption;
        }

        protected override int GetColumnsCount()
        {
            return 2;
        }

        #endregion
    }
}
