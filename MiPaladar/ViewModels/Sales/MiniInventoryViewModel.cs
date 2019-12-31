using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Entities;
using MiPaladar.Converters;

namespace MiPaladar.ViewModels
{
    public class MiniInventoryViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public MiniInventoryViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            filteringByName = true;

            sourceList = new ObservableCollection<InventoryItem>(appvm.InventoryItemsOC);

            RefreshItems();

            appvm.InventoryItemsOC.CollectionChanged +=
                new NotifyCollectionChangedEventHandler(InventoryItemsOC_CollectionChanged);

            //base.RequestClose += new EventHandler(AlmacenViewModel_RequestClose);
        }

        protected override void OnDispose()
        {
            appvm.InventoryItemsOC.CollectionChanged -= InventoryItemsOC_CollectionChanged;
        }

        #region Event Handlers

        //void AlmacenViewModel_RequestClose(object sender, EventArgs e)
        //{
        //    appvm.InventoryItemsOC.CollectionChanged -= InventoryItemsOC_CollectionChanged;
        //}

        void InventoryItemsOC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                InventoryItem ce = e.NewItems[0] as InventoryItem;

                sourceList.Add(ce);

                if (PassesFilter(ce)) filteredItems.Add(ce);

                //ce.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                InventoryItem olditem = e.OldItems[0] as InventoryItem;

                sourceList.Remove(olditem);
                filteredItems.Remove(olditem);
            }
        }

        #endregion

        public override string DisplayName
        {
            get { return "Almacén"; }
        }  

        public ObservableCollection<Category> Categories 
        {
            get { return appvm.CategoriesOC; } 
        }

        #region Filter

        ObservableCollection<InventoryItem> sourceList;
        public ObservableCollection<InventoryItem> ItemsList
        {
            get { return sourceList; }
        }

        ObservableCollection<InventoryItem> filteredItems = new ObservableCollection<InventoryItem>();
        public ObservableCollection<InventoryItem> FilteredItems
        {
            get { return filteredItems; }
        }

        private void RefreshItems()
        {
            List<InventoryItem> toRemove = new List<InventoryItem>();

            //remove those that don't pass the filter
            foreach (var item in filteredItems)
            {
                if (!PassesFilter(item)) toRemove.Add(item);
            }

            foreach (var item in toRemove)
            {
                filteredItems.Remove(item);
            }

            //add those that pass the filter
            foreach (var item in sourceList)
            {
                if (PassesFilter(item))
                {
                    if (!filteredItems.Contains(item)) filteredItems.Add(item);
                }
            }
        }

        public bool PassesFilter(InventoryItem ce)
        {
            //InventoryItem ce = b as InventoryItem;

            if (!ce.Product.IsStorable) return false;

            if (!ce.Inventory.IsFloor) return false;

            //text
            if (filteringByName)
            {
                if (string.IsNullOrWhiteSpace(FindText)) return true;

                //there is some text to find
                string prefix = FindText.Trim();

                if (ce.Product != null && ce.Product.Name != null)
                    return ce.Product.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;

                return false;
            }

            //product lists
            if (filteringByCategory)
            {
                foreach (ProductIndex item in ce.Product.RelatedCategories)
                {
                    if (item.Category == selectedCategory) return true;
                }

                return false;
            }

            //never reach this point
            return true;
        }

        #endregion        

        //CollectionView cvTotals;
        //public CollectionView TotalsAlmacen
        //{
        //    get
        //    {
        //        if (cvTotals == null)                 
        //        {
        //            CollectionViewSource myCVS = new CollectionViewSource();
        //            myCVS.Source = new ObservableCollection<InventoryItem>(appvm.Context.InventoryItems);
        //            cvTotals = (CollectionView)myCVS.View;

        //            //filter
        //            cvTotals.Filter = new Predicate<object>(FilterPredicate);

        //            //sort
        //            cvTotals.SortDescriptions.Add(new SortDescription("Product.Name", ListSortDirection.Ascending));

        //            PropertyGroupDescription pgd = new PropertyGroupDescription("Category.Name");
        //            cvTotals.GroupDescriptions.Add(pgd);
        //        }
                
        //        return cvTotals;
        //    }
        //}

        string findText;
        public string FindText
        {
            get { return findText; }
            set
            {
                if (findText != value)
                {
                    findText = value;
                    if (filteringByName) RefreshItems();
                }
            }
        }

        bool filteringByName;
        public bool FilteringByName
        {
            get { return filteringByName; }
            set
            {
                filteringByName = value;
                OnPropertyChanged("FilteringByName");
            }
        }

        bool filteringByCategory;
        public bool FilteringByCategory
        {
            get { return filteringByCategory; }
            set
            {
                filteringByCategory = value;
                OnPropertyChanged("FilteringByCategory");
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;

                if (filteringByCategory) RefreshItems();
            }
        }

        #region Filter by Category Command

        RelayCommand filterByCategoryCommand;
        public ICommand FilterByCategoryCommand
        {
            get
            {
                if (filterByCategoryCommand == null)
                    filterByCategoryCommand = new RelayCommand(x => FilterByCategory());
                return filterByCategoryCommand;
            }
        }

        void FilterByCategory()
        {
            FilteringByName = false;
            FilteringByCategory = true;

            RefreshItems();
        }

        #endregion        

        #region Filter by Name Command

        RelayCommand filterByNameCommand;
        public ICommand FilterByNameCommand
        {
            get
            {
                if (filterByNameCommand == null)
                    filterByNameCommand = new RelayCommand(x => FilterByName());
                return filterByNameCommand;
            }
        }

        void FilterByName()
        {
            FilteringByName = true;
            FilteringByCategory = false;

            RefreshItems();
        }

        #endregion 

        #region Print IPV Command

        RelayCommand printIpvCommand;
        public ICommand PrintIPVCommand
        {
            get
            {
                if (printIpvCommand == null)
                    printIpvCommand = new RelayCommand(x => PrintIPV());
                return printIpvCommand;
            }
        }

        bool CanPrintIPV { get { return filteredItems.Count > 0; } }

        void PrintIPV()
        {
            Printer.PrintAlmacen(filteredItems);
        }

        #endregion
    }
}
