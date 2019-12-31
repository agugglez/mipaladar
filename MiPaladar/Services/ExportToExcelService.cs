using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

using MiPaladar.ViewModels;
using MiPaladar.Classes;

namespace MiPaladar.Services
{
    public interface IExcelExporter 
    {
        void ExportToExcel<T>(IEnumerable<T> items, Action<Excel.Range> DisplayHeader, 
            Action<T, Excel.Range> DisplayFunc, int columnCount);

        void ExportToExcel<T>(IEnumerable<T> items, Action<Excel.Range> DisplayHeader,
            Action<T, Excel.Range> DisplayFunc, int columnCount, BackgroundWorker bWorker);

        void ExportDayAveragesReport(IEnumerable<ReportItemViewModel> itemsList, BackgroundWorker bWorker);

        void Export101ReportInfo(IEnumerable<LineOfWork> linesOfWork, BackgroundWorker bWorker);
    }
    public class ExportToExcelService : IExcelExporter
    {
        public void ExportToExcel<T>(IEnumerable<T> items, Action<Excel.Range> displayHeader, Action<T, Excel.Range> displayItem, int columnCount)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Add a new Excel workbook.
            excelApp.Workbooks.Add();

            excelApp.Range["A1"].Select();
            displayHeader(excelApp.ActiveCell);

            //select next row, leave a blank row
            excelApp.ActiveCell.Offset[2, 0].Select();

            foreach (var item in items)
            {
                displayItem(item, excelApp.ActiveCell);
                excelApp.ActiveCell.Offset[1, 0].Select();
            }

            //autofit columns
            for (int i = 1; i <= columnCount; i++)
            {
                excelApp.Columns[i].AutoFit();
            }

            excelApp.Visible = true;
        }

        public void ExportToExcel<T>(IEnumerable<T> items, Action<Excel.Range> displayHeader, Action<T, Excel.Range> displayItem, int columnCount, BackgroundWorker bWorker)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Add a new Excel workbook.
            excelApp.Workbooks.Add();

            excelApp.Range["A1"].Select();
            displayHeader(excelApp.ActiveCell);

            //select next row, leave a blank row
            excelApp.ActiveCell.Offset[2, 0].Select();

            int items_count = items.Count();
            int count = 0;

            foreach (var item in items)
            {
                displayItem(item, excelApp.ActiveCell);
                excelApp.ActiveCell.Offset[1, 0].Select();

                bWorker.ReportProgress(++count * 100 / items_count);
            }

            //autofit columns
            for (int i = 1; i <= columnCount; i++)
            {
                excelApp.Columns[i].AutoFit();
            }

            excelApp.Visible = true;
        }

        public void ExportDayAveragesReport(IEnumerable<ReportItemViewModel> itemsList, BackgroundWorker bWorker) 
        {
            Action<Excel.Range> displayHeader = (cell) =>
            {
                cell.Offset[0, 0].Value = "Producto";
                cell.Offset[0, 0].Font.Bold = "True";

                cell.Offset[0, 1].Value = "Lunes";
                cell.Offset[0, 1].Font.Bold = "True";

                cell.Offset[0, 2].Value = "Martes";
                cell.Offset[0, 2].Font.Bold = "True";

                cell.Offset[0, 3].Value = "Miércoles";
                cell.Offset[0, 3].Font.Bold = "True";

                cell.Offset[0, 4].Value = "Jueves";
                cell.Offset[0, 4].Font.Bold = "True";

                cell.Offset[0, 5].Value = "Viernes";
                cell.Offset[0, 5].Font.Bold = "True";

                cell.Offset[0, 6].Value = "Sábado";
                cell.Offset[0, 6].Font.Bold = "True";

                cell.Offset[0, 7].Value = "Domingo";
                cell.Offset[0, 7].Font.Bold = "True";

                cell.Offset[0, 8].Value = "Lunes - Jueves";
                cell.Offset[0, 8].Font.Bold = "True";

                cell.Offset[0, 9].Value = "Viernes - Domingo";
                cell.Offset[0, 9].Font.Bold = "True";
            };

            Action<ReportItemViewModel, Excel.Range> displayItem = (lineitem, cell) => 
            {
                cell.Offset[0, 0].Value = lineitem.Product.Name;

                cell.Offset[0, 1].Value = lineitem.Quantities[1];

                cell.Offset[0, 2].Value = lineitem.Quantities[2];

                cell.Offset[0, 3].Value = lineitem.Quantities[3];

                cell.Offset[0, 4].Value = lineitem.Quantities[4];

                cell.Offset[0, 5].Value = lineitem.Quantities[5];

                cell.Offset[0, 6].Value = lineitem.Quantities[6];

                cell.Offset[0, 7].Value = lineitem.Quantities[0];

                cell.Offset[0, 8].Value = lineitem.MondayToThursday;

                cell.Offset[0, 9].Value = lineitem.FridayToSunday;
            };

            var sorted = from item in itemsList
                         orderby item.Product.Name
                         select item;

            ExportToExcel<ReportItemViewModel>(sorted, displayHeader, displayItem, 10, bWorker);
        }

        public void Export101ReportInfo(IEnumerable<LineOfWork> linesOfWork, BackgroundWorker bWorker)
        {
            Action<Excel.Range> displayHeader = (cell) =>
            {
                cell.Offset[0, 0].Value = "# Factura";
                cell.Offset[0, 0].Font.Bold = "True";

                cell.Offset[0, 1].Value = "Inicio";
                cell.Offset[0, 1].Font.Bold = "True";

                cell.Offset[0, 2].Value = "Recibo";
                cell.Offset[0, 2].Font.Bold = "True";

                cell.Offset[0, 3].Value = "Factura";
                cell.Offset[0, 3].Font.Bold = "True";

                cell.Offset[0, 4].Value = "Servicio (mins)";
                cell.Offset[0, 4].Font.Bold = "True";

                cell.Offset[0, 5].Value = "Importe";
                cell.Offset[0, 5].Font.Bold = "True";
            };

            Action<LineOfWork, Excel.Range> displayItem = (low, cell) =>
            {
                cell.Offset[0, 0].Value = low.FacturaNumber;

                cell.Offset[0, 1].Value2 = string.Format("'{0:d/MMM h:mm tt}", low.StartDate);

                DateTime pd = low.PrintTicketDate;
                cell.Offset[0, 2].Value = pd.Year == 1 ? "-" : string.Format("'{0:d/MMM h:mm tt}", low.PrintTicketDate);

                DateTime fd = low.FacturaDate;
                cell.Offset[0, 3].Value = fd.Year == 1 ? "-" : string.Format("'{0:d/MMM h:mm tt}", low.FacturaDate);

                cell.Offset[0, 4].Value = low.ServiceTime;

                cell.Offset[0, 5].Value = low.Total;
                cell.Offset[0, 5].Style = "Currency";
            };

            //var sorted = from item in itemsList
            //             orderby item.Product.Name
            //             select item;

            ExportToExcel<LineOfWork>(linesOfWork, displayHeader, displayItem, 10, bWorker);
        }
    }
}
