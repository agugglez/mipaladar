using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Views;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;

namespace MiPaladar.ViewModels
{
    public class PersonalViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        IConfirmator confirmator;

        public PersonalViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            this.confirmator = appvm.Confirmator;

            //this.RequestClose += new EventHandler(PersonalViewModel_RequestClose);

            //employeeDataVisible = appvm.EmployeesOC.Count > 0;
        }

        public override string DisplayName
        {
            get { return "LISTA DE USUARIOS"; }
        }

        //void PersonalViewModel_RequestClose(object sender, EventArgs e)
        //{
        //    foreach (var item in appvm.EmployeesOC)
        //    {
        //        item.PropertyChanged -= item_PropertyChanged;
        //    }
        //}

        public MainWindowViewModel AppVM { get { return appvm; } }

        //ICollectionView icvEmployees;
        //public ICollectionView Employees
        //{
        //    get 
        //    {
        //        if (icvEmployees == null) 
        //        {
        //            CollectionViewSource cvs = new CollectionViewSource();
        //            cvs.Source = appvm.EmployeesOC;
        //            icvEmployees = cvs.View;

        //            SortDescription sd = new SortDescription("Name", ListSortDirection.Ascending);
        //            icvEmployees.SortDescriptions.Add(sd);

        //            icvEmployees.Filter = EmployeeFilter;
        //        }
        //        return icvEmployees; 
        //    }
        //}

        #region Expand Item Command

        RelayCommand expandItemCommand;
        public ICommand ExpandItemCommand
        {
            get
            {
                if (expandItemCommand == null)
                    expandItemCommand = new RelayCommand(x => this.ExpandItem(selectedEmployee));
                return expandItemCommand;
            }
        }

        bool CanExpand
        {
            get { return selectedEmployee != null; }
        }

        void ExpandItem(Employee toExpand)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is EmployeeViewModel)) return false;

                EmployeeViewModel svm = (EmployeeViewModel)wsvm;

                return svm.WrappedEmployee == toExpand;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                EmployeeViewModel avm = new EmployeeViewModel(appvm, toExpand, OnAssociationChanged);

                windowManager.ShowChildWindow(avm, appvm);
            }
        }

        void OnAssociationChanged(Employee emp)
        {
            int index = employees_filtered.IndexOf(emp);

            if (index >= 0)
            {
                employees_filtered.RemoveAt(index);
                employees_filtered.Insert(index, emp);

                SelectedEmployee = emp;
            }
        }

        #endregion

        #region Filter

        ObservableCollection<Employee> employees_source;
        //public ObservableCollection<InventoryItemViewModel> ItemsList
        //{
        //    get { return sourceList; }
        //}

        ObservableCollection<Employee> employees_filtered;
        public ObservableCollection<Employee> Employees
        {
            get
            {
                if (employees_filtered == null)
                {
                    employees_source = new ObservableCollection<Employee>(appvm.EmployeesOC);
                    employees_filtered = new ObservableCollection<Employee>();

                    RefreshItems();
                }

                return employees_filtered;
            }
        }

        private void RefreshItems()
        {
            List<InventoryItemViewModel> toRemove = new List<InventoryItemViewModel>();

            employees_filtered.Clear();

            //add those that pass the filter
            foreach (var item in employees_source)
            {
                if (PassesFilter(item))
                {
                    employees_filtered.Add(item);
                }
            }
        }

        public bool PassesFilter(Employee emp)
        {
            bool cond = false;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                cond = true;
            }
            else
            {
                string prefix = searchText.Trim();

                if (emp.Name != null)
                    cond = emp.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            if (!cond) return false;

            if (showOnlyActiveEmployees) 
            {
                cond = emp.IsActive;
            }

            return cond;
        }

        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RefreshItems();
            }
        }

        bool showOnlyActiveEmployees;
        public bool ShowOnlyActiveEmployees 
        {
            get { return showOnlyActiveEmployees; }
            set
            {
                showOnlyActiveEmployees = value;
                RefreshItems();
            }
        }

        //void RefreshView()
        //{
        //    if (icvEmployees != null) 
        //    {
        //        icvEmployees.Refresh();

        //        NoSearchResults = icvEmployees.IsEmpty;

        //        //EmployeeDataVisible = !NoSearchResults;

        //        if (selectedEmployee == null && !icvEmployees.IsEmpty)
        //        {
        //            //can't find a better way to get first item
        //            foreach (var item in icvEmployees)
        //            {
        //                Employee current = (Employee)item;
        //                SelectedEmployee = current;
        //                break;
        //            }
        //        }
        //    }
        //}

        #endregion        

        Employee selectedEmployee;
        public Employee SelectedEmployee 
        {
            get { return selectedEmployee; }
            set 
            {
                selectedEmployee = value;
                //SelectedEmployeeViewModel = value == null ? null : new EmployeeViewModel(appvm, value, HideRoles);
                OnPropertyChanged("SelectedEmployee");
            }
        }

        //EmployeeViewModel selectedEmployeeViewModel;
        //public EmployeeViewModel SelectedEmployeeViewModel 
        //{
        //    get { return selectedEmployeeViewModel; }
        //    set
        //    {
        //        selectedEmployeeViewModel = value;
        //        OnPropertyChanged("SelectedEmployeeViewModel");
        //        //OnPropertyChanged("EmployeeDetailsVisible");
        //    }
        //}

        //void HideRoles() 
        //{
        //    ShowingRolesAndPermissions = true;
        //}

        //public bool EmployeeDetailsVisible 
        //{
        //    get { return selectedEmployee != null; }
        //}

        bool noSearchResults;
        public bool NoSearchResults
        {
            get { return noSearchResults; }
            set
            {
                noSearchResults = value;
                OnPropertyChanged("NoSearchResults");
            }
        }

        public bool ThereAreNoEmployees
        {
            get { return appvm.EmployeesOC.Count == 0; }
        }

        //bool employeeDataVisible;
        //public bool EmployeeDataVisible 
        //{
        //    get { return employeeDataVisible;}
        //    set
        //    {
        //        employeeDataVisible = value;
        //        OnPropertyChanged("EmployeeDataVisible");
        //    }
        //}

        //void CopyData(Employee employee, EmployeeViewModel employeeViewModel) 
        //{
        //    employee.Name = employeeViewModel.Name;
        //    //employee.Position = employeeViewModel.Position;
        //    employee.IsActive = employeeViewModel.IsActive;
        //    employee.CanSell = employeeViewModel.CanSell;
        //    employee.CanPurchase = employeeViewModel.CanPurchase;

        //    appvm.SaveChanges();
        //}

        #region Add Command

        RelayCommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(x => this.Add());
                }

                return addCommand;
            }
        }

        //bool CanAdd
        //{
        //    get
        //    {
        //        bool result = !(string.IsNullOrWhiteSpace(NewEmployeeName) ||
        //        Personal.Where(x => x.Name == NewEmployeeName.Trim()).Count() > 0);

        //        return result;
        //    }
        //}

        void Add()
        {
            EmployeeViewModel nevm = new EmployeeViewModel(appvm, OnCreated, OnAssociationChanged);

            var windowManager = base.GetService<IWindowManager>();

            windowManager.ShowChildWindow(nevm, appvm);

            //SelectedEmployeeViewModel = nevm;

            //EmployeeView newEmployeeView = new EmployeeView();

            //newEmployeeView.DataContext = nevm;

            //if (newEmployeeView.ShowDialog() == true)
            //{
            //    Employee newEmployee = new Employee();

            //    CopyData(newEmployee, nevm);

            //    appvm.EmployeesOC.Add(newEmployee);

            //    if (newEmployee.IsActive && newEmployee.CanSell) appvm.CanSellEmployees.Add(newEmployee);
            //    if (newEmployee.IsActive && newEmployee.CanPurchase) appvm.CanPurchaseEmployees.Add(newEmployee);
            //}            
        }

        void OnCreated(Employee newEmp) 
        {
            employees_source.Add(newEmp);
            if (PassesFilter(newEmp)) employees_filtered.Add(newEmp);
        }

        #endregion

        //#region Edit Command

        //RelayCommand editCommand;
        //public ICommand EditCommand 
        //{
        //    get 
        //    {
        //        if (editCommand == null)
        //            editCommand = new RelayCommand(x => this.Edit((Employee)x));
        //        return editCommand;
        //    }
        //}

        //void Edit(Employee emp)         
        //{
        //    bool couldSell = emp.CanSell;
        //    bool couldPurchase = emp.CanPurchase;

        //    EmployeeViewModel nevm = new EmployeeViewModel(appvm, emp, HideRoles);

        //    EmployeeView employeeView = new EmployeeView();

        //    employeeView.DataContext = nevm;

        //    //if (employeeView.ShowDialog() == true)
        //    //{
        //    //    CopyData(emp, nevm);

        //    //    appvm.CanSellEmployees.Remove(emp);
        //    //    appvm.CanPurchaseEmployees.Remove(emp);

        //    //    if (emp.IsActive && emp.CanSell && !appvm.CanSellEmployees.Contains(emp))
        //    //        appvm.CanSellEmployees.Add(emp);

        //    //    if (emp.IsActive && emp.CanPurchase && !appvm.CanPurchaseEmployees.Contains(emp))
        //    //        appvm.CanPurchaseEmployees.Add(emp);
        //    //}            
        //}

        //#endregion

        #region Delete Command

        RelayCommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(x => this.Delete(selectedEmployee));
                }

                return deleteCommand;
            }
        }

        bool CanDelete
        {
            get
            {
                return selectedEmployee != null;
                    //&& selectedEmployeeViewModel != null && selectedEmployeeViewModel.WrappedEmployee != null;
            }
        }

        void Delete(Employee emp)
        {
            if (!confirmator.AskForConfirmation("Está seguro que desea eliminar a " + emp.Name + " ?")) return;

            employees_source.Remove(emp);
            employees_filtered.Remove(emp);

            appvm.CanSellEmployees.Remove(emp);
            appvm.CanPurchaseEmployees.Remove(emp);          

            appvm.EmployeesOC.Remove(emp);

            appvm.Context.Employees.DeleteObject(emp);
            appvm.SaveChanges();
        }

        #endregion

    }
}
