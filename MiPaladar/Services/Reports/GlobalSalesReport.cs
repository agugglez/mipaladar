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
    public class GlobalSalesReport : ReportBase, IGlobalSalesRS, IGlobalSalesEE
    {
        public GlobalSalesReport()
            : base()
        {
            IgnoreCategories = true;
            IgnoreTags = true;
        }

        protected override string ReportName
        {
            get { return "Ventas Globales"; }
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
            int daysCount = filtered.GroupBy(x => x.Date.Date).Count();
            int monthCount = filtered.GroupBy(x => x.Date.Month).Count();

            List<ReportLineViewModel> result = new List<ReportLineViewModel>();

            //group by month
            if (monthCount >= 3)
            {
                //months data
                var queryNestedGroups = from item in filtered
                                        group item by item.Date.Year into yearGroup
                                        from monthGroup in
                                            (from item in yearGroup
                                             orderby item.Date.Year, item.Date.Month
                                             group item by item.Date.Month)
                                        group monthGroup by yearGroup.Key;

                foreach (var yearGroup in queryNestedGroups)
                {
                    foreach (var monthGroup in yearGroup)
                    {
                        ReportLineViewModel rLine = new ReportLineViewModel();
                        rLine.DateSpanString = monthGroup.First().Date.ToString("MMM/yyy");

                        MakeSum(monthGroup, rLine);

                        result.Add(rLine);
                    }
                }
            }
            //group by week
            else if (daysCount >= 21)
            {
                var query = from item in filtered
                            group item by item.MondayDate;

                foreach (var weekGroup in query)
                {
                    ReportLineViewModel rLine = new ReportLineViewModel();
                    rLine.DateSpanString = weekGroup.First().WeekString;

                    MakeSum(weekGroup, rLine);

                    result.Add(rLine);
                }
            }
            //group by date
            else
            {
                var query = from item in filtered
                            group item by item.Date;

                foreach (var dateGroup in query)
                {
                    ReportLineViewModel rLine = new ReportLineViewModel();
                    rLine.DateSpanString = dateGroup.Key.ToString("d/MMM");

                    MakeSum(dateGroup, rLine);

                    result.Add(rLine);
                }
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
            return transformed;
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Intervalo");
            PrintHeader(cell, 0, 1, "Ventas");
            PrintHeader(cell, 0, 2, "Clientes");
            PrintHeader(cell, 0, 3, "$/Cliente");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.DateSpanString;

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
