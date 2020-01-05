using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.MVVM;
using MiPaladar.Entities;
using MiPaladar.Services;

using System.Text.RegularExpressions;

namespace MiPaladar.ViewModels
{
    public class QuorionConfigDialogViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public QuorionConfigDialogViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            CopyFromMisc();
            //CopyFromAppConfig();

            //hasPendingChanges = false;
            //HasPendingNetworkChanges = false;
        }

        public override string DisplayName
        {
            get { return "MIPALADAR"; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        #region Restaurant Info

        //string companyName;
        //public string CompanyName
        //{
        //    get { return companyName; }
        //    set
        //    {
        //        companyName = value;
        //        OnPropertyChanged("CompanyName");

        //        HasPendingChanges = true;
        //    }
        //}

        string reportsFolder;

        public string ReportsFolder
        {
            get { return reportsFolder; }
            set 
            {
                reportsFolder = value;
                OnPropertyChanged("ReportsFolder");

                //HasPendingChanges = true;
            }
        }

        //decimal defaultTax;

        //public decimal DefaultTax
        //{
        //    get { return defaultTax; }
        //    set
        //    {
        //        defaultTax = value;
        //        OnPropertyChanged("DefaultTax");

        //        HasPendingChanges = true;
        //    }
        //}

        //decimal startingShiftAmount;

        //public decimal StartingShiftAmount
        //{
        //    get { return startingShiftAmount; }
        //    set
        //    {
        //        startingShiftAmount = value;
        //        OnPropertyChanged("StartingShiftAmount");

        //        HasPendingChanges = true;
        //    }
        //}

        string registerIP;
        public string RegisterIP
        {
            get { return registerIP; }
            set
            {
                registerIP = value.Trim();
                OnPropertyChanged("RegisterIP");

                //HasPendingChanges = true;
            }
        }

        public bool IsValidIP(string ip_string)
        {
            string pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

            Regex rgx = new Regex(pattern);

            Match m = rgx.Match(ip_string);

            return m.Success;
        }

        //bool hasPendingChanges;

        //public bool HasPendingChanges
        //{
        //    get { return hasPendingChanges; }
        //    set
        //    {
        //        hasPendingChanges = value;
        //        OnPropertyChanged("HasPendingChanges");
        //    }
        //}        

        #region Browse Reports Folder Command

        RelayCommand browseReportFolderCommand;
        public RelayCommand BrowseReportFolderCommand
        {
            get
            {
                if (browseReportFolderCommand == null)
                    browseReportFolderCommand = new RelayCommand(x => BrowseReportFolder());
                return browseReportFolderCommand;
            }
        }

        void BrowseReportFolder()
        {
            var folderBrowser = base.GetService<IBrowseFolderService>();

            if (folderBrowser.ShowDialog("Seleccione la carpeta donde quiere guardar los reportes") == true)
            {
                ReportsFolder = folderBrowser.SelectedPath;
            }

        }

        #endregion

        #region Save Command

        RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => Save(), x => this.CanSave);
                return saveCommand;
            }
        }

        bool CanSave
        {
            get { return IsValidIP(registerIP); }
        }

        void Save()
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Misc companyInfo = unitOfWork.MiscRepository.Get().First();

                //if (companyInfo.CompanyName != companyName) companyInfo.CompanyName = companyName;
                //if (companyInfo.DefaultTax != defaultTax) companyInfo.DefaultTax = defaultTax;
                //if (companyInfo.StartingShiftAmount != startingShiftAmount) companyInfo.StartingShiftAmount = startingShiftAmount;
                if (companyInfo.ReportsFolder != reportsFolder)
                {
                    companyInfo.ReportsFolder = reportsFolder;
                }
                if (companyInfo.RegisterIP != registerIP)
                {
                    companyInfo.RegisterIP = registerIP;
                }

                unitOfWork.SaveChanges();
            }            

            //HasPendingChanges = false;
        }

        #endregion

        //#region Cancel Command

        //RelayCommand cancelCommand;
        //public RelayCommand CancelCommand
        //{
        //    get
        //    {
        //        if (cancelCommand == null)
        //            cancelCommand = new RelayCommand(x => Cancel());
        //        return cancelCommand;
        //    }
        //}

        //void Cancel()
        //{
        //    RestoreRestaurantInfo();

        //    HasPendingChanges = false;
        //}

        //#endregion

        void CopyFromMisc()
        {
            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                Misc companyInfo = unitOfWork.MiscRepository.GetById(1);

                //CompanyName = companyInfo.CompanyName;
                //DefaultTax = companyInfo.DefaultTax;
                //StartingShiftAmount = companyInfo.StartingShiftAmount;
                ReportsFolder = companyInfo.ReportsFolder;
                RegisterIP = companyInfo.RegisterIP;
            }            
        }

        #endregion

        #region Network Configuration

        //string hostName;
        //public string HostName
        //{
        //    get { return hostName; }
        //    set
        //    {
        //        hostName = value;
        //        OnPropertyChanged("HostName");

        //        HasPendingNetworkChanges = true;
        //    }
        //}

        //string databaseName;
        //public string DatabaseName
        //{
        //    get { return databaseName; }
        //    set
        //    {
        //        databaseName = value;
        //        OnPropertyChanged("DatabaseName");

        //        HasPendingNetworkChanges = true;
        //    }
        //}

        //bool hasPendingNetworkhanges;

        //public bool HasPendingNetworkChanges
        //{
        //    get { return hasPendingNetworkhanges; }
        //    set
        //    {
        //        hasPendingNetworkhanges = value;
        //        OnPropertyChanged("HasPendingNetworkChanges");
        //    }
        //}

        //#region Save Network Command

        //RelayCommand saveNetworkCommand;
        //public RelayCommand SaveNetworkCommand
        //{
        //    get
        //    {
        //        if (saveNetworkCommand == null)
        //            saveNetworkCommand = new RelayCommand(x => SaveNetwork(), x => this.CanSaveNetwork);
        //        return saveNetworkCommand;
        //    }
        //}

        //bool CanSaveNetwork
        //{
        //    get { return !string.IsNullOrWhiteSpace(hostName) && !string.IsNullOrWhiteSpace(databaseName); }
        //}

        //void SaveNetwork()
        //{
        //    var connectionString = ConfigurationManager.ConnectionStrings["RestaurantDBEntities"].ConnectionString;
        //    var connectionStringBuilder = new EntityConnectionStringBuilder(connectionString);

        //    var scsb = new SqlConnectionStringBuilder(connectionStringBuilder.ProviderConnectionString);

        //    bool somethingChanged = false;
        //    if (scsb.DataSource != hostName) { scsb.DataSource = hostName; somethingChanged = true; }
        //    if (scsb.InitialCatalog != databaseName) { scsb.InitialCatalog = databaseName; somethingChanged = true; };

        //    if (somethingChanged)
        //    {
        //        SaveConfig();
        //    }

        //    HasPendingNetworkChanges = false;
        //}

        //void SaveConfig()
        //{
        //    //create new config
        //    var scsb = new SqlConnectionStringBuilder();
        //    scsb.DataSource = hostName;
        //    scsb.InitialCatalog = databaseName;
        //    scsb.IntegratedSecurity = true;
        //    scsb.MultipleActiveResultSets = true;

        //    var builder = new EntityConnectionStringBuilder();
        //    builder.Metadata = "res://*/RestaurantModel.csdl|res://*/RestaurantModel.ssdl|res://*/RestaurantModel.msl";
        //    builder.Provider = "System.Data.SqlClient";
        //    builder.ProviderConnectionString = scsb.ConnectionString;

        //    // Get current configuration.
        //    Configuration currentconfiguration =
        //        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //    // Get the connection strings section
        //    ConnectionStringsSection currentConnectionStringsSection =
        //        currentconfiguration.ConnectionStrings;

        //    // Write the connection string and save the changed config file.
        //    currentconfiguration.ConnectionStrings
        //                        .ConnectionStrings["RestaurantDBEntities"].ConnectionString = builder.ConnectionString;
        //    currentconfiguration.Save(ConfigurationSaveMode.Minimal);

        //    // This is this needed to refresh the configuration manager and get it to
        //    // read the config file again.
        //    ConfigurationManager.RefreshSection("connectionStrings");
        //}

        //#endregion

        //#region Cancel Network Command

        //RelayCommand cancelNetworkCommand;
        //public RelayCommand CancelNetworkCommand
        //{
        //    get
        //    {
        //        if (cancelNetworkCommand == null)
        //            cancelNetworkCommand = new RelayCommand(x => CancelNetWork());
        //        return cancelNetworkCommand;
        //    }
        //}

        //void CancelNetWork()
        //{
        //    CopyFromAppConfig();
        //}

        //void CopyFromAppConfig()
        //{
        //    var connectionString = ConfigurationManager.ConnectionStrings["RestaurantDBEntities"].ConnectionString;
        //    var connectionStringBuilder = new EntityConnectionStringBuilder(connectionString);

        //    var scsb = new SqlConnectionStringBuilder(connectionStringBuilder.ProviderConnectionString);

        //    HostName = scsb.DataSource;
        //    DatabaseName = scsb.InitialCatalog;

        //    HasPendingNetworkChanges = false;
        //} 

        //#endregion

        #endregion

        #region Export Command

        //BackgroundWorker exportWorker;

        //RelayCommand exportCommand;
        //public ICommand ExportCommand
        //{
        //    get
        //    {
        //        if (exportCommand == null)
        //            exportCommand = new RelayCommand(o => this.Export());

        //        return exportCommand;
        //    }
        //}

        //ProgressDialogViewModel pdvm;

        //void Export()
        //{
        //    var folderBrowser = base.GetService<IBrowseFolderService>();

        //    if (folderBrowser.ShowDialog("Seleccione la carpeta a donde quiere exportar los datos") == true)
        //    {
        //        string folderPath = folderBrowser.SelectedPath;

        //        string companyName = appvm.Context.Miscs.First().CompanyName;
        //        string newFolder = folderPath + "\\" + companyName + " " + DateTime.Now.ToString("yyyy-MM-dd, h 'y' mm tt");

        //        if (Directory.Exists(newFolder))
        //        {
        //            var msgBox = base.GetService<IMessageBoxService>();
        //            msgBox.ShowMessage("No se pueden exportar los datos, ya existe una carpeta con la misma fecha.");
        //            return;
        //        }

        //        Directory.CreateDirectory(newFolder);

        //        pdvm = new ProgressDialogViewModel();
        //        pdvm.Message = "Exportando...";
        //        pdvm.IsBusy = true;

        //        //init export worker
        //        if (exportWorker == null)
        //        {
        //            exportWorker = new BackgroundWorker();
        //            exportWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //            exportWorker.WorkerReportsProgress = true;
        //            exportWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        //            exportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        //        }

        //        var windowManager = base.GetService<IWindowManager>();

        //        exportWorker.RunWorkerAsync(newFolder);

        //        windowManager.ShowDialog(pdvm, appvm);
        //    }
        //}

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    string folder = (string)e.Argument;

        //    //var serializer = base.GetService<IXmlSerializationSvc>();
        //    //serializer.Serialize(folder, exportWorker);
        //    //ExportImport.Export(context, folder, exportWorker);
        //}

        //private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    //this.CurrentProgress = e.ProgressPercentage;
        //    pdvm.Progress = e.ProgressPercentage;
        //}

        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    pdvm.IsBusy = false;

        //    var windowManager = base.GetService<IWindowManager>();

        //    //close progress dialog
        //    windowManager.Close(pdvm);
        //}

        #endregion

        #region Import Command

        //RelayCommand importCommand;
        //public RelayCommand ImportCommand
        //{
        //    get
        //    {
        //        if (importCommand == null)
        //            importCommand = new RelayCommand(o => this.Import());

        //        return importCommand;
        //    }
        //}

        //void Import()
        //{
        //    ImportDialogViewModel idvm = new ImportDialogViewModel(appvm);

        //    appvm.ShowDialog("importar", idvm);
        //}

        #endregion

        //#region Clear Command

        //BackgroundWorker clearWorker;

        //RelayCommand clearCommand;
        //public ICommand ClearCommand
        //{
        //    get
        //    {
        //        if (clearCommand == null)
        //        {
        //            clearWorker = new BackgroundWorker();
        //            clearWorker.DoWork += new DoWorkEventHandler(clearWorker_DoWork);
        //            clearWorker.WorkerReportsProgress = true;
        //            clearWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
        //            clearWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(clearWorker_RunWorkerCompleted);

        //            clearCommand = new RelayCommand(x => this.Clear());
        //        }
        //        return clearCommand;
        //    }
        //}

        //void Clear()
        //{
        //    //while (_workspaces.Count > 0)
        //    //_workspaces[0].CloseCommand.Execute(null);
        //    var windowManager = base.GetService<IWindowManager>();
        //    windowManager.CloseAllWindowsButMain();

        //    //CurrentProgress = 0;

        //    //IsBusy = true;

        //    clearWorker.RunWorkerAsync();
        //}

        //void clearWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //ExportImport.ClearDatabase(context, importWorker);
        //}
        //void clearWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    //IsBusy = false;

        //    //messageBoxService.ShowMessage("Terminado");

        //    appvm.LoadData();
        //}

        //#endregion 

        
    }
}