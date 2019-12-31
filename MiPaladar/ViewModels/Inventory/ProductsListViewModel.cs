using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public class ProductsListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public ProductsListViewModel(MainWindowViewModel appvm)         
        {
            this.appvm = appvm;
        }

        public override string DisplayName
        {
            get { return "PRODUCTOS"; }
        }

        protected override void OnDispose()
        {
            appvm.InventoryAreasOC.CollectionChanged -= InventoryAreasOC_CollectionChanged;
            appvm.InventoryItemsOC.CollectionChanged -= InventoryItemsOC_CollectionChanged;

            foreach (var item in products_source)
            {
                item.Dispose();
            }
        }

        public ObservableCollection<Category> Categories
        {
            get { return appvm.CategoriesOC; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        #region New Product Command

        RelayCommand newProductCmd;
        public ICommand NewProductCommand
        {
            get
            {
                if (newProductCmd == null)
                    newProductCmd = new RelayCommand(x => NewProduct());
                return newProductCmd;
            }
        }

        public void NewProduct()
        {
            //ProductViewModelQuick pvm = new ProductViewModelQuick(appvm, onProductCreated);

            ProductViewModel pvm = new ProductViewModel(appvm, OnProductCreated, OnProductRemoved);

            var windowManager = base.GetService<IWindowManager>();

            windowManager.ShowChildWindow(pvm, appvm);

            //pvm.Creating = true;

            //SelectedProductViewModel = pvm;

            //ProductView productView = new ProductView();
            //productView.DataContext = pvm;

            //if (productView.ShowDialog() == true) 
            //{
            //    Product p = new Product();                

            //    appvm.ProductsOC.Add(p);

            //    CopyToProduct(p, pvm);                
            //}
        }

        void OnProductCreated(Product p) 
        {
            ProductRowViewModel iitvm = new ProductRowViewModel(appvm, p);
            products_source.Add(iitvm);

            if (PassesFilter(iitvm)) products_filtered.Add(iitvm);

            ItemsCount++;
        }

        void OnProductRemoved(Product p)
        {
            ProductRowViewModel iitvm = products_source.Single(x => x.Product == p);

            iitvm.Dispose();

            products_source.Remove(iitvm);

            products_filtered.Remove(iitvm);

            //TotalCost -= (decimal)iitvm.TotalCost;
            ItemsCount--;
        }

        #endregion

        #region Event handlers

        void InventoryAreasOC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove) 
            {
                Inventory invAreaRemoved = e.OldItems[0] as Inventory;

                foreach (var item in invAreaRemoved.InventoryItems)
                {
                    var iiTotalized = products_source.First(x => x.Product == item.Product);
                    iiTotalized.RemoveItem(item);
                }
            }
        }

        void InventoryItemsOC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //this happens when you make an operation for the first time with a storable product
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                InventoryItem ce = e.NewItems[0] as InventoryItem;

                var query = products_source.Where(x => x.Product == ce.Product);

                if (query.Count() == 0)
                {
                    ProductRowViewModel iiTotalized =
                        new ProductRowViewModel(appvm, ce.Product, new List<InventoryItem>() { ce });
                    products_source.Add(iiTotalized);

                    if (PassesFilter(iiTotalized)) products_filtered.Add(iiTotalized);
                }
                else
                {
                    //it should be 1 at most btw
                    ProductRowViewModel iiTotalized = query.First();
                    iiTotalized.AddNewItem(ce);
                }

                //ce.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }
            //this happens when a product stops being storable or when an inventory area is removed
            //else if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    InventoryItem olditem = e.OldItems[0] as InventoryItem;

            //    InventoryItemTotalizedViewModel iiTotalized = products_source.FirstOrDefault(x => x.Product == olditem.Product);

            //    if (iiTotalized != null) iiTotalized.RemoveItem(olditem);

            //    //olditem.PropertyChanged -= item_PropertyChanged;
            //}
        }

        //void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{            
        //    InventoryItem iiSender = (InventoryItem)sender;
        //    InventoryItemTotalizedViewModel iiTotalized = inventoryViewModels.Single(x => x.Product == iiSender.Product);

        //    if (e.PropertyName == "Quantity")
        //    {                
        //        iiTotalized.Quantity += iiSender.Quantity;                
        //    }
        //    else if (e.PropertyName == "MinimumQuantity")
        //    {
        //        iiTotalized.MinimumQuantity += iiSender.MinimumQuantity;
        //    }
        //}

        //void AlmacenViewModel_RequestClose(object sender, EventArgs e)
        //{
        //    appvm.InventoryAreasOC.CollectionChanged -= InventoryAreasOC_CollectionChanged;
        //    appvm.InventoryItemsOC.CollectionChanged -= InventoryItemsOC_CollectionChanged;

        //    foreach (var item in products_source)
        //    {
        //        item.UnsubscribeEvents();
        //    }
        //}

        #endregion 

        int items_count;
        public int ItemsCount
        {
            get { return items_count; }
            set
            {
                items_count = value;
                OnPropertyChanged("ItemsCount");
            }
        }

        //decimal totalCost;
        //public decimal TotalCost
        //{
        //    get { return totalCost; }
        //    set
        //    {
        //        totalCost = value;
        //        OnPropertyChanged("TotalCost");
        //    }
        //}

        #region Filtering
        ObservableCollection<ProductRowViewModel> products_source = new ObservableCollection<ProductRowViewModel>();
        //public ObservableCollection<InventoryItemViewModel> ItemsList
        //{
        //    get { return sourceList; }
        //}

        ObservableCollection<ProductRowViewModel> products_filtered;
        public ObservableCollection<ProductRowViewModel> FilteredItems
        {
            get
            {
                if (products_filtered == null)
                {
                    products_filtered = new ObservableCollection<ProductRowViewModel>();

                    OnFirstTime();
                }

                return products_filtered;
            }
        }

        void OnFirstTime()
        {
            CreateInventoryItems();            

            RefreshItems();

            //events
            appvm.InventoryAreasOC.CollectionChanged += new NotifyCollectionChangedEventHandler(InventoryAreasOC_CollectionChanged);
            appvm.InventoryItemsOC.CollectionChanged += new NotifyCollectionChangedEventHandler(InventoryItemsOC_CollectionChanged);

            //base.RequestClose += new EventHandler(AlmacenViewModel_RequestClose);
        }        

        void CreateInventoryItems()
        {
            var query = from ii in appvm.InventoryItemsOC
                        group ii by ii.Product into g
                        //orderby g.Key.Name
                        select g;

            foreach (var group in query)
            {
                Product group_product = group.Key;
                //double quantity = group.Sum(x => x.Quantity);
                //double minimum_quantity = group.Sum(x => x.MinimumQuantity);

                ProductRowViewModel iiTotalized =
                    new ProductRowViewModel(appvm, group_product, group.ToList());
                products_source.Add(iiTotalized);
            }

            //add rest of products that don't have inventory items
            foreach (var prod in appvm.ProductsOC)
            {
                if (products_source.Where(x => x.Product == prod).Count() > 0) continue;

                ProductRowViewModel iiTotalized = new ProductRowViewModel(appvm, prod);

                products_source.Add(iiTotalized);
            }
        }

        //void OnCostChanged()
        //{
        //    TotalCost = (decimal)products_filtered.Sum(x => x.TotalCost);
        //}

        private void RefreshItems()
        {
            products_filtered.Clear();

            //add those that pass the filter
            foreach (var item in products_source)
            {
                if (PassesFilter(item))
                {
                    //if (!filteredItems.Contains(item)) 
                    products_filtered.Add(item);
                }
            }

            //TotalCost = (decimal)products_filtered.Sum(x => x.TotalCost);
            ItemsCount = products_filtered.Count;
        }

        private bool PassesFilter(ProductRowViewModel inventoryItem)
        {
            Product p = inventoryItem.Product;
            //if (!inventoryItem.Product.IsStorable) return false;

            ////current inventory

            //if (inventoryItem.Inventory != selectedInventory) return false;


            //text
            bool cond = false;
            if (string.IsNullOrWhiteSpace(findText)) cond = true;
            else
            {
                string prefix = FindText.Trim();

                if (p != null && p.Name != null)
                    cond = p.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            if (!cond) return false;

            //product lists
            //if (filteringByCategory)
            //{
            //    //cond = (from rc in ce.Product.RelatedCategories
            //    //         where rc.Category == SelectedCategory
            //    //         select rc).Count() > 0;

            //    //ProductList listaProdsCurrent = (ProductList)cbListasProds.SelectedItem;   

            //    cond = false;

            //    foreach (ProductIndex item in inventoryItem.Product.RelatedCategories)
            //    {
            //        if (item.Category == selectedCategory)
            //        {
            //            //ce.CategoryIndex = item.Index;
            //            cond = true;
            //            break;
            //        }
            //    }
            //}

            bool cond2 = true;

            if (filteringByCategory && selectedCategory != null)
            {
                cond2 = false;

                foreach (var item in p.RelatedCategories)
                {
                    if (item.Category == selectedCategory)
                    {
                        cond2 = true;
                        break;
                    }
                }
                //var query = from c in p.RelatedCategories
                //            where c.Category.Id == SelectedCategory.Id
                //            select c;
                //cond2 = query.Count() > 0;
            }

            if (!cond2) return false;

            //if (FilterProductVM != null) return FilterProductVM.PassesFilter(p);

            return true;
        }

        string findText;
        public string FindText
        {
            get { return findText; }
            set
            {
                if (findText != value)
                {
                    findText = value == string.Empty ? null : value;

                    OnPropertyChanged("FindText");

                    RefreshItems();

                    //TotalCost = (decimal)products_filtered.Sum(x => x.TotalCost);
                    //ItemsCount = products_filtered.Count;

                    //totalCost = 0;
                    //foreach (var item in cvTotals)
                    //{
                    //    InventoryItemTotalizedViewModel ii = (InventoryItemTotalizedViewModel)item;
                    //    totalCost += ii.TotalCost.HasValue ? ii.TotalCost.Value : 0;
                    //}

                    //OnPropertyChanged("TotalCost");
                }
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

                RefreshItems();
            }
        }

        Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (value != null)
                {
                    selectedCategory = value;
                    if (filteringByCategory) RefreshItems();
                }                
            }
        }

        #endregion        

        #region Expand Product Command

        public ProductRowViewModel SelectedItem { get; set; }

        RelayCommand expandCommand;
        public ICommand ExpandCommand
        {
            get
            {
                if (expandCommand == null)
                    expandCommand = new RelayCommand(x => ExpandProduct(), x => this.CanExpand);
                return expandCommand;
            }
        }

        bool CanExpand { get { return SelectedItem != null; } }

        void ExpandProduct()
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is ProductViewModel)) return false;

                ProductViewModel svm = (ProductViewModel)wsvm;

                return svm.WrappedProduct == SelectedItem.Product;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                ProductViewModel pvm = new ProductViewModel(appvm, SelectedItem.Product, OnProductRemoved);
                windowManager.ShowChildWindow(pvm, appvm);
            }
        }

        #endregion

        //bool showOnlyNegative;
        //public bool ShowOnlyNegative 
        //{
        //    get { return showOnlyNegative; }
        //    set
        //    {
        //        showOnlyNegative = value;
        //        if (cvTotals != null) cvTotals.Refresh();
        //    }
        //}

        //bool filteringByCategory;
        //public bool FilteringByCategory
        //{
        //    get { return filteringByCategory; }
        //    set
        //    {
        //        filteringByCategory = value;

        //        //if (!filteringByCategory) cvTotals.SortDescriptions.Clear();
        //        //else
        //        //{
        //        //    SortDescription sd = new SortDescription("CategoryIndex", ListSortDirection.Ascending);
        //        //    cvTotals.SortDescriptions.Add(sd);
        //        //}

        //        cvTotals.Refresh();
        //    }
        //}

        //Category selectedCategory;
        //public Category SelectedCategory
        //{
        //    get { return selectedCategory; }
        //    set 
        //    {
        //        selectedCategory = value; 
        //        if (FilteringByCategory) cvTotals.Refresh(); 
        //    }
        //}

        //Inventory selectedInventory;
        //public Inventory SelectedInventory
        //{
        //    get { return selectedInventory; }
        //    set
        //    {
        //        selectedInventory = value;
        //        if (cvTotals != null) cvTotals.Refresh();
        //    }
        //}

        int CountVisibleColumns()
        {
            //inventory areas + product name
            return appvm.InventoryAreasOC.Count() + 1;
        }

        #region Export to Excel Command

        RelayCommand exportToExcel;
        public ICommand ExportToExcelCommand
        {
            get
            {
                if (exportToExcel == null)
                    exportToExcel = new RelayCommand(x => Export());
                return exportToExcel;
            }
        }

        BackgroundWorker excelWorker;
        ProgressDialogViewModel pdvm;

        private void Export()
        {
            if (excelWorker == null)
            {
                excelWorker = new BackgroundWorker();

                excelWorker.DoWork += new DoWorkEventHandler(excelWorker_DoWork);

                excelWorker.WorkerReportsProgress = true;
                excelWorker.ProgressChanged += new ProgressChangedEventHandler(excelWorker_ProgressChanged);

                excelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(excelWorker_RunWorkerCompleted);
            }

            pdvm = new ProgressDialogViewModel();
            pdvm.Message = "Exportando a Excel...";
            pdvm.IsBusy = true;

            var windowManager = base.GetService<IWindowManager>();

            excelWorker.RunWorkerAsync();

            windowManager.ShowDialog(pdvm, appvm);

        }

        void excelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Action<Excel.Range> displayHeader = (cell) =>
            {
                int column = 0;

                //one column for product name
                cell.Offset[0, column].Value = "Producto";
                cell.Offset[0, column++].Font.Bold = "True";

                //column for each inventory area
                foreach (var item in appvm.InventoryAreasOC)
                {
                    cell.Offset[0, column].Value = item.Name;
                    cell.Offset[0, column++].Font.Bold = "True";
                }
            };

            Action<ProductRowViewModel, Excel.Range> displayItem = (pr, cell) =>
            {
                int column = 0;

                //product name
                if (pr.RedNumbers) cell.EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                cell.Offset[0, column++].Value = pr.Product.Name;

                //qtty in each inventory Area
                foreach (var item in appvm.InventoryAreasOC)
                {
                    InventoryItemViewModel ii = pr.InventoryItems[item.Id];
                    if (ii == null) { column++; continue; }
                    cell.Offset[0, column++].Value = string.Format("{0:0.##} {1}", ii.Quantity, ii.UnitMeasure.Caption);
                }
            };

            int numberOfColumns = CountVisibleColumns();

            var excelExporter = base.GetService<IExcelExporter>();

            var sorted = from p in products_filtered
                         orderby p.Product.Name
                         select p;

            excelExporter.ExportToExcel<ProductRowViewModel>(sorted, displayHeader, displayItem, numberOfColumns, excelWorker);
        }

        void excelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pdvm.Progress = e.ProgressPercentage;
        }

        void excelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pdvm.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();

            //close progress dialog
            windowManager.Close(pdvm);
        }

        #endregion
    }
}
