using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MiPaladar.ViewModels 
{
    public class TableViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        Table table;

        Action<Table> onAssociationChanged;
        Action<Table> onCreated;
        Action<Table> onRemoved;

        public TableViewModel(MainWindowViewModel appvm, Action<Table> onCreated, 
            Action<Table> onRemoved, Action<Table> onAssociationChanged)
        {
            this.appvm = appvm;
            this.onCreated = onCreated;
            this.onRemoved = onRemoved;                
            this.onAssociationChanged = onAssociationChanged;

            creating = true;
            changeOcurred = true;
        }
        public TableViewModel(MainWindowViewModel appvm, Table table, Action<Table> onRemoved, Action<Table> onAssociationChanged)
        {
            this.appvm = appvm;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            this.table = table;

            number = table.Number;
            description = table.Description;
            capacity = table.Capacity;
            area = table.PriceList;
            isBar = table.IsBar;
        }

        public Table WrappedTable { get { return table; } }

        #region Properties

        int number;
        public int Number 
        {
            get { return number; }
            set
            {
                if (number != value) 
                {
                    number = value;
                    OnPropertyChanged("Number");

                    ChangeOcurred = true;
                }                
            }
        }

        string description;
        public string Description 
        {
            get { return description; }
            set 
            {
                if (description != value) 
                {
                    description = value;
                    OnPropertyChanged("Description");

                    ChangeOcurred = true;
                }                
            }
        }

        int capacity;
        public int Capacity 
        {
            get { return capacity; }
            set
            {
                if (capacity != value) 
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");

                    ChangeOcurred = true;
                }                
            }
        }

        PriceList area;
        public PriceList Area 
        {
            get { return area; }
            set
            {
                if (area != value) 
                {
                    area = value;
                    OnPropertyChanged("Area");

                    ChangeOcurred = true;
                }                
            }
        }

        bool isBar;
        public bool IsBar 
        {
            get { return isBar; }
            set
            {
                if (isBar != value) 
                {
                    isBar = value;
                    OnPropertyChanged("IsBar");

                    ChangeOcurred = true;
                }                
            }
        }

        #endregion

        public ObservableCollection<PriceList> Areas 
        {
            get { return appvm.PriceListsOC; }
        }

        bool creating;

        bool changeOcurred;
        public bool ChangeOcurred 
        {
            get { return changeOcurred; }
            set
            {
                changeOcurred = value;
                OnPropertyChanged("ChangeOcurred");
            }
        }

        #region Delete Command

        RelayCommand deleteCommand;
        public ICommand DeleteCommand 
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new RelayCommand(x => Delete());
                return deleteCommand; 
            }
        }

        bool CanDelete { get { return !creating; } }

        void Delete() 
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar esta mesa ?";
            if (msgBox.ShowYesNoDialog(message) == true)
            {
                appvm.TablesOC.Remove(table);

                if (onRemoved != null) onRemoved(table);

                appvm.Context.Tables.DeleteObject(table);
                appvm.SaveChanges();

                var windowManager = base.GetService<IWindowManager>();
                windowManager.Close(this);

            }
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
            if (creating) 
            {
                //create the new table
                table = new Table();

                //add to database
                appvm.Context.Tables.AddObject(table);
                appvm.TablesOC.Add(table);              
            }

            //save changes
            table.Number = number;
            table.Description = description;
            table.Capacity = capacity;
            table.PriceList = area;
            table.IsBar = isBar;

            appvm.SaveChanges();

            if (creating)
            {
                creating = false;

                if (onCreated != null) onCreated(table);
            }
            else if (onAssociationChanged != null) onAssociationChanged(table);

            //reset
            ChangeOcurred = false;
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
            //new table
            if (creating)
            {
                var windowManager = base.GetService<IWindowManager>();
                windowManager.Close(this);
            }
            //editing table
            else
            {
                //restore originals
                Number = table.Number;
                Description = table.Description;
                Capacity = table.Capacity;
                Area = table.PriceList;
                IsBar = table.IsBar;

                //reset
                ChangeOcurred = false;
            }
        }

        #endregion

        
    }
}
