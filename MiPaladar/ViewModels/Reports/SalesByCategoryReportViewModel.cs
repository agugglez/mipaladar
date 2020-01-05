using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class SalesByCategoryReportViewModel : ReportsWindowViewModel
    {
        public SalesByCategoryReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
        }

        protected override List<ChartItemViewModel> GetGraphData()
        {
            List<ChartItemViewModel> result = new List<ChartItemViewModel>();

            foreach (var item in 
                ItemsShowing.Where(x => x.ChildrenProductsTotalSale > 0).OrderBy(x => -x.ChildrenProductsTotalSale))
            {
                ChartItemViewModel scc = new ChartItemViewModel();

                scc.X = item.CategoryName;

                scc.TotalSale = Math.Round(item.ChildrenProductsTotalSale);

                result.Add(scc);
            }

            return result;
        }

        #region Totals

        protected override void UpdateTotals()
        {
            var rootCategories = ItemsShowing.Where(x => x.Category == null || x.Category.ParentCategory == null);
            //sum only root categories
            TotalSales = rootCategories.Sum(x => x.Amount);
            TotalCost = rootCategories.Sum(x => x.Cost);

            TotalProfit = totalSales - totalCost;
            AverageCostPercent = totalSales == 0 ? 0 : totalCost / totalSales;
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

        decimal totalProfit;
        public decimal TotalProfit
        {
            get { return totalProfit; }
            set
            {
                totalProfit = value;
                OnPropertyChanged("TotalProfit");
            }
        }

        decimal aveCostPct;
        public decimal AverageCostPercent
        {
            get { return aveCostPct; }
            set
            {
                aveCostPct = value;
                OnPropertyChanged("AverageCostPercent");
            }
        }

        #endregion

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            return TextCondition(item.CategoryName);
        }

        protected override IReportingService GetReportService()
        {
            return base.GetService<ISalesByCategoryRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<ISalesByCategoryEE>();
        }
    }
}
