using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class RoleViewModel :  ViewModelWithClose
    {
        //Action onFieldChanged;

        MainWindowViewModel appvm;
        bool creating;

        Action<int> onRoleRemoved;
        Action<int> onRoleModified;

        //Role role;
        bool isAdmin; 
        int roleId;

        public RoleViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;
            creating = true;
            HasPendingChanges = true;
        }

        public RoleViewModel(MainWindowViewModel appvm, int roleId, Action<int> onRoleRemoved, Action<int> onRoleModified)
        {
            this.appvm = appvm;
            this.onRoleRemoved = onRoleRemoved;
            this.onRoleModified = onRoleModified;
            this.roleId = roleId;

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var role = unitOfWork.RoleRepository.GetById(roleId);

                CopyFromRole(role);
            }            

            isAdmin = roleId == 1; //role.Name == "Administrador";

            HasPendingChanges = false;
        }

        public int RoleId
        {
            get { return roleId; }
        }

        #region Copy Methods

        private void CopyFromRole(Role role)
        {
            Name = role.Name;

            //permissions
            CanLogin = role.CanLogin;
            CanSeeDashboard = role.CanSeeDashboard;
            CanSeeSales = role.CanSeeSales;
            CanRemoveSales = role.CanRemoveSales;
            //targetVM.CanSeeOldSales = role.CanSeeOldSales;

            //targetVM.CanSeePurchases = role.CanSeePurchases;
            //targetVM.CanRemovePurchases = role.CanRemovePurchases;
            //targetVM.CanSeeOldPurchases = role.CanSeeOldPurchases;

            CanSeeInventory = role.CanSeeInventory;
            CanCreateProducts = role.CanCreateProducts;
            CanEditProducts = role.CanEditProducts;
            CanRemoveProducts = role.CanRemoveProducts;

            CanSeeEmployees = role.CanSeeEmployees;
            CanSeeRoles = role.CanSeeRoles;
            CanCreateEmployees = role.CanCreateEmployees;
            CanEditEmployees = role.CanEditEmployees;
            CanRemoveEmployees = role.CanRemoveEmployees;

            //targetVM.CanSeeMiPaladar = role.CanSeeMiPaladar;
            CanExportImport = role.CanExportImport;
            //targetVM.CanDesignRestaurant = role.CanDesignRestaurant;

            CanSeeReports = role.CanSeeReports;
            //targetVM.CanSeeSalesReport = role.CanSeeSalesReport;
            //targetVM.CanSeeSalesByItemReport = role.CanSeeSalesByItemReport;
            //targetVM.CanSeeFollowProductReport = role.CanSeeFollowProductReport;
            //targetVM.CanSeeDayAveragesReport = role.CanSeeDayAveragesReport;
        }

        void CopyToRole(Role role)
        {
            if (role.Name != name) role.Name = name;

            //permissions
            if (role.CanLogin != canLogin) role.CanLogin = canLogin;
            if (role.CanSeeDashboard != canSeeDashboard) role.CanSeeDashboard = canSeeDashboard;
            if (role.CanSeeSales != canSeeSales) role.CanSeeSales = canSeeSales;
            if (role.CanRemoveSales != canRemoveSales) role.CanRemoveSales = canRemoveSales;
            //if (role.CanSeeOldSales != sourceVM.CanSeeOldSales) role.CanSeeOldSales = sourceVM.CanSeeOldSales;

            //if (role.CanSeePurchases != sourceVM.CanSeePurchases) role.CanSeePurchases = sourceVM.CanSeePurchases;
            //if (role.CanRemovePurchases != sourceVM.CanRemovePurchases) role.CanRemovePurchases = sourceVM.CanRemovePurchases;
            //if (role.CanSeeOldPurchases != sourceVM.CanSeeOldPurchases) role.CanSeeOldPurchases = sourceVM.CanSeeOldPurchases;

            if (role.CanSeeInventory != canSeeInventory) role.CanSeeInventory = canSeeInventory;
            if (role.CanCreateProducts != canCreateProducts) role.CanCreateProducts = canCreateProducts;
            if (role.CanEditProducts != canEditProducts) role.CanEditProducts = canEditProducts;
            if (role.CanRemoveProducts != canRemoveProducts) role.CanRemoveProducts = canRemoveProducts;

            if (role.CanSeeEmployees != canSeeEmployees) role.CanSeeEmployees = canSeeEmployees;
            if (role.CanSeeRoles != canSeeRoles) role.CanSeeRoles = canSeeRoles;
            if (role.CanCreateEmployees != canCreateEmployees) role.CanCreateEmployees = canCreateEmployees;
            if (role.CanEditEmployees != canEditEmployees) role.CanEditEmployees = canEditEmployees;
            if (role.CanRemoveEmployees != canRemoveEmployees) role.CanRemoveEmployees = canRemoveEmployees;
            
            if (role.CanExportImport != canExportImport) role.CanExportImport = canExportImport;            

            if (role.CanSeeReports != canSeeReports) role.CanSeeReports = canSeeReports;
            //if (role.CanSeeSalesReport != sourceVM.CanSeeSalesReport) role.CanSeeSalesReport = sourceVM.CanSeeSalesReport;
            //if (role.CanSeeSalesByItemReport != sourceVM.CanSeeSalesByItemReport) role.CanSeeSalesByItemReport = sourceVM.CanSeeSalesByItemReport;
            //if (role.CanSeeFollowProductReport != sourceVM.CanSeeFollowProductReport) role.CanSeeFollowProductReport = sourceVM.CanSeeFollowProductReport;
            //if (role.CanSeeDayAveragesReport != sourceVM.CanSeeDayAveragesReport) role.CanSeeDayAveragesReport = sourceVM.CanSeeDayAveragesReport;
        }

        #endregion 

        //bool hasPendingChanges;

        //public bool HasPendingChanges
        //{
        //    get { return hasPendingChanges; }
        //    set
        //    {
        //        hasPendingChanges = value;
        //        OnPropertyChanged("HasPendingChanges");
        //    }
        //}


        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;

                HasPendingChanges = true;

                OnPropertyChanged("Name");
            }
        }

        bool canLogin;
        public bool CanLogin
        {
            get { return canLogin; }
            set
            {
                canLogin = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanLogin");
            }
        }

        bool canSeeDashboard;
        public bool CanSeeDashboard
        {
            get { return canSeeDashboard; }
            set
            {
                canSeeDashboard = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanSeeDashboard");
            }
        }

        #region Sales

        bool canSeeSales;
        public bool CanSeeSales
        {
            get { return canSeeSales; }
            set
            {
                canSeeSales = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanSeeSales");
            }
        }

        bool canRemoveSales;
        public bool CanRemoveSales
        {
            get { return canRemoveSales; }
            set
            {
                canRemoveSales = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanRemoveSales");
            }
        }

        //bool canSeeOldSales;
        //public bool CanSeeOldSales
        //{
        //    get { return canSeeOldSales; }
        //    set
        //    {
        //        canSeeOldSales = value;
        //        OnPropertyChanged("CanSeeOldSales");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        #endregion

        //#region Purchases

        //bool canSeePurchases;
        //public bool CanSeePurchases
        //{
        //    get { return canSeePurchases; }
        //    set
        //    {
        //        canSeePurchases = value;
        //        OnPropertyChanged("CanSeePurchases");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        //bool canRemovePurchases;
        //public bool CanRemovePurchases
        //{
        //    get { return canRemovePurchases; }
        //    set
        //    {
        //        canRemovePurchases = value;
        //        OnPropertyChanged("CanRemovePurchases");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        //bool canSeeOldPurchases;
        //public bool CanSeeOldPurchases
        //{
        //    get { return canSeeOldPurchases; }
        //    set
        //    {
        //        canSeeOldPurchases = value;
        //        OnPropertyChanged("CanSeeOldPurchases");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        //#endregion

        #region Inventory

        bool canSeeInventory;

        public bool CanSeeInventory
        {
            get { return canSeeInventory; }
            set
            {
                canSeeInventory = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanSeeInventory");
            }
        }

        bool canCreateProducts;
        public bool CanCreateProducts
        {
            get { return canCreateProducts; }
            set
            {
                canCreateProducts = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanCreateProducts");
            }
        }

        bool canEditProducts;
        public bool CanEditProducts
        {
            get { return canEditProducts; }
            set
            {
                canEditProducts = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanEditProducts");
            }
        }

        bool canRemoveProducts;
        public bool CanRemoveProducts
        {
            get { return canRemoveProducts; }
            set
            {
                canRemoveProducts = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanRemoveProducts");
            }
        }

        #endregion

        #region Employees

        bool canSeeEmployees;

        public bool CanSeeEmployees
        {
            get { return canSeeEmployees; }
            set
            {                
                canSeeEmployees = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanSeeEmployees");
            }
        }

        bool canSeeRoles;

        public bool CanSeeRoles
        {
            get { return canSeeRoles; }
            set 
            {
                canSeeRoles = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanSeeRoles");
            }
        }

        bool canCreateEmployees;
        public bool CanCreateEmployees
        {
            get { return canCreateEmployees; }
            set
            {
                canCreateEmployees = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanCreateEmployees");
            }
        }

        bool canEditEmployees;
        public bool CanEditEmployees
        {
            get { return canEditEmployees; }
            set
            {
                canEditEmployees = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanEditEmployees");
            }
        }

        bool canRemoveEmployees;
        public bool CanRemoveEmployees
        {
            get { return canRemoveEmployees; }
            set
            {
                canRemoveEmployees = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanRemoveEmployees");
            }
        }

        #endregion

        #region MiPaladar

        //bool canSeeMiPaladar;
        //public bool CanSeeMiPaladar
        //{
        //    get { return canSeeMiPaladar; }
        //    set
        //    {
        //        canSeeMiPaladar = value;
        //        OnPropertyChanged("CanSeeMiPaladar");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        bool canExportImport;
        public bool CanExportImport
        {
            get { return canExportImport; }
            set
            {
                canExportImport = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanExportImport");
            }
        }

        //bool canDesignRestaurant;
        //public bool CanDesignRestaurant
        //{
        //    get { return canDesignRestaurant; }
        //    set
        //    {
        //        canDesignRestaurant = value;
        //        OnPropertyChanged("CanDesignRestaurant");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        #endregion

        #region Reports

        bool canSeeReports;

        public bool CanSeeReports
        {
            get { return canSeeReports; }
            set 
            {
                canSeeReports = value;

                HasPendingChanges = true;

                OnPropertyChanged("CanSeeReports");
            }
        }

        //bool canSeeSalesReport;

        //public bool CanSeeSalesReport
        //{
        //    get { return canSeeSalesReport; }
        //    set
        //    {
        //        canSeeSalesReport = value;
        //        OnPropertyChanged("CanSeeSalesReport");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        //bool canSeeSalesByItemReport;

        //public bool CanSeeSalesByItemReport
        //{
        //    get { return canSeeSalesByItemReport; }
        //    set
        //    {
        //        canSeeSalesByItemReport = value;
        //        OnPropertyChanged("CanSeeSalesByItemReport");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        //bool canSeeFollowProductReport;

        //public bool CanSeeFollowProductReport
        //{
        //    get { return canSeeFollowProductReport; }
        //    set
        //    {
        //        canSeeFollowProductReport = value;
        //        OnPropertyChanged("CanSeeFollowProductReport");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}

        //bool canSeeDayAveragesReport;

        //public bool CanSeeDayAveragesReport
        //{
        //    get { return canSeeDayAveragesReport; }
        //    set
        //    {
        //        canSeeDayAveragesReport = value;
        //        OnPropertyChanged("CanSeeDayAveragesReport");

        //        if (onFieldChanged != null) onFieldChanged();
        //    }
        //}
        
        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(x => RemoveRole(), x => CanRemove);
                }

                return removeCommand;
            }
        }

        bool CanRemove
        {
            get { return !creating && !isAdmin; }
        }

        void RemoveRole()
        {
            string message = "Está seguro que desea eliminar el rol \'" + name + "\' ?";

            var msgBox = base.GetService<IMessageBoxService>();

            if (msgBox.ShowYesNoDialog(message) != true) return;

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Role role = unitOfWork.RoleRepository.GetById(roleId);

                //check is someone is using this role
                foreach (var emp in unitOfWork.EmployeeRepository.Get())
                {
                    if (emp.Role.Id == role.Id)
                    {
                        msgBox.ShowMessage("Hay usuarios asignados a este rol");
                        return;
                    }
                }

                unitOfWork.RoleRepository.Remove(role.Id);
                unitOfWork.SaveChanges();
            }

            if (onRoleRemoved != null) onRoleRemoved(roleId);
        }

        #endregion


        #region Save Command

        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => SaveAndClose(), x => CanSave);
                return saveCommand;
            }
        }

        protected override bool CanSave { get { return !isAdmin; } }

        protected override void Save()
        {
            if (HasPendingChanges)
            {
                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    Role role;
                    if (creating)
                    {
                        role = new Role();
                        unitOfWork.RoleRepository.Add(role);
                    }
                    else
                    {
                        role = unitOfWork.RoleRepository.GetById(roleId);
                    }

                    CopyToRole(role);

                    unitOfWork.SaveChanges();

                    if (creating)
                    {
                        roleId = role.Id;
                    }
                    else
                    {
                        onRoleModified(roleId);
                    }
                }

                HasPendingChanges = false;
            }
        }

        void SaveAndClose()
        {
            Save();
            CloseMe();
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
            CloseMe();
        }

        #endregion

        //void CloseMe()
        //{
        //    selfClosing = true;

        //    var windowManager = base.GetService<IWindowManager>();            
        //    windowManager.Close(this);
        //}
        
    }
}
