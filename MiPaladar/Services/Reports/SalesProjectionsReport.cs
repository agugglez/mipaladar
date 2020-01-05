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
    public class SalesProjectionsReport : ProjectionsReport, ISalesProjectionsRS
    {
        protected override List<ReportLineViewModel> Extract(IUnitOfWork unitOfWork, CustomizeReportOptions cro)
        {
            var queryService = ServiceContainer.GetService<IQueryService>();

            return queryService.GetSalesByItemData(unitOfWork, cro.FromDate, cro.ToDate, false);
        }

        protected override bool OfflineFilter(ReportLineViewModel li)
        {
            return true;
            //return li.ProductType == ProductType.FinishedGoods || li.ProductType == ProductType.CompraVenta;
        }

        protected override string ReportName
        {
            get { return "Proyecciones de Ventas"; }
        }
    }
}
