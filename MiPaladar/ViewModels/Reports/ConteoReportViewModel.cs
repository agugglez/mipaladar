using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class ConteoReportViewModel : ReportsWindowViewModel
    {
        public ConteoReportViewModel(MainWindowViewModel appvm)
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
            return base.GetService<IConteoRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IConteoEE>();
        }
    }
}
