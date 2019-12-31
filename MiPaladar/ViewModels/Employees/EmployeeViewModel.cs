using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;

using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MiPaladar.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        //Action showRoles;

        Employee employee;

        Action<Employee> onCreated;
        Action<Employee> onAssociationChanged;

        public EmployeeViewModel(MainWindowViewModel appvm, Action<Employee> onCreated, Action<Employee> onAssociationChanged)
        {
            this.appvm = appvm;
            this.onCreated = onCreated;
            this.onAssociationChanged = onAssociationChanged;

            creating = true;

            HasPendingChanges = true;
        }
        public EmployeeViewModel(MainWindowViewModel appvm, Employee employee, Action<Employee> onAssociationChanged)
        {
            this.appvm = appvm;
            //this.showRoles = showRoles;
            this.onAssociationChanged = onAssociationChanged;

            this.employee = employee;

            CopyFromEmployee(employee);

            HasPendingChanges = false;
        }

        public override string DisplayName
        {
            get { return creating ? "Nuevo Empleado" : "Personal : " + name; }
        }

        public Employee WrappedEmployee 
        {
            get { return employee; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        public ObservableCollection<Role> Roles 
        {
            get { return appvm.RolesOC; }
        }

        #region Properties

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
                HasPendingChanges = true;
            }
        }

        Role role;
        public Role Role
        {
            get { return role; }
            set 
            {
                role = value;
                OnPropertyChanged("Role");

                HasPendingChanges = true;
            }
        }

        //JobPosition position;
        //public JobPosition Position 
        //{
        //    get { return position; }
        //    set
        //    {
        //        position = value;
        //        OnPropertyChanged("Position");
        //    }
        //}
        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
                HasPendingChanges = true;
            }
        }

        bool canSell;
        public bool CanSell
        {
            get { return canSell; }
            set
            {
                canSell = value;
                OnPropertyChanged("CanSell");
                HasPendingChanges = true;
            }
        }

        bool canPurchase;
        public bool CanPurchase
        {
            get { return canPurchase; }
            set
            {
                canPurchase = value;
                OnPropertyChanged("CanPurchase");
                HasPendingChanges = true;
            }
        }

        string imageFileName;
        public string ImageFileName
        {
            get { return imageFileName; }
            set
            {
                imageFileName = value;
                OnPropertyChanged("ImageFileName");
            }
        }

        string fullImagePath;
        public string ImageFullPath
        {
            get { return fullImagePath; }
            set
            {
                fullImagePath = value;
                OnPropertyChanged("ImageFullPath");
                OnPropertyChanged("NoPicture");
            }
        }

        public bool NoPicture
        {
            get { return string.IsNullOrEmpty(fullImagePath); }
        }

        //ObservableCollection<PermissionViewModel> permissions = new ObservableCollection<PermissionViewModel>();
        //public ObservableCollection<PermissionViewModel> UserPermissions 
        //{
        //    get { return permissions; }
        //}

        //ObservableCollection<RoleViewModel> employeRoles = new ObservableCollection<RoleViewModel>();
        //public IEnumerable<RoleViewModel> Roles 
        //{
        //    get { return employeRoles; }
        //}

        //RoleViewModel selectedRole;
        //public RoleViewModel SelectedRole
        //{
        //    get { return selectedRole; }
        //    set
        //    {
        //        selectedRole = value;
        //        OnPropertyChanged("SelectedRole");
        //    }
        //}

        #endregion        

        //public IEnumerable<JobPosition> JobPositions 
        //{
        //    get { return appvm.Context.JobPositions; }
        //}

        #region Copy Methods

        private void CopyFromEmployee(Employee emp)
        {
            Name = emp.Name;
            Role = emp.Role;
            IsActive = emp.IsActive;
            CanSell = emp.CanSell;
            CanPurchase = emp.CanPurchase;

            //image
            ImageFileName = emp.ImageFileName;
            BuildImageFullPath();

            //permissions
            //CanLogin = emp.Permissions.CanLogin;
            //CanSeeSales = emp.Permissions.CanSeeSales;
            //CanRemoveSales = emp.Permissions.CanRemoveSales;
            //CanSeeOldSales = emp.Permissions.CanSeeOldSales;

            //CanSeePurchases = emp.Permissions.CanSeePurchases;
            //CanRemovePurchases = emp.Permissions.CanRemovePurchases;
            //CanSeeOldPurchases = emp.Permissions.CanSeeOldPurchases;

            //CanSeeInventory = emp.Permissions.CanSeeInventory;
            //CanCreateProducts = emp.Permissions.CanCreateProducts;
            //CanEditProducts = emp.Permissions.CanEditProducts;
            //CanRemoveProducts = emp.Permissions.CanRemoveProducts;

            //CanSeeEmployees = emp.Permissions.CanSeeEmployees;
            //CanCreateEmployees = emp.Permissions.CanCreateEmployees;
            //CanEditEmployees = emp.Permissions.CanEditEmployees;
            //CanRemoveEmployees = emp.Permissions.CanRemoveEmployees;

            //CanSeeMiPaladar = emp.Permissions.CanSeeMiPaladar;
            //CanExportImport = emp.Permissions.CanExportImport;
            //CanDesignRestaurant = emp.Permissions.CanDesignRestaurant;

            //CanSeeSalesReport = emp.Permissions.CanSeeSalesReport;
            //CanSeeSalesByItemReport = emp.Permissions.CanSeeSalesByItemReport;
            //CanSeeFollowProductReport = emp.Permissions.CanSeeFollowProductReport;
            //CanSeeDayAveragesReport = emp.Permissions.CanSeeDayAveragesReport;

            //permissions.Clear();

            //foreach (var perm in appvm.Context.Permissions)
            //{
            //    bool allowed = emp.Permissions.Contains(perm);

            //    PermissionViewModel pvm = new PermissionViewModel(allowed, perm, OnPermissionChecked);

            //    permissions.Add(pvm);
            //}

            //employeRoles.Clear();
            //foreach (var item in emp.Roles)
            //{
            //    employeRoles.Add(new RoleViewModel(item, OnAllowedChanged));
            //}
        }

        private void BuildImageFullPath()
        {
            if (string.IsNullOrWhiteSpace(imageFileName)) return;

            string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                App.ApplicationFolderName);
            tempFolder = Path.Combine(tempFolder, App.AppEmployeesFolderName);

            ImageFullPath = Path.Combine(tempFolder, imageFileName);
        }

        void CopyToEmployee(Employee emp)
        {
            if (emp.Name != name) emp.Name = Name;
            if (emp.Role != role) emp.Role = role;
            if (emp.IsActive != isActive) emp.IsActive = isActive;
            if (emp.CanSell != canSell) emp.CanSell = canSell;
            if (emp.CanPurchase != canPurchase) emp.CanPurchase = canPurchase;
            //image
            if (emp.ImageFileName != imageFileName) emp.ImageFileName = imageFileName;      
      
            //permissions
            //if (emp.Permissions.CanLogin != canLogin) emp.Permissions.CanLogin = canLogin;
            //if (emp.Permissions.CanSeeSales != canSeeSales) emp.Permissions.CanSeeSales = canSeeSales;
            //if (emp.Permissions.CanRemoveSales != canRemoveSales) emp.Permissions.CanRemoveSales = canRemoveSales;
            //if (emp.Permissions.CanSeeOldSales != canSeeOldSales) emp.Permissions.CanSeeOldSales = canSeeOldSales;

            //if (emp.Permissions.CanSeePurchases != canSeePurchases) emp.Permissions.CanSeePurchases = canSeePurchases;
            //if (emp.Permissions.CanRemovePurchases != canRemovePurchases) emp.Permissions.CanRemovePurchases = canRemovePurchases;
            //if (emp.Permissions.CanSeeOldPurchases != canSeeOldPurchases) emp.Permissions.CanSeeOldPurchases = canSeeOldPurchases;

            //if (emp.Permissions.CanSeeInventory != canSeeInventory) emp.Permissions.CanSeeInventory = canSeeInventory;
            //if (emp.Permissions.CanCreateProducts != canCreateProducts) emp.Permissions.CanCreateProducts = canCreateProducts;
            //if (emp.Permissions.CanEditProducts != canEditProducts) emp.Permissions.CanEditProducts = canEditProducts;
            //if (emp.Permissions.CanRemoveProducts != canRemoveProducts) emp.Permissions.CanRemoveProducts = canRemoveProducts;

            //if (emp.Permissions.CanSeeEmployees != canSeeEmployees) emp.Permissions.CanSeeEmployees = canSeeEmployees;
            //if (emp.Permissions.CanCreateEmployees != canCreateEmployees) emp.Permissions.CanCreateEmployees = canCreateEmployees;
            //if (emp.Permissions.CanEditEmployees != canEditEmployees) emp.Permissions.CanEditEmployees = canEditEmployees;
            //if (emp.Permissions.CanRemoveEmployees != canRemoveEmployees) emp.Permissions.CanRemoveEmployees = canRemoveEmployees;

            //if (emp.Permissions.CanSeeMiPaladar != canSeeMiPaladar) emp.Permissions.CanSeeMiPaladar = canSeeMiPaladar;
            //if (emp.Permissions.CanExportImport != canExportImport) emp.Permissions.CanExportImport = canExportImport;
            //if (emp.Permissions.CanDesignRestaurant != canDesignRestaurant) emp.Permissions.CanDesignRestaurant = canDesignRestaurant;

            //if (emp.Permissions.CanSeeSalesReport != canSeeSalesReport) emp.Permissions.CanSeeSalesReport = canSeeSalesReport;
            //if (emp.Permissions.CanSeeSalesByItemReport != canSeeSalesByItemReport) emp.Permissions.CanSeeSalesByItemReport = canSeeSalesByItemReport;
            //if (emp.Permissions.CanSeeFollowProductReport != canSeeFollowProductReport) emp.Permissions.CanSeeFollowProductReport = canSeeFollowProductReport;
            //if (emp.Permissions.CanSeeDayAveragesReport != canSeeDayAveragesReport) emp.Permissions.CanSeeDayAveragesReport = canSeeDayAveragesReport;

            //remove permissions not present
            //List<Permission> toRemove = new List<Permission>();
            //foreach (var item in emp.Permissions)
            //{
            //    bool contains = permissions.Where(x => x.Allowed && x.WrappedPermission == item).Count() > 0;
                
            //    if (!contains) toRemove.Add(item);
            //}
            //foreach (var item in toRemove)
            //{
            //    emp.Permissions.Remove(item);                    
            //}

            ////add new permissions
            //foreach (var item in permissions)
            //{    
            //    //if it's allowed, check if emp didnt have it already
            //    if (item.Allowed)
            //    {
            //        bool contains = emp.Permissions.Where(x => x == item.WrappedPermission).Count() > 0;
            //        if(!contains) emp.Permissions.Add(item.WrappedPermission);
            //    }
            //}

            ////clear roles
            //while (emp.Roles.Count > 0) 
            //{
            //    appvm.Context.Roles.DeleteObject(emp.Roles.First());
            //}
            ////appvm.SaveChanges();

            //foreach (var item in employeRoles)
            //{
            //    Role newRole = new Role();
            //    newRole.RoleDefinition = item.RoleDefinition;

            //    foreach (var perm in item.Permissions)
            //    {
            //        if (perm.Allowed) newRole.Permissions.Add(perm.WrappedPermission);
            //    }

            //    emp.Roles.Add(newRole);

            //    //appvm.SaveChanges();
            //}            
        }

        #endregion

        //#region Edit Command

        //RelayCommand editCommand;
        //public ICommand EditCommand
        //{
        //    get
        //    {
        //        if (editCommand == null)
        //            editCommand = new RelayCommand(x => Edit());
        //        return editCommand;
        //    }
        //}

        //public void Edit()
        //{
        //    Editing = true;
        //}

        //#endregion

        #region Save Command

        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => Save());
                return saveCommand;
            }
        }

        void Save()
        {
            if (creating)
            {
                employee = new Employee();

                appvm.Context.Employees.AddObject(employee);

                appvm.EmployeesOC.Add(employee);
            }
            
            CopyToEmployee(employee);                

                //personnelManager.ThisGuyChanged(employee);
            
            //Employee emp = appvm.EmployeesOC.Single(x => x.Id == employeeId);

            appvm.SaveChanges();

            if (creating)
            {
                creating = false;

                if (onCreated != null) onCreated(employee);
                
            }
            else if (onAssociationChanged != null) onAssociationChanged(employee);

            if (employee.CanSell && !appvm.CanSellEmployees.Contains(employee))
                appvm.CanSellEmployees.Add(employee);

            if (employee.CanPurchase && !appvm.CanPurchaseEmployees.Contains(employee))
                appvm.CanPurchaseEmployees.Add(employee);

            if (!employee.CanSell) appvm.CanSellEmployees.Remove(employee);

            if (!employee.CanPurchase) appvm.CanPurchaseEmployees.Remove(employee);            

            HasPendingChanges = false;
        }

        #endregion

        #region Cancel Command

        RelayCommand cancelCommand;
        public ICommand CancelCommand
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
            if (creating)
            {
                var windowManager = base.GetService<IWindowManager>();
                windowManager.Close(this);

                //if (appvm.EmployeesOC.Count > 0)
                //    CopyFromEmployee(appvm.EmployeesOC[0]);

                //creating = false;
            }
            else 
            {
                //Employee prod = appvm.EmployeesOC.Single(x => x.Id == employeeId);
                CopyFromEmployee(employee);

                //ResetAllRoles();
            }

            HasPendingChanges = false;
        }

        #endregion

        #region Select Image Command

        RelayCommand selectImageCommand;
        public ICommand SelectImageCommand
        {
            get
            {
                if (selectImageCommand == null)
                    selectImageCommand = new RelayCommand(x => SelectImage());
                return selectImageCommand;
            }
        }

        void SelectImage()
        {
            var open_file_dialog =  base.GetService<IOpenFileDialogService>();
            string title = "Seleccione una imagen";
            string filter = "Imágenes|*.bmp;*.gif;*.ico;*.jpg;*.png;*.wdp;*.tiff";
            
            if (open_file_dialog.ShowDialog(title, filter) == true) 
            {
                string path_to_file = open_file_dialog.FileName;

                ImageFullPath = path_to_file;

                ImageFileName = Path.GetFileName(path_to_file);

                var copySvc = base.GetService<IFileCopyService>();

                copySvc.CopyImage(path_to_file, App.AppEmployeesFolderName);

                HasPendingChanges = true;
            }
            
        }

        #endregion        

        bool creating; 

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

    }
}
