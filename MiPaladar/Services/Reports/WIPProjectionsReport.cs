using MiPaladar.Classes;
using MiPaladar.Entities;
using MiPaladar.MVVM;
using MiPaladar.ViewModels;
using MiPaladar.Repository;

using Excel = Microsoft.Office.Interop.Excel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPaladar.Services
{
    public class WIPProjectionsReport : ProjectionsReport, IWIPProjectionsRS
    {
        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, true);
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return li.ProductType == ProductType.WorkInProcess;
        }

        protected override string ReportName
        {
            get { return "Proyecciones de Elaboración"; }
        }
    }
}
