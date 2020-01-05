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
    public class SalesPersonReport : ReportBase, ISalesPersonRS, ISalesPersonEE
    {
        public SalesPersonReport()
            : base()
        {
            IgnoreCategories = true;
            IgnoreTags = true;
        }

        protected override string ReportName
        {
            get { return "Ventas por dependiente"; }
        }

        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            //get data
            var queryService = ServiceContainer.GetService<IQueryService>();

           return queryService.GetSalesData(unitOfWork, cro.FromDate, cro.ToDate);
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return true;
        }

        protected override List<ReportLineViewModel> Transform(List<ReportLineViewModel> filtered, CustomizeReportOptions cro)
        {
            List<ReportLineViewModel> result = new List<ReportLineViewModel>();

            var query = from sale in filtered
                        group sale by sale.SalesPerson;

            foreach (var salesPersonGroup in query)
            {
                ReportLineViewModel line = new ReportLineViewModel();

                if (salesPersonGroup.Key == null)
                {
                    //NO  SALESPERSON
                    line.SalesPersonName = "Sin Dependiente";
                }
                else
                {
                    line.SalesPerson = salesPersonGroup.Key;
                    line.SalesPersonName = salesPersonGroup.Key.Name;                    
                }

                MakeSum(salesPersonGroup, line);

                result.Add(line);
            }

            return result;
        }

        private void MakeSum(IEnumerable<ReportLineViewModel> group, ReportLineViewModel lic)
        {
            lic.Amount = group.Sum(x => x.Amount);

            lic.Clients = group.Sum(x => x.Clients);
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.Amount).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Dependiente");
            PrintHeader(cell, 0, 1, "Ventas");
            PrintHeader(cell, 0, 2, "Clientes");
            PrintHeader(cell, 0, 3, "$/Cliente");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.SalesPersonName;

            PrintMoney(cell, 0, 1, line.Amount);

            cell.Offset[0, 2].Value = line.Clients;

            PrintMoney(cell, 0, 3, line.SalesByClient);
        }

        protected override int GetColumnsCount()
        {
            return 4;
        }

        #endregion
    }
}
