using System;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace MiPaladar.ViewModels
{
    public class NewSaleDialogViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public NewSaleDialogViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "Crear nuevo Vale"; }
        }

        //public ObservableCollection<Table> Tables 
        //{
        //    get { return appvm.TablesOC; }
        //}

        //ICollectionView icvTables;
        //public ICollectionView IcvAvailableTables
        //{
        //    get
        //    {
        //        if (icvTables == null)
        //        {
        //            CollectionViewSource cvs = new CollectionViewSource();
        //            cvs.Source = appvm.TablesOC;
        //            icvTables = cvs.View;

        //            var sortDesc = new SortDescription("Number", ListSortDirection.Ascending);
        //            icvTables.SortDescriptions.Add(sortDesc);

        //            var groupDesc = new PropertyGroupDescription("PriceList");
        //            icvTables.GroupDescriptions.Add(groupDesc);
        //        }
        //        return icvTables;
        //    }
        //}        

        public ObservableCollection<Employee> Waiters
        {
            get { return appvm.CanSellEmployees; }
        }

        //Table selectedTable;
        //public Table SelectedTable
        //{
        //    get { return selectedTable; }
        //    set
        //    {
        //        selectedTable = value;
        //        OnPropertyChanged("SelectedTable");
        //    }
        //}

        Employee selectedWaiter;
        public Employee SelectedWaiter
        {
            get { return selectedWaiter; }
            set
            {
                selectedWaiter = value;
                OnPropertyChanged("SelectedWaiter");
            }
        }

        int numberOfPersons;
        public int NumberOfPersons
        {
            get { return numberOfPersons; }
            set
            {
                numberOfPersons = value;
                OnPropertyChanged("NumberOfPersons");
            }
        }

        RelayCommand newSaleCommand;
        public ICommand NewSaleCommand
        {
            get
            {
                if (newSaleCommand == null)
                    newSaleCommand = new RelayCommand(x => this.DoNewSale(), x => this.CanNewSale);

                return newSaleCommand;
            }
        }

        bool CanNewSale
        {
            get { return /*selectedTable != null &&*/ selectedWaiter != null && numberOfPersons > 0; }
        }

        void DoNewSale()
        {
            //Just close the dialog
        }
    }
}
