//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.Services;

//using System.Collections.ObjectModel;
//using System.Windows.Input;

//namespace MiPaladar.ViewModels
//{
//    public class InventoryOperationsListViewModel : ViewModelBase
//    {
//        MainWindowViewModel appvm;

//        public InventoryOperationsListViewModel(MainWindowViewModel appvm) 
//        {
//            FromDate = ToDate = DateTime.Today;

//            this.appvm = appvm;
//        }

//        public override string DisplayName
//        {
//            get { return "MOVIMIENTOS DE INVENTARIO"; }
//        }

//        ObservableCollection<InventoryOperationViewModel> inventoryOperations;

//        public ObservableCollection<InventoryOperationViewModel> InventoryOperations
//        {
//            get
//            {
//                if (inventoryOperations == null)
//                {
//                    inventoryOperations = new ObservableCollection<InventoryOperationViewModel>();

//                    FindOperations();
//                }
//                return inventoryOperations;
//            }
//        }

//        #region Find Command

//        public DateTime FromDate { get; set; }

//        public DateTime ToDate { get; set; }

//        public void FindOperations()
//        {
//            inventoryOperations.Clear();

//            DateTime toDatePlusOne = ToDate.AddDays(1);

//            var query_adj = from adj in appvm.Context.Orders.OfType<Adjustment>()
//                            where adj.Date >= FromDate && adj.Date < toDatePlusOne
//                            select adj;

//            foreach (var item in query_adj)
//            {
//                InventoryOperationViewModel iovm = new InventoryOperationViewModel();

//                CopyData(item, iovm);

//                inventoryOperations.Add(iovm);
//            }

//            var query_trans = from trans in appvm.Context.Orders.OfType<Transfer>()
//                              where trans.Date >= FromDate && trans.Date < toDatePlusOne
//                              select trans;

//            foreach (var item in query_trans)
//            {
//                InventoryOperationViewModel iovm = new InventoryOperationViewModel();

//                CopyData(item, iovm);

//                inventoryOperations.Add(iovm);
//            }

//            var query_prod = from prod in appvm.Context.Orders.OfType<Production>()
//                             where prod.Date >= FromDate && prod.Date < toDatePlusOne
//                             select prod;

//            foreach (var item in query_prod)
//            {
//                InventoryOperationViewModel iovm = new InventoryOperationViewModel();

//                CopyData(item, iovm);

//                inventoryOperations.Add(iovm);
//            }
//        }

//        #endregion

//        #region New Adjustment Command

//        RelayCommand newAdjustmentCommand;
//        public ICommand NewAdjustmentCommand
//        {
//            get
//            {
//                if (newAdjustmentCommand == null)
//                    newAdjustmentCommand = new RelayCommand(x => NewAdjustment());
//                return newAdjustmentCommand;
//            }
//        }

//        void NewAdjustment()
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            AdjustInventoryViewModel aivm = new AdjustInventoryViewModel(appvm, OnNewAdjustmentCreated);

//            windowManager.Show(aivm);
//        }

//        void OnNewAdjustmentCreated(Adjustment newAdj)
//        {
//            InventoryOperationViewModel iovm = new InventoryOperationViewModel();

//            CopyData(newAdj, iovm);

//            inventoryOperations.Add(iovm);

//            ExpandOperation(iovm);
//        }

//        #endregion

//        #region New Transfer Command

//        RelayCommand newTransferCommand;
//        public ICommand NewTransferCommand
//        {
//            get
//            {
//                if (newTransferCommand == null)
//                    newTransferCommand = new RelayCommand(x => DoNewTransfer());
//                return newTransferCommand;
//            }
//        }

//        void DoNewTransfer()
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            TransferViewModel fvm = new TransferViewModel(appvm, OnCreated, OnRemoved, OnChanged);

//            windowManager.Show(fvm);
//        }

//        #endregion

//        #region New Production Command

//        RelayCommand newProductionCommand;
//        public ICommand NewProductionCommand
//        {
//            get
//            {
//                if (newProductionCommand == null)
//                    newProductionCommand = new RelayCommand(x => DoNewProduction());
//                return newProductionCommand;
//            }
//        }

//        void DoNewProduction()
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            ProductionViewModel pvm = new ProductionViewModel(appvm, OnCreated, OnRemoved, OnChanged);

//            windowManager.Show(pvm);
//        }

//        #endregion

//        #region Expand Operation Command

//        public InventoryOperationViewModel SelectedInventoryOperation { get; set; }

//        RelayCommand expandOperationCommand;
//        public ICommand ExpandOperationCommand
//        {
//            get
//            {
//                if (expandOperationCommand == null)
//                    expandOperationCommand = new RelayCommand(x => this.ExpandOperation(SelectedInventoryOperation), x => this.CanExpand);
//                return expandOperationCommand;
//            }
//        }

//        bool CanExpand { get { return SelectedInventoryOperation != null; } }

//        void ExpandOperation(InventoryOperationViewModel targetOperation)
//        {
//            var windowManager = base.GetService<IWindowManager>();

//            Order selected_order = targetOperation.Order;

//            if (selected_order is Adjustment) 
//            {
//                Adjustment adjustment = (Adjustment)selected_order;

//                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
//                {
//                    if (!(wsvm is AdjustmentViewModel)) return false;

//                    AdjustmentViewModel pvm = (AdjustmentViewModel)wsvm;

//                    return pvm.WrappedAdjustment == adjustment;
//                };

//                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
//                else
//                {
//                    AdjustmentViewModel viewmodel = new AdjustmentViewModel(appvm, adjustment, OnRemoved, OnChanged);
//                    windowManager.ShowChildWindow(viewmodel, this);
//                }
//            }
//            else if (selected_order is Transfer) 
//            {
//                Transfer transfer = (Transfer)selected_order;

//                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
//                {
//                    if (!(wsvm is TransferViewModel)) return false;

//                    TransferViewModel pvm = (TransferViewModel)wsvm;

//                    return pvm.WrappedTransfer == transfer;
//                };

//                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
//                else
//                {
//                    TransferViewModel viewmodel = new TransferViewModel(appvm, transfer, OnRemoved, OnChanged);
//                    windowManager.ShowChildWindow(viewmodel, this);
//                }
//            }
//            else if (selected_order is Production)
//            {
//                Production production = (Production)selected_order;

//                Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
//                {
//                    if (!(wsvm is ProductionViewModel)) return false;

//                    ProductionViewModel pvm = (ProductionViewModel)wsvm;

//                    return pvm.WrappedProduction == production;
//                };

//                if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
//                else
//                {
//                    ProductionViewModel viewmodel = new ProductionViewModel(appvm, production, OnRemoved, OnChanged);
//                    windowManager.ShowChildWindow(viewmodel, this);
//                }
//            }
//        }        

//        #endregion

//        void OnCreated(Order order)         
//        {
//            InventoryOperationViewModel iovm = new InventoryOperationViewModel();

//            CopyData(order, iovm);

//            inventoryOperations.Add(iovm);
//        }

//        void OnChanged(Order order)
//        {
//            InventoryOperationViewModel targetOperation = null;
//            foreach (var item in inventoryOperations)
//            {
//                if (item.Order == order)
//                {
//                    targetOperation = item;
//                    break;
//                }
//            }

//            if (targetOperation != null) CopyData(order, targetOperation);
//        }

//        void OnRemoved(Order order)
//        {
//            foreach (var item in inventoryOperations)
//            {
//                if (item.Order == order)
//                {
//                    inventoryOperations.Remove(item);
//                    break;
//                }
//            }
//        }        

//        void CopyData(Order original, InventoryOperationViewModel copy)
//        {
//            copy.Date = original.Date;
//            copy.DateCreated = original.DateCreated;
//            copy.Responsible = original.Employee;
//            copy.Memo = original.Memo;
//            copy.Order = original;

//            if (original is Adjustment) 
//            {
//                Adjustment adj = (Adjustment)original;
//                copy.OperationType = "Ajuste";
//                copy.Inventory = adj.Inventory.Name;
//            }
//            else if (original is Transfer)
//            {
//                Transfer trans = (Transfer)original;
//                copy.OperationType = "Transferencia";
//                copy.Inventory = trans.InventoryFrom.Name + " -> " + trans.InventoryTo.Name;
//            } 
//            else if (original is Production)
//            {
//                Production prod = (Production)original;
//                copy.OperationType = "Producción";
//                copy.Inventory = prod.Inventory.Name;
//            }
//        }

//        //#region Show Inventory Areas List Command

//        //RelayCommand showInventoryListCommand;
//        //public ICommand ShowInventoryListCommand
//        //{
//        //    get
//        //    {
//        //        if (showInventoryListCommand == null)
//        //            showInventoryListCommand = new RelayCommand(x => ShowInventoryList());
//        //        return showInventoryListCommand;
//        //    }
//        //}

//        //void ShowInventoryList()
//        //{
//        //    var windowManager = base.GetService<IWindowManager>();

//        //    if (windowManager.Exists<InventoryAreasListViewModel>())
//        //        windowManager.Activate<InventoryAreasListViewModel>();
//        //    else
//        //    {
//        //        InventoryAreasListViewModel alvm = new InventoryAreasListViewModel(appvm);
//        //        windowManager.Show(alvm);
//        //    }
//        //}

//        //#endregion
//    }
//}
