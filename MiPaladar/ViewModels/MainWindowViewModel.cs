using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Classes;
using MiPaladar.Services;
using MiPaladar.Entities;
using MiPaladar.Views;

using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data.EntityClient;


namespace MiPaladar.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        RestaurantDBEntities context;

        //IMessageBoxService messageBoxService;
        IInventoryService inventorySvc;
        //IWindowManager windowManager;
        //IPasswordAsker passwordAsker;
        IConfirmator confirmator;
        //IExcelExporter excelExporter;
        //IEncrypter encrypter;
        //IXmlSerializationSvc serializer;
        //PatternMatcher patternMatcher;

        ProductManager productManager;
        //UserManager personnelManager;
        UnitMeasureManager unitMeasureManager;

        public MainWindowViewModel()
        {
            LoginViewModel lvm = new LoginViewModel(OnFirstLoginSuccess);

            ShowDialog("Inicio", lvm);
        }

        void OnFirstLoginSuccess(string username)
        {
            RunLoaderWorker();

            OnLoginSuccess(username);

            //Employee emp = employeesOC.Single(x => string.Compare(x.Name, username, true) == 0);

            //LoggedInUser = emp;            

            //GoToMainView();
        }

        //just need to load data on first login
        //bool firstLogin = true;

        //public bool FirstLogin
        //{
        //    get { return firstLogin; }
        //}

        #region Loader Worker

        BackgroundWorker loaderWorker;
        ProgressDialogViewModel pdvm;

        void RunLoaderWorker()
        {
            if (loaderWorker == null)
            {
                loaderWorker = new BackgroundWorker();

                loaderWorker.DoWork += new DoWorkEventHandler(loaderWorker_DoWork);

                loaderWorker.WorkerReportsProgress = true;
                loaderWorker.ProgressChanged += new ProgressChangedEventHandler(loaderWorker_ProgressChanged);

                loaderWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loaderWorker_RunWorkerCompleted);
            }

            pdvm = new ProgressDialogViewModel();
            pdvm.Message = "Cargando Datos...";
            pdvm.IsBusy = true;

            var windowManager = base.GetService<IWindowManager>();

            loaderWorker.RunWorkerAsync();

            windowManager.ShowDialog(pdvm, this);
        }       

        void loaderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Load();
        }

        private void loaderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pdvm.Progress = e.ProgressPercentage;
        }

        void loaderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pdvm.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();

            //close progress dialog
            windowManager.Close(pdvm);
        }

        void Load()
        {
            //var scsb = new SqlConnectionStringBuilder();
            //scsb.DataSource = "localhost";
            //scsb.InitialCatalog = "RestaurantDB";
            //scsb.IntegratedSecurity = true;
            //scsb.MultipleActiveResultSets = true;

            //var builder = new EntityConnectionStringBuilder();
            //builder.Metadata = "res://*/RestaurantModel.csdl|res://*/RestaurantModel.ssdl|res://*/RestaurantModel.msl";
            //builder.Provider = "System.Data.SqlClient";
            //builder.ProviderConnectionString = scsb.ConnectionString;

            context = new RestaurantDBEntities();

            LoadData();

            LoadServices();

            int count = 0;
            int total = context.Products.Count();

            //RegisterViews();            

            var query = from p in context.Products.Include("RelatedCategories").Include("Ingredients")
                            select p;
            //pre-load data to improve performance
            foreach (var item in query)
            {
                //item.RelatedCategories.Load();
                //item.Ingredients.Load();

                loaderWorker.ReportProgress(++count * 100 / total);
            }
        }

        #endregion

        //public void ReLoadServices()
        //{
        //    //this.inventorySvc = new InventoryService(this);
        //    //this.encrypter = new Encrypter(context);
        //    //this.serializer = new XmlSerializerSvc(context);
        //    //this.personnelManager = new UserManager(employeesOC);
        //}               

        void LoadData()
        {
            LoadProducts();

            LoadCategories();

            LoadInventoryAreas();

            LoadInventory();

            LoadPersonal();

            LoadTables();

            LoadSaleAreas();

            //LoadPurchaseTags();

            LoadProductionAreas();

            LoadShifts();

            unitMeasureManager = new UnitMeasureManager(this);

            productManager = new ProductManager(this);

            //this.patternMatcher = new PatternMatcher(unitMeasureManager);

            //ShowVentas();
        }

        void LoadServices()
        {
            ServiceContainer.AddService<ITransactionService>(new TransactionService(this));

            //this.messageBoxService = new MessageBoxService();
            this.inventorySvc = new InventoryService(this);
            //this.windowManager = new WindowManager();
            //this.encrypter = new Encrypter(context);
            //this.passwordAsker = new PasswordService(encrypter);
            this.confirmator = new Confirmator();
            //this.excelExporter = new ExportToExcelService();
            //this.serializer = new XmlSerializerSvc(context);
            //this.personnelManager = new UserManager(employeesOC);
        }

        #region Navigation

        //bool appWideDialogVisible;
        //public bool AppWideDialogVisible
        //{
        //    get { return appWideDialogVisible; }
        //    set
        //    {
        //        appWideDialogVisible = value;
        //        OnPropertyChanged("AppWideDialogVisible");
        //    }
        //}

        //ViewModelBase appWideViewModel;
        //public ViewModelBase AppWideDialogViewModel
        //{
        //    get { return appWideViewModel; }
        //    set
        //    {
        //        appWideViewModel = value;
        //        OnPropertyChanged("AppWideDialogViewModel");
        //    }
        //}

        void GoToMainView()
        {
            BuildMainMenu();

            //select first
            SelectedMainMenu = mainMenuLinks.FirstOrDefault(); 
        }

        List<MainMenuLinkViewModel> defaultLinks;

        void BuildMainMenu() 
        {
            if (defaultLinks == null) 
            {
                defaultLinks = new List<MainMenuLinkViewModel>();

                if (loggedInUser.Role.CanSeeSales)
                {
                    //SALES
                    MainMenuLinkViewModel salesLink = new MainMenuLinkViewModel(this, "ventas", new ViewModelBase[] 
                    {
                        new SalesListViewModel(this)
                        //new PointOfSaleViewModel(this)
                        //, 
                        //new DesignRestaurantViewModel(this), 
                        //new ProductionAreasListViewModel(this) 
                    });

                    defaultLinks.Add(salesLink);
                }
                if (loggedInUser.Role.CanSeePurchases)
                {
                    //PURCHASES
                    MainMenuLinkViewModel purchasesLink = new MainMenuLinkViewModel(this, "compras", new ViewModelBase[] 
                    {
                        new PurchasesListViewModel(this) 
                    });

                    defaultLinks.Add(purchasesLink);
                }

                if (loggedInUser.Role.CanSeeInventory)
                {
                    //INVENTORY
                    MainMenuLinkViewModel inventoryLink = new MainMenuLinkViewModel(this, "inventario", new ViewModelBase[]
                    {
                        new ProductsListViewModel(this), 
                        new InventoryOperationsListViewModel(this),
                        new ListsTabViewModel(this)
                        //new InventoryAreasListViewModel(this),
                        //new InventoryAdvancedSearchViewModel(this) 
                    });

                    defaultLinks.Add(inventoryLink);
                }

                if (loggedInUser.Role.CanSeeEmployees)
                {
                    List<ViewModelBase> empSubMenus = new List<ViewModelBase>();
                    if (loggedInUser.Role.CanSeeEmployees) empSubMenus.Add(new PersonalViewModel(this));
                    if (loggedInUser.Role.CanSeeRoles) empSubMenus.Add(new RolesListViewModel(this));
                    //EMPLOYEES
                    MainMenuLinkViewModel personalLink = new MainMenuLinkViewModel(this, "personal", empSubMenus.ToArray());

                    defaultLinks.Add(personalLink);
                }

                if (loggedInUser.Role.CanSeeMiPaladar || loggedInUser.Role.CanSeeReports)
                {
                    //MIPALADAR
                    List<ViewModelBase> mpSubMenus = new List<ViewModelBase>();
                    if (loggedInUser.Role.CanSeeReports) mpSubMenus.Add(new ReportsViewModel(this));
                    if (loggedInUser.Role.CanSeeMiPaladar) mpSubMenus.Add(new MyCompanyViewModel(this));

                    MainMenuLinkViewModel mipaladarLink = new MainMenuLinkViewModel(this, "mipaladar", mpSubMenus.ToArray());

                    defaultLinks.Add(mipaladarLink);
                }
                
            }

            SelectedMainMenu = null;

            mainMenuLinks.Clear();

            foreach (var item in defaultLinks)
            {
                mainMenuLinks.Add(item);
            }           
        }

        public void ShowDialog(string title, ViewModelBase dialogViewModel)
        {
            if (selectedMainMenu != null)
            {
                //save current selected link
                activeLinkBeforeDialog = selectedMainMenu;
            }

            SelectedMainMenu = null;

            mainMenuLinks.Clear();

            //create a temp link for the dialog
            MainMenuLinkViewModel dialogLink = new MainMenuLinkViewModel(this, title, new ViewModelBase[] { dialogViewModel });

            mainMenuLinks.Add(dialogLink);

            SelectedMainMenu = dialogLink;
        }        

        MainMenuLinkViewModel activeLinkBeforeDialog;

        public void GoBack() 
        {
            BuildMainMenu();

            SelectedMainMenu = activeLinkBeforeDialog;
        }

        //public void HideAppWideDialog()
        //{
        //    AppWideDialogVisible = false;
        //    AppWideDialogViewModel = null;
        //}

        #endregion        

        //public void LoginSuccessful(string username) 
        //{           
        //    Employee emp = employeesOC.Single(x => x.Name == username);

        //    LoggedInUser = emp;

        //    workspaces.Add(new PointOfSaleViewModel(this));
        //    //workspaces.Add(new SalesListViewModel(this));
        //    workspaces.Add(new PurchasesListViewModel(this));
        //    //workspaces.Add(new MenuViewModel(this));
        //    workspaces.Add(new InventoryViewModel(this));
        //    workspaces.Add(new PersonalViewModel(this, confirmator));
        //    workspaces.Add(new MyCompanyViewModel(this));

        //    firstLogin = false;
        //}

        public void CloseWorkspaces() 
        {
            //if (posVM != null) { posVM.Dispose(); posVM = null; }

            //if (salesVM != null) { salesVM.Dispose(); salesVM = null; }

            //if (purchasesVM != null) { purchasesVM.Dispose(); purchasesVM = null; }

            //if (inventoryVM != null) { inventoryVM.Dispose(); inventoryVM = null; }

            //if (personalVM != null) { personalVM.Dispose(); personalVM = null; }

            //if (myCompanyVM != null) { myCompanyVM.Dispose(); myCompanyVM = null; }

            if (defaultLinks != null)
            {
                foreach (var item in defaultLinks)
                {
                    foreach (var subMenu in item.SubMenuLinks)
                    {
                        subMenu.Dispose();
                    }
                }
            }

            defaultLinks = null;
        }

        public void EmptyWrappers() 
        {
            //unload events
            //productsOC.CollectionChanged -= productsOC_CollectionChanged;
            //categoriesOC.CollectionChanged -= categoriesOC_CollectionChanged;
            //inventoryOC.CollectionChanged -= inventoryOC_CollectionChanged;
            //tablesOC.CollectionChanged -= tablesOC_CollectionChanged;
            //areasOC.CollectionChanged -= areasOC_CollectionChanged;
            //productionAreasOC.CollectionChanged -= productionAreasOC_CollectionChanged;
            //employeesOC.CollectionChanged -= employeesOC_CollectionChanged;                        

            productsOC.Clear();
            productManager.ClearAll();

            categoriesOC.Clear();
            inventoryOC.Clear();
            tablesOC.Clear();
            areasOC.Clear();
            productionAreasOC.Clear();
            inventoryAreasOC.Clear();

            employeesOC.Clear();
            LoggedInUser = null;
            canSellEmployees.Clear();
            canPurchaseEmployees.Clear();
        }

        public void Import(string parentFolder, BackgroundWorker bw) 
        {
            //import
            var serializer = base.GetService<IXmlSerializationSvc>();
            serializer.Deserialize(parentFolder, bw);
            //ExportImport.Import(context, parentFolder, importWorker);

            context = new RestaurantDBEntities();

            LoadData();

            //pre-load data to improve performance
            foreach (var item in context.Products)
            {
                item.RelatedCategories.Load();
                item.Ingredients.Load();
            }

        }

        //#region Side Menu

        ////void NavigateTo(ViewModelBase viewmodel)
        ////{
        ////    //if (currentViewModel != null) currentViewModel.CloseCommand.Execute(null);
        ////    CurrentViewModel = viewmodel;
        ////}

        //RelayCommand navigateToPointOfSaleCommand;
        //public ICommand NavigateToPointOfSaleCommand
        //{
        //    get
        //    {
        //        if (navigateToPointOfSaleCommand == null)
        //            navigateToPointOfSaleCommand = new RelayCommand(x => NavigateToPointOfSale());
        //        return navigateToPointOfSaleCommand;
        //    }
        //}

        //PointOfSaleViewModel posVM;
        //public void NavigateToPointOfSale()
        //{
        //    if (posVM == null) posVM = new PointOfSaleViewModel(this);
        //    CurrentViewModel = posVM;
        //}

        //RelayCommand navigateToVentasCommand;
        //public ICommand NavigateToVentasCommand
        //{
        //    get
        //    {
        //        if (navigateToVentasCommand == null)
        //            navigateToVentasCommand = new RelayCommand(x => NavigateToVentas());
        //        return navigateToVentasCommand;
        //    }
        //}

        //SalesListViewModel salesListVM;
        //public void NavigateToVentas()
        //{
        //    if (salesListVM == null) salesListVM = new SalesListViewModel(this, inventorySvc, /*passwordAsker,*/ confirmator);
        //    CurrentViewModel = salesListVM;
        //}

        //RelayCommand navigateToComprasCommand;
        //public ICommand NavigateToComprasCommand
        //{
        //    get
        //    {
        //        if (navigateToComprasCommand == null)
        //            navigateToComprasCommand = new RelayCommand(x => NavigateToCompras());
        //        return navigateToComprasCommand;
        //    }
        //}

        //PurchasesListViewModel purchasesListVM;
        //void NavigateToCompras()
        //{
        //    if (purchasesListVM == null) purchasesListVM = new PurchasesListViewModel(this);
        //    CurrentViewModel = purchasesListVM;
        //}

        //RelayCommand navigateToMenuCommand;
        //public ICommand NavigateToMenuCommand
        //{
        //    get
        //    {
        //        if (navigateToMenuCommand == null)
        //            navigateToMenuCommand = new RelayCommand(x => NavigateToMenu());
        //        return navigateToMenuCommand;
        //    }
        //}

        //MenuViewModel menuVM;
        //void NavigateToMenu()
        //{
        //    if (menuVM == null) menuVM = new MenuViewModel(this);
        //    CurrentViewModel = menuVM;
        //}

        //RelayCommand navigateToInventoryCommand;
        //public ICommand NavigateToInventoryCommand
        //{
        //    get
        //    {
        //        if (navigateToInventoryCommand == null)
        //            navigateToInventoryCommand = new RelayCommand(x => NavigateToInventory());
        //        return navigateToInventoryCommand;
        //    }
        //}

        //InventoryViewModel inventoryVM;
        //void NavigateToInventory()
        //{
        //    if (inventoryVM == null) inventoryVM = new InventoryViewModel(this, excelExporter);
        //    CurrentViewModel = inventoryVM;
        //}

        //RelayCommand navigateToEmployeesCommand;
        //public ICommand NavigateToEmployeesCommand
        //{
        //    get
        //    {
        //        if (navigateToEmployeesCommand == null)
        //            navigateToEmployeesCommand = new RelayCommand(x => NavigateToEmployees());
        //        return navigateToEmployeesCommand;
        //    }
        //}

        //PersonalViewModel employeesVM;
        //void NavigateToEmployees()
        //{
        //    if (employeesVM == null) employeesVM = new PersonalViewModel(this, confirmator);
        //    CurrentViewModel = employeesVM;
        //}

        //RelayCommand navigateToMyCompanyCommand;
        //public ICommand NavigateToMyCompanyCommand
        //{
        //    get
        //    {
        //        if (navigateToMyCompanyCommand == null)
        //            navigateToMyCompanyCommand = new RelayCommand(x => NavigateToMyCompany());
        //        return navigateToMyCompanyCommand;
        //    }
        //}

        //MyCompanyViewModel myCompanyVM;
        //void NavigateToMyCompany()
        //{
        //    if (myCompanyVM == null) myCompanyVM = new MyCompanyViewModel(this);
        //    CurrentViewModel = myCompanyVM;
        //}

        //#endregion

        public int SaveChanges() 
        {
            return context.SaveChanges();
        }

        #region ObservableCollection Wrappers

        ObservableCollection<Product> productsOC;
        ObservableCollection<Category> categoriesOC;
        //ObservableCollection<ProductTemplate> templatesOC;
        ObservableCollection<InventoryItem> inventoryOC;
        ObservableCollection<Employee> employeesOC;
        ObservableCollection<Table> tablesOC;
        ObservableCollection<PriceList> areasOC;
        //ObservableCollection<PurchaseTag> purchaseTagsOC;
        ObservableCollection<ProductionArea> productionAreasOC;
        ObservableCollection<Shift> shiftsOC;

        private void LoadProducts()
        {
            productsOC = new ObservableCollection<Product>();

            foreach (Product item in context.Products)
            {
                productsOC.Add(item);
            }

            //productsOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(productsOC_CollectionChanged);
        }

        private void LoadCategories()
        {
            categoriesOC = new ObservableCollection<Category>();

            foreach (var item in context.Categories)
            {
                categoriesOC.Add(item);
            }

            //categoriesOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(categoriesOC_CollectionChanged);
        }

        private void LoadInventory()
        {
            inventoryOC = new ObservableCollection<InventoryItem>();

            foreach (var item in context.InventoryItems)
            {
                inventoryOC.Add(item);
            }

            //inventoryOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(inventoryOC_CollectionChanged);
        }

        void LoadTables()
        {
            tablesOC = new ObservableCollection<Table>();

            foreach (var item in context.Tables)
            {
                tablesOC.Add(item);

                //item.PropertyChanged += (sender, e) =>
                //{
                //    if (e.PropertyName == "Name") context.SaveChanges();
                //};
            }

            //tablesOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(tablesOC_CollectionChanged);
        }

        void LoadSaleAreas()
        {
            areasOC = new ObservableCollection<PriceList>();

            foreach (var item in context.PriceLists)
            {
                areasOC.Add(item);
            }

            //areasOC.CollectionChanged += new NotifyCollectionChangedEventHandler(areasOC_CollectionChanged);
        }

        private void LoadProductionAreas()
        {
            productionAreasOC = new ObservableCollection<ProductionArea>();

            foreach (ProductionArea item in context.ProductionAreas)
            {
                productionAreasOC.Add(item);
            }

            //productionAreasOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(productionAreasOC_CollectionChanged);
        }

        private void LoadShifts()
        {
            shiftsOC = new ObservableCollection<Shift>();

            foreach (Shift item in context.Shifts)
            {
                shiftsOC.Add(item);
            }

            //productionAreasOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(productionAreasOC_CollectionChanged);
        }

        public ObservableCollection<Product> ProductsOC
        {
            get { return productsOC; }
        }

        public ObservableCollection<Category> CategoriesOC
        {
            get { return categoriesOC; }
        }

        //public ObservableCollection<ProductTemplate> ProductTemplatesOC
        //{
        //    get { return templatesOC; }
        //}

        public ObservableCollection<InventoryItem> InventoryItemsOC
        {
            get { return inventoryOC; }
        }

        public ObservableCollection<Table> TablesOC
        {
            get { return tablesOC; }
        }

        public ObservableCollection<PriceList> PriceListsOC
        {
            get { return areasOC; }
        }

        //public ObservableCollection<PurchaseTag> PurchaseTagsOC
        //{
        //    get { return purchaseTagsOC; }
        //}

        public ObservableCollection<ProductionArea> ProductionAreasOC
        {
            get { return productionAreasOC; }
        }

        public ObservableCollection<Shift> ShiftsOC
        {
            get { return shiftsOC; }
        }

        #endregion        

        public RestaurantDBEntities Context
        {
            get { return context; }
            set { context = value; }
        }

        #region Employees

        void LoadPersonal()
        {
            rolesOC = new ObservableCollection<Role>();
            employeesOC = new ObservableCollection<Employee>();
            canSellEmployees = new ObservableCollection<Employee>();
            canPurchaseEmployees = new ObservableCollection<Employee>();

            foreach (var item in context.Roles)
            {
                rolesOC.Add(item);
            }

            foreach (var item in context.Employees)
            {
                employeesOC.Add(item);

                if (item.IsActive)
                {
                    if (item.CanSell) canSellEmployees.Add(item);
                    if (item.CanPurchase) canPurchaseEmployees.Add(item);
                }
            }

            //employeesOC.CollectionChanged += 
            //    new NotifyCollectionChangedEventHandler(employeesOC_CollectionChanged);
        }

        //void employeesOC_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        Employee newEmp = (Employee)e.NewItems[0];

        //        context.Employees.AddObject(newEmp);

        //        context.SaveChanges();
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        Employee oldEmp = (Employee)e.OldItems[0];

        //        context.Employees.DeleteObject(oldEmp);

        //        context.SaveChanges();
        //    }
        //}        

        ObservableCollection<Employee> canSellEmployees;
        public ObservableCollection<Employee> CanSellEmployees
        {
            get { return canSellEmployees; }
        }

        ObservableCollection<Employee> canPurchaseEmployees;
        public ObservableCollection<Employee> CanPurchaseEmployees
        {
            get { return canPurchaseEmployees; }
        }

        public ObservableCollection<Employee> EmployeesOC
        {
            get { return employeesOC; }
        }

        ObservableCollection<Role> rolesOC;
        public ObservableCollection<Role> RolesOC
        {
            get { return rolesOC; }
        }

        #endregion

        #region Inventory Areas

        private void LoadInventoryAreas()
        {
            inventoryAreasOC = new ObservableCollection<Inventory>();

            foreach (var item in context.Inventories)
            {
                inventoryAreasOC.Add(item);
            }

            //inventoryAreasOC.CollectionChanged += (sender, e) =>
            //{
            //    if (e.Action == NotifyCollectionChangedAction.Add)
            //    {
            //        Inventory p = e.NewItems[0] as Inventory;

            //        context.Inventories.AddObject(p);

            //        context.SaveChanges();
            //    }
            //    else if (e.Action == NotifyCollectionChangedAction.Remove)
            //    {
            //        Inventory p = e.OldItems[0] as Inventory;

            //        context.Inventories.DeleteObject(p);

            //        context.SaveChanges();
            //    }
            //};
        }

        ObservableCollection<Inventory> inventoryAreasOC;
        public ObservableCollection<Inventory> InventoryAreasOC 
        {
            get { return inventoryAreasOC; }
        }

        #endregion      

        #region File Commands        

        #region Change Password Command

        RelayCommand changePasswordCommand;
        public ICommand ChangePasswordCommand 
        {
            get 
            {
                if (changePasswordCommand == null)
                    changePasswordCommand = new RelayCommand(x => ShowChangePasswordDialog());
                return changePasswordCommand;
            }            
        }

        void ShowChangePasswordDialog() 
        {
            var windowManager = base.GetService<IWindowManager>();

            ChangePasswordViewModel cpvm = new ChangePasswordViewModel(this);

            windowManager.ShowDialog(cpvm, this);
            //if (windowManager.Exists<ChangePasswordViewModel>())
            //    windowManager.Activate<ChangePasswordViewModel>();
            //else
            //{
                
            //}
        }

        #endregion

        #region Logout Command

        RelayCommand logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                    logoutCommand = new RelayCommand(x => Logout());
                return logoutCommand;
            }
        }

        public void Logout()
        {
            LoginViewModel lvm = new LoginViewModel(OnLoginSuccess);

            ShowDialog("Inicio", lvm);

            var windowManager = base.GetService<IWindowManager>();
            windowManager.CloseAllWindowsButMain();

            CloseWorkspaces();

            if (loggedInUser != null)
            {
                loggedInUser.Role.PropertyChanged -= Role_PropertyChanged;

                LoggedInUser = null;
            }

        }

        void OnLoginSuccess(string username) 
        {
            Employee emp = employeesOC.Single(x => x.Name == username);

            LoggedInUser = emp;

            emp.Role.PropertyChanged += Role_PropertyChanged;

            GoToMainView();
        }

        void Role_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Role sender_role = sender as Role;

            //all permissions start with Can
            if (e.PropertyName.StartsWith("Can")) 
            {
                //if (sender_role.CanSeeSales ) 
                //{
                //    IEnumerable<MainMenuLinkViewModel> vms = mainMenuLinks.Where(x => x.DisplayName == "ventas");
                //    bool contains = vms.Count() > 0;

                //    if(!contains)
                //}
            }
        }

        #endregion                        

        #endregion        

        #region Tools Commands
                
        //public bool ListsMenuItemVisible 
        //{
        //    get
        //    {
        //        return loggedInUser != null &&
        //            (loggedInUser.Permissions.CanSeeInventory ||
        //            loggedInUser.Permissions.CanSeeEmployees ||
        //            loggedInUser.Permissions.CanExportImport);
        //    }
        //}

        //public bool ReportsMenuItemVisible 
        //{
        //    get
        //    {
        //        return loggedInUser != null &&
        //            (loggedInUser.Permissions.CanSeeSalesReport ||
        //            loggedInUser.Permissions.CanSeeSalesByItemReport ||
        //            loggedInUser.Permissions.CanSeeFollowProductReport ||
        //            loggedInUser.Permissions.CanSeeDayAveragesReport);
        //    }
        //}        

        #region Inventory History Command

        RelayCommand showInventoryHistoryCommand;
        public ICommand InventoryHistoryCommand
        {
            get
            {
                if (showInventoryHistoryCommand == null)
                {
                    showInventoryHistoryCommand = new RelayCommand(x => this.ShowInventoryHistory());
                }
                return showInventoryHistoryCommand;
            }
        }

        void ShowInventoryHistory()
        {
            var windowManager = base.GetService<IWindowManager>();
            if (windowManager.Exists<InventoryHistoryViewModel>())
                windowManager.Activate<InventoryHistoryViewModel>();
            else
            {
                InventoryHistoryViewModel ihvm = new InventoryHistoryViewModel(this);
                windowManager.Show(ihvm);
            }
        }

        #endregion

        //#region Cost Traces Command

        //RelayCommand showCostTracesListCommand;
        //public ICommand ShowCostTracesListCommand
        //{
        //    get
        //    {
        //        if (showCostTracesListCommand == null)
        //        {
        //            showCostTracesListCommand = new RelayCommand(x => this.ShowCostTracesList());
        //        }
        //        return showCostTracesListCommand;
        //    }
        //}

        //void ShowCostTracesList()
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    if (windowManager.Exists<CostTracesListViewModel>())
        //        windowManager.Activate<CostTracesListViewModel>();
        //    else
        //    {
        //        CostTracesListViewModel ihvm = new CostTracesListViewModel(this);
        //        windowManager.Show(ihvm);
        //    }
        //}

        //#endregion

//        string drawing_image =
//        @"
//            <DrawingImage xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
//                          xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
//                <DrawingImage.Drawing>
//                    <DrawingGroup>
//                        <DrawingGroup.Children>
//                            <GeometryDrawing Geometry='F1 M 216,343L 200,343L 200,327L 216,327L 216,343 Z '>
//                                <GeometryDrawing.Pen>
//                                    <Pen Thickness='0.049998' StartLineCap='Square' EndLineCap='Square' MiterLimit='2.75' Brush='#00FF0000'/>
//                                </GeometryDrawing.Pen>
//                            </GeometryDrawing>
//                            <GeometryDrawing Geometry='F1 M 204.5,334.35L 204.5,330.5L 215.5,330.5L 215.5,342.5L 204.5,342.5L 204.5,340.35'>
//                                <GeometryDrawing.Pen>
//                                    <Pen Thickness='0.700002' StartLineCap='Square' EndLineCap='Square' MiterLimit='2.75' Brush='#FF4F9CF3'/>
//                                </GeometryDrawing.Pen>
//                                <GeometryDrawing.Brush>
//                                    <LinearGradientBrush StartPoint='0.527575,0.945297' EndPoint='0.527575,0.0476626'>
//                                        <LinearGradientBrush.GradientStops>
//                                            <GradientStop Color='#FFF6F9FE' Offset='0'/>
//                                            <GradientStop Color='#FFEFF4FD' Offset='0.207185'/>
//                                            <GradientStop Color='#FFE8F0FC' Offset='0.609375'/>
//                                            <GradientStop Color='#FFF1F6FD' Offset='0.648437'/>
//                                            <GradientStop Color='#FFFAFCFE' Offset='1'/>
//                                        </LinearGradientBrush.GradientStops>
//                                    </LinearGradientBrush>
//                                </GeometryDrawing.Brush>
//                            </GeometryDrawing>
//                            <GeometryDrawing Brush='#FF175CAB' Geometry='F1 M 207.111,333.455L 212.164,337.35L 207.111,341.28L 207.111,333.455 Z '/>
//                            <GeometryDrawing Brush='#FF175CAB' Geometry='F1 M 209.139,337.35L 200.5,337.35'>
//                                <GeometryDrawing.Pen>
//                                    <Pen Thickness='2.66667' MiterLimit='2.75' Brush='#FF175CAB'/>
//                                </GeometryDrawing.Pen>
//                            </GeometryDrawing>
//                        </DrawingGroup.Children>
//                    </DrawingGroup>
//                </DrawingImage.Drawing>
//            </DrawingImage>
//        ";

        //public DrawingImage ExampleImage 
        //{
        //    get 
        //    {
        //        using (var reader = new System.Xml.XmlTextReader(new StringReader(drawing_image)))
        //        {
        //            return System.Windows.Markup.XamlReader.Load(reader) as DrawingImage;
        //        }                
        //    }
        //}
        
        #endregion

        #region Services

        //public IMessageBoxService MessageBoxService
        //{
        //    get { return messageBoxService; }
        //}

        public IInventoryService InventoryService
        {
            get { return inventorySvc; }
        }

        //public IEncrypter Encrypter
        //{
        //    get { return encrypter; }
        //}

        //public IPasswordAsker PasswordAsker
        //{
        //    get { return passwordAsker; }
        //}

        public IConfirmator Confirmator
        {
            get { return confirmator; }
        }

        //public IExcelExporter ExcelExporter
        //{
        //    get { return excelExporter; }
        //}

        //public IXmlSerializationSvc XMLSerializer
        //{
        //    get { return serializer; }
        //}

        //public PatternMatcher PatternMatcher 
        //{
        //    get { return patternMatcher; }
        //}

        public ProductManager ProductManager 
        {
            get { return productManager; }
        }

        Employee loggedInUser;
        public Employee LoggedInUser
        {
            get { return loggedInUser; }
            set
            {
                loggedInUser = value;
                OnPropertyChanged("LoggedInUser");
                OnPropertyChanged("UserIsLoggedIn");

                //if (loggedInUser != null) UpdatePermissionProperties();
            }
        }

        public bool UserIsLoggedIn 
        {
            get { return loggedInUser != null; }
        }

        //public UserManager PersonnelManager 
        //{
        //    get { return personnelManager; }
        //}

        public UnitMeasureManager UnitMeasureManager 
        {
            get { return unitMeasureManager; }
        }

        #endregion

        #region Menu Links

        //PointOfSaleViewModel posVM;
        //SalesListViewModel salesVM;

        //PurchasesListViewModel purchasesVM;

        //InventoryViewModel inventoryVM;
        //ProductListViewModel productsVM;

        //PersonalViewModel personalVM;

        //MyCompanyViewModel myCompanyVM;

        ObservableCollection<MainMenuLinkViewModel> mainMenuLinks = new ObservableCollection<MainMenuLinkViewModel>();

        public ObservableCollection<MainMenuLinkViewModel> MainMenuLinks 
        {
            get { return mainMenuLinks; }
        }

        MainMenuLinkViewModel selectedMainMenu;
        public MainMenuLinkViewModel SelectedMainMenu 
        {
            get { return selectedMainMenu; }
            set
            {
                if (selectedMainMenu != value)
                {
                    selectedMainMenu = value;
                    OnPropertyChanged("SelectedMainMenu");

                    //if (selectedMainMenu != null) OnSelectedMenuChanged();
                }
            }
        }

        //void OnSelectedMenuChanged()
        //{
        //    if (selectedMainMenu == null || selectedMainMenu.SelectedSubMenu == null) return; 

        //    string link = selectedMainMenu.DisplayName;
        //    string subMenuLink = selectedMainMenu.SelectedSubMenu;

        //    if (link == "ventas")
        //    {
        //        if (posVM == null) posVM = new PointOfSaleViewModel(this);

        //        CurrentViewModel = posVM;

        //        //if (salesVM == null) salesVM = new SalesListViewModel(this);

        //        //CurrentViewModel = salesVM;
        //    }
        //    else if (link == "compras")
        //    {
        //        if (purchasesVM == null) purchasesVM = new PurchasesListViewModel(this);

        //        CurrentViewModel = purchasesVM;
        //    }
        //    else if (link == "inventario")
        //    {
        //        if (string.Compare("productos", subMenuLink, true) == 0) 
        //        {
        //            if (productsVM == null) productsVM = new ProductListViewModel(this);

        //            CurrentViewModel = productsVM;
        //        }
        //        else if (string.Compare("moviemientos de inventario", subMenuLink, true) == 0)
        //        {
        //        }
        //    }
        //    else if (link == "personal")
        //    {
        //        if (personalVM == null) personalVM = new PersonalViewModel(this);

        //        CurrentViewModel = personalVM;
        //    }
        //    else if (link == "mipaladar")
        //    {
        //        if (myCompanyVM == null) myCompanyVM = new MyCompanyViewModel(this);

        //        CurrentViewModel = myCompanyVM;
        //    }
        //}

        //ViewModelBase currentViewModel;
        //public ViewModelBase CurrentViewModel 
        //{
        //    get { return currentViewModel; }
        //    set
        //    {
        //        currentViewModel = value;
        //        OnPropertyChanged("CurrentViewModel");
        //    }
        //}

        //ObservableCollection<ViewModelBase> workspaces = new ObservableCollection<ViewModelBase>();

        //public ObservableCollection<ViewModelBase> Workspaces         
        //{
        //    get { return workspaces; }
        //}

        #endregion
    }
}
