using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class ProductClassesReportViewModel : ReportsWindowViewModel
    {
        public ProductClassesReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var tDate = unitOfWork.OrderRepository.GetLastSaleDate();
                var fDate = tDate.AddDays(-6);//last week  

                SetCustomDates(fDate, tDate);
            }
        }

        protected override List<ChartItemViewModel> GetGraphData()
        {
            List<ChartItemViewModel> result = new List<ChartItemViewModel>();

            foreach (var group in ItemsShowing.GroupBy(x => x.ProductClass).OrderBy(x => x.Key))
            {
                ChartItemViewModel scc = new ChartItemViewModel();

                scc.X = group.Key.ToString();

                scc.Count = group.Count();

                result.Add(scc);
            }

            return result;
        }

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            return TextCondition(item.Product.Name);
        }

        #region Totals

        protected override void UpdateTotals()
        {
            if (ItemsShowing.Count == 0)
            {
                AveragePreference = 0;
                AverageProfitMargin = 0;
                return;
            }
            AveragePreference = ItemsShowing.Average(x => x.Preference);
            AverageProfitMargin = ItemsShowing.Average(x => x.ProfitMargin);
        }

        double avgPref;
        public double AveragePreference
        {
            get { return avgPref; }
            set
            {
                avgPref = value;
                OnPropertyChanged("AveragePreference");
            }
        }

        decimal avgProfitMargin;
        public decimal AverageProfitMargin
        {
            get { return avgProfitMargin; }
            set
            {
                avgProfitMargin = value;
                OnPropertyChanged("AverageProfitMargin");
            }
        }

        #endregion

        protected override IReportingService GetReportService()
        {
            return base.GetService<IProductClassesRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IProductClassesEE>();
        }
    }
}
