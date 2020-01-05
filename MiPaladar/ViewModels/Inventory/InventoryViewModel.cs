using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Services;
using MiPaladar.MVVM;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace MiPaladar.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public InventoryViewModel(MainWindowViewModel appvm)         
        {
            this.appvm = appvm;            
        }       

        public override string DisplayName
        {
            get { return "PRODUCTOS"; }
        }

        protected override void OnDispose()
        {
            UnLoadEvents();

            if (inventoryItems == null) return;

            foreach (var item in inventoryItems)
            {
                item.Dispose();
            }
        }

        #region Global Events

        void SetUpEvents()
        {
            //categories
            appvm.GlobalEventsManager.CategoryCreated += appvm_CategoryCreated;
            appvm.GlobalEventsManager.CategoryRemoved += appvm_CategoryRemoved;
            appvm.GlobalEventsManager.CategoryModified += appvm_CategoryModified;

            //products
            appvm.GlobalEventsManager.ProductCreated += GlobalEventsManager_ProductCreated;
            appvm.GlobalEventsManager.ProductRemoved += GlobalEventsManager_ProductRemoved;
            appvm.GlobalEventsManager.ProductModified += GlobalEventsManager_ProductModified;
        }

        private void UnLoadEvents()
        {
            //categories
            appvm.GlobalEventsManager.CategoryCreated -= appvm_CategoryCreated;
            appvm.GlobalEventsManager.CategoryRemoved -= appvm_CategoryRemoved;
            appvm.GlobalEventsManager.CategoryModified -= appvm_CategoryModified;

            //products
            appvm.GlobalEventsManager.ProductCreated -= GlobalEventsManager_ProductCreated;
            appvm.GlobalEventsManager.ProductRemoved -= GlobalEventsManager_ProductRemoved;
            appvm.GlobalEventsManager.ProductModified -= GlobalEventsManager_ProductModified;

            ////workers
            //if (complPercentWorker != null)
            //{
            //    complPercentWorker.DoWork -= complPercentWorker_DoWork;
            //    complPercentWorker.RunWorkerCompleted -= complPercentWorker_RunWorkerCompleted;
            //}
        }

        ////PRODUCTS
        void GlobalEventsManager_ProductCreated(object sender, ProductInfoEventArgs e)
        {
            OnProductCreated(e.ProductId);
        }

        void OnProductCreated(int prodId)
        {
            var inventorySvc = base.GetService<IInventoryService>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Product prod = unitOfWork.ProductRepository.GetById(prodId);

                if (PassesFilter(prod))
                {
                    inventorySvc.AddProductItem(prod, inventoryItems);

                    ItemsCount++;
                }
            }            

            //CalculateInventoryCompletionPercent();
        }

        void GlobalEventsManager_ProductRemoved(object sender, ProductInfoEventArgs e)
        {
            OnProductRemoved(e.ProductId);
        }

        private void OnProductRemoved(int prodId)
        {
            var inventorySvc = base.GetService<IInventoryService>();

            if (inventorySvc.RemoveProductItem(prodId, inventoryItems))
            {
                ItemsCount--;
            }

            //CalculateInventoryCompletionPercent();
        }

        void GlobalEventsManager_ProductModified(object sender, ProductInfoEventArgs e)
        {
            OnProductModified(e.ProductId);
        }

        private void OnProductModified(int prodId)
        {
            //remove the node and add it right away
            //in case name, category or product type changes

            var inventorySvc = base.GetService<IInventoryService>();

            if (inventorySvc.RemoveProductItem(prodId, inventoryItems))
            {
                ItemsCount--;
            }

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Product prod = unitOfWork.ProductRepository.GetById(prodId);

                if (PassesFilter(prod))
                {
                    inventorySvc.AddProductItem(prod, inventoryItems);

                    ItemsCount++;
                }
            }            

            UpdateCompletionPercent();
        }

        //CATEGORIES
        
        void appvm_CategoryModified(object sender, CategoryInfoEventArgs e)
        {
            UpdateInventoryItems();
        }

        void appvm_CategoryRemoved(object sender, CategoryInfoEventArgs e)
        {
            UpdateInventoryItems();

            //CalculateInventoryCompletionPercent();
        }

        void appvm_CategoryCreated(object sender, CategoryInfoEventArgs e)
        {
            UpdateInventoryItems();
        }

        #endregion        

        //public ObservableCollection<Category> Categories
        //{
        //    get { return appvm.CategoriesOC; }
        //}

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

            ProductViewModel pvm = new ProductViewModel(appvm);

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

        #endregion

        //recreate inventory tree
        void UpdateInventoryItems()
        {
            if (inventoryItems == null) return;

            inventoryItems.Clear();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var inventorySvc = base.GetService<IInventoryService>();
                ItemsCount = inventorySvc.CreateInventoryTreeLikeList(unitOfWork, inventoryItems, PassesFilter);
            }
        }

        //#region Expand Tree Command

        //RelayCommand expandTreeCommand;

        //public RelayCommand ExpandTreeCommand
        //{
        //    get
        //    {
        //        if (expandTreeCommand == null)
        //        {
        //            expandTreeCommand = new RelayCommand(x => ExpandTree());
        //        }
        //        return expandTreeCommand;
        //    }
        //}

        //void ExpandTree() 
        //{
        //    var inventoryService = base.GetService<IInventoryService>();
        //    inventoryService.ExpandCollapseInventoryTree(finishedGoodsTree, true);
        //}

        //#endregion

        //#region Collapse Tree Command

        //RelayCommand collapseTreeCommand;

        //public RelayCommand CollapseTreeCommand
        //{
        //    get
        //    {
        //        if (collapseTreeCommand == null)
        //        {
        //            collapseTreeCommand = new RelayCommand(x => CollapseTree());
        //        }
        //        return collapseTreeCommand;
        //    }
        //}

        //void CollapseTree()
        //{
        //    var inventoryService = base.GetService<IInventoryService>();
        //    inventoryService.ExpandCollapseInventoryTree(finishedGoodsTree, false);
        //}

        //#endregion

        #region VerifyCosts Command

        RelayCommand verifyCostsCommand;
        public RelayCommand VerifyCostsCommand
        {
            get
            {
                if (verifyCostsCommand == null)
                {
                    verifyCostsCommand = new RelayCommand(x => VerifyCosts());
                }
                return verifyCostsCommand;
            }
        }

        void VerifyCosts()
        {
            FixProductsCostsViewModel dialog = new FixProductsCostsViewModel(appvm);

            var windowManager = base.GetService<IWindowManager>();
            windowManager.ShowChildWindow(dialog, appvm);
        }

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

        #region Filtering

        ObservableCollection<InventoryItemViewModel> inventoryItems;
        public ObservableCollection<InventoryItemViewModel> InventoryItems
        {
            get
            {
                if (inventoryItems == null)
                {
                    inventoryItems = new ObservableCollection<InventoryItemViewModel>();

                    UpdateInventoryItems();

                    SetUpEvents();
                }
                return inventoryItems;
            }
        }        

        private bool PassesFilter(Product p)
        {
            //Product p = inventoryItem.Product;
            //if (!inventoryItem.Product.IsStorable) return false;

            ////current inventory

            //if (inventoryItem.Inventory != selectedInventory) return false;


            //text

            if (!string.IsNullOrWhiteSpace(findText)) 
            {
                string prefix = findText.Trim();

                if (p.Name.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) < 0) return false;            
            }

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

            //bool cond2 = true;

            if (filteringByCategory && selectedCategory != null)
            {
                if (p.Category == null) return false;

                if (p.CategoryId != selectedCategory.Id) return false;

                //cond2 = false;

                //foreach (var item in p.RelatedCategories)
                //{
                //    if (item.Category == selectedCategory)
                //    {
                //        cond2 = true;
                //        break;
                //    }
                //}
                //var query = from c in p.RelatedCategories
                //            where c.Category.Id == SelectedCategory.Id
                //            select c;
                //cond2 = query.Count() > 0;
            }

            //if (!cond2) return false;

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
                    findText = value;

                    UpdateInventoryItems();

                    OnPropertyChanged("FindText");

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

                UpdateInventoryItems();

                OnPropertyChanged("FilteringByCategory");
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
                    if (filteringByCategory) UpdateInventoryItems();
                }                
            }
        }

        #endregion        

        #region Open Product Command

        public InventoryItemViewModel SelectedItem { get; set; }

        RelayCommand openProductCommand;
        public ICommand OpenProductCommand
        {
            get
            {
                if (openProductCommand == null)
                    openProductCommand = new RelayCommand(x => OpenProduct(SelectedItem), x => CanOpen);
                return openProductCommand;
            }
        }

        bool CanOpen { get { return SelectedItem != null && SelectedItem.ItemType == InventoryItemType.Product; } }

        void OpenProduct(InventoryItemViewModel pNode)
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is ProductViewModel)) return false;

                ProductViewModel svm = (ProductViewModel)wsvm;

                return svm.ProductId == pNode.Id;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                ProductViewModel pvm = new ProductViewModel(appvm, pNode.Id);
                windowManager.ShowChildWindow(pvm, appvm);
            }
        }

        //Product GetProductFromId(int id) 
        //{
        //    return appvm.ProductsOC.Single(x => x.Id == id);
        //}

        #endregion

        #region Inventory Completion Percent Helper

        //BackgroundWorker complPercentWorker; bool workBusy;

        void UpdateCompletionPercent()
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                var invSvc = base.GetService<IInventoryService>();
                InventoryCompletionPercent = invSvc.GetInventoryCompletionPercent(unitOfWork);
            }

            //if (workBusy) return;

            //if (complPercentWorker == null)
            //{
            //    complPercentWorker = new BackgroundWorker();

            //    complPercentWorker.DoWork += complPercentWorker_DoWork;

            //    complPercentWorker.RunWorkerCompleted += complPercentWorker_RunWorkerCompleted;
            //}

            //complPercentWorker.RunWorkerAsync();
            //workBusy = true;
        }

        //void complPercentWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    var invSvc = base.GetService<IInventoryService>();
        //    e.Result = invSvc.GetInventoryCompletionPercent();
        //}

        //void complPercentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    InventoryCompletionPercent = (double)e.Result;
        //    workBusy = false;

        //    complPercentWorker.DoWork -= complPercentWorker_DoWork;
        //    complPercentWorker.RunWorkerCompleted -= complPercentWorker_RunWorkerCompleted;
        //    complPercentWorker = null;
        //}

        double invComplPercent = -1;
        public double InventoryCompletionPercent
        {
            get 
            {
                if (invComplPercent == -1)
                {
                    UpdateCompletionPercent();                   
                }
                return invComplPercent; 
            }
            set
            {
                invComplPercent = value;
                OnPropertyChanged("InventoryCompletionPercent");
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

        //int CountVisibleColumns()
        //{
        //    //inventory areas + product name
        //    return appvm.InventoryAreasOC.Count() + 1;
        //}

        //#region Export to Excel Command

        //RelayCommand exportToExcel;
        //public ICommand ExportToExcelCommand
        //{
        //    get
        //    {
        //        if (exportToExcel == null)
        //            exportToExcel = new RelayCommand(x => Export());
        //        return exportToExcel;
        //    }
        //}

        //BackgroundWorker excelWorker;
        //ProgressDialogViewModel pdvm;

        //private void Export()
        //{
        //    if (excelWorker == null)
        //    {
        //        excelWorker = new BackgroundWorker();

        //        excelWorker.DoWork += new DoWorkEventHandler(excelWorker_DoWork);

        //        excelWorker.WorkerReportsProgress = true;
        //        excelWorker.ProgressChanged += new ProgressChangedEventHandler(excelWorker_ProgressChanged);

        //        excelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(excelWorker_RunWorkerCompleted);
        //    }

        //    pdvm = new ProgressDialogViewModel();
        //    pdvm.Message = "Exportando a Excel...";
        //    pdvm.IsBusy = true;

        //    var windowManager = base.GetService<IWindowManager>();

        //    excelWorker.RunWorkerAsync();

        //    windowManager.ShowDialog(pdvm, appvm);

        //}

        //void excelWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Action<Excel.Range> displayHeader = (cell) =>
        //    {
        //        int column = 0;

        //        //one column for PLU
        //        cell.Offset[0, column].Value = "PLU";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        //one column for product name
        //        cell.Offset[0, column].Value = "Producto";
        //        cell.Offset[0, column++].Font.Bold = "True";

        //        //column for each inventory area
        //        //foreach (var item in appvm.InventoryAreasOC)
        //        //{
        //        //    cell.Offset[0, column].Value = item.Name;
        //        //    cell.Offset[0, column++].Font.Bold = "True";
        //        //}
        //    };

        //    Action<ProductRowViewModel, Excel.Range> displayItem = (pr, cell) =>
        //    {
        //        int column = 0;

        //        //product name
        //        //if (pr.RedNumbers) cell.EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        //        cell.Offset[0, column++].Value = pr.Product.Name;

        //        //qtty in each inventory Area
        //        //foreach (var item in appvm.InventoryAreasOC)
        //        //{
        //        //    InventoryItemViewModel ii = pr.InventoryItems[item.Id];
        //        //    if (ii == null) { column++; continue; }
        //        //    cell.Offset[0, column++].Value = string.Format("{0:0.##} {1}", ii.Quantity, ii.UnitMeasure.Caption);
        //        //}
        //    };

        //    int numberOfColumns = 2;//CountVisibleColumns();

        //    var excelExporter = base.GetService<IExcelExporter>();

        //    var sorted = from p in products_filtered
        //                 orderby p.Product.Name
        //                 select p;

        //    excelExporter.ExportToExcel<ProductRowViewModel>(sorted, displayHeader, displayItem, numberOfColumns, excelWorker);
        //}

        //void excelWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    pdvm.Progress = e.ProgressPercentage;
        //}

        //void excelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    pdvm.IsBusy = false;

        //    var windowManager = base.GetService<IWindowManager>();

        //    //close progress dialog
        //    windowManager.Close(pdvm);
        //}

        //#endregion
    }
}
