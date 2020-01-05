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
    public class DayOfWeekSalesReport : ReportBase, IDayOfWeekSalesRS, IDayOfWeekSalesEE
    {
        public DayOfWeekSalesReport()
            : base()
        {
            IgnoreCategories = true;
            IgnoreTags = true;
        }

        protected override string ReportName
        {
            get { return "Promedio Semanal"; }
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

            var query = from item in filtered
                        //orderby item.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)item.DayOfWeek
                        group item by item.DayOfWeek;

            foreach (var dowGroup in query)
            {
                ReportLineViewModel lic = new ReportLineViewModel();

                lic.DayOfWeek = dowGroup.Key;

                //MakeSum(dowGroup, lic);

                int days = dowGroup.GroupBy(x => x.Date).Count();

                lic.Amount = dowGroup.Sum(x => x.Amount) / days;
                lic.Clients = dowGroup.Sum(x => x.Clients) / days;

                result.Add(lic);
            }

            return result;
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)x.DayOfWeek).ToList();
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Día");
            PrintHeader(cell, 0, 1, "Ventas");
            PrintHeader(cell, 0, 2, "Clientes");
            PrintHeader(cell, 0, 3, "$/Cliente");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.DiaEnEspanol.ToString();

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
