using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;

using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.MVVM;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public DashboardViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
            //dates
            //DateTime today = DateTime.Today;
            //DayOfWeek dow = today.DayOfWeek;
            //sevenDaysDate = today.Add(TimeSpan.FromDays(-6));
            //fourWeeksDate = today.AddDays(-(dow != DayOfWeek.Sunday ? 27 + (int)dow : 34));

            LoadCompanyInfo();

        }

        //DateTime sevenDaysDate;
        //DateTime fourWeeksDate;

        public override string DisplayName
        {
            get { return "TABLERO"; }
        }

        protected override void OnDispose()
        {
            if (bWorker != null)
            {
                bWorker.DoWork -= bWorker_DoWork;
                bWorker.RunWorkerCompleted -= bWorker_RunWorkerCompleted;
            }

        }

        #region Company Info

        void LoadCompanyInfo()
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Misc companyInfo = unitOfWork.MiscRepository.Get().FirstOrDefault();

                if (companyInfo == null)
                {
                    companyInfo = new Misc();
                    companyInfo.CompanyName = "Sin Nombre";

                    unitOfWork.MiscRepository.Add(companyInfo);
                    unitOfWork.SaveChanges();
                }

                CompanyName = companyInfo.CompanyName;

                RestaurantCapacity = companyInfo.Capacity;
            }
            
            //DefaultTax = companyInfo.DefaultTax;
            //StartingShiftAmount = companyInfo.StartingShiftAmount;
            //ReportsFolder = companyInfo.ReportsFolder;
            //RegisterIP = companyInfo.RegisterIP;

            HasPendingChanges = false;
        }

        string companyName;
        public string CompanyName
        {
            get { return companyName; }
            set
            {
                companyName = value;
                OnPropertyChanged("CompanyName");

                HasPendingChanges = true;
            }
        }

        int capacity;

        public int RestaurantCapacity
        {
            get { return capacity; }
            set
            {
                capacity = value;

                UpdateRotation();

                OnPropertyChanged("RestaurantCapacity");

                HasPendingChanges = true;
            }
        }

        private void UpdateRotation()
        {
            if (answer == null) return;

            int different_dates = answer.SevenDaysColumnData.Count;

            if (capacity > 0 && answer.SevenDaysColumnData.Count > 0)
            {
                Rotation = (double)answer.TotalClients / capacity / different_dates; //daily
            }
            else Rotation = 0;
        }

        #region Save Command

        RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => Save(), x => this.CanSave);
                return saveCommand;
            }
        }

        bool CanSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(companyName) && capacity > 0;
            }
        }

        void Save()
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Misc companyInfo = unitOfWork.MiscRepository.GetById(1);

                if (companyInfo.CompanyName != companyName) companyInfo.CompanyName = companyName;
                if (companyInfo.Capacity != capacity) companyInfo.Capacity = capacity;

                unitOfWork.SaveChanges();
            }            

            HasPendingChanges = false;
        }

        #endregion

        #region Cancel Command

        RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(x => Cancel());
                return cancelCommand;
            }
        }

        void Cancel()
        {
            LoadCompanyInfo();

            HasPendingChanges = false;
        }

        #endregion

        bool hasPendingChanges;

        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                OnPropertyChanged("HasPendingChanges");
            }
        }

        #endregion

        

        //SEVEN DAYS
        ObservableCollection<ChartItemViewModel> sevenDaysItems;
        public ObservableCollection<ChartItemViewModel> SevenDaysData
        {
            get 
            {
                if (sevenDaysItems == null)
                {
                    sevenDaysItems = new ObservableCollection<ChartItemViewModel>();

                    //DateTime lastDate = GetLastDate();

                    //sevenDaysDate = lastDate.Add(TimeSpan.FromDays(-6));

                    DoQuerysAsync();
                }
                return sevenDaysItems; 
            }
        }

        

        double rotation;

        public double Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                OnPropertyChanged("Rotation");
            }
        }

        //decimal total_sales;
        //int total_clients;
        //int different_dates;//usually 7
        //int total_orders;

        decimal spendingByClient;

        public decimal SpendingByClient
        {
            get { return spendingByClient; }
            set
            {
                spendingByClient = value;
                OnPropertyChanged("SpendingByClient");
            }
        }

        //string priceRange;

        //public string PriceRange
        //{
        //    get {
        //        if (priceRange == null) GetPriceRange();
        //        return priceRange; 
        //    }
        //    set
        //    {
        //        priceRange = value;
        //        OnPropertyChanged("PriceRange");
        //    }
        //}

        decimal spendingDeviation;
        public decimal SpendingDeviation
        {
            get { return spendingDeviation; }
            set
            {
                spendingDeviation = value;
                OnPropertyChanged("SpendingDeviation");
            }
        }

        //decimal spendingByOrder;

        //public decimal SpendingByOrder
        //{
        //    get { return spendingByOrder; }
        //    set
        //    {
        //        spendingByOrder = value;
        //        OnPropertyChanged("SpendingByOrder");
        //    }
        //}

        //FOUR WEEKS
        //ObservableCollection<SalesChartColumnViewModel> fourWeeksItems = new ObservableCollection<SalesChartColumnViewModel>();
        //public ObservableCollection<SalesChartColumnViewModel> FourWeeksData
        //{
        //    get { return fourWeeksItems; }
        //}

        ////BEST DAY
        //private decimal bestDay;
        //public decimal BestDay
        //{
        //    get { return bestDay; }
        //    set
        //    {
        //        bestDay = value;
        //        OnPropertyChanged("BestDay");
        //    }
        //}

        //string bestDayTooltip;
        //public string BestDayTooltip
        //{
        //    get { return bestDayTooltip; }
        //    set
        //    {
        //        bestDayTooltip = value;
        //        OnPropertyChanged("BestDayTooltip");
        //    }
        //}

        ////BEST WEEK
        //decimal bestWeek;
        //public decimal BestWeek
        //{
        //    get { return bestWeek; }
        //    set
        //    {
        //        bestWeek = value;
        //        OnPropertyChanged("BestWeek");
        //    }
        //}

        //string bestWeekTooltip;
        //public string BestWeekTooltip
        //{
        //    get { return bestWeekTooltip; }
        //    set
        //    {
        //        bestWeekTooltip = value;
        //        OnPropertyChanged("BestWeekTooltip");
        //    }
        //}

        ////BEST MONTH
        //decimal bestMonth;

        //public decimal BestMonth
        //{
        //    get { return bestMonth; }
        //    set
        //    {
        //        bestMonth = value;
        //        OnPropertyChanged("BestMonth");
        //    }
        //}

        //string bestMonthTooltip;
        //public string BestMonthTooltip 
        //{
        //    get { return bestMonthTooltip; }
        //    set
        //    {
        //        bestMonthTooltip = value;
        //        OnPropertyChanged("BestMonthTooltip");
        //    }
        //}

        //TOP PRODUCTS
        ObservableCollection<ReportLineViewModel> topProducts = new ObservableCollection<ReportLineViewModel>();

        public ObservableCollection<ReportLineViewModel> TopProducts
        {
            get { return topProducts; }
        }

        BackgroundWorker bWorker;

        private void DoQuerysAsync()
        {
            if (bWorker == null)
            {
                bWorker = new BackgroundWorker();

                bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
                bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);
            }

            bWorker.RunWorkerAsync();
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var queryService = base.GetService<IQueryService>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                e.Result = queryService.GetLastSevenDaysData(unitOfWork);
            }            
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            answer = (SevenDaysDataAnswer)e.Result;

            UpdateRotation();

            SpendingByClient = answer.SpendingByClient;
            SpendingDeviation = answer.SpendingDeviation;

            sevenDaysItems.Clear();
            foreach (var item in answer.SevenDaysColumnData)
            {
                sevenDaysItems.Add(item);
            }

            topProducts.Clear();
            foreach (var item in answer.TopProducts)
            {
                topProducts.Add(item);
            }
        }

        SevenDaysDataAnswer answer;
    }
}
