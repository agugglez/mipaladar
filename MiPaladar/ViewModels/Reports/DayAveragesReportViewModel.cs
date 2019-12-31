using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.ViewModels;
using MiPaladar.Entities;

using System.Windows.Input;
using System.Data.Objects.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Excel = Microsoft.Office.Interop.Excel;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class DayAveragesReportViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public DayAveragesReportViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
        }

        #region Dates

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        #endregion

        public ObservableCollection<Category> Categories
        {
            get { return appvm.CategoriesOC; }
        }

        bool busy;

        public bool Busy
        {
            get { return busy; }
            set 
            {
                busy = value;
                OnPropertyChanged("Busy");
            }
        }

        #region Command

        bool searchIngredients;
        public bool SearchInIngredients
        {
            get { return searchIngredients; }
            set
            {
                searchIngredients = value;
                OnPropertyChanged("SearchInIngredients");
            }
        }

        RelayCommand dayAverageReportCommand;
        public ICommand DayAverageReportCommand
        {
            get
            {
                if (dayAverageReportCommand == null)
                    dayAverageReportCommand = new RelayCommand(x => ShowDayAverageReport(), x => CanSearch);
                return dayAverageReportCommand;
            }
        }

        bool CanSearch { get { return !busy; } }

        BackgroundWorker bWorker;

        private void ShowDayAverageReport()
        {
            if (bWorker == null)
            {
                bWorker = new BackgroundWorker();

                bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
                bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);
            }

            Busy = true;
            CommandManager.InvalidateRequerySuggested();            

            bWorker.RunWorkerAsync(new DateTime[] { FromDate, ToDate });
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime[] arg = (DateTime[])e.Argument;
            e.Result = DoDatabaseQuery(arg[0], arg[1]);
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sourceList = (ObservableCollection<ReportItemViewModel>)e.Result;

            filteredItems.Clear();

            RefreshItems();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }
        
        #endregion        

        #region Query

        private ObservableCollection<ReportItemViewModel> DoDatabaseQuery(DateTime from_date, DateTime to_date)
        {
            //DateTime firstSunday = new DateTime(1753, 1, 7);
            //var bookings = from b in this.db.Bookings
            //               where EntityFunctions.DiffDays(firstSunday, b.StartDateTime) % 7 == 1
            //               select b;

            RestaurantDBEntities context = new RestaurantDBEntities();

            DateTime toDatePlusOne = to_date.AddDays(1);

            var sale_lineitems = from sli in context.LineItems.OfType<SaleLineItem>()
                                 where sli.Order.Date >= from_date && sli.Order.Date < toDatePlusOne
                                 select sli;

            List<SaleLineItemReportViewModel> operation_items = new List<SaleLineItemReportViewModel>();
            foreach (var sli in sale_lineitems)
            {
                operation_items.Add(new SaleLineItemReportViewModel(sli));

                if (searchIngredients && sli.Product.IsRecipe)                 
                {
                    foreach (var ingredient in sli.Product.Ingredients)
                    {
                        SaleLineItemReportViewModel ing_op = new SaleLineItemReportViewModel();

                        ing_op.Product = ingredient.IngredientProduct;
                        ing_op.Date = sli.Order.Date;
                        ing_op.DayOfWeek = sli.Order.Date.DayOfWeek;
                        ing_op.Quantity = sli.Quantity * ingredient.Quantity * ingredient.UnitMeasure.ToBaseConversion;

                        operation_items.Add(ing_op);
                    }
                }
            }

            var queryNestedGroups = from opi in operation_items
                                    group opi by opi.Product into newGroup1
                                    from newGroup2 in
                                        (from opi in newGroup1
                                         group opi by opi.DayOfWeek)
                                    group newGroup2 by newGroup1.Key;

            //select new ReportItemViewModel
            //{
            //    Product = newGroup1.Key,
            //    DayOfWeekInt = newGroup2.Key,
            //    Quantity = newGroup2.Sum(x => x.Quantity)
            //    //UnitMeasure = appvm.UnitMeasureManager.Unit
            //    //Price = newGroup2.Sum(x=>x.Amount),
            //    //Cost = newGroup2.Sum(x=>x.Cost)
            //};

            IEqualityComparer<SaleLineItemReportViewModel> comparer = new OperationItemDateComparer();

            ObservableCollection<ReportItemViewModel> items = new ObservableCollection<ReportItemViewModel>();
            foreach (var outerGroup in queryNestedGroups)
            {
                ReportItemViewModel rivm = new ReportItemViewModel();
                rivm.Product = outerGroup.Key;

                foreach (var innerGroup in outerGroup)
                {
                    DayOfWeek weekday = innerGroup.Key;

                    int distinct_count = innerGroup.Distinct(comparer).Count();

                    rivm.Quantities[(int)weekday] = Math.Round(innerGroup.Sum(x => x.Quantity) / distinct_count);
                }

                items.Add(rivm);
            }

            return items;
        }

        #endregion

        ObservableCollection<ReportItemViewModel> sourceList = new ObservableCollection<ReportItemViewModel>();
        public ObservableCollection<ReportItemViewModel> ItemsList 
        {
            get { return sourceList; }
        }

        #region Filter

        ObservableCollection<ReportItemViewModel> filteredItems = new ObservableCollection<ReportItemViewModel>();
        public ObservableCollection<ReportItemViewModel> FilteredItems
        {
            get { return filteredItems; }
        }

        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;

                    RefreshItems();
                }
            }
        }

        bool filteringByCategory;
        public bool FilteringByCategory
        {
            get { return filteringByCategory; }
            set
            {
                if (filteringByCategory != value) 
                {
                    filteringByCategory = value;
                    OnPropertyChanged("FilteringByCategory");

                    if (selectedCategory != null) RefreshItems();
                }                
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (selectedCategory != value) 
                {
                    selectedCategory = value;

                    if (filteringByCategory) RefreshItems();                    
                }                
            }
        }

        private void RefreshItems()
        {
            List<ReportItemViewModel> toRemove = new List<ReportItemViewModel>();

            //remove those that don't pass the filter
            foreach (var item in filteredItems)
            {
                if (!PassesFilter(item)) toRemove.Add(item);
            }

            foreach (var item in toRemove)
            {
                filteredItems.Remove(item);
            }

            //add those that pass the filter
            foreach (var item in sourceList)
            {
                if (PassesFilter(item))
                {                    
                    if (!filteredItems.Contains(item)) filteredItems.Add(item);
                }
            }
        }

        bool PassesFilter(ReportItemViewModel rivm) 
        {
            if (!string.IsNullOrWhiteSpace(searchText)) 
            {
                if (rivm.Product.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    return false;
            }

            if (filteringByCategory && selectedCategory != null)
            {
                bool found = false;
                foreach (var item in rivm.Product.RelatedCategories)
                {
                    if (item.Category == null) continue;
                    if (item.Category == selectedCategory)
                    {
                        found = true; break;
                    }
                }

                if (!found) return false;
            }

            return true;
        }

        #endregion        

        #region Export to Excel Command

        RelayCommand exportToExcel;
        public ICommand ExportToExcelCommand
        {
            get
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => Export());
                return exportToExcel;
            }
        }

        BackgroundWorker excelWorker;
        ProgressDialogViewModel pdvm;

        private void Export()
        {
            if (excelWorker == null) 
            {
                excelWorker = new BackgroundWorker();

                excelWorker.DoWork += new DoWorkEventHandler(excelWorker_DoWork);

                excelWorker.WorkerReportsProgress = true;
                excelWorker.ProgressChanged += new ProgressChangedEventHandler(excelWorker_ProgressChanged);

                excelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(excelWorker_RunWorkerCompleted);
            }

            pdvm = new ProgressDialogViewModel();
            pdvm.Message = "Exportando a Excel...";
            pdvm.IsBusy = true;

            var windowManager = base.GetService<IWindowManager>();

            excelWorker.RunWorkerAsync();

            windowManager.ShowDialog(pdvm, this);
            
        }                

        void excelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var excelExporter = base.GetService<IExcelExporter>();
            excelExporter.ExportDayAveragesReport(filteredItems, excelWorker);
            //excelExporter.ExportToExcel<ReportItemViewModel>(sorted,                DisplayHeader, DisplayItem, 10);
        }

        void excelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pdvm.Progress = e.ProgressPercentage;
        }

        void excelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pdvm.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();

            //close progress dialog
            windowManager.Close(pdvm);
        }

        //void DisplayHeader(Excel.Range cell)
        //{
        //    cell.Offset[0, 0].Value = "Producto";
        //    cell.Offset[0, 0].Font.Bold = "True";

        //    cell.Offset[0, 1].Value = "Lunes";
        //    cell.Offset[0, 1].Font.Bold = "True";

        //    cell.Offset[0, 2].Value = "Martes";
        //    cell.Offset[0, 2].Font.Bold = "True";

        //    cell.Offset[0, 3].Value = "Miércoles";
        //    cell.Offset[0, 3].Font.Bold = "True";

        //    cell.Offset[0, 4].Value = "Jueves";
        //    cell.Offset[0, 4].Font.Bold = "True";

        //    cell.Offset[0, 5].Value = "Viernes";
        //    cell.Offset[0, 5].Font.Bold = "True";

        //    cell.Offset[0, 6].Value = "Sábado";
        //    cell.Offset[0, 6].Font.Bold = "True";

        //    cell.Offset[0, 7].Value = "Domingo";
        //    cell.Offset[0, 7].Font.Bold = "True";

        //    cell.Offset[0, 8].Value = "Lunes - Jueves";
        //    cell.Offset[0, 8].Font.Bold = "True";

        //    cell.Offset[0, 9].Value = "Viernes - Domingo";
        //    cell.Offset[0, 9].Font.Bold = "True";
        //}

        //void DisplayItem(ReportItemViewModel lineitem, Excel.Range cell)
        //{
        //    cell.Offset[0, 0].Value = lineitem.Product.Name;

        //    cell.Offset[0, 1].Value = lineitem.Quantities[0];

        //    cell.Offset[0, 2].Value = lineitem.Quantities[1];

        //    cell.Offset[0, 3].Value = lineitem.Quantities[2];

        //    cell.Offset[0, 4].Value = lineitem.Quantities[3];

        //    cell.Offset[0, 5].Value = lineitem.Quantities[4];

        //    cell.Offset[0, 6].Value = lineitem.Quantities[5];

        //    cell.Offset[0, 7].Value = lineitem.Quantities[6];

        //    cell.Offset[0, 8].Value = lineitem.MondayToThursday;

        //    cell.Offset[0, 9].Value = lineitem.FridayToSunday;
        //}

        #endregion
    }

    public class ReportItemViewModel
    {
        public Product Product { get; set; }

        public UnitMeasure UnitMeasure 
        {
            get { return Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase); }
        }

        double[] qtties = new double[7];
        public double[] Quantities { get { return qtties; } }

        public double MondayToThursday 
        {
            get { return qtties[1] + qtties[2] + qtties[3] + qtties[4]; }
        }

        public double FridayToSunday
        {
            get { return qtties[5] + qtties[6] + qtties[0]; }
        }
    }

    class OperationItemDateComparer : IEqualityComparer<SaleLineItemReportViewModel>
    {
        public bool Equals(SaleLineItemReportViewModel x, SaleLineItemReportViewModel y)
        {
            return x.Date.Date == y.Date.Date;
        }

        public int GetHashCode(SaleLineItemReportViewModel obj)
        {
            return obj.Date.DayOfYear;
        }
    }

}
