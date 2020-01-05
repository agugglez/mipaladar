//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.ComponentModel;
//using System.Collections.ObjectModel;
//using System.Windows.Input;
//using System.Windows.Data;

//using MiPaladar.Entities;

//namespace MiPaladar.ViewModels
//{
//    public class InventoryHistoryViewModel : ViewModelBase
//    {
//        MainWindowViewModel appvm;
//        public InventoryHistoryViewModel(MainWindowViewModel appvm)
//        {
//            this.appvm = appvm;

//            fromDate = DateTime.Today;
//            toDate = DateTime.Today;
//        }

//        public override string DisplayName
//        {
//            get { return "Historial Almacén"; }
//        }

//        DateTime fromDate;
//        public DateTime FromDate
//        {
//            get { return fromDate; }
//            set { fromDate = value; }
//        }

//        DateTime toDate;
//        public DateTime ToDate
//        {
//            get { return toDate; }
//            set { toDate = value; }
//        }

//        public ObservableCollection<Inventory> InventoryAreas
//        {
//            get { return appvm.InventoryAreasOC; }
//        }

//        ObservableCollection<InventoryTrace> inventoryTracesOC;
//        ICollectionView icvInventoryTraceItems;
//        public ICollectionView InventoryTraceItems
//        {
//            get
//            {
//                if (icvInventoryTraceItems == null)
//                {
//                    inventoryTracesOC = new ObservableCollection<InventoryTrace>();

//                    CollectionViewSource cvs = new CollectionViewSource();
//                    cvs.Source = inventoryTracesOC;
//                    icvInventoryTraceItems = cvs.View;

//                    icvInventoryTraceItems.Filter = FilterProduct;

//                    UpdateSearch();
//                }
//                return icvInventoryTraceItems;
//            }
//        }

//        #region Filtering

//        bool FilterProduct(object o)
//        {
//            InventoryTrace trace = (InventoryTrace)o;

//            bool cond;

//            if (trace.Inventory != selectedInventoryArea) return false;

//            if (string.IsNullOrWhiteSpace(searchText)) cond = true;
//            else cond = trace.Product == null ? false : trace.Product.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;

//            if (!cond) return false;

//            if (filteringByCategory)
//            {
//                cond = false;

//                if (trace.Product != null)

//                    foreach (var item in trace.Product.RelatedCategories)
//                    {
//                        if (item.Category == selectedCategory)
//                        {
//                            cond = true;
//                            break;
//                        }
//                    }
//            }            

//            return cond;
//        }

//        string searchText;
//        public string SearchText
//        {
//            get { return searchText; }
//            set
//            {
//                searchText = value;

//                if (icvInventoryTraceItems != null)
//                {
//                    icvInventoryTraceItems.Refresh();
//                }
//            }
//        }

//        bool filteringByCategory;
//        public bool FilteringByCategory
//        {
//            get { return filteringByCategory; }
//            set
//            {
//                filteringByCategory = value;
//                OnPropertyChanged("FilteringByCategory");

//                icvInventoryTraceItems.Refresh();
//            }
//        }

//        Category selectedCategory;
//        public Category SelectedCategory
//        {
//            get { return selectedCategory; }
//            set
//            {
//                selectedCategory = value;

//                if (filteringByCategory)
//                {
//                    icvInventoryTraceItems.Refresh();
//                }
//            }
//        }

//        Inventory selectedInventoryArea;
//        public Inventory SelectedInventoryArea
//        {
//            get { return selectedInventoryArea; }
//            set
//            {
//                selectedInventoryArea = value;

//                if (icvInventoryTraceItems != null) icvInventoryTraceItems.Refresh();
//            }
//        }

//        #endregion        

//        #region Find Command

//        RelayCommand findCommand;
//        public ICommand FindCommand
//        {
//            get
//            {
//                if (findCommand == null)
//                {
//                    findCommand = new RelayCommand(x => this.UpdateSearch());
//                }
//                return findCommand;
//            }
//        }

//        public void UpdateSearch()
//        {
//            inventoryTracesOC.Clear();

//            //get data
//            IEnumerable<InventoryTrace> query =
//                from lineitem in appvm.Context.InventoryTraces
//                where lineitem.Date >= fromDate && lineitem.Date <= toDate
//                select lineitem;

//            foreach (var item in query)
//            {
//                inventoryTracesOC.Add(item);
//            }
//        }

//        #endregion
                
//    }
//}
