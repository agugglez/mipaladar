using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Input;

using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class Report101InfoViewModel : ViewModelBase
    {
        public string FilePath { get; set; }
        public decimal TotalClients { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalDiscount { get; set; }

        //voids
        public int VoidCount { get; set; }
        public int CriticalVoidCount { get; set; } //voids after printing a ticket
        public string CriticalVoidFacturas { get; set; }

        public int OpenDrawerCount { get; set; }

        List<LineOfWork> lows = new List<LineOfWork>();
        public List<LineOfWork> LinesOfWork { get { return lows; } }

        //tiempo de servicio, agrupados
        List<ServiceTimeTotal> stts = new List<ServiceTimeTotal>();        
        public List<ServiceTimeTotal> ServiceTimeTotals { get { return stts; } }

        //numero de clientes en el restaurant en el tiempo, por hora
        List<ClientsByHour> cbh = new List<ClientsByHour>();
        public List<ClientsByHour> ClientsByHourTotals { get { return cbh; } }

        //exportar a Excel COmmand
        #region Export to Excel Command

        RelayCommand exportToExcel;
        public ICommand ExportToExcelCommand
        {
            get
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => ExportToExcel());
                return exportToExcel;
            }
        }

        BackgroundWorker excelWorker;
        ProgressDialogViewModel pdvm;

        private void ExportToExcel()
        {
            if (excelWorker == null)
            {
                excelWorker = new BackgroundWorker();

                excelWorker.DoWork += new DoWorkEventHandler(excelWorker_DoWork);

                excelWorker.WorkerReportsProgress = true;
                excelWorker.ProgressChanged += new ProgressChangedEventHandler(excelWorker_ProgressChanged);

                excelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(excelWorker_RunWorkerCompleted);
            }

            pdvm = new ProgressDialogViewModel();
            pdvm.Message = "Exportando a Excel...";
            pdvm.IsBusy = true;

            var windowManager = base.GetService<IWindowManager>();

            excelWorker.RunWorkerAsync();

            windowManager.ShowDialog(pdvm, this);
        }

        void excelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var excelExporter = base.GetService<IExcelExporter>();
            //excelExporter.Export101ReportInfo(lows, excelWorker);
        }

        void excelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pdvm.Progress = e.ProgressPercentage;
        }

        void excelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pdvm.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();

            //close progress dialog
            windowManager.Close(pdvm);
        }

        #endregion
    }

    //quantity of clients present at the place at an specific hour
    public class ClientsByHour
    {
        public string Hour { get; set; }
        public int Clients { get; set; }
    }

    //quantity of orders served at an specific amount of minutes
    public class ServiceTimeTotal
    {
        public string ServiceTimeMinutes { get; set; }
        public int Quantity { get; set; }
    }
}
