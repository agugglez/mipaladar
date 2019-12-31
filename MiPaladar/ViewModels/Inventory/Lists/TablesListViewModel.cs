using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;

namespace MiPaladar.ViewModels
{
    public class TablesListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        //ObservableCollection<AreaViewModel> areasVM;

        public TablesListViewModel(MainWindowViewModel appvm) 
        {
            this.appvm = appvm;
        }

        protected override void OnDispose()
        {            
            base.OnDispose();

            appvm.TablesOC.CollectionChanged -= tablesOC_CollectionChanged;
        }

        public override string DisplayName
        {
            get { return "MESAS"; }
        }

        Table selectedTable;
        public Table SelectedTable 
        {
            get { return selectedTable; }
            set
            {
                selectedTable = value;
                OnPropertyChanged("SelectedTable");
            }
        }

        ObservableCollection<Table> tables;
        public ObservableCollection<Table> Tables
        {
            get 
            {
                if (tables == null)
                {
                    tables = new ObservableCollection<Table>(appvm.TablesOC);

                    appvm.TablesOC.CollectionChanged += tablesOC_CollectionChanged;
                }
                return tables; 
            }
        }

        void tablesOC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Table removedTable = (Table)e.OldItems[0];

                tables.Remove(removedTable);
            }
        }

        #region New Table Command

        RelayCommand newTableCommand;
        public ICommand NewTableCommand
        {
            get
            {
                if (newTableCommand == null)
                    newTableCommand = new RelayCommand(x => NewTable());
                return newTableCommand;
            }
        }

        void NewTable()
        {
            TableViewModel tvm = new TableViewModel(appvm, OnCreated, OnRemoved, OnAssociationChanged);

            IWindowManager windowManager = base.GetService<IWindowManager>();
            windowManager.ShowChildWindow(tvm, appvm);
        }

        #endregion

        #region Expand Table Command

        RelayCommand expandCommand;
        public ICommand ExpandCommand
        {
            get
            {
                if (expandCommand == null)
                    expandCommand = new RelayCommand(x => ExpandTable(), x => this.CanExpand);
                return expandCommand;
            }
        }

        bool CanExpand { get { return SelectedTable != null; } }

        void ExpandTable()
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is TableViewModel)) return false;

                TableViewModel svm = (TableViewModel)wsvm;

                return svm.WrappedTable == SelectedTable;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                TableViewModel pvm = new TableViewModel(appvm, SelectedTable, OnRemoved, OnAssociationChanged);
                windowManager.ShowChildWindow(pvm, appvm);
            }
        }

        void OnCreated(Table tab)
        {
            tables.Add(tab);
        }

        void OnRemoved(Table tab)
        {
            tables.Remove(tab);
        }

        void OnAssociationChanged(Table tab)
        {
            int index = appvm.TablesOC.IndexOf(tab);

            if (index >= 0)
            {
                tables.RemoveAt(index);
                tables.Insert(index, tab);

                SelectedTable = tab;
            }
        }

        #endregion
    }
}
