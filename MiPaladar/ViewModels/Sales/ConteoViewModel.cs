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
using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class ConteoViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        //IExcelExporter excelExporter;
        // passwordAsker;

        public ConteoViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
        }
        
        #region Dates

        DateTime fromDate, toDate;

        string[] dateOptions = new string[] { "Hoy", "Ayer", "Específico" };

        public string[] DateOptions
        {
            get { return dateOptions; }
        }

        string dateOption = "Hoy";
        public string SelectedDateOption
        {
            get { return dateOption; }
            set
            {
                dateOption = value;

                if (dateOption == "Hoy")
                {
                    fromDate = toDate = DateTime.Today;

                    UpdateSearch();
                }
                else if (dateOption == "Ayer")
                {
                    fromDate = toDate = DateTime.Today.AddDays(-1);

                    UpdateSearch();
                }
                else if (dateOption == "Específico")
                {
                    if (ShowCustomDateRangeDialog()) UpdateSearch();
                }
            }
        }

        bool ShowCustomDateRangeDialog()
        {
            var windowManager = base.GetService<IWindowManager>();

            CustomDatesDialogViewModel dvm = new CustomDatesDialogViewModel();

            if (windowManager.ShowDialog(dvm, this) == true)
            {
                fromDate = dvm.FromDate;
                toDate = dvm.ToDate;

                return true;
            }

            return false;
        }

        #endregion                                
        
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

        BackgroundWorker bWorker;

        List<ReportLineViewModel> lineitems_source;       

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
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var queryService = base.GetService<IQueryService>();

            e.Result = queryService.GetSalesByItemData(fromDate, toDate, true);
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<ReportLineViewModel> result = (List<ReportLineViewModel>)e.Result;

            lineitems_source = result;

            FilterItems();

            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion        

        #region Filtering

        ObservableCollection<ReportLineViewModel> lineitems_filtered;

        public ObservableCollection<ReportLineViewModel> FilteredItems
        {
            get 
            {
                if (lineitems_filtered == null)
                {
                    lineitems_filtered = new ObservableCollection<ReportLineViewModel>();

                    UpdateSearch();
                }
                return lineitems_filtered; 
            }
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

        bool PassesFilter(ReportLineViewModel li)
        {
            //SEARCH TEXT
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string prefix = searchText.Trim();

                if (li.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) < 0) return false;
            }
            //TAGS
            if (selectedTag.Id != 0)
            {
                if (!li.TagIds.Contains(selectedTag.Id)) return false;
            }
            //CATEGORY
            if (selectedCategory.Id != 0)
            {
                if (li.CategoryId == 0) return false;

                //travel up the category tree
                Category cat = appvm.CategoriesOC.Single(x => x.Id == li.CategoryId);
                bool success = false;
                do
                {
                    if (cat.Id == selectedCategory.Id)
                    {
                        success = true;
                        break;
                    }
                    cat = cat.ParentCategory;

                } while (cat != null);

                if (!success) return false;
            }

            return true;
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

                    FilterItems();
                }                
            }
        }

        ObservableCollection<ShiftViewModel> shifts;
        public ObservableCollection<ShiftViewModel> Shifts
        {
            get
            {
                if (shifts == null)
                {
                    shifts = new ObservableCollection<ShiftViewModel>();

                    ShiftViewModel allShifts = new ShiftViewModel();
                    allShifts.Name = "Todos los turnos";

                    shifts.Add(allShifts);

                    selectedShift = allShifts;

                    foreach (var item in appvm.ShiftsOC)
                    {
                        ShiftViewModel svm = new ShiftViewModel();
                        svm.Id = item.Id;
                        svm.Name = item.Name;

                        shifts.Add(svm);
                    }
                }
                return shifts;
            }
        }        

        ShiftViewModel selectedShift;

        public ShiftViewModel SelectedShift
        {
            get { return selectedShift; }
            set
            {
                if (selectedShift != value)
                {
                    selectedShift = value;

                    UpdateSearch();
                }
            }
        }

        ObservableCollection<CategoryRowViewModel> categories;
        public ObservableCollection<CategoryRowViewModel> Categories
        {
            get
            {
                if (categories == null)
                {
                    categories = new ObservableCollection<CategoryRowViewModel>();

                    //show all categories by default
                    CategoryRowViewModel allCategoriesRow = new CategoryRowViewModel();
                    allCategoriesRow.Id = 0;
                    allCategoriesRow.Name = "Todas las Categorías";

                    categories.Add(allCategoriesRow);

                    //create category tree
                    var invSvc = base.GetService<IInventoryService>();
                    invSvc.CreateCategoryList(categories, -1, NamingModes.SimpleName);

                    selectedCategory = allCategoriesRow;

                    //foreach (var item in appvm.CategoriesOC.OrderBy(x => x.Name))
                    //{
                    //    CategoryRowViewModel crvm = new CategoryRowViewModel();
                    //    crvm.Id = item.Id;
                    //    crvm.Name = item.Name;

                    //    categories.Add(crvm);
                    //}
                }
                return categories;
            }
        }

        CategoryRowViewModel selectedCategory;

        public CategoryRowViewModel SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;

                    FilterItems();
                }
            }
        }

        ObservableCollection<TagViewModel> tags;
        public ObservableCollection<TagViewModel> Tags
        {
            get
            {
                if (tags == null)
                {
                    tags = new ObservableCollection<TagViewModel>();

                    TagViewModel allTags = new TagViewModel(0,"Todas las Etiquetas");

                    tags.Add(allTags);

                    selectedTag = allTags;

                    foreach (var item in appvm.TagsOC.OrderBy(x => x.Name))
                    {
                        TagViewModel crvm = new TagViewModel(item.Id, item.Name);

                        tags.Add(crvm);
                    }
                }
                return tags;
            }
        }

        TagViewModel selectedTag;

        public TagViewModel SelectedTag
        {
            get { return selectedTag; }
            set
            {
                if (selectedTag != value)
                {
                    selectedTag = value;

                    FilterItems();
                }
            }
        }


        #endregion

    }
}
