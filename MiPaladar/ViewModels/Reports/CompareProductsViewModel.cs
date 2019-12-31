using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

using MiPaladar.Entities;
using MiPaladar.Classes;
using MiPaladar.Enums;
using MiPaladar.Services;
using MiPaladar.Views;


namespace MiPaladar.ViewModels
{
    //public class CompareProductsViewModel : ViewModelBase
    //{
    //    MainWindowViewModel appvm;
    //    IExcelExporter excelExporter;
    //    // passwordAsker;

    //    UserManager personnelManager;

    //    public CompareProductsViewModel(MainWindowViewModel appvm)
    //    {
    //        this.appvm = appvm;
    //        this.excelExporter = appvm.ExcelExporter;
    //        //this.passwordAsker = appvm.PasswordAsker;

    //        this.personnelManager = appvm.PersonnelManager;

    //        fromDate = DateTime.Today;
    //        toDate = DateTime.Today;

    //        //UpdateSearch();
    //    }

    //    public override string DisplayName
    //    {
    //        get { return "Operaciones"; }
    //    }
        
    //    #region Dates

    //    //string[] dateChoices = new string[] { "Hoy", "Semana", "Mes", "Específico" };
    //    //public string[] DateChoices
    //    //{
    //    //    get { return dateChoices; }
    //    //}

    //    //int selectedDateChoice;
    //    //public int SelectedDateChoice
    //    //{
    //    //    get { return selectedDateChoice; }
    //    //    set
    //    //    {
    //    //        selectedDateChoice = value;
    //    //        switch (selectedDateChoice)
    //    //        {
    //    //            case 1:
    //    //                fromDate = DateTime.Today;
    //    //                toDate = DateTime.Today;
    //    //                break;
    //    //            case 2:
    //    //                fromDate = DateTime.Today;
    //    //                toDate = DateTime.Today;
    //    //                break;
    //    //            default:
    //    //                break;
    //    //        }
    //    //    }
    //    //}

    //    DateTime fromDate;
    //    public DateTime FromDate
    //    {
    //        get { return fromDate; }
    //        set { fromDate = value; }
    //    }

    //    DateTime toDate;
    //    public DateTime ToDate
    //    {
    //        get { return toDate; }
    //        set { toDate = value; }
    //    }

    //    #endregion                

    //    public ObservableCollection<Product> Products 
    //    {
    //        get { return appvm.ProductsOC; }
    //    }

    //    public ObservableCollection<Category> Categories 
    //    {
    //        get { return appvm.CategoriesOC; }
    //    }

    //    public ObservableCollection<PriceList> PriceLists 
    //    {
    //        get { return appvm.PriceListsOC; }
    //    }

    //    //public ObservableCollection<Employee> Waiters         
    //    //{
    //    //    get { return appvm.EmployeesOC; }
    //    //}

    //    public ICollectionView CanSellEmployees
    //    {
    //        get { return personnelManager.CanSellEmployees; }
    //    }

    //    public ICollectionView CanPurchaseEmployees
    //    {
    //        get { return personnelManager.CanPurchaseEmployees; }
    //    }

    //    //public ObservableCollection<PurchaseType> PurchaseTypes
    //    //{
    //    //    get { return new ObservableCollection<PurchaseType>(appvm.Context.PurchaseTypes); }
    //    //}

    //    ICollectionView icvIngredients;
    //    public ICollectionView Ingredients
    //    {
    //        get
    //        {
    //            if (icvIngredients == null)
    //            {
    //                //create the view
    //                CollectionViewSource myCVS = new CollectionViewSource();
    //                myCVS.Source = appvm.ProductsOC;
    //                icvIngredients = myCVS.View;

    //                //sort by name
    //                icvIngredients.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
    //                //filter by category
    //                icvIngredients.Filter = new Predicate<object>(x => (x as Product).IsIngredient);
    //            }
    //            return icvIngredients;
    //        }
    //    }
        
    //    ObservableCollection<OperationItemViewModel> itemsShowing;
    //    public ObservableCollection<OperationItemViewModel> ItemsShowing
    //    {
    //        get 
    //        {
    //            if (itemsShowing == null) 
    //            {
    //                lineitems = new ObservableCollection<OperationItemViewModel>();
    //                itemsShowing = new ObservableCollection<OperationItemViewModel>();

    //                CollectionViewSource cvs = new CollectionViewSource();
    //                cvs.Source = lineitems;
    //                auxView = (CollectionView)cvs.View;

    //                auxView.Filter = FilterProduct;

    //                UpdateSearch();
    //            }
    //            return itemsShowing; 
    //        }
    //    }

    //    #region Advanced Search Visibility

    //    bool advancedSearchVisible;
    //    public bool AdvancedSearchVisible
    //    {
    //        get { return advancedSearchVisible; }
    //        set
    //        {
    //            advancedSearchVisible = value;
    //            OnPropertyChanged("AdvancedSearchVisible");
    //        }
    //    }

    //    RelayCommand showAdvancedSearchPopupCommand;
    //    public ICommand ShowAdvancedSearchPopupCommand
    //    {
    //        get
    //        {
    //            if (showAdvancedSearchPopupCommand == null)
    //                showAdvancedSearchPopupCommand = new RelayCommand(x => ShowProductTemplatePopup());
    //            return showAdvancedSearchPopupCommand;
    //        }
    //    }

    //    void ShowProductTemplatePopup()
    //    {
    //        AdvancedSearchVisible = true;
    //    }

    //    #endregion

    //    #region Show Order Command

    //    bool showingSale;
    //    public bool ShowingSale
    //    {
    //        get { return showingSale; }
    //        set
    //        {
    //            if (showingSale != value)
    //            {
    //                showingSale = value;
    //                OnPropertyChanged("ShowingSale");
    //            }
    //        }
    //    }
    //    bool showingPurchase;
    //    public bool ShowingPurchase
    //    {
    //        get { return showingPurchase; }
    //        set
    //        {
    //            if (showingPurchase != value)
    //            {
    //                showingPurchase = value;
    //                OnPropertyChanged("ShowingPurchase");
    //            }
    //        }
    //    }

    //    SaleViewModel selectedSale;
    //    public SaleViewModel SelectedSale
    //    {
    //        get { return selectedSale; }
    //        set
    //        {
    //            selectedSale = value;
    //            OnPropertyChanged("SelectedSale");
    //        }
    //    }

    //    PurchaseViewModel selectedPurchase;
    //    public PurchaseViewModel SelectedPurchase
    //    {
    //        get { return selectedPurchase; }
    //        set
    //        {
    //            selectedPurchase = value;
    //            OnPropertyChanged("SelectedPurchase");
    //        }
    //    }

    //    RelayCommand showOrderCommand;
    //    public ICommand ShowOrderCommand
    //    {
    //        get
    //        {
    //            if (showOrderCommand == null)
    //                showOrderCommand = new RelayCommand(x => ShowOrder((Order)x));
    //            return showOrderCommand;
    //        }
    //    }

    //    private void ShowOrder(Order x)
    //    {
    //        //if (x is Sale)
    //        //{
    //        //    Sale sale = (Sale)x;
    //        //    SaleViewModel viewmodel = new SaleViewModel(appvm, sale);

    //        //    SelectedSale = viewmodel;
    //        //    ShowingSale = true;
    //        //}
    //        //else if (x is Purchase)
    //        //{
    //        //    Purchase purchase = (Purchase)x;
    //        //    PurchaseViewModel viewmodel = new PurchaseViewModel(appvm, purchase, inventoryService);

    //        //    SelectedPurchase = viewmodel;
    //        //    ShowingPurchase = true;
    //        //}
    //        var windowManager = base.GetService<IWindowManager>();

    //        if (x is Sale)
    //        {
    //            Sale sale = (Sale)x;

    //            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
    //            {
    //                if (!(wsvm is SaleViewModel)) return false;

    //                SaleViewModel svm = (SaleViewModel)wsvm;

    //                return svm.SalesOrder == sale;
    //            };

    //            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
    //            else
    //            {
    //                SaleViewModel viewmodel = new SaleViewModel(appvm, sale, OnRemoved);
    //                windowManager.ShowChildWindow(viewmodel, this);
    //            }
    //        }
    //        else if (x is Purchase)
    //        {
    //            Purchase purchase = (Purchase)x;

    //            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
    //            {
    //                if (!(wsvm is PurchaseViewModel)) return false;

    //                PurchaseViewModel pvm = (PurchaseViewModel)wsvm;

    //                return pvm.WrappedPurchase == purchase;
    //            };

    //            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
    //            else
    //            {
    //                PurchaseViewModel viewmodel = new PurchaseViewModel(appvm, purchase);
    //                windowManager.ShowChildWindow(viewmodel, this);
    //            }
    //        }
    //    }

    //    void OnRemoved(Sale s)
    //    {
    //        foreach (var item in itemsShowing)
    //        {
    //            if (item.Order == s)
    //            {
    //                itemsShowing.Remove(item);
    //                UpdateTotals();
    //                break;
    //            }
    //        }
    //    }

    //    #endregion

    //    #region Hide Order Command

    //    RelayCommand hideOrderCommand;
    //    public ICommand HideOrderCommand
    //    {
    //        get
    //        {
    //            if (hideOrderCommand == null)
    //                hideOrderCommand = new RelayCommand(x => HideOrder());
    //            return hideOrderCommand;
    //        }
    //    }

    //    void HideOrder()
    //    {
    //        if (showingSale)
    //        {
    //            ShowingSale = false;
    //            SelectedSale = null;
    //        }
    //        if (showingPurchase)
    //        {
    //            ShowingPurchase = false;
    //            //selectedPurchase.RemoveEvents();
    //            SelectedPurchase = null;
    //        }
    //    }

    //    #endregion

    //    #region Add Product Command

    //    Product productToAdd;
    //    public Product ProductToAdd
    //    {
    //        get { return productToAdd; }
    //        set
    //        {
    //            productToAdd = value;
    //            OnPropertyChanged("ProductToAdd");
    //        }
    //    }
        
    //    public string SearchText { get; set; }

    //    RelayCommand addProductCommand;
    //    public ICommand AddProductCommand
    //    {
    //        get
    //        {
    //            if (addProductCommand == null)
    //                addProductCommand = new RelayCommand(x => AddProduct(), x => this.CanAdd);
    //            return addProductCommand;
    //        }
    //    }

    //    bool CanAdd { get { return productToAdd != null; } }

    //    void AddProduct()
    //    {
    //        Action onChecked = () => { RefreshView(); };
    //        AddedProductViewModel ap = new AddedProductViewModel(productToAdd,onChecked);

    //        addedProducts.Add(ap);

    //        RefreshView();

    //        ProductToAdd = null;
    //        //clear automcompletebox
    //        SearchText = "";
    //        OnPropertyChanged("SearchText");            
    //    }

    //    #endregion
        
    //    #region Find Command

    //    RelayCommand findCommand;
    //    public ICommand FindCommand
    //    {
    //        get
    //        {
    //            if (findCommand == null)
    //            {
    //                findCommand = new RelayCommand(x => this.UpdateSearch());
    //            }
    //            return findCommand;
    //        }
    //    }

    //    CollectionView auxView;
    //    ObservableCollection<OperationItemViewModel> lineitems;        

    //    public void UpdateSearch()
    //    {
    //        lineitems.Clear();

    //        //get data
    //        IEnumerable<LineItem> query =
    //            from lineitem in appvm.Context.LineItems
    //            where lineitem.Order.Date >= fromDate && lineitem.Order.Date <= toDate && (lineitem.Order is Sale || lineitem.Order is Purchase)
    //            select lineitem;

    //        foreach (var item in query)
    //        {
    //            if (item.Product == null) continue;
    //            OperationItemViewModel copy = new OperationItemViewModel(item);

    //            lineitems.Add(copy);
    //        }

    //        //IEnumerable<LineItem> query2 =
    //        //    from lineitem in appvm.Context.LineItems.OfType<PurchaseLineItem>()
    //        //    where lineitem.Purchase.Date >= fromDate && lineitem.Purchase.Date <= toDate
    //        //    select lineitem;

    //        //foreach (var item in query2)
    //        //{
    //        //    if (item.Product == null) continue;
    //        //    OperationItemViewModel copy = new OperationItemViewModel(item);

    //        //    lineitems.Add(copy);
    //        //}

    //        RefreshView();

    //        //UpdateGroupDescriptions();

    //        if (filteringByIngredient) CalculateIngredientQuantities();

    //        //UpdateItems();

    //        convertedToIngredients = false;
    //    }

    //    void RefreshView()
    //    {
    //        UpdateColumnsVisibility();
    //        UpdateGroupDescriptions();
    //        UpdateItems();
    //    }

    //    void UpdateGroupDescriptions()
    //    {
    //        auxView.GroupDescriptions.Clear();

    //        if (groupingByProduct)
    //        {
    //            auxView.GroupDescriptions.Add(new PropertyGroupDescription("Product"));
    //        }
    //        else if (groupingByDate)
    //        {
    //            auxView.GroupDescriptions.Add(new PropertyGroupDescription("Product"));
    //            auxView.GroupDescriptions.Add(new PropertyGroupDescription("Order.Date"));
    //        }
    //        else if (groupingByEmployee)
    //        {
    //            auxView.GroupDescriptions.Add(new PropertyGroupDescription("Product"));
    //            auxView.GroupDescriptions.Add(new PropertyGroupDescription("Employee"));
    //        }
    //    }

    //    void UpdateItems()
    //    {
    //        itemsShowing.Clear();

    //        if (noGrouping) 
    //        {
    //            foreach (var item in auxView)
    //            {
    //                OperationItemViewModel li = (OperationItemViewModel)item;

    //                itemsShowing.Add(li);
    //            }                
    //        }
    //        else 
    //        {
    //            foreach (var item in auxView.Groups)
    //            {
    //                CollectionViewGroup topLevelGroup = (CollectionViewGroup)item;

    //                //grouping by product
    //                if (topLevelGroup.IsBottomLevel)
    //                {
    //                    OperationItemViewModel lic = new OperationItemViewModel();

    //                    if (groupingByProduct) 
    //                    {
    //                        lic.Product = (Product)topLevelGroup.Name;
    //                        lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
    //                    }

    //                    MakeSum(topLevelGroup, lic);

    //                    itemsShowing.Add(lic);
    //                }
    //                else
    //                {
    //                    foreach (var item2 in topLevelGroup.Items)
    //                    {
    //                        CollectionViewGroup innerGroup = (CollectionViewGroup)item2;

    //                        OperationItemViewModel lic = new OperationItemViewModel();

    //                        lic.Product = (Product)topLevelGroup.Name;
    //                        lic.UnitMeasure = lic.Product.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);

    //                        if (groupingByDate)
    //                        {
    //                            lic.Date = (DateTime)innerGroup.Name;                                
    //                        }
    //                        else if (groupingByEmployee)
    //                        {
    //                            lic.Employee = (Employee)innerGroup.Name;
    //                        }

    //                        MakeSum(innerGroup, lic);

    //                        itemsShowing.Add(lic);
    //                    }
    //                }
    //            }
    //        }

    //        UpdateTotals();
    //    }

    //    private void MakeSum(CollectionViewGroup group, OperationItemViewModel lic)
    //    {
    //        //sum quantity and price if filtering by ordertype
    //        if (showSaleOrders || showPurchaseOrders)
    //        {
    //            double tempQuantity = 0;
    //            decimal tempPrice = 0;

    //            decimal tempCost = 0;
    //            decimal tempProfit = 0;

    //            foreach (var item in group.Items)
    //            {
    //                OperationItemViewModel x = (OperationItemViewModel)item;

    //                tempQuantity += x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion;
    //                tempPrice += x.Price;

    //                tempCost += x.Cost;
    //                tempProfit += x.Profit;
    //            }
    //            //lic.Quantity = group.Items.Sum(x => ((LineItemCopy)x).Quantity);
    //            //lic.Price = group.Items.Sum(x => ((LineItemCopy)x).Price);
    //            lic.Quantity = tempQuantity;
    //            lic.Price = tempPrice;

    //            lic.Cost = tempCost;
    //            lic.Profit = tempProfit;
    //        }
    //        //separate between sales and purchases if showing all operation types
    //        else
    //        {                
    //            //sales
    //            double quantity_sold = 0;
    //            decimal sale_price = 0;
    //            //purchases
    //            double quantity_purchased = 0;
    //            decimal purchase_price = 0;

    //            foreach (var item in group.Items)
    //            {
    //                OperationItemViewModel x = (OperationItemViewModel)item;

    //                if (x.OrderType == OrderType.Sale) 
    //                {
    //                    quantity_sold += x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion;
    //                    sale_price += x.Price;
    //                }
    //                else if (x.OrderType == OrderType.Purchase) 
    //                {
    //                    quantity_purchased += x.UnitMeasure.IsFamilyBase ? x.Quantity : x.Quantity * x.UnitMeasure.ToBaseConversion;
    //                    purchase_price += x.Price;
    //                }                    
    //            }

    //            lic.QuantitySold = quantity_sold;
    //            lic.SalePrice = sale_price;
    //            lic.QuantityPurchased = quantity_purchased;
    //            lic.PurchasePrice = purchase_price;
    //            //lic.QuantitySold = group.Items.Where(x => ((LineItemCopy)x).OrderType == OrderType.Sale).Sum(x => ((LineItemCopy)x).Quantity);
    //            //lic.SalePrice = group.Items.Where(x => ((LineItemCopy)x).OrderType == OrderType.Sale).Sum(x => ((LineItemCopy)x).Price);
    //            //lic.QuantityPurchased = group.Items.Where(x => ((LineItemCopy)x).OrderType == OrderType.Purchase).Sum(x => ((LineItemCopy)x).Quantity);
    //            //lic.PurchasePrice = group.Items.Where(x => ((LineItemCopy)x).OrderType == OrderType.Purchase).Sum(x => ((LineItemCopy)x).Price);
    //        }

    //        lic.IngredientQuantity = group.Items.Sum(x => ((OperationItemViewModel)x).IngredientQuantity);
    //    }        

    //    void CalculateIngredientQuantities()
    //    {
    //        foreach (var item in auxView)
    //        {
    //            OperationItemViewModel li = (OperationItemViewModel)item;
    //            double quantity = 0;
    //            li.IngredientUM = appvm.UnitMeasureManager.Unit;
    //            //find quantity of ingredient current product contains
    //            if (li.Product == selectedIngredient) quantity = li.Quantity;
    //            else
    //            {
    //                //find in ingredients
    //                Ingredient ing = (from i in li.Product.Ingredients
    //                                  where i.IngredientProduct == selectedIngredient
    //                                  select i).First();
    //                quantity = li.Quantity * ing.Quantity;
    //                li.IngredientUM = ing.UnitMeasure;
    //            }

    //            li.IngredientQuantity = quantity;
                
    //        }
    //    }

    //    #endregion        

    //    #region Filtering

    //    bool FilterProduct(object o)
    //    {
    //        OperationItemViewModel li = (OperationItemViewModel)o;

    //        bool cond = false;       
     
    //        //check if product appears in added products list
    //        foreach (var ap in addedProducts)
    //        {
    //            if (ap.IsChecked && ap.Product == li.Product)
    //            { cond = true; break; }
    //        }

    //        if (!cond) return false;

    //        if (showSaleOrders)
    //        {
    //            if (li.OrderType != OrderType.Sale) return false;

    //            Sale sale = (Sale)li.Order;

    //            if (filteringByPriceList)
    //            {
    //                if (sale.Table == null) cond = false;
    //                else cond = sale.Table.PriceList == selectedPriceList;
    //            }

    //            if (!cond) return false;

    //            if (filteringByWaiter)
    //            {
    //                cond = sale.Employee == selectedWaiter;
    //            }

    //            if (!cond) return false;

    //            //ingrediente condition
    //            if (filteringByIngredient)
    //            {
    //                if (li.Product == null) cond = false;

    //                else if (li.Product != selectedIngredient)
    //                {
    //                    cond = false;
    //                    foreach (var item in li.Product.Ingredients)
    //                    {
    //                        if (item.IngredientProduct == selectedIngredient)
    //                        {
    //                            cond = true; break;
    //                        }
    //                    }
    //                }
    //            }

    //            if (!cond) return false;
    //        }
    //        else if (showPurchaseOrders)
    //        {
    //            if (li.OrderType != OrderType.Purchase) return false;

    //            Purchase purchase = (Purchase)li.Order;

    //            if (filteringByResponsible)
    //                if (selectedResponsible != purchase.Employee) return false;

    //            //if (filteringByPurchaseType)
    //            //    if (selectedPurchaseType != purchase.PurchaseType) return false;
    //        }                                     

    //        return cond;
    //    }

    //    int count = 0;
    //    //bool showAllOrders;
    //    //public bool ShowAllOrders
    //    //{
    //    //    get { return showAllOrders; }
    //    //    set
    //    //    {
    //    //        if (showAllOrders != value)
    //    //        {
    //    //            showAllOrders = value;

    //    //            count++;

    //    //            if (count % 2 == 0)
    //    //            {
    //    //                RefreshView();
    //    //            }
    //    //        }                
    //    //    }
    //    //}

    //    bool showSaleOrders = true;
    //    public bool ShowSaleOrders
    //    {
    //        get { return showSaleOrders; }
    //        set
    //        {
    //            if (showSaleOrders != value)
    //            {
    //                showSaleOrders = value;

    //                count++;

    //                if (count % 2 == 0)
    //                {
    //                    RefreshView();
    //                }

    //                OnPropertyChanged("ShowSaleOrders");
    //            }
    //        }
    //    }

    //    bool showPurchaseOrders;
    //    public bool ShowPurchaseOrders
    //    {
    //        get { return showPurchaseOrders; }
    //        set
    //        {
    //            if (showPurchaseOrders != value)
    //            {
    //                showPurchaseOrders = value;

    //                count++;

    //                if (count % 2 == 0)
    //                {
    //                    RefreshView();
    //                }

    //                OnPropertyChanged("ShowPurchaseOrders");
    //            }
    //        }
    //    }

    //    bool filteringByPriceList;
    //    public bool FilteringByPriceList
    //    {
    //        get { return filteringByPriceList; }
    //        set
    //        {
    //            filteringByPriceList = value;
    //            OnPropertyChanged("FilteringByPriceList");

    //            auxView.Refresh();
    //            UpdateItems();
    //        }
    //    }

    //    PriceList selectedPriceList;
    //    public PriceList SelectedPriceList
    //    {
    //        get { return selectedPriceList; }
    //        set
    //        {
    //            selectedPriceList = value;

    //            if (filteringByPriceList)
    //            {
    //                auxView.Refresh();
    //                UpdateItems();
    //            }
    //        }
    //    }

    //    bool filteringByWaiter;
    //    public bool FilteringByWaiter
    //    {
    //        get { return filteringByWaiter; }
    //        set
    //        {
    //            filteringByWaiter = value;
    //            OnPropertyChanged("FilteringByWaiter");

    //            auxView.Refresh();
    //            UpdateItems();
    //        }
    //    }

    //    Employee selectedWaiter;
    //    public Employee SelectedWaiter
    //    {
    //        get { return selectedWaiter; }
    //        set
    //        {
    //            selectedWaiter = value;

    //            if (filteringByWaiter)
    //            {
    //                auxView.Refresh();
    //                UpdateItems();
    //            }
    //        }
    //    }

    //    bool filteringByIngredient;
    //    public bool FilteringByIngredient
    //    {
    //        get { return filteringByIngredient; }
    //        set
    //        {
    //            filteringByIngredient = value;
    //            OnPropertyChanged("FilteringByIngredient");                

    //            //if (selectedIngredient != null) 
    //            {
    //                //auxView.Refresh(); 

    //                RefreshView();

    //                if (filteringByIngredient) CalculateIngredientQuantities();

    //                UpdateTotals();                    

    //                //UpdateItems();
    //            }
    //        }
    //    }        

    //    Product selectedIngredient;
    //    public Product SelectedIngredient
    //    {
    //        get { return selectedIngredient; }
    //        set
    //        {
    //            selectedIngredient = value;
    //            OnPropertyChanged("SelectedIngredient");

    //            if (filteringByIngredient) 
    //            {
    //                auxView.Refresh();

    //                CalculateIngredientQuantities();

    //                UpdateItems();
    //            }
    //        }
    //    }

    //    bool filteringByResponsible;
    //    public bool FilteringByResponsible
    //    {
    //        get { return filteringByResponsible; }
    //        set
    //        {
    //            filteringByResponsible = value;
    //            OnPropertyChanged("FilteringByResponsible");

    //            if (selectedResponsible != null)
    //            {
    //                auxView.Refresh();
    //                UpdateItems();
    //            }
    //        }
    //    }

    //    Employee selectedResponsible;
    //    public Employee SelectedResponsible
    //    {
    //        get { return selectedResponsible; }
    //        set
    //        {
    //            selectedResponsible = value;
    //            //OnPropertyChanged("SelectedWaiter");

    //            if (filteringByResponsible)
    //            {

    //                auxView.Refresh();
    //                UpdateItems();
    //            }
    //        }
    //    }

    //    ObservableCollection<AddedProductViewModel> addedProducts;
    //    public ObservableCollection<AddedProductViewModel> AddedProducts 
    //    {
    //        get
    //        {
    //            if (addedProducts == null) addedProducts = new ObservableCollection<AddedProductViewModel>();
    //            return addedProducts;
    //        }
    //    }

    //    #endregion

    //    #region Grouping
        
    //    int groupingCount;

    //    bool noGrouping = true;
    //    public bool NoGrouping 
    //    {
    //        get { return noGrouping; }
    //        set
    //        {
    //            if (noGrouping != value) 
    //            {
    //                noGrouping = value;

    //                groupingCount++;

    //                if (groupingCount % 2 == 0)
    //                {
    //                    RefreshView();
    //                }
    //            }                
    //        }
    //    }
    //    bool groupingByProduct;
    //    public bool GroupingByProduct         
    //    {
    //        get { return groupingByProduct; }
    //        set
    //        {
    //            if (groupingByProduct != value) 
    //            {
    //                groupingByProduct = value;

    //                groupingCount++;

    //                if (groupingCount % 2 == 0)
    //                {
    //                    RefreshView();
    //                }
    //            }                
    //        }
    //    }

    //    bool groupingByDate;
    //    public bool GroupingByDate 
    //    {
    //        get { return groupingByDate; }
    //        set
    //        {
    //            if (groupingByDate != value) 
    //            {
    //                groupingByDate = value;

    //                groupingCount++;

    //                if (groupingCount % 2 == 0)
    //                {
    //                    RefreshView();
    //                }
    //            }                
    //        }
    //    }

    //    bool groupingByEmployee;
    //    public bool GroupingByEmployee
    //    {
    //        get { return groupingByEmployee; }
    //        set
    //        {
    //            if (groupingByEmployee != value)
    //            {
    //                groupingByEmployee = value;

    //                groupingCount++;

    //                if (groupingCount % 2 == 0)
    //                {
    //                    RefreshView();
    //                }
    //            }
    //        }
    //    }

    //    #endregion

    //    #region Totals

    //    void UpdateTotals()
    //    {
    //        total = 0;
    //        totalprice = 0;
    //        totalCost = 0;
    //        totalProfit = 0;
    //        totalSales = 0;
    //        totalSalesPrice = 0;
    //        totalPurchases = 0;
    //        totalPurchasesPrice = 0;

    //        double totalingredient = 0;

    //        int count = 0;

    //        foreach (var item in itemsShowing)
    //        {
    //            count++;

    //            total += item.Quantity;
    //            totalprice += item.Price;

    //            if (noGrouping) 
    //            {
    //                if (item.OrderType == OrderType.Sale)
    //                {
    //                    totalSales += item.Quantity;
    //                    totalSalesPrice += item.Price;
    //                }
    //                else 
    //                {
    //                    totalPurchases += item.Quantity;
    //                    totalPurchasesPrice += item.Price;
    //                }
    //            }
    //            else 
    //            {
    //                totalCost += item.Cost;
    //                totalProfit += item.Profit;
    //                totalSales += item.QuantitySold;
    //                totalSalesPrice += item.SalePrice;
    //                totalPurchases += item.QuantityPurchased;
    //                totalPurchasesPrice += item.PurchasePrice;
    //            }                

    //            totalingredient += item.IngredientQuantity;
    //        }

    //        Total = total;
    //        TotalPrice = totalprice;
    //        TotalCost = totalCost;
    //        TotalProfit = totalProfit;
    //        if (totalprice > 0) CostPercentAverage = totalCost / totalprice;
    //        TotalSales = totalSales;
    //        TotalSalesPrice = totalSalesPrice;
    //        TotalPurchases = totalPurchases;
    //        TotalPurchasesPrice = totalPurchasesPrice;
    //        TotalIngredient = totalingredient;
    //    }

    //    double total;
    //    public double Total
    //    {
    //        get { return total; }
    //        set
    //        {
    //            total = value;
    //            OnPropertyChanged("Total");
    //        }
    //    }

    //    decimal totalprice;
    //    public decimal TotalPrice
    //    {
    //        get { return totalprice; }
    //        set
    //        {
    //            totalprice = value;
    //            OnPropertyChanged("TotalPrice");
    //        }
    //    }

    //    decimal totalCost;
    //    public decimal TotalCost
    //    {
    //        get { return totalCost; }
    //        set
    //        {
    //            totalCost = value;
    //            OnPropertyChanged("TotalCost");
    //        }
    //    }

    //    decimal totalProfit;
    //    public decimal TotalProfit
    //    {
    //        get { return totalProfit; }
    //        set
    //        {
    //            totalProfit = value;
    //            OnPropertyChanged("TotalProfit");
    //        }
    //    }

    //    decimal costPercentAverage;
    //    public decimal CostPercentAverage
    //    {
    //        get { return costPercentAverage; }
    //        set
    //        {
    //            costPercentAverage = value;
    //            OnPropertyChanged("CostPercentAverage");
    //        }
    //    }

    //    double totalSales;
    //    public double TotalSales
    //    {
    //        get { return totalSales; }
    //        set
    //        {
    //            totalSales = value;
    //            OnPropertyChanged("TotalSales");
    //        }
    //    }

    //    decimal totalSalesPrice;
    //    public decimal TotalSalesPrice
    //    {
    //        get { return totalSalesPrice; }
    //        set
    //        {
    //            totalSalesPrice = value;
    //            OnPropertyChanged("TotalSalesPrice");
    //        }
    //    }

    //    double totalPurchases;
    //    public double TotalPurchases
    //    {
    //        get { return totalPurchases; }
    //        set
    //        {
    //            totalPurchases = value;
    //            OnPropertyChanged("TotalPurchases");
    //        }
    //    }

    //    decimal totalPurchasesPrice;
    //    public decimal TotalPurchasesPrice
    //    {
    //        get { return totalPurchasesPrice; }
    //        set
    //        {
    //            totalPurchasesPrice = value;
    //            OnPropertyChanged("TotalPurchasesPrice");
    //        }
    //    }

    //    double totalingredient;
    //    public double TotalIngredient
    //    {
    //        get { return totalingredient; }
    //        set
    //        {
    //            totalingredient = value;
    //            OnPropertyChanged("TotalIngredient");
    //        }
    //    }

    //    #endregion

    //    #region Convert To Ingredientes

    //    RelayCommand convertToIngredientCommand;
    //    public ICommand ConvertToIngredientCommand 
    //    {
    //        get 
    //        {
    //            if (convertToIngredientCommand == null)
    //                convertToIngredientCommand = new RelayCommand(x => ConvertToIngredients(), x => CanConvert);
    //            return convertToIngredientCommand;
    //        }
    //    }

    //    bool convertedToIngredients;
    //    bool CanConvert { get { return !convertedToIngredients; } }

        
    //    void ConvertToIngredients() 
    //    {
    //        ObservableCollection<OperationItemViewModel> ingredientItems = new ObservableCollection<OperationItemViewModel>();

    //        foreach (var item in auxView)
    //        {
    //            OperationItemViewModel lic = (OperationItemViewModel)item;
    //            ConvertRecipeToIngredients(lic, ingredientItems);
    //        }

    //        ResetFilterConditions();

    //        //copy ingredient elements
    //        lineitems.Clear();
    //        foreach (var item in ingredientItems)
    //        {
    //            lineitems.Add(item);
    //        }            

    //        convertedToIngredients = true;

    //        RefreshView();
    //    }

    //    private void ResetFilterConditions()
    //    {
    //        filteringByWaiter = false;
    //        filteringByPriceList = false;
    //        filteringByIngredient = false;

    //        OnPropertyChanged("FilteringByWaiter");
    //        OnPropertyChanged("FilteringByPriceList");
    //        OnPropertyChanged("FilteringByIngredient");
    //    }        
        
    //    //bool showingIngredients;
    //    //public bool ShowingIngredients
    //    //{
    //    //    get { return showingIngredients; }
    //    //    set
    //    //    {
    //    //        showingIngredients = value;

    //    //        if (showingIngredients)
    //    //        {
    //    //            ingredientItems = new ObservableCollection<LineItemCopy>();

    //    //            foreach (var item in auxView)
    //    //            {
    //    //                LineItemCopy lic = (LineItemCopy)item;

    //    //                ConvertRecipeToIngredients(lic, ingredientItems);
    //    //            }

    //    //            //update the view
    //    //            CollectionViewSource cvs = new CollectionViewSource();
    //    //            cvs.Source = ingredientItems;
    //    //            auxView = (CollectionView)cvs.View;

    //    //            auxView.Filter = FilterProduct;

    //    //            auxView.Refresh();
    //    //            UpdateItems();
    //    //        }
    //    //        else { }
    //    //    }
    //    //}

    //    private void ConvertRecipeToIngredients(OperationItemViewModel lineitem, ObservableCollection<OperationItemViewModel> ingredientList)
    //    {
    //        Product product = lineitem.Product;
    //        double quantity = lineitem.Quantity;

    //        if (product == null) return;

    //        //if it's recipe add its ingredients
    //        if (product.IsRecipe)
    //        {
    //            foreach (var item in product.Ingredients)
    //            {
    //                OperationItemViewModel lic = new OperationItemViewModel();

    //                lic.Quantity = quantity * item.Quantity;
    //                lic.UnitMeasure = item.UnitMeasure;
    //                lic.Product = item.IngredientProduct;                    
    //                lic.Order = lineitem.Order;
    //                lic.OrderType = lineitem.OrderType;
    //                lic.Date = lineitem.Date;

    //                ConvertRecipeToIngredients(lic, ingredientList);
    //            }
    //        }
    //        else //if(product.IsIngredient)
    //        {
    //            //lineitem.Price = 0;
    //            ingredientList.Add(lineitem);
    //        }
    //    }

    //    #endregion        

    //    #region Export to Excel Command

    //    RelayCommand exportToExcel;
    //    public ICommand ExportToExcelCommand
    //    {
    //        get
    //        {
    //            if (exportToExcel == null)
    //                exportToExcel = new RelayCommand(x => Export(false));
    //            return exportToExcel;
    //        }
    //    }

    //    RelayCommand exportByCateogry;
    //    public ICommand ExportByCateogry
    //    {
    //        get
    //        {
    //            if (exportByCateogry == null)
    //                exportByCateogry = new RelayCommand(x => Export(true));
    //            return exportByCateogry;
    //        }
    //    }

    //    private void Export(bool groupByCategory)
    //    {
    //        Action<CollectionViewGroup, Excel.Range> displayGroup = (group, cell) =>
    //        {
    //            if (group.Name != DependencyProperty.UnsetValue)
    //            {
    //                //write the name of the category
    //                cell.Value = group.Name;
    //                cell.Font.Bold = true;
    //            }
    //            cell.Offset[1, 0].Select();

    //            int row = 1;
    //            foreach (var item in group.Items)
    //            {
    //                DisplayItem((OperationItemViewModel)item, cell.Offset[row, 0]);
    //                row++;
    //                cell.Offset[row, 0].Select();
    //            }
    //        };

    //        int numberOfColumns = CalculateNumberOfColumns();

    //        ICollectionView view = CollectionViewSource.GetDefaultView(itemsShowing);

    //        if (groupByCategory)
    //        {
    //            CollectionViewSource cvs = new CollectionViewSource();
    //            cvs.Source = itemsShowing;
    //            ICollectionView groupedView = cvs.View;

    //            PropertyGroupDescription pgd = new PropertyGroupDescription("Category.Name");
    //            groupedView.GroupDescriptions.Add(pgd);

    //            foreach (var item in view.SortDescriptions)
    //            {
    //                groupedView.SortDescriptions.Add(item);
    //            }

    //            excelExporter.ExportToExcel<CollectionViewGroup>(groupedView.Groups.Cast<CollectionViewGroup>(),
    //                DisplayHeader, displayGroup, numberOfColumns);

    //            //DisplayInGroups(cvTotals.Groups.Cast<CollectionViewGroup>(), displayGroup);
    //        }
    //        else 
    //        {
    //            excelExporter.ExportToExcel<OperationItemViewModel>(view.Cast<OperationItemViewModel>(),
    //            DisplayHeader, DisplayItem, numberOfColumns);
    //        }            
    //    }

    //    void DisplayHeader(Excel.Range cell)
    //    {
    //        int column = 0;

    //        if (DateColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Fecha";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (QuantityColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Cantidad";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        //product column
    //        if (ProductColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Producto";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (CategoryColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Categoría";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (PriceColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Precio";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (CostColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Costo";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (ProfitColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Ganacia";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (CostPercentColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Costo %";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (IngredientQuantityColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Cant. Ing.";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (IngredientColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Ingrediente";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (ResponsibleColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Responsable";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }

    //        if (WaiterColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Dependiente";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        } 

    //        if (AreaColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Area";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }
    //        if (OrderColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = "Vale";
    //            cell.Offset[0, column++].Font.Bold = "True";
    //        }
    //    }

    //    void DisplayItem(OperationItemViewModel lineitem, Excel.Range cell) 
    //    {
    //        int column = 0;

    //        if (DateColumnVisible)
    //        {
    //            cell.Offset[0, column++].Value = lineitem.Date.ToString("m"); ;
    //        }

    //        if (QuantityColumnVisible) 
    //            cell.Offset[0, column++].Value = lineitem.Quantity;

    //        //product column
    //        if(ProductColumnVisible) 
    //            cell.Offset[0, column++].Value = lineitem.Product != null ? lineitem.Product.Name : string.Empty;

    //        if (CategoryColumnVisible)
    //            cell.Offset[0, column++].Value = lineitem.Category != null ? lineitem.Category.Name : "Sin categoría";

    //        if (PriceColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = lineitem.Price;
    //            cell.Offset[0, column++].Style = "Currency";
    //        }

    //        if (CostColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = lineitem.Cost;
    //            cell.Offset[0, column++].Style = "Currency";
    //        }

    //        if (ProfitColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = lineitem.Profit;
    //            cell.Offset[0, column++].Style = "Currency";
    //        }

    //        if (CostPercentColumnVisible)
    //        {
    //            cell.Offset[0, column].Value = lineitem.CostToPriceRatio;
    //            cell.Offset[0, column++].Style = "Percent";
    //        }

    //        if (IngredientQuantityColumnVisible)
    //        {
    //            cell.Offset[0, column++].Value = lineitem.IngredientQuantity;
    //        }

    //        if (IngredientColumnVisible)
    //        {
    //            cell.Offset[0, column++].Value = selectedIngredient.Name;
    //        }

    //        if (WaiterColumnVisible)
    //        {
    //            Sale sale = (Sale)lineitem.Order;
    //            cell.Offset[0, column++].Value = sale.Employee != null ? sale.Employee.Name : "";
    //        }

    //        //if (PurchaseTypeColumnVisible) 
    //        //{
    //        //    Purchase purchase = (Purchase)lineitem.Order;
    //        //    cell.Offset[0, column++].Value = purchase.PurchaseType != null ? purchase.PurchaseType.Name : "";
    //        //}

    //        if (ResponsibleColumnVisible)
    //        {
    //            Purchase purchase = (Purchase)lineitem.Order;
    //            cell.Offset[0, column++].Value = purchase.Employee != null ? purchase.Employee.Name : "";
    //        }

    //        if (AreaColumnVisible)
    //        {
    //            Sale sale = (Sale)lineitem.Order;
    //            cell.Offset[0, column++].Value = sale.Table.PriceList.Name;
    //        }

    //        if (OrderColumnVisible)
    //        {
    //            string orderstring = null;

    //             if( lineitem.OrderType == OrderType.Sale)
    //             {
    //                 orderstring="Vale " + ((Sale)lineitem.Order).Number;
    //             }

    //            cell.Offset[0, column++].Value = orderstring;
    //        }
    //    }

    //    private int CalculateNumberOfColumns()
    //    {
    //        int numberOfColumns = 0;

    //        if (DateColumnVisible) numberOfColumns++;

    //        if (QuantityColumnVisible) numberOfColumns++;            

    //        //product column
    //        if(ProductColumnVisible) numberOfColumns++;

    //        if (CategoryColumnVisible) numberOfColumns++;

    //        if (PriceColumnVisible) numberOfColumns++;

    //        if (CostColumnVisible) numberOfColumns++;

    //        if (ProfitColumnVisible) numberOfColumns++;

    //        if (CostPercentColumnVisible) numberOfColumns++;

    //        if (IngredientQuantityColumnVisible) numberOfColumns++;

    //        if (IngredientColumnVisible) numberOfColumns++;

    //        if (WaiterColumnVisible) numberOfColumns++;

    //        if (ResponsibleColumnVisible) numberOfColumns++;

    //        if (OrderColumnVisible) numberOfColumns++;

    //        if (AreaColumnVisible) numberOfColumns++;

    //        return numberOfColumns;
    //    }

        

    //    //Action<Excel.Range> displayHeader = (cell) =>
    //    //{
    //    //    int column = 0;

    //    //    if (DateColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Fecha";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    if (TypeColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Tipo";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    if (QuantityColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Cantidad";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    //product column
    //    //    cell.Offset[0, column].Value = "Producto";
    //    //    cell.Offset[0, column++].Font.Bold = "True";

    //    //    if (PriceColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Importe";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    if (SaleQuantityColumnVisible) 
    //    //    {
    //    //        cell.Offset[0, column].Value = "Venta";
    //    //        cell.Offset[0, column++].Font.Bold = "True";                    
    //    //    }
    //    //    if (SalePriceColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Precio Venta";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    if (PurchaseQuantityColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Compra";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }
    //    //    if (PurchasePriceColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Precio Compra";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    if (IngredienteQuantityColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Cant. Ing.";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }

    //    //    if (IngredienteColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Ingrediente";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }                                

    //    //    if (WaiterColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Dependiente";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }
    //    //    if (OrderColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Vale";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }
    //    //    if (AreaColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = "Area";
    //    //        cell.Offset[0, column++].Font.Bold = "True";
    //    //    }
    //    //};

    //    //Action<LineItemCopy, Excel.Range> displayItem = (lineitem, cell) =>
    //    //{
    //    //    int column = 0;

    //    //    if (DateColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column++].Value = lineitem.Date.ToString("m"); ;
    //    //    }

    //    //    if (TypeColumnVisible) cell.Offset[0, column++].Value = lineitem.OrderType.ToString();

    //    //    if (QuantityColumnVisible) cell.Offset[0, column++].Value = lineitem.Quantity;

    //    //    //product column
    //    //    cell.Offset[0, column++].Value = lineitem.Product != null ? lineitem.Product.Name : "";

    //    //    if (PriceColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = lineitem.Price;
    //    //        cell.Offset[0, column++].Style = "Currency";
    //    //    } 

    //    //    if (SaleQuantityColumnVisible)
    //    //    {                    
    //    //        cell.Offset[0, column++].Value = lineitem.QuantitySold;
    //    //    }
    //    //    if (SalePriceColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column++].Value = lineitem.SalePrice;
    //    //    }

    //    //    if (PurchaseQuantityColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = lineitem.QuantityPurchased;
    //    //    }
    //    //    if (PurchasePriceColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column].Value = lineitem.PurchasePrice;
    //    //    }

    //    //    if (IngredienteQuantityColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column++].Value = lineitem.IngredientQuantity;
    //    //    }

    //    //    if (IngredienteColumnVisible)
    //    //    {
    //    //        cell.Offset[0, column++].Value = selectedIngredient.Name;
    //    //    }                

    //    //    if (WaiterColumnVisible)
    //    //    {
    //    //        Sale sale = (Sale)lineitem.Order;
    //    //        cell.Offset[0, column++].Value = sale.Waiter != null ? sale.Waiter.Name : "";
    //    //    }

    //    //    if (OrderColumnVisible)
    //    //    {
    //    //        Sale sale = (Sale)lineitem.Order;
    //    //        cell.Offset[0, column++].Value = "Vale " + sale.Number;
    //    //    }

    //    //    if (AreaColumnVisible)
    //    //    {
    //    //        Sale sale = (Sale)lineitem.Order;
    //    //        cell.Offset[0, column++].Value = sale.PriceList.Name;
    //    //    }
    //    //};

    //    #endregion

    //    #region Columns Visibility

    //    void UpdateColumnsVisibility() 
    //    {
    //        DateColumnVisible = noGrouping || groupingByDate;
    //        QuantityColumnVisible = true;
    //        ProductColumnVisible = true;
    //        CategoryColumnVisible = showSaleOrders;
    //        PriceColumnVisible = !convertedToIngredients && !filteringByIngredient;
    //        CostColumnVisible = showSaleOrders && groupingByProduct && !convertedToIngredients;
    //        ProfitColumnVisible = showSaleOrders && groupingByProduct && !convertedToIngredients;
    //        CostPercentColumnVisible = showSaleOrders && groupingByProduct && !convertedToIngredients;
    //        IngredientQuantityColumnVisible = showSaleOrders && filteringByIngredient;
    //        IngredientColumnVisible = showSaleOrders && filteringByIngredient;
    //        WaiterColumnVisible = showSaleOrders && (noGrouping ||  groupingByEmployee);
    //        ResponsibleColumnVisible = showPurchaseOrders && noGrouping;
    //        AreaColumnVisible = showSaleOrders && noGrouping;
    //        OrderColumnVisible = noGrouping;
    //    }

    //    bool dateColumnVisible;
    //    public bool DateColumnVisible
    //    {
    //        get { return dateColumnVisible; }
    //        set 
    //        {
    //            dateColumnVisible = value;
    //            OnPropertyChanged("DateColumnVisible");
    //        }
    //    }
    //    bool quantityColumnVisible;
    //    public bool QuantityColumnVisible
    //    {
    //        get { return quantityColumnVisible; }
    //        set 
    //        {
    //            quantityColumnVisible = value;
    //            OnPropertyChanged("QuantityColumnVisible");
    //        }
    //    }

    //    bool productColumnVisible;
    //    public bool ProductColumnVisible
    //    {
    //        get { return productColumnVisible; }
    //        set
    //        {
    //            productColumnVisible = value;
    //            OnPropertyChanged("ProductColumnVisible");
    //        }
    //    }

    //    bool categoryColumnVisible;
    //    public bool CategoryColumnVisible
    //    {
    //        get { return categoryColumnVisible; }
    //        set
    //        {
    //            categoryColumnVisible = value;
    //            OnPropertyChanged("CategoryColumnVisible");
    //        }
    //    }

    //    bool priceColumnVisible;
    //    public bool PriceColumnVisible
    //    {
    //        get { return priceColumnVisible; }
    //        set 
    //        {
    //            priceColumnVisible = value;
    //            OnPropertyChanged("PriceColumnVisible");
    //        }
    //    }

    //    bool costColumnVisible;
    //    public bool CostColumnVisible
    //    {
    //        get { return costColumnVisible; }
    //        set
    //        {
    //            costColumnVisible = value;
    //            OnPropertyChanged("CostColumnVisible");
    //        }
    //    }

    //    bool profitColumnVisible;
    //    public bool ProfitColumnVisible
    //    {
    //        get { return profitColumnVisible; }
    //        set
    //        {
    //            profitColumnVisible = value;
    //            OnPropertyChanged("ProfitColumnVisible");
    //        }
    //    }

    //    bool costPercentColumnVisible;
    //    public bool CostPercentColumnVisible
    //    {
    //        get { return costPercentColumnVisible; }
    //        set
    //        {
    //            costPercentColumnVisible = value;
    //            OnPropertyChanged("CostPercentColumnVisible");
    //        }
    //    }

    //    bool ingredientQuantityColumnVisible;

    //    public bool IngredientQuantityColumnVisible
    //    {
    //        get { return ingredientQuantityColumnVisible; }
    //        set
    //        {
    //            ingredientQuantityColumnVisible = value;
    //            OnPropertyChanged("IngredientQuantityColumnVisible");
    //        }
    //    }

    //    bool ingredientColumnVisible;
    //    public bool IngredientColumnVisible
    //    {
    //        get { return ingredientColumnVisible; }
    //        set 
    //        {
    //            ingredientColumnVisible = value;
    //            OnPropertyChanged("IngredientColumnVisible");
    //        }
    //    }

    //    bool waiterColumnVisible;
    //    public bool WaiterColumnVisible
    //    {
    //        get { return waiterColumnVisible; }
    //        set 
    //        {
    //            waiterColumnVisible = value;
    //            OnPropertyChanged("WaiterColumnVisible");
    //        }
    //    }

    //    bool responsibleColumnVisible;
    //    public bool ResponsibleColumnVisible
    //    {
    //        get { return responsibleColumnVisible; }
    //        set 
    //        {
    //            responsibleColumnVisible = value;
    //            OnPropertyChanged("ResponsibleColumnVisible");
    //        }
    //    }

    //    bool areaColumnVisible;
    //    public bool AreaColumnVisible
    //    {
    //        get { return areaColumnVisible; }
    //        set
    //        {
    //            areaColumnVisible = value;
    //            OnPropertyChanged("AreaColumnVisible");
    //        }
    //    }

    //    bool orderColumnVisible;
    //    public bool OrderColumnVisible
    //    {
    //        get { return orderColumnVisible; }
    //        set
    //        {
    //            orderColumnVisible = value;
    //            OnPropertyChanged("OrderColumnVisible");
    //        }
    //    }

    //    #endregion
    //}
}
