//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.ViewModels;
//using MiPaladar.Entities;

//using System.Collections.ObjectModel;

//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Windows.Data;

//namespace MiPaladar.Services
//{
//    public class UserManager : ViewModelBase
//    {
//        ObservableCollection<Employee> employeesOC;

//        public UserManager(ObservableCollection<Employee> employeesOC)
//        {
//            this.employeesOC = employeesOC;

//            canSellEmployees = new ObservableCollection<Employee>();
//            canPurchaseEmployees = new ObservableCollection<Employee>();

//            foreach (var item in employeesOC)
//            {
//                if (item.CanSell) canSellEmployees.Add(item);
//                if (item.CanPurchase) canPurchaseEmployees.Add(item);
//            }

//            //CreateCollections();

//            //appvm.ProductsOC.CollectionChanged += new NotifyCollectionChangedEventHandler(employees_CollectionChanged);
//        }

//        #region Can sell/purchase employees

//        ObservableCollection<Employee> canSellEmployees;

//        public ObservableCollection<Employee> CanSellEmployees 
//        {
//            get { return canSellEmployees; }
//        }

//        //ICollectionView icvCanSellEmployees;
//        //public ICollectionView CanSellEmployees
//        //{
//        //    get 
//        //    {
//        //        if (icvCanSellEmployees == null) 
//        //        {
//        //            CollectionViewSource cvs = new CollectionViewSource();
//        //            cvs.Source = employeesOC;
//        //            icvCanSellEmployees = cvs.View;

//        //            icvCanSellEmployees.Filter = CanSellFilter;

//        //            SortDescription sd = new SortDescription("Name", ListSortDirection.Ascending);
//        //            icvCanSellEmployees.SortDescriptions.Add(sd);
//        //        }
//        //        return icvCanSellEmployees; 
//        //    }
//        //}

//        //public bool CanSellFilter(object obj)
//        //{
//        //    Employee emp = (Employee)obj;

//        //    return emp.IsActive && emp.CanSell;
//        //}

//        ObservableCollection<Employee> canPurchaseEmployees;

//        public ObservableCollection<Employee> CanPurchaseEmployees
//        {
//            get { return canPurchaseEmployees; }
//        }

//        //ICollectionView icvCanPurchaseEmployees;
//        //public ICollectionView CanPurchaseEmployees
//        //{
//        //    get
//        //    {
//        //        if (icvCanPurchaseEmployees == null)
//        //        {
//        //            CollectionViewSource cvs = new CollectionViewSource();
//        //            cvs.Source = employeesOC;
//        //            icvCanPurchaseEmployees = cvs.View;

//        //            SortDescription sd = new SortDescription("Name", ListSortDirection.Ascending);
//        //            icvCanPurchaseEmployees.SortDescriptions.Add(sd);
                    
//        //            icvCanPurchaseEmployees.Filter = CanPurchaseFilter;
//        //        }
//        //        return icvCanPurchaseEmployees;
//        //    }
//        //}

//        //public bool CanPurchaseFilter(object obj)
//        //{
//        //    Employee emp = (Employee)obj;

//        //    return emp.IsActive && emp.CanPurchase;
//        //}

//        //void CreateCollections()
//        //{
//        //    foreach (var item in employeesOC)
//        //    {
//        //        if (item.IsActive)
//        //        {
//        //            if (item.CanSell) icvCanSellEmployees.Add(item);
//        //            if (item.CanPurchase) canPurchaseEmployees.Add(item);
//        //        }
//        //    }
//        //}

//        //public void ThisGuyChanged(Employee emp)
//        //{
//        //    CanSellEmployees.Refresh();
//        //    CanPurchaseEmployees.Refresh();

//        //    UpdatePermissionProperties();
//        //}

//        //public const int Access_PermissionCode = 1;
//        //public const int OpenSaleOrder_PermissionCode = 2;
//        //public const int RemoveCalledLineItem_PermissionCode = 3;
//        //public const int RemoveSaleOrder_PermissionCode = 4;
//        //public const int FullAccess_PermissionCode = 5;
//        //public const int ChangeDate_PermissionCode = 6;

//        //public static bool UserHasAccessPermission(Employee user)
//        //{
//        //    foreach (var permission in user.Permissions)
//        //    {
//        //        if (permission.Code == Access_PermissionCode) return true;
//        //    }
//        //    return false;
//        //}

//        //public static bool UserHasPermission(Employee user, int permission_code)
//        //{
//        //    foreach (var permission in user.Permissions)
//        //    {
//        //        if (permission.Code == permission_code) return true;
//        //    }
//        //    return false;
//        //}

//        //Employee loggedInUser;
//        //public Employee LoggedInUser
//        //{
//        //    get { return loggedInUser; }
//        //    set
//        //    {
//        //        loggedInUser = value;
//        //        OnPropertyChanged("LoggedInUser");

//        //        //if (loggedInUser != null) UpdatePermissionProperties();
//        //    }
//        //}

//        //public void UpdatePermissionProperties() 
//        //{
//        //    UserCanRemoveSaleOrders = UserHasPermission(loggedInUser, RemoveSaleOrder_PermissionCode);
//        //    //UserCanOpenSaleOrders = UserHasPermission(loggedInUser, OpenSaleOrder_PermissionCode);
//        //    //UserHasFullAccess = UserHasPermission(loggedInUser, FullAccess_PermissionCode);
//        //    //UserCanRemoveCalledLineItem = UserHasPermission(loggedInUser, RemoveCalledLineItem_PermissionCode);
//        //    UserCanChangeDate = UserHasPermission(loggedInUser, ChangeDate_PermissionCode);
//        //}

//        //bool userCanRemoveSaleOrders;

//        //public bool UserCanRemoveSaleOrders
//        //{
//        //    get { return userCanRemoveSaleOrders; }
//        //    set
//        //    {
//        //        userCanRemoveSaleOrders = value;
//        //        OnPropertyChanged("UserCanRemoveSaleOrders");
//        //    }
//        //}

//        //bool userCanOpenSaleOrders;

//        //public bool UserCanOpenSaleOrders
//        //{
//        //    get { return userCanOpenSaleOrders; }
//        //    set 
//        //    {
//        //        userCanOpenSaleOrders = value;
//        //        OnPropertyChanged("UserCanOpenSaleOrders");
//        //    }
//        //}

//        //bool userHasFullAccess;

//        //public bool UserHasFullAccess
//        //{
//        //    get { return userHasFullAccess; }
//        //    set 
//        //    {
//        //        userHasFullAccess = value;
//        //        OnPropertyChanged("UserHasFullAccess");
//        //    }
//        //}

//        //bool userCanRemoveCalledLineItem;
//        //public bool UserCanRemoveCalledLineItem 
//        //{
//        //    get { return userCanRemoveCalledLineItem; }
//        //    set 
//        //    {
//        //        userCanRemoveCalledLineItem = value;
//        //        OnPropertyChanged("UserCanRemoveCalledLineItem");
//        //    }
//        //}

//        //bool userCanChangeDate;
//        //public bool UserCanChangeDate
//        //{
//        //    get { return userCanChangeDate; }
//        //    set
//        //    {
//        //        userCanChangeDate = value;
//        //        OnPropertyChanged("UserCanChangeDate");
//        //    }
//        //}   

//        //void employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        //{
//        //    if (e.PropertyName == "CanPurchase") 
//        //    {
//        //        Employee emp = (Employee)sender;

//        //        if (emp.CanPurchase)
//        //        {
//        //            if (!canPurchaseEmployees.Contains(emp)) canPurchaseEmployees.Add(emp);
//        //        }
//        //        else                 
//        //        {
//        //            canPurchaseEmployees.Remove(emp);
//        //        }
                    
//        //    }
//        //    else if (e.PropertyName == "CanSell") 
//        //    {
//        //        Employee emp = (Employee)sender;

//        //        if (emp.CanSell)
//        //        {
//        //            if (!canSellEmployees.Contains(emp)) canSellEmployees.Add(emp);
//        //        }
//        //        else                 
//        //        {
//        //            canSellEmployees.Remove(emp);
//        //        }
//        //    }
//        //    else if (e.PropertyName == "IsActive")
//        //    {
//        //        Employee emp = (Employee)sender;

//        //        if (!emp.IsActive)
//        //        {
//        //            canPurchaseEmployees.Remove(emp);
//        //            canSellEmployees.Remove(emp);
//        //        }
//        //    }
//        //}

//        //void employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        //{
//        //    if (e.Action == NotifyCollectionChangedAction.Add)
//        //    {
//        //        Employee item = (Employee)e.NewItems[0];

//        //        item.PropertyChanged += new PropertyChangedEventHandler(employee_PropertyChanged);

//        //        if (item.IsActive) 
//        //        {
//        //            if (item.CanSell) canSellEmployees.Add(item);
//        //            if (item.CanPurchase) canPurchaseEmployees.Add(item);
//        //        }
//        //    }
//        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
//        //    {
//        //        Employee item = (Employee)e.OldItems[0];

//        //        item.PropertyChanged -= employee_PropertyChanged;

//        //        canPurchaseEmployees.Remove(item);
//        //        canSellEmployees.Remove(item);
//        //    }
//        //}
//    }
//}
