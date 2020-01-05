using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class CostByItemReportViewModel : ReportsWindowViewModel
    {
        public CostByItemReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
        }

        protected override List<ChartItemViewModel> GetGraphData()
        {
            return null;
        }

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            return TextCondition(item.Product.Name);
        }

        #region Totals

        protected override void UpdateTotals()
        {
            TotalCost = ItemsShowing.Sum(x => x.Cost);
            TotalRealCost = ItemsShowing.Sum(x => x.RealCost); ;
        }

        decimal totalCost;
        public decimal TotalCost
        {
            get { return totalCost; }
            set
            {
                totalCost = value;
                OnPropertyChanged("TotalCost");
            }
        }

        decimal totalRealCost;
        public decimal TotalRealCost
        {
            get { return totalRealCost; }
            set
            {
                totalRealCost = value;
                OnPropertyChanged("TotalRealCost");
            }
        }

        #endregion

        protected override IReportingService GetReportService()
        {
            return base.GetService<ICostByItemRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<ICostByItemEE>();
        }
    }
}
