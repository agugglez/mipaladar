using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;
using MiPaladar.Enums;

namespace MiPaladar.ViewModels
{
    public class DayOfWeekSalesReportViewModel : ReportsWindowViewModel
    {
        public DayOfWeekSalesReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
            //use last to week of sales to project
            //var querySvc = base.GetService<IQueryService>();
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var tDate = unitOfWork.OrderRepository.GetLastSaleDate();
                var fDate = tDate.AddDays(-27);//last 4 weeks   

                SetCustomDates(fDate, tDate);
            }            
        }

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            //no text search
            return true;
        }

        protected override List<ChartItemViewModel> GetGraphData()
        {
            List<ChartItemViewModel> result = new List<ChartItemViewModel>();

            foreach (var item in ItemsShowing.OrderBy(x => x.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)x.DayOfWeek))
            {
                ChartItemViewModel scc = new ChartItemViewModel();

                scc.X = ((Dias)item.DayOfWeek).ToString();

                scc.TotalSale = Math.Round(item.Amount);

                result.Add(scc);
            }

            return result;
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
            return base.GetService<IDayOfWeekSalesRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IDayOfWeekSalesEE>();
        }
    }
}
