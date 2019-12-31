using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;
using MiPaladar.Entities;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class RolesListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public RolesListViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "ROLES Y PERMISOS"; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        ObservableCollection<RoleViewModel> roles;
        //public ObservableCollection<RoleViewModel> Roles 
        //{
        //    get 
        //    {
        //        if (roles == null)
        //        {
                    
        //        }
        //        return roles;
        //    }
        //}

        //void roleViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    HasPendingChanges = true;
        //}

        void OnFieldChanged() 
        {
            HasPendingChanges = true;
        }

        RoleViewModel selectedRole;
        public RoleViewModel SelectedRole
        {
            get 
            {
                if (roles == null) 
                {
                    CreateRoleViewModels();

                    HasPendingChanges = false;

                    selectedRole = roles.FirstOrDefault();
                }
                return selectedRole;
            }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
            }
        }

        private void CreateRoleViewModels()
        {
            roles = new ObservableCollection<RoleViewModel>();

            foreach (var item in appvm.RolesOC)
            {
                RoleViewModel rvm = new RoleViewModel(OnFieldChanged);

                rvm.WrappedRole = item;

                CopyFromRole(item, rvm);

                roles.Add(rvm);
            }
        }

        #region Move Next Command

        RelayCommand moveNextCommand;
        public ICommand MoveNextCommand
        {
            get
            {
                if (moveNextCommand == null)
                    moveNextCommand = new RelayCommand(x => MoveNext(), x => CanMoveNext);
                return moveNextCommand;
            }
        }

        bool CanMoveNext { get { return !selectedRole.Creating; } }

        void MoveNext() 
        {
            //undo any changes made
            if (hasPendingChanges) Cancel();

            //move next
            SelectedRole = selectedRole == roles.Last() ? roles.First() : roles[roles.IndexOf(selectedRole) + 1];
        }

        //RoleViewModel GetNextRole(RoleViewModel targetRole) 
        //{
        //    return selectedRole == roles.Last() ? roles.First() : roles[roles.IndexOf(selectedRole) + 1];
        //}

        #endregion

        #region New Role Command

        RelayCommand newRoleCommand;
        public ICommand NewRoleCommand
        {
            get
            {
                if (newRoleCommand == null)
                {
                    newRoleCommand = new RelayCommand(x => NewRole());
                }

                return newRoleCommand;
            }
        }

        void NewRole()
        {
            RoleViewModel rvm = new RoleViewModel(OnFieldChanged);

            rvm.Name = FindAvailableName();

            rvm.Creating = true;

            roles.Add(rvm);

            SelectedRole = rvm;

            HasPendingChanges = true;
        }

        string FindAvailableName() 
        {
            int count = 1;
            bool exists;

            do
            {
                exists = false;
                foreach (var item in roles)
                {
                    if (item.Name == "Nuevo Rol " + count)
                    {
                        exists = true;
                        count++;
                        break;
                    }
                }
            } while (exists);

            return "Nuevo Rol " + count;
        }

        #endregion

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(x => RemoveRole(selectedRole), x => CanRemove);
                }

                return removeCommand;
            }
        }

        bool CanRemove
        {
            get
            {
                return selectedRole.WrappedRole != null;
            }
        }

        void RemoveRole(RoleViewModel role)
        {            
            var msgBox = base.GetService<IMessageBoxService>();

            //if (role.Name == "Administrador")
            //{
            //    msgBox.ShowMessage("El rol 'Administrador' no se puede eliminar.");
            //    return;
            //}

            string message = "Está seguro que desea eliminar el rol \'" + role.Name + "\' ?";

            if (msgBox.ShowYesNoDialog(message) != true) return;

            //check is someone is using this role
            foreach (var emp in appvm.EmployeesOC)
            {
                if (emp.Role == role.WrappedRole) 
                {
                    msgBox.ShowMessage("No se puede eliminar este Rol porque está asignado a al menos 1 persona.");
                    return;
                }
            }

            roles.Remove(role);

            appvm.RolesOC.Remove(role.WrappedRole);

            appvm.Context.Roles.DeleteObject(role.WrappedRole);
            appvm.SaveChanges();

            SelectedRole = roles.First();
        }

        #endregion

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
            if (selectedRole.Creating)
            {
                Role role = new Role();

                selectedRole.WrappedRole = role;

                selectedRole.Creating = false;

                appvm.Context.Roles.AddObject(role);

                appvm.RolesOC.Add(role);
            }
            //else if (selectedRole.Name == "Administrador")
            //{
            //    var msgBox = base.GetService<IMessageBoxService>();
            //    msgBox.ShowMessage("El rol 'Administrador' no se puede eliminar.");
            //    return;
            //}

            CopyToRole(selectedRole.WrappedRole, selectedRole);

            appvm.SaveChanges();

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
            if (selectedRole.Creating)
            {
                roles.Remove(selectedRole);

                SelectedRole = roles.First();
            }
            else
            {
                //Employee prod = appvm.EmployeesOC.Single(x => x.Id == employeeId);
                CopyFromRole(selectedRole.WrappedRole,selectedRole);

                //ResetAllRoles();
            }

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

        #region Copy Methods

        private void CopyFromRole(Role role, RoleViewModel targetVM)
        {
            targetVM.Name = role.Name;

            //permissions
            targetVM.CanLogin = role.CanLogin;
            targetVM.CanSeeSales = role.CanSeeSales;
            targetVM.CanRemoveSales = role.CanRemoveSales;
            targetVM.CanSeeOldSales = role.CanSeeOldSales;

            targetVM.CanSeePurchases = role.CanSeePurchases;
            targetVM.CanRemovePurchases = role.CanRemovePurchases;
            targetVM.CanSeeOldPurchases = role.CanSeeOldPurchases;

            targetVM.CanSeeInventory = role.CanSeeInventory;
            targetVM.CanCreateProducts = role.CanCreateProducts;
            targetVM.CanEditProducts = role.CanEditProducts;
            targetVM.CanRemoveProducts = role.CanRemoveProducts;

            targetVM.CanSeeEmployees = role.CanSeeEmployees;
            targetVM.CanSeeRoles = role.CanSeeRoles;
            targetVM.CanCreateEmployees = role.CanCreateEmployees;
            targetVM.CanEditEmployees = role.CanEditEmployees;
            targetVM.CanRemoveEmployees = role.CanRemoveEmployees;

            targetVM.CanSeeMiPaladar = role.CanSeeMiPaladar;
            targetVM.CanExportImport = role.CanExportImport;
            //targetVM.CanDesignRestaurant = role.CanDesignRestaurant;

            targetVM.CanSeeReports = role.CanSeeReports;
            targetVM.CanSeeSalesReport = role.CanSeeSalesReport;
            targetVM.CanSeeSalesByItemReport = role.CanSeeSalesByItemReport;
            //targetVM.CanSeeFollowProductReport = role.CanSeeFollowProductReport;
            //targetVM.CanSeeDayAveragesReport = role.CanSeeDayAveragesReport;
        }

        void CopyToRole(Role role, RoleViewModel sourceVM)
        {
            if (role.Name != sourceVM.Name) role.Name = sourceVM.Name;

            //permissions
            if (role.CanLogin != sourceVM.CanLogin) role.CanLogin = sourceVM.CanLogin;
            if (role.CanSeeSales != sourceVM.CanSeeSales) role.CanSeeSales = sourceVM.CanSeeSales;
            if (role.CanRemoveSales != sourceVM.CanRemoveSales) role.CanRemoveSales = sourceVM.CanRemoveSales;
            if (role.CanSeeOldSales != sourceVM.CanSeeOldSales) role.CanSeeOldSales = sourceVM.CanSeeOldSales;

            if (role.CanSeePurchases != sourceVM.CanSeePurchases) role.CanSeePurchases = sourceVM.CanSeePurchases;
            if (role.CanRemovePurchases != sourceVM.CanRemovePurchases) role.CanRemovePurchases = sourceVM.CanRemovePurchases;
            if (role.CanSeeOldPurchases != sourceVM.CanSeeOldPurchases) role.CanSeeOldPurchases = sourceVM.CanSeeOldPurchases;

            if (role.CanSeeInventory != sourceVM.CanSeeInventory) role.CanSeeInventory = sourceVM.CanSeeInventory;
            if (role.CanCreateProducts != sourceVM.CanCreateProducts) role.CanCreateProducts = sourceVM.CanCreateProducts;
            if (role.CanEditProducts != sourceVM.CanEditProducts) role.CanEditProducts = sourceVM.CanEditProducts;
            if (role.CanRemoveProducts != sourceVM.CanRemoveProducts) role.CanRemoveProducts = sourceVM.CanRemoveProducts;

            if (role.CanSeeEmployees != sourceVM.CanSeeEmployees) role.CanSeeEmployees = sourceVM.CanSeeEmployees;
            if (role.CanSeeRoles != sourceVM.CanSeeRoles) role.CanSeeRoles = sourceVM.CanSeeRoles;
            if (role.CanCreateEmployees != sourceVM.CanCreateEmployees) role.CanCreateEmployees = sourceVM.CanCreateEmployees;
            if (role.CanEditEmployees != sourceVM.CanEditEmployees) role.CanEditEmployees = sourceVM.CanEditEmployees;
            if (role.CanRemoveEmployees != sourceVM.CanRemoveEmployees) role.CanRemoveEmployees = sourceVM.CanRemoveEmployees;

            if (role.CanSeeMiPaladar != sourceVM.CanSeeMiPaladar) role.CanSeeMiPaladar = sourceVM.CanSeeMiPaladar;
            if (role.CanExportImport != sourceVM.CanExportImport) role.CanExportImport = sourceVM.CanExportImport;
            //if (role.CanDesignRestaurant != sourceVM.CanDesignRestaurant) role.CanDesignRestaurant = sourceVM.CanDesignRestaurant;

            if (role.CanSeeReports != sourceVM.CanSeeReports) role.CanSeeReports = sourceVM.CanSeeReports;
            if (role.CanSeeSalesReport != sourceVM.CanSeeSalesReport) role.CanSeeSalesReport = sourceVM.CanSeeSalesReport;
            if (role.CanSeeSalesByItemReport != sourceVM.CanSeeSalesByItemReport) role.CanSeeSalesByItemReport = sourceVM.CanSeeSalesByItemReport;
            //if (role.CanSeeFollowProductReport != sourceVM.CanSeeFollowProductReport) role.CanSeeFollowProductReport = sourceVM.CanSeeFollowProductReport;
            //if (role.CanSeeDayAveragesReport != sourceVM.CanSeeDayAveragesReport) role.CanSeeDayAveragesReport = sourceVM.CanSeeDayAveragesReport;
        }

        #endregion 
    }
}
