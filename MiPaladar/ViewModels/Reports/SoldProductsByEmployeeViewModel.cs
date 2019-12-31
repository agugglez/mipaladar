using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Enums;
using MiPaladar.Services;
using MiPaladar.Views;


namespace MiPaladar.ViewModels
{
    public class SoldProductsByEmployeeViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        //IExcelExporter excelExporter;
        // passwordAsker;

        public SoldProductsByEmployeeViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            fromDate = DateTime.Today;
            toDate = DateTime.Today;

            //UpdateSearch();

            lineitems_source = new ObservableCollection<SaleLineItemReportViewModel>();
            lineitems_filtered = new ObservableCollection<SaleLineItemReportViewModel>();
        }

        ObservableCollection<Employee> presentEmployees = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> PresentEmployees
        {
            get { return presentEmployees; }
        }
        
        #region Dates

        DateTime fromDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        DateTime toDate;
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        #endregion                

        public ObservableCollection<Category> Categories 
        {
            get { return appvm.CategoriesOC; }
        }
        
        #region Find Command

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

        RelayCommand findCommand;
        public ICommand FindCommand
        {
            get
            {
                if (findCommand == null)
                {
                    findCommand = new RelayCommand(x => this.UpdateSearch(), x => this.CanFind);
                }
                return findCommand;
            }
        }

        bool CanFind { get { return !busy; } }

        //CollectionView auxView;
        BackgroundWorker bWorker;

        ObservableCollection<SaleLineItemReportViewModel> lineitems_source;        

        private void UpdateSearch()
        {
            if (bWorker == null)
            {
                bWorker = new BackgroundWorker();

                bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
                bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bWorker_RunWorkerCompleted);
            }

            Busy = true;
            CommandManager.InvalidateRequerySuggested();

            bWorker.RunWorkerAsync();

            //UpdateGroupDescriptions();

            //if (filteringByIngredient) CalculateIngredientQuantities();

            //UpdateItems();

            //convertedToIngredients = false;
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            lineitems_source.Clear();

            DateTime toDatePlusOne = toDate.AddDays(1);

            //get data
            var query =
                from lineitem in appvm.Context.LineItems.OfType<SaleLineItem>()
                where lineitem.Order.Date >= fromDate && lineitem.Order.Date < toDatePlusOne
                select lineitem;

            foreach (var item in query)
            {
                SaleLineItemReportViewModel copy = new SaleLineItemReportViewModel(item);

                lineitems_source.Add(copy);

                if (SearchInIngredients && item.Product.IsRecipe)
                {
                    foreach (var ingredient in item.Product.Ingredients)
                    {
                        SaleLineItemReportViewModel ing_op = new SaleLineItemReportViewModel();

                        ing_op.Product = ingredient.IngredientProduct;
                        ing_op.Quantity = item.Quantity * ingredient.Quantity;
                        ing_op.UnitMeasure = ingredient.UnitMeasure;
                        ing_op.Date = item.Order.Date;
                        ing_op.DateCreated = item.Order.DateCreated;
                        ing_op.DayOfWeek = item.Order.Date.DayOfWeek;
                        ing_op.MondayDate = SaleLineItemReportViewModel.GetWeekMonday(item.Order.Date);
                        ing_op.Employee = item.Order.Employee;
                        ing_op.Order = (Sale)item.Order;

                        lineitems_source.Add(ing_op);
                    }
                }
            }

            FilterItems();
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateItems();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        void OnFilterConditionsChanged()
        {
            FilterItems();

            UpdateItems();
        }

        void UpdateItems()
        {
            lineitems_showing.Clear();
            presentEmployees.Clear();


            var queryNestedGroups = from item in lineitems_filtered
                                    group item by item.Product into productGroup
                                    from employeeGroup in
                                        (from item in productGroup
                                         group item by item.Employee)
                                    group employeeGroup by productGroup.Key;

            foreach (var productGroup in queryNestedGroups)
            {
                SoldProductsByEmployeeItemViewModel lic = new SoldProductsByEmployeeItemViewModel();

                lic.Product = productGroup.Key;
                UnitMeasure um = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                UnitMeasure costUM = lic.Product.CostUnitMeasure;

                foreach (var employeeGroup in productGroup)
                {
                    if (!presentEmployees.Contains(employeeGroup.Key)) presentEmployees.Add(employeeGroup.Key);
                    
                    double qtty = employeeGroup.Sum(x => x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion);

                    UnitMeasure finalUM = um;

                    if (qtty > costUM.ToBaseConversion)
                    {
                        qtty = qtty / costUM.ToBaseConversion;
                        finalUM = costUM;
                    }

                    lic.QuantityItems.Add(employeeGroup.Key.Id, new ProductQuantityViewModel(lic.Product, qtty, finalUM));
                    //lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
                    //lic.Employee = employeeGroup.Key;

                    //MakeSum(employeeGroup, lic);
                }

                lineitems_showing.Add(lic);
            }

            OnEmployeesUpdated();
        }

        public event EventHandler EmployeesUpdated;

        protected void OnEmployeesUpdated()
        {
            EventHandler handler = this.EmployeesUpdated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion        

        #region Filtering

        ObservableCollection<SaleLineItemReportViewModel> lineitems_filtered;
        public ObservableCollection<SaleLineItemReportViewModel> FilteredItems
        {
            get { return lineitems_filtered; }
        }

        private void FilterItems()
        {

            lineitems_filtered.Clear();

            //add those that pass the filter
            foreach (var item in lineitems_source)
            {
                if (PassesFilter(item))
                {
                    lineitems_filtered.Add(item);
                }
            }
        }

        bool PassesFilter(SaleLineItemReportViewModel li)
        {
            bool cond = true;            

            //if (showSaleOrders)
            {
                //if (li.OrderType != OrderType.Sale) return false;

                Sale sale = (Sale)li.Order;

                if (!cond) return false;
            }

            if (!cond) return false;

            //name condition
            if (string.IsNullOrWhiteSpace(searchText)) cond = true;
            else cond = li.Product == null ? false : li.Product.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;            

            if (!cond) return false;                        

            //category condition
            if (filteringByCategory )
            {
                cond = false;

                if (li.Product != null)
                {
                    foreach (var item in li.Product.RelatedCategories)
                    {
                        if (item.Category == selectedCategory)
                        {
                            cond = true;
                            break;
                        }
                    }
                }
            }                            

            return cond;
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

                    OnFilterConditionsChanged();
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

                    OnFilterConditionsChanged();
                }                
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;

                if (filteringByCategory) 
                {
                    OnFilterConditionsChanged();
                }
            }
        }

        #endregion


        ObservableCollection<SoldProductsByEmployeeItemViewModel> lineitems_showing;
        public ObservableCollection<SoldProductsByEmployeeItemViewModel> ItemsShowing
        {
            get
            {
                if (lineitems_showing == null)
                {
                    lineitems_showing = new ObservableCollection<SoldProductsByEmployeeItemViewModel>();

                    UpdateSearch();
                }
                return lineitems_showing;
            }
        }

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
            Action<Excel.Range> displayHeader = (cell) =>
            {
                int column = 0;

                //one column for product name
                cell.Offset[0, column].Value = "Producto";
                cell.Offset[0, column++].Font.Bold = "True";

                //column for each inventory area
                foreach (var item in presentEmployees)
                {
                    cell.Offset[0, column].Value = item.Name;
                    cell.Offset[0, column++].Font.Bold = "True";
                }
            };

            Action<SoldProductsByEmployeeItemViewModel, Excel.Range> displayItem = (pr, cell) =>
            {
                int column = 0;

                //product name
                cell.Offset[0, column++].Value = pr.Product.Name;

                //qtty in each inventory Area
                foreach (var item in presentEmployees)
                {
                    ProductQuantityViewModel ii = pr.QuantityItems[item.Id];
                    if (ii == null) { column++; continue; }
                    cell.Offset[0, column++].Value = string.Format("{0:0.##} {1}", ii.Quantity, ii.UnitMeasure.Caption);
                }
            };

            int numberOfColumns = CountVisibleColumns();

            var excelExporter = base.GetService<IExcelExporter>();

            var sorted = from p in lineitems_showing
                         orderby p.Product.Name
                         select p;

            excelExporter.ExportToExcel<SoldProductsByEmployeeItemViewModel>(sorted, displayHeader, displayItem, numberOfColumns, excelWorker);
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

        int CountVisibleColumns()
        {
            //inventory areas + product name
            return presentEmployees.Count() + 1;
        }

        #endregion  

        //#region Advanced Search Visibility

        //bool advancedSearchVisible;
        //public bool AdvancedSearchVisible
        //{
        //    get { return advancedSearchVisible; }
        //    set
        //    {
        //        advancedSearchVisible = value;
        //        OnPropertyChanged("AdvancedSearchVisible");
        //    }
        //}

        //RelayCommand showAdvancedSearchPopupCommand;
        //public ICommand ShowAdvancedSearchPopupCommand
        //{
        //    get
        //    {
        //        if (showAdvancedSearchPopupCommand == null)
        //            showAdvancedSearchPopupCommand = new RelayCommand(x => ShowProductTemplatePopup());
        //        return showAdvancedSearchPopupCommand;
        //    }
        //}

        //void ShowProductTemplatePopup()
        //{
        //    AdvancedSearchVisible = true;
        //}

        //#endregion

        
    }
}
