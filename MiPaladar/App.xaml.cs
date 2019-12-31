using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

using MiPaladar.ViewModels;
using MiPaladar.Services;
using MiPaladar.Views;

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
            ServiceContainer.AddService<IExcelExporter>(new ExportToExcelService());
            ServiceContainer.AddService<IXmlSerializationSvc>(new XmlSerializerSvc());
            ServiceContainer.AddService<ICreateFileService>(new CreateFileService());
            ServiceContainer.AddService<IFileCopyService>(new FileCopyService());
            ServiceContainer.AddService<IReport101Reader>(new Report101Reader());
        }

        void RegisterViews(IWindowManager windowManager)
        {
            windowManager.Associate<ChangePasswordViewModel, ChangePasswordView>();
            windowManager.Associate<ProgressDialogViewModel, ProgressDialog>();

            //sales
            windowManager.Associate<NewSaleDialogViewModel, NewSaleDialog>();
            windowManager.Associate<LoadReportDialogViewModel, LoadReportDialog>();
            windowManager.Associate<LoadFromRegisterDialogViewModel, LoadFromRegister>();
            //windowManager.Associate<UnknownProductDialogViewModel, UnknownProductDialog>();
            windowManager.Associate<Load101DialogViewModel, Load101Dialog>();
            windowManager.Associate<Report101InfoViewModel, Report101InfoView>();
            
            windowManager.Associate<SaleViewModel, ValeView>();
            windowManager.Associate<OfflineSaleViewModel, OfflineSaleView>();
            windowManager.Associate<ChargeDialogViewModel, ChargeDialog>();
            windowManager.Associate<DayReportViewModel, DayReportView>();
            windowManager.Associate<DiscountOrTaxDialogViewModel, DiscountOrTaxDialog>();
            //windowManager.Associate<ShiftsListViewModel, ShiftsList>();
            //windowManager.Associate<ShiftViewModel, ShiftView>();

            //purchases
            windowManager.Associate<PurchaseViewModel, PurchaseView>();
            //windowManager.Associate<ComprasViewModel, Compras>();

            //inventory
            windowManager.Associate<InventoryHistoryViewModel, InventoryHistory>();
            //windowManager.Associate<InventoryDetailedViewModel, InventoryDetailed>();
            //windowManager.Associate<InventoryViewModel, InventoryView>();
            //windowManager.Associate<InventoryAreasListViewModel, InventoryAreasList>();
            //windowManager.Associate<CostTracesListViewModel, CostTracesList>();

            windowManager.Associate<AdjustmentsListViewModel, AdjustmentsListView>();
            //windowManager.Associate<NewAdjustmentDialogViewModel, NewAdjustmentDialog>();
            windowManager.Associate<AdjustInventoryViewModel, AdjustInventory>();
            windowManager.Associate<AdjustmentViewModel, AdjustmentView>();

            windowManager.Associate<ProductionsListViewModel, ProductionsList>();
            windowManager.Associate<ProductionViewModel, ProductionView>();

            //windowManager.Associate<FaenasListViewModel, FaenasList>();
            //windowManager.Associate<FaenaViewModel, FaenaView>();

            windowManager.Associate<TransfersListViewModel, TransfersList>();
            windowManager.Associate<TransferViewModel, TransferView>();

            //products
            windowManager.Associate<ProductViewModel, ProductViewFull>();
            //windowManager.Associate<ProductTemplatesViewModel, ProductTemplatesView>();
            //windowManager.Associate<ProductionAreaViewModel, ProductionAreaView>();
            //windowManager.Associate<ProductionAreasListViewModel, ProductionAreasList>();
            //windowManager.Associate<FilterProductViewModel, FilterProductView>();

            //employees
            //windowManager.Associate<PersonalViewModel, Personal>();
            windowManager.Associate<EmployeeViewModel, EmployeeView>();

            //table
            windowManager.Associate<TableViewModel, TableView>();

            //reports
            windowManager.Associate<TotalesViewModel, Totales>();
            windowManager.Associate<OrdersViewModel, Orders>();
            windowManager.Associate<SalesChartViewModel, SalesChart>();

            windowManager.Associate<SalesByItemViewModel, Operations>();
            windowManager.Associate<SoldProductsByEmployeeViewModel, SoldProductsByEmployee>();
            windowManager.Associate<SalesByItemByShiftViewModel, SalesByItemByShift>();
            //windowManager.Associate<CompareProductsViewModel, CompareProducts>();
            windowManager.Associate<FollowProductViewModel, FollowProduct>();
            windowManager.Associate<DayAveragesReportViewModel, DayAveragesReportView>();

            //mainwindow
            windowManager.Associate<MainWindowViewModel, MainWindow>();
        } 
    }
}
