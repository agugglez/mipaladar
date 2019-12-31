using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Windows.Input;
using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class TransfersListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;
        public TransfersListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

            FindTransfers();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        ObservableCollection<Transfer> transfersOC = new ObservableCollection<Transfer>();
        public IEnumerable<Transfer> Transfers
        {
            get { return transfersOC; }
        }

        public void FindTransfers()
        {
            transfersOC.Clear();

            DateTime toDatePlusOne = ToDate.AddDays(1);

            var query = from trans in appvm.Context.Orders.OfType<Transfer>()
                        where trans.Date >= FromDate && trans.Date < toDatePlusOne
                        select trans;

            foreach (var item in query)
            {
                transfersOC.Add(item);
            }
        }

        #region New Transfer Command

        RelayCommand newTransferCommand;
        public ICommand NewTransferCommand
        {
            get
            {
                if (newTransferCommand == null)
                    newTransferCommand = new RelayCommand(x => DoNewTransfer());
                return newTransferCommand;
            }
        }

        void DoNewTransfer()
        {
            var windowManager = base.GetService<IWindowManager>();

            TransferViewModel fvm = new TransferViewModel(appvm, OnCreated, OnRemoved, OnAssociationChanged);

            windowManager.Show(fvm);
        }

        void OnCreated(Transfer tra)
        {
            transfersOC.Add(tra);
        }

        //some properties don't fire propertychanged events
        void OnAssociationChanged(Transfer tra)
        {
            int index = transfersOC.IndexOf(tra);

            if (index >= 0) 
            {
                transfersOC.RemoveAt(index);
                transfersOC.Insert(index, tra);
            }            
        }

        #endregion

        #region Expand Item Command

        public Transfer SelectedTransfer { get; set; }

        RelayCommand expandItemCommand;
        public ICommand ExpandItemCommand
        {
            get
            {
                if (expandItemCommand == null)
                    expandItemCommand = new RelayCommand(x => this.ExpandItem());
                return expandItemCommand;
            }
        }

        void ExpandItem()
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is TransferViewModel)) return false;

                TransferViewModel svm = (TransferViewModel)wsvm;

                return svm.WrappedTransfer == SelectedTransfer;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                TransferViewModel avm = new TransferViewModel(appvm, SelectedTransfer, OnRemoved, OnAssociationChanged);

                windowManager.ShowChildWindow(avm, this);
            }
        }

        void OnRemoved(Transfer tra)
        {
            transfersOC.Remove(tra);
        }

        #endregion
    }
}
