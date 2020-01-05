using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using MiPaladar.Repository;

namespace MiPaladar.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        //Action showRoles;

        //Employee employee;

        int empId;

        Action<Employee> onCreated;
        Action<Employee> onModified;
        Action<int> onRemoved;

        public EmployeeViewModel(MainWindowViewModel appvm, Action<Employee> onCreated)
        {
            this.appvm = appvm;
            this.onCreated = onCreated;

            creating = true;

            HasPendingChanges = true;
        }
        public EmployeeViewModel(MainWindowViewModel appvm, Employee employee, Action<Employee> onModified, Action<int> onRemoved)
        {
            this.appvm = appvm;
            //this.showRoles = showRoles;
            this.onModified = onModified;
            this.onRemoved = onRemoved;

            this.empId = employee.Id;

            CopyFromEmployee(employee);

            HasPendingChanges = false;
        }

        public override string DisplayName
        {
            get { return creating ? "Nuevo Empleado" : "Personal : " + name; }
        }

        public int EmployeeId { get { return empId; } }

        //public Employee WrappedEmployee 
        //{
        //    get { return employee; }
        //}

        public MainWindowViewModel AppVM { get { return appvm; } }

        public List<Role> Roles 
        {
            get { return base.GetNewUnitOfWork().RoleRepository.Get(); }
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

        int roleId;
        public int RoleId
        {
            get { return roleId; }
            set 
            {
                roleId = value;
                OnPropertyChanged("RoleId");

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

        //bool canPurchase;
        //public bool CanPurchase
        //{
        //    get { return canPurchase; }
        //    set
        //    {
        //        canPurchase = value;
        //        OnPropertyChanged("CanPurchase");
        //        HasPendingChanges = true;
        //    }
        //}

        //string imageFileName;
        //public string ImageFileName
        //{
        //    get { return imageFileName; }
        //    set
        //    {
        //        imageFileName = value;
        //        OnPropertyChanged("ImageFileName");
        //    }
        //}

        //string fullImagePath;
        //public string ImageFullPath
        //{
        //    get { return fullImagePath; }
        //    set
        //    {
        //        fullImagePath = value;
        //        OnPropertyChanged("ImageFullPath");
        //        OnPropertyChanged("NoPicture");
        //    }
        //}

        //public bool NoPicture
        //{
        //    get { return string.IsNullOrEmpty(fullImagePath); }
        //}

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
            RoleId = emp.Role_Id;
            IsActive = emp.IsActive;
            CanSell = emp.CanSell;
            //CanPurchase = emp.CanPurchase;

            //image
            //ImageFileName = emp.ImageFileName;
            //BuildImageFullPath();

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

        //private void BuildImageFullPath()
        //{
        //    if (string.IsNullOrWhiteSpace(imageFileName)) return;

        //    string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //        App.ApplicationFolderName);
        //    tempFolder = Path.Combine(tempFolder, App.AppEmployeesFolderName);

        //    ImageFullPath = Path.Combine(tempFolder, imageFileName);
        //}

        void CopyToEmployee(Employee emp)
        {
            if (emp.Name != name) emp.Name = Name;
            if (emp.Role_Id != roleId) emp.Role_Id = roleId;
            if (emp.IsActive != isActive) emp.IsActive = isActive;
            if (emp.CanSell != canSell) emp.CanSell = canSell;
            //if (emp.CanPurchase != canPurchase) emp.CanPurchase = canPurchase;
            //image
            //if (emp.ImageFileName != imageFileName) emp.ImageFileName = imageFileName;

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
            if (hasPendingChanges)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    Employee employee;

                    if (creating)
                    {
                        employee = new Employee();

                        unitOfWork.EmployeeRepository.Add(employee);
                    }
                    else
                    {
                        employee = unitOfWork.EmployeeRepository.GetById(empId);
                    }

                    CopyToEmployee(employee);

                    //personnelManager.ThisGuyChanged(employee);

                    //Employee emp = appvm.EmployeesOC.Single(x => x.Id == employeeId);

                    unitOfWork.SaveChanges();

                    var temp = employee.Role;

                    if (creating)
                    {
                        creating = false;

                        if (onCreated != null) onCreated(employee);

                    }
                    else if (onModified != null) onModified(employee);

                    //if (employee.CanSell && !appvm.CanSellEmployees.Contains(employee))
                    //    appvm.CanSellEmployees.Add(employee);

                    //if (employee.CanPurchase && !appvm.CanPurchaseEmployees.Contains(employee))
                    //    appvm.CanPurchaseEmployees.Add(employee);

                    //if (!employee.CanSell) appvm.CanSellEmployees.Remove(employee);

                    //if (!employee.CanPurchase) appvm.CanPurchaseEmployees.Remove(employee);            

                    //HasPendingChanges = false;
                }
                
            }

            Close();
        }

        #endregion

        //#region Cancel Command

        //RelayCommand cancelCommand;
        //public ICommand CancelCommand
        //{
        //    get
        //    {
        //        if (cancelCommand == null)
        //            cancelCommand = new RelayCommand(x => Cancel());
        //        return cancelCommand;
        //    }
        //}

        //void Cancel()
        //{
        //    if (creating)
        //    {
        //        var windowManager = base.GetService<IWindowManager>();
        //        windowManager.Close(this);

        //        //if (appvm.EmployeesOC.Count > 0)
        //        //    CopyFromEmployee(appvm.EmployeesOC[0]);

        //        //creating = false;
        //    }
        //    else 
        //    {
        //        //Employee prod = appvm.EmployeesOC.Single(x => x.Id == employeeId);
        //        CopyFromEmployee(employee);

        //        //ResetAllRoles();
        //    }

        //    HasPendingChanges = false;
        //}

        //#endregion

        //#region Select Image Command

        //RelayCommand selectImageCommand;
        //public ICommand SelectImageCommand
        //{
        //    get
        //    {
        //        if (selectImageCommand == null)
        //            selectImageCommand = new RelayCommand(x => SelectImage());
        //        return selectImageCommand;
        //    }
        //}

        //void SelectImage()
        //{
        //    var open_file_dialog =  base.GetService<IOpenFileDialogService>();
        //    string title = "Seleccione una imagen";
        //    string filter = "Imágenes|*.bmp;*.gif;*.ico;*.jpg;*.png;*.wdp;*.tiff";
            
        //    if (open_file_dialog.ShowDialog(title, filter) == true) 
        //    {
        //        string path_to_file = open_file_dialog.FileName;

        //        ImageFullPath = path_to_file;

        //        ImageFileName = Path.GetFileName(path_to_file);

        //        var copySvc = base.GetService<IFileCopyService>();

        //        copySvc.CopyImage(path_to_file, App.AppEmployeesFolderName);

        //        HasPendingChanges = true;
        //    }
            
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
                    deleteCommand = new RelayCommand(x => this.Delete());
                }

                return deleteCommand;
            }
        }

        bool CanDelete
        {
            get
            {
                return !creating && appvm.LoggedInUser.Role.CanRemoveEmployees;
            }
        }

        void Delete()
        {
            var msgBox = base.GetService<IMessageBoxService>();
            bool? result = msgBox.ShowYesNoDialog("Está seguro que desea eliminar este empleado?");

            //if (!confirmator.AskForConfirmation("Está seguro que desea eliminar a " + emp.Name + " ?")) return;

            if (result == true)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    unitOfWork.EmployeeRepository.Remove(empId);
                    unitOfWork.SaveChanges();
                }                

                if (onRemoved != null) onRemoved(empId);

                Close();
            }
        }

        #endregion

        void Close()        
        {
            var windowManager = base.GetService<IWindowManager>();
            windowManager.Close(this);
        }

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
