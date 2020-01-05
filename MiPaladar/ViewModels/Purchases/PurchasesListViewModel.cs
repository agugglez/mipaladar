//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Services;
//using MiPaladar.Entities;

//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Windows.Data;
//using System.Windows.Input;
//using MiPaladar.Enums;

//namespace MiPaladar.ViewModels
//{
//    public class PurchasesListViewModel : ViewModelBase
//    {
//        MainWindowViewModel appvm;
//        IInventoryService inventoryService;
//        //IPasswordAsker passwordAsker;
//        IConfirmator confirmator;

//        public PurchasesListViewModel(MainWindowViewModel appvm) 
//        {
//            this.appvm = appvm;
//            this.inventoryService = appvm.InventoryService;
//            this.confirmator = appvm.Confirmator;
//            //this.passwordAsker = passwordAsker;

//            FromDate = DateTime.Today;
//            ToDate = DateTime.Today;            

//            //base.RequestClose += new EventHandler(ComprasViewModel_RequestClose);
//        }

//        //void ComprasViewModel_RequestClose(object sender, EventArgs e)
//        //{
//        //    foreach (var item in compras)
//        //    {
//        //        item.RemoveEvents();
//        //    }
//        //}

//        public override string DisplayName
//        {
//            get { return "LISTA DE COMPRAS"; }
//        }

//        public MainWindowViewModel AppVM { get { return appvm; } }

//        public DateTime FromDate { get; set; }

//        public DateTime ToDate { get; set; }

//        //public MainWindowViewModel AppVM
//        //{
//        //    get { return appvm; }
//        //}

//        //public void ChangeDay(DateTime date)
//        //{
//        //    this.workingDate = date;

//        //    LoadCompras();

//        //    inventoryService.CreateInventorySnapshot(workingDate);
//        //}

//        public void LoadCompras()
//        {
//            compras.Clear();

//            DateTime toDatePlusOne = ToDate.AddDays(1);

//            var queryResult = from o in appvm.Context.Orders.OfType<Purchase>()
//                              where o.Date >= FromDate && o.Date < toDatePlusOne
//                              select o;

//            foreach (var item in queryResult)
//            {
//                //PurchaseViewModel pvm = new PurchaseViewModel(appvm, item);

//                compras.Add(item);

//                //TODO
//                //load property change events and all that here
//                //AddCompraEvents(item);
//            }

//            //if there wasnt any vale, create one
//            //if (compras.Count == 0) CreateNewCompra();

//            //show first vale
//            //if (compras.Count > 0) SelectedPurchase = compras.First();
//        }        

//        #region Show Create Purchase Dialog

//        //bool showingCreateDialog;

//        //public bool ShowingCreateDialog
//        //{
//        //    get { return showingCreateDialog; }
//        //    set 
//        //    {
//        //        showingCreateDialog = value;
//        //        OnPropertyChanged("ShowingCreateDialog");
//        //    }
//        //}

//        RelayCommand showCreateDialogCommand;
//        public ICommand NewPurchaseCommand 
//        {
//            get
//            {
//                if (showCreateDialogCommand == null)
//                    showCreateDialogCommand = new RelayCommand(x => ShowCreateDialog());
//                return showCreateDialogCommand;
//            }
//        }

//        void ShowCreateDialog() 
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            PurchaseViewModel pvm = new PurchaseViewModel(appvm, OnCreated, OnRemoved, OnAssociationChanged);

//            windowManager.Show(pvm);
//        }

//        #endregion

//        #region New Purchase Command

//        public Purchase CreateNewCompra(DateTime date_param, string title_param,
//            Employee employee_param, Inventory inventory_param)
//        {
//            //int newID = compras.Count == 0 ? 1 : compras.Last().TheNumber + 1;

//            Purchase newvale = new Purchase();
//            //newvale.TheNumber = newID;

//            //current day
//            newvale.Date = date_param;
//            newvale.DateCreated = DateTime.Now;

//            newvale.Memo = title_param;
//            newvale.Employee = employee_param;
//            newvale.Inventory = inventory_param;

//            appvm.Context.Orders.AddObject(newvale);

//            appvm.SaveChanges();

//            //ExpandPurchase(newvale);

//            //PurchaseViewModel pvm = new PurchaseViewModel(appvm, newvale);

//            //compras.Add(newvale);

//            return newvale;
//        }

//        #endregion

//        //#region Remove Command       

//        //RelayCommand removeCommand;
//        //public ICommand RemoveCommand 
//        //{
//        //    get 
//        //    {
//        //        if (removeCommand == null) 
//        //        {
//        //            removeCommand = new RelayCommand(x => this.RemoveSelectedPurchase(), 
//        //                x => SelectedPurchase != null);
//        //        }
//        //        return removeCommand;
//        //    }
//        //}
//        //public void RemoveSelectedPurchase()
//        //{
//        //    if (!confirmator.AskForConfirmation("Está seguro que desea eliminar la compra?")) return;
//        //    //if (!appvm.AdminMode && !passwordAsker.AskForPassword()) return;

//        //    var windowManager = base.GetService<IWindowManager>();
//        //    if (windowManager.Exists(selectedPurchase)) windowManager.Close(selectedPurchase);            

//        //    //remove lineitems
//        //    while (selectedPurchase.LineItems.Count > 0)
//        //    {
//        //        PurchaseLineItemViewModel pi = selectedPurchase.LineItems[0];

//        //        selectedPurchase.RemoveLineItem(pi);
//        //        //appvm.Context.PurchaseItems.DeleteObject(pi);
//        //    }

//        //    appvm.Context.Orders.DeleteObject(selectedPurchase.Purchase);

//        //    appvm.SaveChanges();

//        //    compras.Remove(selectedPurchase);

//        //    //if (compras.Count == 0) CreateNewCompra();

//        //    //OnPropertyChanged("LastVale");

//        //    //OnPropertyChanged("TotalVentas");
//        //}

//        //#endregion        

//        #region Hide Purchase Command

//        RelayCommand expandPurchaseCommand;
//        public ICommand ExpandPurchaseCommand
//        {
//            get
//            {
//                if (expandPurchaseCommand == null)
//                    expandPurchaseCommand = new RelayCommand(x => ExpandPurchase(selectedPurchase), x => CanExpand);
//                return expandPurchaseCommand;
//            }
//        }

//        bool CanExpand
//        {
//            get { return selectedPurchase != null; }
//        }

//        void ExpandPurchase(Purchase pur)
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
//            {
//                if (!(wsvm is PurchaseViewModel)) return false;

//                PurchaseViewModel pvm = (PurchaseViewModel)wsvm;

//                return pvm.WrappedPurchase == pur;
//            };

//            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
//            else
//            {
//                PurchaseViewModel pvm = new PurchaseViewModel(appvm, pur, OnRemoved, OnAssociationChanged);
//                windowManager.ShowChildWindow(pvm, appvm);
//            }

//            //ShowingPurchase = !showingPurchase;
//            //SelectedOrder = null;
//        }

//        void OnCreated(Purchase pur) 
//        {
//            compras.Add(pur);
//        }

//        void OnRemoved(Purchase pur) 
//        {
//            compras.Remove(pur);
//        }

//        void OnAssociationChanged(Purchase pur)
//        {
//            int index = compras.IndexOf(pur);

//            if (index >= 0)
//            {
//                compras.RemoveAt(index);
//                compras.Insert(index, pur);

//                SelectedPurchase = pur;
//            }
//        }

//        #endregion
//        //public void ComplexRemoveCompra(Purchase compra)
//        //{
//        //    foreach (PurchaseItem ci in compra.PurchaseItems)
//        //    {
//        //        if (ci.Product == null) continue;

//        //        ComplexProductOperation(compra.Date.AddDays(1), ci.Product, -ci.Quantity);
//        //    }
//        //}

//        Purchase selectedPurchase;
//        public Purchase SelectedPurchase 
//        {
//            get { return selectedPurchase; }
//            set 
//            {
//                selectedPurchase = value;
//                OnPropertyChanged("SelectedPurchase");
//            }
//        }

//        ObservableCollection<Purchase> compras;
//        public ObservableCollection<Purchase> Compras
//        {
//            get 
//            {
//                if (compras == null) 
//                {
//                    compras = new ObservableCollection<Purchase>();

//                    inventoryService.CreateInventorySnapshot(DateTime.Today);

//                    LoadCompras();
//                }
//                return compras; 
//            }
//        }

//        //DateTime workingDate;
//        //public DateTime WorkingDate
//        //{
//        //    get { return workingDate; }
//        //    set
//        //    {
//        //        workingDate = value;
//        //        ChangeDay(workingDate);
//        //    }
//        //}

//        //public string ShortWorkingDate
//        //{
//        //    get
//        //    {
//        //        StringBuilder sb = new StringBuilder();
//        //        sb.Append(((Dias)workingDate.DayOfWeek).ToString());
//        //        sb.Append(", " + workingDate.Day);
//        //        sb.Append(" " + ((Meses)workingDate.Month - 1).ToString());
//        //        sb.Append("/" + workingDate.Year);

//        //        return sb.ToString();
//        //    }
//        //}
//    }
//}
