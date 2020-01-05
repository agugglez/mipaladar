using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;

using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public class SalesByItemReportViewModel : ReportsWindowViewModel
    {
        public SalesByItemReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
        }

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            return TextCondition(item.Product.Name);
        }

        protected override List<ChartItemViewModel> GetGraphData()
        {
            return null;
        }

        #region Totals

        protected override void UpdateTotals()
        {
            TotalQuantity = ItemsShowing.Sum(x => x.Quantity);
            TotalSales = ItemsShowing.Sum(x => x.Amount);
            TotalCost = ItemsShowing.Sum(x => x.Cost);
            TotalProfit = ItemsShowing.Sum(x => x.Profit);
            AverageCostPercent = totalSales == 0 ? 0 : totalCost / totalSales;
        }

        double totalQtty;
        public double TotalQuantity
        {
            get { return totalQtty; }
            set
            {
                totalQtty = value;
                OnPropertyChanged("TotalQuantity");
            }
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

        protected override IReportingService GetReportService()
        {
            return base.GetService<ISalesByItemRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<ISalesByItemEE>();
        }
    }
}
