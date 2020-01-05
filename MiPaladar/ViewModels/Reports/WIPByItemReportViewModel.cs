using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class WIPByItemReportViewModel : ReportsWindowViewModel
    {
        public WIPByItemReportViewModel(MainWindowViewModel appvm)
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

        protected override void UpdateTotals()
        {
            //no totals
        }

        protected override IReportingService GetReportService()
        {
            return base.GetService<IWIPByItemRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IWIPByItemEE>();
        }
    }
}
