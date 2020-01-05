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
    public class ServiceTimeReport : ReportBase, IServiceTimeRS, IServiceTimeEE
    {
        public ServiceTimeReport()
            : base()
        {
            IgnoreCategories = true;
            IgnoreTags = true;
        }

        protected override string ReportName
        {
            get { return "Tiempo de servicio"; }
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
            return filtered;         
        }

        protected override List<ReportLineViewModel> Load(List<ReportLineViewModel> transformed)
        {
            return transformed.OrderBy(x => x.InvoiceNumber).ToList();        
        }

        #region Excel

        protected override void DisplayHeader(Excel.Application excelApp)
        {
            var cell = excelApp.ActiveCell;

            PrintHeader(cell, 0, 0, "Fecha");
            PrintHeader(cell, 0, 1, "Turno");
            PrintHeader(cell, 0, 2, "# Factura");
            PrintHeader(cell, 0, 3, "Apertura");
            PrintHeader(cell, 0, 4, "Recibo");
            PrintHeader(cell, 0, 5, "Factura");
            PrintHeader(cell, 0, 6, "Tiempo");
        }

        protected override void DisplayItem(ReportLineViewModel line, Excel.Range cell)
        {
            cell.Offset[0, 0].Value = line.Date.ToString("d");

            cell.Offset[0, 1].Value = line.ShiftName;

            cell.Offset[0, 2].Value = line.InvoiceNumber;

            cell.Offset[0, 3].Value = line.DateCreated.ToString("t");

            cell.Offset[0, 4].Value = line.DatePrinted == null ? "-" : line.DatePrinted.Value.ToString("t");

            cell.Offset[0, 5].Value = line.DateClosed == null ? "-" : line.DateClosed.Value.ToString("t");

            cell.Offset[0, 6].Value = line.ServiceTime + " mins";
        }

        protected override int GetColumnsCount()
        {
            return 7;
        }

        #endregion
    }
}
