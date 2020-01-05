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
    public class ServiceTimeReportViewModel : ReportWithOpenLine
    {
        public ServiceTimeReportViewModel(MainWindowViewModel appvm)
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

            //service time data
            var groupQuery = from low in ItemsShowing
                             let span = GetSpanKey(low)
                             where span >= 0
                             orderby span
                             group low by span;

            foreach (var group in groupQuery)
            {
                ChartItemViewModel stt = new ChartItemViewModel();

                int minutes = group.Key * 5;
                stt.X = minutes > 60 ? "> 60" : minutes.ToString();
                stt.Quantity = group.Count();

                result.Add(stt);
            }

            return result;
        }

        private static int GetSpanKey(ReportLineViewModel line)
        {
            int inMinutes = line.ServiceTime;

            if (inMinutes > 60) inMinutes = 65;

            int result = inMinutes == 0 ? 1 : (inMinutes - 1) / 5 + 1;

            return result;
        }

        #region Totals

        protected override void UpdateTotals()
        {
            int count = ItemsShowing.Count;
            if (count == 0) { MedianServiceTime = 0; return; }

            MedianServiceTime = ItemsShowing.OrderBy(x => x.ServiceTime).ElementAt(count / 2).ServiceTime;
        }

        int medianMins;
        public int MedianServiceTime
        {
            get { return medianMins; }
            set
            {
                medianMins = value;
                OnPropertyChanged("MedianServiceTime");
            }
        }

        #endregion

        protected override IReportingService GetReportService()
        {
            return base.GetService<IServiceTimeRS>();
        }

        protected override IExcelExporter GetExcelService()
        {
            return base.GetService<IServiceTimeEE>();
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
