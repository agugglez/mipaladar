using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.MVVM;
using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.Classes;

using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class CustomizeReportViewModel : ViewModelBase
    {
        //MainWindowViewModel appvm;
        CustomizeReportOptions cro;

        public CustomizeReportViewModel(CustomizeReportOptions cro,
            bool showCategoryTab, bool showTagsTab)
        {
            this.cro = cro;

            Initialize();

            this.FromDate = cro.FromDate;
            this.ToDate = cro.ToDate;

            this.ShowCategoryTab = showCategoryTab;
            this.ShowTagsTab = showTagsTab;
        }

        public bool ShowCategoryTab { get; set; }
        public bool ShowTagsTab { get; set; }

        void Initialize()
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                //SHIFTS
                allShifts = new ObservableCollection<CheckBoxItemViewModel>();

                //no shift
                CheckBoxItemViewModel noShift = new CheckBoxItemViewModel(0, "Sin Turno", cro.AdmitsNoShift);

                allShifts.Add(noShift);

                foreach (var item in unitOfWork.ShiftRepository.Get())
                {
                    bool isChecked = cro.SelectedShifts.FirstOrDefault(x => x.Id == item.Id) != null;
                    CheckBoxItemViewModel vm = new CheckBoxItemViewModel(item.Id, item.Name, isChecked);

                    allShifts.Add(vm);
                }

                //SALESPERSONS
                allSalesPersons = new ObservableCollection<CheckBoxItemViewModel>();
                CheckBoxItemViewModel noSalesPerson = new CheckBoxItemViewModel(0, "Sin Dependiente", cro.AdmitsNoSalesPerson);

                allSalesPersons.Add(noSalesPerson);

                foreach (var item in unitOfWork.EmployeeRepository.Get().OrderBy(x => x.Name))
                {
                    bool isChecked = cro.SelectedSalesPersons.FirstOrDefault(x => x.Id == item.Id) != null;
                    CheckBoxItemViewModel vm = new CheckBoxItemViewModel(item.Id, item.Name, isChecked);

                    allSalesPersons.Add(vm);
                }

                //CATEGORIES            
                allCategories = new ObservableCollection<CheckBoxItemViewModel>();
                //no category
                CheckBoxItemViewModel noCategory = new CheckBoxItemViewModel(0, "Sin Categoría", cro.AdmitsNoCategory);

                allCategories.Add(noCategory);

                //create category tree
                var tmpCategories = new ObservableCollection<CategoryRowViewModel>();

                var inventorySvc = base.GetService<IInventoryService>();
                inventorySvc.CreateCategoryList(unitOfWork, tmpCategories);
                //copy
                foreach (var item in tmpCategories)
                {
                    bool isChecked = cro.SelectedCategories.FirstOrDefault(x => x.Id == item.Id) != null;
                    CheckBoxItemViewModel vm = new CheckBoxItemViewModel(item.Id, item.Name, isChecked, OnCategoryChecked, item.Level);

                    allCategories.Add(vm);
                }

                //TAGS
                allTags = new ObservableCollection<CheckBoxItemViewModel>();

                CheckBoxItemViewModel noTag = new CheckBoxItemViewModel(0, "Sin Etiqueta", cro.AdmitsNoTag);

                allTags.Add(noTag);

                foreach (var item in unitOfWork.TagRepository.Get().OrderBy(x => x.Name))
                {
                    bool isChecked = cro.SelectedTags.FirstOrDefault(x => x.Id == item.Id) != null;
                    CheckBoxItemViewModel vm = new CheckBoxItemViewModel(item.Id, item.Name, isChecked);

                    allTags.Add(vm);
                }
            }
        }

        void OnCategoryChecked(int catId)
        {
            var checkedItem = allCategories.Single(x => x.Id == catId);
            int parentLevel=checkedItem.Level;
            bool checkedState = checkedItem.IsChecked;

            int index = allCategories.IndexOf(checkedItem);

            //check/uncheck children
            for (int i = index + 1; i < allCategories.Count; i++)
            {
                //no more children
                if (allCategories[i].Level <= parentLevel) break;

                allCategories[i].IsChecked = checkedState;                
            }
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        ObservableCollection<CheckBoxItemViewModel> allShifts;
        public ObservableCollection<CheckBoxItemViewModel> Shifts
        {
            get { return allShifts; }
        }

        ObservableCollection<CheckBoxItemViewModel> allSalesPersons;
        public ObservableCollection<CheckBoxItemViewModel> SalesPersons
        {
            get { return allSalesPersons; }
        }

        ObservableCollection<CheckBoxItemViewModel> allCategories;
        public ObservableCollection<CheckBoxItemViewModel> Categories
        {
            get { return allCategories; }
        }

        ObservableCollection<CheckBoxItemViewModel> allTags;
        public ObservableCollection<CheckBoxItemViewModel> Tags
        {
            get { return allTags; }
        }

        #region Select All Command

        RelayCommand slctAllCmd;
        public RelayCommand SelectAllCommand
        {
            get
            {
                if (slctAllCmd == null)
                {
                    slctAllCmd = new RelayCommand(x => SelectAll((string)x));
                }
                return slctAllCmd;
            }
        }

        #endregion

        #region Clear All Command

        RelayCommand cleartAllCmd;
        public RelayCommand ClearAllCommand
        {
            get
            {
                if (cleartAllCmd == null)
                {
                    cleartAllCmd = new RelayCommand(x => ClearAll((string)x));
                }
                return cleartAllCmd;
            }
        }

        #endregion

        void SelectAll(string list)
        {
            foreach (var item in GetTargetList(list))
            {
                item.IsChecked = true;
            }
        }

        void ClearAll(string list)
        {
            foreach (var item in GetTargetList(list))
            {
                item.IsChecked = false;
            }
        }

        IEnumerable<CheckBoxItemViewModel> GetTargetList(string name)
        {
            IEnumerable<CheckBoxItemViewModel> targetList = null;

            if (name == "shift") targetList = allShifts;
            else if (name == "salesPerson") targetList = allSalesPersons;
            else if (name == "categories") targetList = allCategories;
            else if (name == "tags") targetList = allTags;

            return targetList;
        }

        public void SyncCustomizeOptions(CustomizeReportOptions cro)
        {
            cro.FromDate = FromDate;
            cro.ToDate = ToDate;

            bool shiftsAllGood = cro.ShiftsAllGood = allShifts.FirstOrDefault(x => !x.IsChecked) == null;
            bool salesPersonsAllGood = cro.SalesPersonsAllGood = allSalesPersons.FirstOrDefault(x => !x.IsChecked) == null;
            bool categoriesAllGood = cro.CategoriesAllGood = allCategories.FirstOrDefault(x => !x.IsChecked) == null;
            bool tagsAllGood = cro.TagsAllGood = allTags.FirstOrDefault(x => !x.IsChecked) == null;

            cro.AllGood = shiftsAllGood && salesPersonsAllGood && categoriesAllGood && tagsAllGood;

            //admits no
            cro.AdmitsNoShift = allShifts[0].IsChecked;
            cro.AdmitsNoSalesPerson = allSalesPersons[0].IsChecked;
            cro.AdmitsNoCategory = allCategories[0].IsChecked;
            cro.AdmitsNoTag = allTags[0].IsChecked;

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                //COPY SHIFT SELECTION
                cro.SelectedShifts.Clear();
                foreach (var item in Shifts)
                {
                    if (item.IsChecked)
                    {
                        if (item.Id == 0) cro.AdmitsNoShift = true;
                        else cro.SelectedShifts.Add(unitOfWork.ShiftRepository.GetById(item.Id));
                    }
                }
                //COPY SALESPERSON SELECTION
                cro.SelectedSalesPersons.Clear();
                foreach (var item in SalesPersons)
                {
                    if (item.IsChecked)
                    {
                        if (item.Id == 0) cro.AdmitsNoSalesPerson = true;
                        else cro.SelectedSalesPersons.Add(unitOfWork.EmployeeRepository.GetById(item.Id));
                    }
                }

                //COPY CATEGORY SELECTION
                cro.SelectedCategories.Clear();
                foreach (var item in Categories)
                {
                    if (item.IsChecked)
                    {
                        if (item.Id == 0) cro.AdmitsNoCategory = true;
                        else cro.SelectedCategories.Add(unitOfWork.CategoryRepository.GetById(item.Id));
                    }
                }

                //COPY TAG SELECTION
                cro.SelectedTags.Clear();
                foreach (var item in Tags)
                {
                    if (item.IsChecked)
                    {
                        if (item.Id == 0) cro.AdmitsNoTag = true;
                        else cro.SelectedTags.Add(unitOfWork.TagRepository.GetById(item.Id));
                    }
                }
            }
        }
    }
}
