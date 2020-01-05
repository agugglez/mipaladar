using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;
using MiPaladar.Enums;
using MiPaladar.MVVM;
using MiPaladar.Entities;

namespace MiPaladar.ViewModels
{
    public class DemandByHourReportViewModel : ReportWithOpenLine
    {
        public DemandByHourReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
        }

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            //no text search
            return true;
        }

        protected override List<ChartItemViewModel> GetGraphData()
        {
            List<ChartItemViewModel> result = new List<ChartItemViewModel>();

            var query = from low in ItemsShowing
                        where low.DateCreated.Year > 1
                        let time = DateTime.Today.AddHours(low.DateCreated.Hour)
                        group low by time;

            foreach (var group in query.OrderBy(x => x.Key))
            {
                ChartItemViewModel cbh = new ChartItemViewModel();

                cbh.X = string.Format("{0:h:mm tt}", group.Key);

                cbh.Clients = group.Sum(x => x.Clients);

                result.Add(cbh);
            }

            return result;
        }

        #region Totals

        protected override void UpdateTotals()
        {
            if (GraphData.Count == 0)
            {
                AverageDemand = 0;
                return;
            }

            AverageDemand = GraphData.Average(x => x.Clients);
        }

        double aveDemand;
        public double AverageDemand
        {
            get { return aveDemand; }
            set
            {
                aveDemand = value;
                OnPropertyChanged("AverageDemand");
            }
        }

        #endregion

        protected override IReportingService GetReportService()
        {
            return base.GetService<IDemandByHourRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IDemandByHourEE>();
        }

        #region Double click Command

        protected override void OpenLine(ReportLineViewModel sltdLine)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is OfflineSaleViewModel)) return false;

                OfflineSaleViewModel vm = (OfflineSaleViewModel)wsvm;

                return vm.SaleId == sltdLine.LineId;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                OfflineSaleViewModel vm = new OfflineSaleViewModel(appvm, sltdLine.LineId, OnRemove, null);
                windowManager.ShowChildWindow(vm, this);
            }
        }

        //Sale GetSaleFromId(int id)
        //{
        //    return appvm.Context.Orders.OfType<Sale>().Single(x => x.Id == id);
        //}

        void OnRemove(int saleId)
        {
            RemoveLine(saleId);
        }

        #endregion
    }
}
