using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Classes;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class TotalesPorDependienteViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        TotalesViewModel totalesParent;

        public TotalesPorDependienteViewModel(TotalesViewModel totalesParent, MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            this.totalesParent = totalesParent;
        }

        public void UpdateTotals()
        {
            //totals by waiter
            //var orders = from o in appvm.Context.SalesOrders
            //             where o.RealDateTime >= totalesParent.FromDate && o.RealDateTime <= totalesParent.ToDate
            //             select o;

            totalsWaiter.Clear();
            foreach (var item in Accounter.UpdateTotalsByWaiter(appvm.Context, totalesParent.FromDate, totalesParent.ToDate))
            {
                totalsWaiter.Add(item);
            }
        }

        ObservableCollection<TotalByDependiente> totalsWaiter = new ObservableCollection<TotalByDependiente>();
        public ObservableCollection<TotalByDependiente> TotalsWaiter
        {
            get { return totalsWaiter; }
        }

        RelayCommand exportToExcel;
        public ICommand ExportCommand
        {
            get
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => this.Export());
                return exportToExcel;
            }
        }

        private void Export()
        {

            DisplayInExcel(totalsWaiter, (total, cell) =>
            // This multiline lambda expression sets custom processing rules  
            // for the bankAccounts.
            {
                cell.Value = total.Dependiente;
                
                cell.Offset[0, 1].Value = total.TotalVentas;
                cell.Offset[0, 1].Style = "Currency";
                
                cell.Offset[0, 2].Value = total.TotalMesas;
                cell.Offset[0, 3].Value = total.TotalClients;
                
                cell.Offset[0, 4].Value = total.VentasPorCliente;
                cell.Offset[0, 4].Style = "Currency";
                
                cell.Offset[0, 5].Value = total.VentasPorMesa;
                cell.Offset[0, 5].Style = "Currency";
                
                //if (invItem.Balance < 0)
                //{
                //    cell.Interior.Color = 255;
                //    cell.Offset[0, 1].Interior.Color = 255;
                //}
            });
        }

        void DisplayInExcel(IEnumerable<TotalByDependiente> accounts,
             Action<TotalByDependiente, Excel.Range> DisplayFunc)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();

            // Add a new Excel workbook.
            excelApp.Workbooks.Add();

            excelApp.Range["A1:F1"].Font.Bold = true;

            excelApp.Range["A1"].Value = "Dependiente";            
            excelApp.Range["B1"].Value = "Ventas";
            excelApp.Range["C1"].Value = "Mesas";
            excelApp.Range["D1"].Value = "Clientes";
            excelApp.Range["E1"].Value = "Ventas/Cliente";
            excelApp.Range["F1"].Value = "Ventas/Mesa";
            excelApp.Range["A2"].Select();

            foreach (var ac in accounts)
            {
                DisplayFunc(ac, excelApp.ActiveCell);
                excelApp.ActiveCell.Offset[1, 0].Select();
            }

            // Copy the results to the Clipboard.
            //excelApp.Range["A1:B3"].Copy();

            excelApp.Columns[1].AutoFit();
            excelApp.Columns[2].AutoFit();
            excelApp.Columns[3].AutoFit();
            excelApp.Columns[4].AutoFit();
            excelApp.Columns[5].AutoFit();
            excelApp.Columns[6].AutoFit();

            excelApp.Visible = true;
        }
    }
}
