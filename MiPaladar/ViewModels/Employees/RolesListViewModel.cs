using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiPaladar.Services;
using MiPaladar.Entities;
using MiPaladar.MVVM;

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

        ObservableCollection<Role> allRoles;
        public ObservableCollection<Role> AllRoles
        {
            get
            {
                if (allRoles == null)
                {
                    using (var unitOfWork = base.GetNewUnitOfWork())
                    {
                        allRoles = new ObservableCollection<Role>(unitOfWork.RoleRepository.Get());
                    }
                }
                return allRoles;
            }
        }

        //void roleViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    HasPendingChanges = true;
        //}

        void OnRoleRemoved(int roleId)
        {
            var target = allRoles.Single(x => x.Id == roleId);

            allRoles.Remove(target);
        }

        void OnRoleModified(int roleId)
        {
            var target = allRoles.Single(x => x.Id == roleId);

            int index = allRoles.IndexOf(target);

            allRoles.RemoveAt(index);

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var nuevecito = unitOfWork.RoleRepository.GetById(roleId);
                allRoles.Insert(index, nuevecito);
            }
        }

        //private void CreateRoleViewModels()
        //{
        //    roles = new ObservableCollection<RoleViewModel>();

        //    foreach (var item in appvm.RolesOC)
        //    {
        //        RoleViewModel rvm = new RoleViewModel(OnFieldChanged);

        //        rvm.WrappedRole = item;

        //        CopyFromRole(item, rvm);

        //        roles.Add(rvm);
        //    }
        //}

        //#region Move Next Command

        //RelayCommand moveNextCommand;
        //public ICommand MoveNextCommand
        //{
        //    get
        //    {
        //        if (moveNextCommand == null)
        //            moveNextCommand = new RelayCommand(x => MoveNext(), x => CanMoveNext);
        //        return moveNextCommand;
        //    }
        //}

        //bool CanMoveNext { get { return !selectedRole.Creating; } }

        //void MoveNext() 
        //{
        //    //undo any changes made
        //    if (hasPendingChanges) Cancel();

        //    //move next
        //    SelectedRole = selectedRole == roles.Last() ? roles.First() : roles[roles.IndexOf(selectedRole) + 1];
        //}

        ////RoleViewModel GetNextRole(RoleViewModel targetRole) 
        ////{
        ////    return selectedRole == roles.Last() ? roles.First() : roles[roles.IndexOf(selectedRole) + 1];
        ////}

        //#endregion

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
            RoleViewModel vm = new RoleViewModel(appvm);

            var windowManager = base.GetService<IWindowManager>();
            windowManager.ShowDialog(vm, appvm);

            //if (windowManager.ShowDialog(vm, appvm) == true)
            //{
            //    //create new role
            //    Role newRole = new Role();
            //    newRole.Name = vm.Name;

            //    using (var unitOfWork = base.GetNewUnitOfWork())
            //    {
            //        unitOfWork.RoleRepository.Add(newRole);

            //        unitOfWork.SaveChanges();
            //    }

            //    RoleViewModel rvm = new RoleViewModel(appvm, newRole, OnRoleRemoved);

            //    roleVMs.Add(rvm);

            //    SelectedRole = rvm;
            //}
            
        }

        //string FindAvailableName() 
        //{
        //    int count = 1;
        //    bool exists;

        //    do
        //    {
        //        exists = false;
        //        foreach (var item in roles)
        //        {
        //            if (item.Name == "Nuevo Rol " + count)
        //            {
        //                exists = true;
        //                count++;
        //                break;
        //            }
        //        }
        //    } while (exists);

        //    return "Nuevo Rol " + count;
        //}

        #endregion

        #region Expand Command

        public Role SelectedItem { get; set; }

        RelayCommand expandCommand;
        public ICommand ExpandCommand
        {
            get
            {
                if (expandCommand == null)
                    expandCommand = new RelayCommand(x => ExpandItem(SelectedItem), x => CanOpen);
                return expandCommand;
            }
        }

        bool CanOpen { get { return SelectedItem != null; } }

        void ExpandItem(Role role)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase vm) =>
            {
                if (!(vm is RoleViewModel)) return false;

                RoleViewModel rvm = (RoleViewModel)vm;

                return rvm.RoleId == role.Id;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                RoleViewModel pvm = new RoleViewModel(appvm, role.Id, OnRoleRemoved, OnRoleModified);
                windowManager.ShowChildWindow(pvm, appvm);
            }
        }

        //Product GetProductFromId(int id) 
        //{
        //    return appvm.ProductsOC.Single(x => x.Id == id);
        //}

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

    }
}
