using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

using MiPaladar.ViewModels;
using MiPaladar.Services;
using MiPaladar.Views;
using MiPaladar.MVVM;

using Microsoft.Shell;
using MiPaladar.Stuff;
using System.Windows.Markup;
using System.Globalization;

namespace MiPaladar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "MiPaladarUniqueID";

        // The name of the application folder.  This folder is used to save the 
        // files for this application such as the photos
        public const string ApplicationFolderName = "MiPaladar";
        public const string AppProductsFolderName = "Products";
        public const string AppEmployeesFolderName = "Employees";

        //workaround to solve datagridcolumn datacontext thingy
        static DataGridContextHelper dc = new DataGridContextHelper();

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var app = new App();

                //app.ShutdownMode = ShutdownMode.OnLastWindowClose;

                app.InitializeComponent();                

                app.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }        

        #region ISingleInstanceApp Members

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance
            // ...

            Window mw = Application.Current.MainWindow;

            if (mw.WindowState == WindowState.Minimized)
                mw.WindowState = WindowState.Normal;

            Application.Current.MainWindow.Activate();

            return true;
        }

        #endregion

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //to use current culture (dates in spanish)
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.Name)));
            //

            InjectServices();

            var windowManager = ServiceContainer.GetService<IWindowManager>();

            RegisterViews(windowManager);

            MainWindowViewModel appvm = new MainWindowViewModel();

            //LoginViewModel lvm = new LoginViewModel(() => appvm.LoginSuccessful(lvm.UserName));

            //appvm.ShowDialog("Inicio", lvm);

            windowManager.Show(appvm);
        }

        void InjectServices() 
        {
            ServiceContainer.AddService<IWindowManager>(new WindowManager());
            ServiceContainer.AddService<IMessageBoxService>(new MessageBoxService());
            ServiceContainer.AddService<IOpenFileDialogService>(new OpenFileDialogService());
            ServiceContainer.AddService<IFileReaderService>(new FileReaderService());
            ServiceContainer.AddService<IBrowseFolderService>(new BrowseFolderService());
            ServiceContainer.AddService<IEncrypter>(new Encrypter());
            //ServiceContainer.AddService<IExcelExporter>(new ExportToExcelService());
            ServiceContainer.AddService<ICreateFileService>(new CreateFileService());
            ServiceContainer.AddService<IFileCopyService>(new FileCopyService());
            //ServiceContainer.AddService<IReport101Reader>(new Report101Reader());
            ServiceContainer.AddService<IReportPLUReader>(new ReportPLUReader());
            ServiceContainer.AddService<IReport103Reader>(new Report103Reader());
            ServiceContainer.AddService<IInventoryService>(new InventoryService());
            ServiceContainer.AddService<IQueryService>(new QueryService());

            //ServiceContainer.AddService<IReportingService>(new ReportingService());
            //reports
            ServiceContainer.AddService<IGlobalSalesRS>(new GlobalSalesReport());            
            ServiceContainer.AddService<ISalesByItemRS>(new SalesByItemReport());
            ServiceContainer.AddService<IConteoRS>(new ConteoReport());
            ServiceContainer.AddService<ISalesByCategoryRS>(new SalesByCategoryReport());
            ServiceContainer.AddService<ISalesPersonRS>(new SalesPersonReport());
            ServiceContainer.AddService<IDayOfWeekSalesRS>(new DayOfWeekSalesReport());
            ServiceContainer.AddService<IProductClassesRS>(new ProductClassesReport());
            ServiceContainer.AddService<IWIPByItemRS>(new WIPByItemReport());
            ServiceContainer.AddService<ICostByItemRS>(new CostByItemReport());
            ServiceContainer.AddService<ISalesProjectionsRS>(new SalesProjectionsReport());
            ServiceContainer.AddService<IWIPProjectionsRS>(new WIPProjectionsReport());
            ServiceContainer.AddService<ICostProjectionsRS>(new CostProjectionsReport());
            ServiceContainer.AddService<IServiceTimeRS>(new ServiceTimeReport());
            ServiceContainer.AddService<IDemandByHourRS>(new DemandByHourReport());

            ServiceContainer.AddService<IGlobalSalesEE>(new GlobalSalesReport());
            ServiceContainer.AddService<ISalesByItemEE>(new SalesByItemReport());
            ServiceContainer.AddService<IConteoEE>(new ConteoReport());
            ServiceContainer.AddService<ISalesByCategoryEE>(new SalesByCategoryReport());
            ServiceContainer.AddService<ISalesPersonEE>(new SalesPersonReport());
            ServiceContainer.AddService<IDayOfWeekSalesEE>(new DayOfWeekSalesReport());
            ServiceContainer.AddService<IProductClassesEE>(new ProductClassesReport());
            ServiceContainer.AddService<IWIPByItemEE>(new WIPByItemReport());
            ServiceContainer.AddService<ICostByItemEE>(new CostByItemReport());
            ServiceContainer.AddService<IProjectionsEE>(new SalesProjectionsReport());
            ServiceContainer.AddService<IDemandByHourEE>(new DemandByHourReport());
        }

        void RegisterViews(IWindowManager windowManager)
        {
            windowManager.Associate<ChangePasswordViewModel, ChangePasswordView>();
            windowManager.Associate<ProgressDialogViewModel, ProgressDialog>();
            windowManager.Associate<CustomDatesDialogViewModel, CustomDatesDialog>();
            windowManager.Associate<AddLineDialogViewModel, AddLineDialog>();
            windowManager.Associate<CustomizeReportViewModel, CustomizeReportView>();

            //sales
            //windowManager.Associate<NewSaleDialogViewModel, NewSaleDialog>();
            windowManager.Associate<LoadReportDialogViewModel, LoadReportDialog>();
            windowManager.Associate<LoadFromRegisterDialogViewModel, LoadFromRegister>();
            //windowManager.Associate<UnknownProductDialogViewModel, UnknownProductDialog>();
            //windowManager.Associate<Load101DialogViewModel, Load101Dialog>();
            windowManager.Associate<Report101InfoViewModel, Report101InfoView>();
            windowManager.Associate<Report103ResultViewModel, Report103ResultView>();
            windowManager.Associate<QuorionConfigDialogViewModel, QuorionConfigDialog>();
            
            
            //windowManager.Associate<SaleViewModel, ValeView>();
            windowManager.Associate<OfflineSaleViewModel, OfflineSaleView>();
            //windowManager.Associate<ChargeDialogViewModel, ChargeDialog>();
            //windowManager.Associate<DayReportViewModel, DayReportView>();
            windowManager.Associate<DiscountOrTaxDialogViewModel, DiscountOrTaxDialog>();
            //windowManager.Associate<ShiftsListViewModel, ShiftsList>();
            //windowManager.Associate<ShiftViewModel, ShiftView>();

            //purchases
            //windowManager.Associate<PurchaseViewModel, PurchaseView>();
            //windowManager.Associate<ComprasViewModel, Compras>();

            //inventory
            //windowManager.Associate<InventoryHistoryViewModel, InventoryHistory>();
            //windowManager.Associate<InventoryDetailedViewModel, InventoryDetailed>();
            //windowManager.Associate<InventoryViewModel, InventoryView>();
            //windowManager.Associate<InventoryAreasListViewModel, InventoryAreasList>();
            //windowManager.Associate<CostTracesListViewModel, CostTracesList>();

            //windowManager.Associate<AdjustmentsListViewModel, AdjustmentsListView>();
            //windowManager.Associate<NewAdjustmentDialogViewModel, NewAdjustmentDialog>();
            //windowManager.Associate<AdjustInventoryViewModel, AdjustInventory>();
            //windowManager.Associate<AdjustmentViewModel, AdjustmentView>();

            //products
            windowManager.Associate<ProductViewModel, ProductView>();
            windowManager.Associate<CostHelperViewModel, CostHelperView>();
            windowManager.Associate<CategoryViewModel, CategoryView>();
            windowManager.Associate<TagViewModel, TagView>();
            windowManager.Associate<FixProductsCostsViewModel, FixProductsCosts>();

            //employees
            //windowManager.Associate<PersonalViewModel, Personal>();
            windowManager.Associate<EmployeeViewModel, EmployeeView>();
            windowManager.Associate<RoleViewModel, RoleView>();

            //table
            //windowManager.Associate<TableViewModel, TableView>();

            windowManager.Associate<GlobalSalesReportViewModel, GlobalSalesView>();
            windowManager.Associate<SalesByItemReportViewModel, SalesByItemView>();
            windowManager.Associate<ConteoReportViewModel, ConteoView>();
            windowManager.Associate<SalesByCategoryReportViewModel, SalesByCategoryView>();
            windowManager.Associate<SalesPersonReportViewModel, SalesPersonReportView>();
            windowManager.Associate<DayOfWeekSalesReportViewModel, DayOfWeekSalesView>();
            windowManager.Associate<ProductClassesReportViewModel, ProductClassesView>();
            windowManager.Associate<WIPByItemReportViewModel, WIPByItemView>();
            windowManager.Associate<CostByItemReportViewModel, CostByItemView>();
            windowManager.Associate<SalesProjectionsReportViewModel, ProjectionsByItemView>();
            windowManager.Associate<WIPProjectionsReportViewModel, ProjectionsByItemView>();
            windowManager.Associate<CostProjectionsReportViewModel, ProjectionsByItemView>();
            windowManager.Associate<ServiceTimeReportViewModel, ServiceTimeReportView>();
            windowManager.Associate<DemandByHourReportViewModel, DemandByHourReportView>();

            //mainwindow
            windowManager.Associate<MainWindowViewModel, MainWindow>();
        } 
    }
}

