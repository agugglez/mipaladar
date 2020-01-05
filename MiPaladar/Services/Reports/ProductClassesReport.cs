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
    public class ProductClassesReport : ReportBase, IProductClassesRS, IProductClassesEE
    {
        protected override string ReportName
        {
            get { return "Productos por clases"; }
        }

        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, false);
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return li.Product.ProductType == ProductType.FinishedGoods || li.Product.ProductType == ProductType.CompraVenta;
        }

        protected override List<ReportLineViewModel> Transform(List<ReportLineViewModel> filtered, CustomizeReportOptions cro)
        {
            var groupsByProduct = from item in filtered
                                  group item by item.Product;

            List<ReportLineViewModel> transformed = new List<ReportLineViewModel>();

            double totalQtty = filtered.Sum(x => x.Quantity);

            foreach (var group in groupsByProduct)
            {
                ReportLineViewModel lic = new ReportLineViewModel();

                lic.Product = group.Key;

                lic.Preference = group.Sum(x => x.Quantity) / totalQtty;

                MakeSum(group, lic);

                transformed.Add(lic);
                //salesByItemLines.Add(sbiLine);
            }

            double avePref = 0; decimal aveProfitMargin = 0;
            if (transformed.Count() == 0)
            {
                avePref = 0; aveProfitMargin = 0;
            }
            else
            {
                avePref = transformed.Average(x => x.Preference);
                aveProfitMargin = transformed.Average(x => x.ProfitMargin);
            }                      

            foreach (var item in transformed)
            {
                if (item.Preference >= avePref && item.ProfitMargin >= aveProfitMargin)
                {
                    item.ProductClass = ProductClass.A;
                }
                else if (item.Preference >= avePref && item.ProfitMargin < aveProfitMargin)
                {
                    item.ProductClass = ProductClass.B;
                }
                else if (item.Preference < avePref && item.ProfitMargin >= aveProfitMargin)
                {
                    item.ProductClass = ProductClass.C;
                }
                else //if (item.Preference < avePref && item.ProfitMargin < aveProfitMargin)
                {
                    item.ProductClass = ProductClass.D;
                }
            }

            return transformed;
        }

        private void MakeSum(IEnumerable<ReportLineViewModel> group, ReportLineViewModel lic)
        {                        
            lic.Quantity = group.Sum(x => x.Quantity);
            lic.UnitMeasure = group.First().UnitMeasure;

            lic.Cost = group.Sum(x => x.Cost);
            lic.Amount = group.Sum(x => x.Amount);
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.ProductClass).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Producto");
            PrintHeader(cell, 0, 1, "% Preferencia");
            PrintHeader(cell, 0, 2, "% Contribución");
            PrintHeader(cell, 0, 3, "Clase");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.Product.Name;

            cell.Offset[0, 1].NumberFormat = "0.00%";
            cell.Offset[0, 1].Value = line.Preference;
            //cell.Offset[0, 1].Style = "Percent";

            cell.Offset[0, 2].Value = line.ProfitMargin;
            cell.Offset[0, 2].Style = "Percent";

            cell.Offset[0, 3].Value = line.ProductClass.ToString();
        }

        protected override int GetColumnsCount()
        {
            return 4;
        }

        #endregion
    }
}
