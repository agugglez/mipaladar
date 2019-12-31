using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Windows.Input;

namespace MiPaladar.ViewModels
{
    public class RoleViewModel :  ViewModelBase
    {
        Action onFieldChanged;

        public RoleViewModel(Action onFieldChanged)
        {
            this.onFieldChanged = onFieldChanged;
        }

        public bool Creating { get; set; }

        public Role WrappedRole { get; set; }               

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        #region Permissions

        bool canLogin;
        public bool CanLogin
        {
            get { return canLogin; }
            set
            {
                canLogin = value;
                OnPropertyChanged("CanLogin");

                if (onFieldChanged != null) onFieldChanged();
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
                OnPropertyChanged("CanSeeSales");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canRemoveSales;
        public bool CanRemoveSales
        {
            get { return canRemoveSales; }
            set
            {
                canRemoveSales = value;
                OnPropertyChanged("CanRemoveSales");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canSeeOldSales;
        public bool CanSeeOldSales
        {
            get { return canSeeOldSales; }
            set
            {
                canSeeOldSales = value;
                OnPropertyChanged("CanSeeOldSales");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        #endregion

        #region Purchases

        bool canSeePurchases;
        public bool CanSeePurchases
        {
            get { return canSeePurchases; }
            set
            {
                canSeePurchases = value;
                OnPropertyChanged("CanSeePurchases");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canRemovePurchases;
        public bool CanRemovePurchases
        {
            get { return canRemovePurchases; }
            set
            {
                canRemovePurchases = value;
                OnPropertyChanged("CanRemovePurchases");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canSeeOldPurchases;
        public bool CanSeeOldPurchases
        {
            get { return canSeeOldPurchases; }
            set
            {
                canSeeOldPurchases = value;
                OnPropertyChanged("CanSeeOldPurchases");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        #endregion

        #region Inventory

        bool canSeeInventory;

        public bool CanSeeInventory
        {
            get { return canSeeInventory; }
            set
            {
                canSeeInventory = value;
                OnPropertyChanged("CanSeeInventory");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canCreateProducts;
        public bool CanCreateProducts
        {
            get { return canCreateProducts; }
            set
            {
                canCreateProducts = value;
                OnPropertyChanged("CanCreateProducts");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canEditProducts;
        public bool CanEditProducts
        {
            get { return canEditProducts; }
            set
            {
                canEditProducts = value;
                OnPropertyChanged("CanEditProducts");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canRemoveProducts;
        public bool CanRemoveProducts
        {
            get { return canRemoveProducts; }
            set
            {
                canRemoveProducts = value;
                OnPropertyChanged("CanRemoveProducts");

                if (onFieldChanged != null) onFieldChanged();
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
                OnPropertyChanged("CanSeeEmployees");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canSeeRoles;

        public bool CanSeeRoles
        {
            get { return canSeeRoles; }
            set 
            {
                canSeeRoles = value;
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
                OnPropertyChanged("CanCreateEmployees");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canEditEmployees;
        public bool CanEditEmployees
        {
            get { return canEditEmployees; }
            set
            {
                canEditEmployees = value;
                OnPropertyChanged("CanEditEmployees");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canRemoveEmployees;
        public bool CanRemoveEmployees
        {
            get { return canRemoveEmployees; }
            set
            {
                canRemoveEmployees = value;
                OnPropertyChanged("CanRemoveEmployees");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        #endregion

        #region MiPaladar

        bool canSeeMiPaladar;
        public bool CanSeeMiPaladar
        {
            get { return canSeeMiPaladar; }
            set
            {
                canSeeMiPaladar = value;
                OnPropertyChanged("CanSeeMiPaladar");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canExportImport;
        public bool CanExportImport
        {
            get { return canExportImport; }
            set
            {
                canExportImport = value;
                OnPropertyChanged("CanExportImport");

                if (onFieldChanged != null) onFieldChanged();
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

                OnPropertyChanged("CanSeeSalesReport");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canSeeSalesReport;

        public bool CanSeeSalesReport
        {
            get { return canSeeSalesReport; }
            set
            {
                canSeeSalesReport = value;
                OnPropertyChanged("CanSeeSalesReport");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

        bool canSeeSalesByItemReport;

        public bool CanSeeSalesByItemReport
        {
            get { return canSeeSalesByItemReport; }
            set
            {
                canSeeSalesByItemReport = value;
                OnPropertyChanged("CanSeeSalesByItemReport");

                if (onFieldChanged != null) onFieldChanged();
            }
        }

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

        #endregion

        
    }
}
