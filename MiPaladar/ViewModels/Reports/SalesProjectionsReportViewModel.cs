using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;
using MiPaladar.Entities;

using System.ComponentModel;
using System.Collections.ObjectModel;
using MiPaladar.Enums;

namespace MiPaladar.ViewModels
{
    public class SalesProjectionsReportViewModel : ProjectionsReportViewModel
    {
        public SalesProjectionsReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {           
        }

        public string Title
        {
            get { return "Proyecciones de Venta"; }
        }

        protected override bool ProductIsAvailable(Product p)
        {
            return p.ProductType == ProductType.FinishedGoods || p.ProductType == ProductType.CompraVenta;
        }

        protected override IReportingService GetReportService()
        {
            return base.GetService<ISalesProjectionsRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IProjectionsEE>();
        }
    }
}
