using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using MiPaladar.Services;
using MiPaladar.Enums;

namespace MiPaladar.ViewModels
{
    public class GlobalSalesReportViewModel : ReportsWindowViewModel
    {
        public GlobalSalesReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
        }
        protected override List<ChartItemViewModel> GetGraphData()
        {
            List<ChartItemViewModel> result = new List<ChartItemViewModel>();

            foreach (var item in ItemsShowing)
            {
                ChartItemViewModel scc = new ChartItemViewModel();

                //get month from the first sale
                scc.Date = item.Date;
                scc.X = item.DateSpanString;

                scc.TotalSale = Math.Round(item.Amount);
                //scc.TotalCost = Math.Round(item.Cost);

                result.Add(scc);
            }

            return result;
        }        

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            //no text search
            return true;
        }

        #region Totals

        protected override void UpdateTotals()
        {
            TotalSales = ItemsShowing.Sum(x => x.Amount);
            TotalClients = ItemsShowing.Sum(x => x.Clients);
            SpendingByClient = totalClients == 0 ? 0 : totalSales / totalClients;
        }

        decimal totalSales;
        public decimal TotalSales
        {
            get { return totalSales; }
            set
            {
                totalSales = value;
                OnPropertyChanged("TotalSales");
            }
        }

        int totalClients;
        public int TotalClients
        {
            get { return totalClients; }
            set
            {
                totalClients = value;
                OnPropertyChanged("TotalClients");
            }
        }

        decimal spendByClient;
        public decimal SpendingByClient
        {
            get { return spendByClient; }
            set
            {
                spendByClient = value;
                OnPropertyChanged("SpendingByClient");
            }
        }

        #endregion

        protected override IReportingService GetReportService()
        {
            return base.GetService<IGlobalSalesRS>(); ;
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IGlobalSalesEE>();
        }
    }
}
