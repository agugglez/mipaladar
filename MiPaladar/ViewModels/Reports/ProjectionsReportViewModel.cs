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
using MiPaladar.Repository;

namespace MiPaladar.ViewModels
{
    public abstract class ProjectionsReportViewModel : ReportsWindowViewModel
    {
        public ProjectionsReportViewModel(MainWindowViewModel appvm)
            : base(appvm)
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var tDate = unitOfWork.OrderRepository.GetLastSaleDate();
                var fDate = tDate.AddDays(-13);//last 2 weeks   

                SetCustomDates(fDate, tDate);
            }

            FillProducts();
            if (Products.Count() > 0) sltdProductId = Products.First().Id;
        }

        //SELECT PRODUCTS COMBOBOX
        protected void FillProducts()
        {
            using (var uow = base.GetNewUnitOfWork())
            {
                Products = new ObservableCollection<Product>();

                var query = from p in uow.ProductRepository.Get()
                            where ProductIsAvailable(p)
                            orderby p.Name
                            select p;

                foreach (var item in query)
                {
                    Products.Add(item);
                }
            }            
        }

        protected abstract bool ProductIsAvailable(Product p);

        public ObservableCollection<Product> Products { get; set; }

        int sltdProductId;
        public int SelectedProductId
        {
            get { return sltdProductId; }
            set { sltdProductId = value; UpdateLiveFilter(); UpdateTotals(); UpdateGraphDataAsync(); }
        }

        protected override bool LiveFilter(ReportLineViewModel item)
        {
            //if (SelectedProductId == null) return false;
            return item.Product.Id == SelectedProductId;
        } 

        //PROJECTION PERCENT
        //public double ProjectionPercent
        //{
        //    get { return cro.ProjectionPercent; }
        //    set { cro.ProjectionPercent = value; UpdateProjections(); }
        //}

        //void UpdateProjections()
        //{
        //    foreach (var item in linesBeforeLiveFilter)
        //    {
        //        double newProjQttyInBase = item.AverageQuantity * item.AverageUnitMeasure.ToBaseConversion
        //            * (1 + ProjectionPercent / 100);
        //        //item.ProjectedQuantity = newProjQtty;

        //        var invSVC = base.GetService<IInventoryService>();

        //        //quantity
        //        UnitMeasure bestUM = invSVC.GetBestUM(item.Product.UMFamily, newProjQttyInBase);

        //        item.ProjectedUnitMeasure = bestUM;
        //        item.ProjectedQuantity = newProjQttyInBase / bestUM.ToBaseConversion;
        //    }
        //}

        //GRAPH DATA
        protected override List<ChartItemViewModel> GetGraphData()
        {
            List<ChartItemViewModel> result = new List<ChartItemViewModel>();

            foreach (var item in ItemsShowing)
            {
                ChartItemViewModel scvm = new ChartItemViewModel();

                int dow = (int)item.Date.DayOfWeek;
                scvm.X = item.Date.ToString("ddd d");// string.Format("{0} {1}", (Dias)dow, item.Date.Day); //((Dias)dow).ToString() + " " + item.Date.Day.ToString();
                scvm.Quantity = item.AverageQuantity;

                result.Add(scvm);
            }

            return result;
        }

        #region Totals

        protected override void UpdateTotals()
        {
            if (ItemsShowing.Count() == 0)
            {
                Minimum = Maximum = Average = 0;
                return;
            }

            Minimum = ItemsShowing.Min(x => x.MinimumQuantity * x.MinimumUnitMeasure.ToBaseConversion);
            Maximum = ItemsShowing.Max(x => x.MaximumQuantity * x.MaximumUnitMeasure.ToBaseConversion);
            Average = Math.Round(ItemsShowing.Average(x => x.AverageQuantity * x.AverageUnitMeasure.ToBaseConversion));

            //fit qtties
            var invSvc = base.GetService<IInventoryService>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Product selectedProduct = Products.Single(x => x.Id == SelectedProductId);

                UMFamily umf = unitOfWork.UMFamilyRepository.GetById(selectedProduct.UMFamilyId);
                UnitMeasure bestUM = invSvc.GetBestUM(umf, Minimum);

                MinUnitMeasure = bestUM;
                Minimum = Minimum / bestUM.ToBaseConversion;

                bestUM = invSvc.GetBestUM(umf, Maximum);

                MaxUnitMeasure = bestUM;
                Maximum = Maximum / bestUM.ToBaseConversion;

                bestUM = invSvc.GetBestUM(umf, Average);

                AveUnitMeasure = bestUM;

                int decimalPlaces = umf.Id == 1 ? 0 : 2;
                Average = Math.Round(Average / bestUM.ToBaseConversion, decimalPlaces);
            }
        }

        double min;
        public double Minimum
        {
            get { return min; }
            set
            {
                min = value;
                OnPropertyChanged("Minimum");
            }
        }

        UnitMeasure minUM;
        public UnitMeasure MinUnitMeasure
        {
            get { return minUM; }
            set
            {
                minUM = value;
                OnPropertyChanged("MinUnitMeasure");
            }
        }

        double max;
        public double Maximum
        {
            get { return max; }
            set
            {
                max = value;
                OnPropertyChanged("Maximum");
            }
        }

        UnitMeasure maxUM;
        public UnitMeasure MaxUnitMeasure
        {
            get { return maxUM; }
            set
            {
                maxUM = value;
                OnPropertyChanged("MaxUnitMeasure");
            }
        }

        double ave;
        public double Average
        {
            get { return ave; }
            set
            {
                ave = value;
                OnPropertyChanged("Average");
            }
        }

        UnitMeasure aveUM;
        public UnitMeasure AveUnitMeasure
        {
            get { return aveUM; }
            set
            {
                aveUM = value;
                OnPropertyChanged("AveUnitMeasure");
            }
        }

        #endregion               

        public override void ExportToExcel(BackgroundWorker backWorker)
        {
            GetExcelService().ExportToExcel(linesBeforeLiveFilter, backWorker);
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
