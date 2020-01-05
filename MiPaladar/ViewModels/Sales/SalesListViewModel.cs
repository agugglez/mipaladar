using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Enums;
using MiPaladar.Classes;
using MiPaladar.Entities;
using MiPaladar.MVVM;
using MiPaladar.Repository;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace MiPaladar.ViewModels
{
    public class SalesListViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;

        public delegate Product CreateProductDelegate(int plu, string name, decimal price);


        public SalesListViewModel(MainWindowViewModel appvm)
        {
            this.appvm = appvm;

            fromDate = DateTime.Today;
            toDate = DateTime.Today;            
        }

        public override string DisplayName
        {
            get { return "LISTA DE VENTAS"; }
        }

        //protected override void OnDispose()
        //{
            //if (loadReportWorker != null) 
            //{
            //    loadReportWorker.DoWork -= loadReportWorker_DoWork;
            //    loadReportWorker.ProgressChanged -= loadReportWorker_ProgressChanged;
            //    loadReportWorker.RunWorkerCompleted -= loadReportWorker_RunWorkerCompleted;
            //}
            //if (load101Worker != null) 
            //{
            //    load101Worker.DoWork -= load101Worker_DoWork;
            //    load101Worker.ProgressChanged -= load101Worker_ProgressChanged;
            //    load101Worker.RunWorkerCompleted -= load101Worker_RunWorkerCompleted;
            //}
            //if (load103Worker != null) 
            //{
            //    load103Worker.DoWork -= load103Worker_DoWork;
            //    load103Worker.ProgressChanged -= load103Worker_ProgressChanged;
            //    load103Worker.RunWorkerCompleted -= load103Worker_RunWorkerCompleted;
            //}
            
        //}

        //void Initialize() 
        //{
        //    //miniMenuVM = new MiniMenuViewModel(appvm);
        //    //miniInventoryVM = new MiniInventoryViewModel(appvm);

        //    //ChangeDay(DateTime.Today);

        //    //base.RequestClose += new EventHandler(VentasViewModel_RequestClose);
        //}

        //void VentasViewModel_RequestClose(object sender, EventArgs e)
        //{
        //    //appvm.PropertyChanged -= appvm_PropertyChanged;

        //    foreach (var item in valeModels)
        //    {
        //        item.PropertyChanged -= vale_PropertyChanged;
        //        //item.RemoveEventHandlers();
        //    }
        //}

        public MainWindowViewModel AppVM 
        {
            get { return appvm; }
        }

        DateTime fromDate, toDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }
        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        protected DateOption sltdDateOption;
        public DateOption SelectedDateOption
        {
            get { return sltdDateOption; }
            set
            {
                sltdDateOption = value;
                OnPropertyChanged("SelectedDateOption");
            }
        }

        public void SetDateOption(DateOption option)
        {
            switch (option)
            {
                case DateOption.Hoy:
                    FromDate = ToDate = DateTime.Today;
                    FindSales();
                    break;
                case DateOption.Ayer:
                    FromDate = ToDate = DateTime.Today.AddDays(-1);
                    FindSales();
                    break;
                case DateOption.Específico:
                    if (ShowCustomDateRangeDialog())
                    {
                        FindSales();
                    }
                    //for some reason this works
                    //dateOption = null;
                    break;
                default:
                    break;
            }
        }

        bool ShowCustomDateRangeDialog()
        {
            var windowManager = base.GetService<IWindowManager>();

            CustomDatesDialogViewModel dvm = new CustomDatesDialogViewModel(fromDate, toDate);

            if (windowManager.ShowDialog(dvm, appvm) == true)
            {
                FromDate = dvm.FromDate;
                ToDate = dvm.ToDate;

                return true;
            }

            return false;
        }

        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }

        //public void ChangeDay(DateTime date)
        //{
        //    this.workingDate = date;

        //    inventoryService.CreateInventorySnapshot(date);

        //    FindSales();
        //}        

        public void FindSales()
        {
            //unsubscribe events
            //foreach (var item in valeModels)
            //{
            //    item.PropertyChanged -= vale_PropertyChanged;
            //    //item.RemoveEvents();
            //}
            //clear vales
            visibleSales.Clear();

            //DateTime toDatePlusOne = toDate.AddDays(1);

            //gives error if comparing date directly
            //var queryResult = from o in appvm.Context.Orders.OfType<Sale>()
            //                  where o.Date >= fromDate && o.Date < toDatePlusOne
            //                  //where o.Waiter.Name=="Diango"
            //                  select o;

            using (var unitOfWOrk = base.GetNewUnitOfWork())
            {
                var queryResult = unitOfWOrk.OrderRepository.QuerySalesByWorkDate(fromDate, toDate);

                foreach (var item in queryResult)
                {
                    //Action<SaleViewModel> showOrder = x => 
                    //{
                    //    ShowingSale = true;
                    //    //SelectedOrder = x; 
                    //};
                    //Action<List<InventoryItem>> updateMessage = x => 
                    //{
                    //    UpdateProductsUnavailableMessage(x);
                    //};
                    //SaleViewModel sovm = new SaleViewModel(appvm, item, updateMessage);
                    SalePreViewModel vm = new SalePreViewModel(item);
                    visibleSales.Add(vm);

                    //sovm.PropertyChanged += new PropertyChangedEventHandler(vale_PropertyChanged);                
                }
            }            

            //if (icvAvailableTables != null) icvAvailableTables.Refresh();

            //if (valeModels.Count > 0) SelectedOrder = valeModels[0];
        }

        #region Create New Vale

        //int GenerateId()
        //{
        //    if (visibleSales.Count() > 0)
        //    {
        //        int? maxIDvale = visibleSales.Max(x => x.Number);

        //        return (maxIDvale.HasValue ? maxIDvale.Value : 0) + 1;
        //    }

        //    return 1;
        //}

        //public Sale CreateNewVale()
        //{
        //    return CreateNewVale(null, null, 0);
        //}

        //public Sale CreateNewVale(Employee waiter, int numberOfPersons)
        //{
        //    return CreateNewVale(DateTime.Today, waiter, numberOfPersons);
        //}

        //public Sale CreateNewVale(DateTime date, Employee waiter, int numberOfPersons)
        //{
        //    //find a new ID for the new order

        //    int newID = GenerateId();

        //    //create the new order
        //    Sale newvale = new Sale();
        //    newvale.Number = newID;

        //    newvale.DateCreated = DateTime.Now;
        //    newvale.Date = date;

        //    //if (table != null) newvale.Table = table;
        //    if (waiter != null) newvale.Employee = waiter;
        //    if (numberOfPersons > 0) newvale.Persons = numberOfPersons;

        //    appvm.Context.Orders.AddObject(newvale);

        //    appvm.SaveChanges();

        //    //Action<SaleViewModel> showOrder = x =>
        //    //{
        //    //    ShowingSale = true;
        //    //    //SelectedOrder = x;
        //    //};

        //    //Action<List<InventoryItem>> updateMessage = x =>
        //    //{
        //    //    UpdateProductsUnavailableMessage(x);
        //    //};

        //    visibleSales.Add(newvale);

        //    Expand(newvale);

        //    return newvale;
        //}

        #endregion        

        //public void RemoveVale(Sale vale)
        //{
        //    //vale.PropertyChanged -= vale_PropertyChanged;

        //    //remove from viewmodel list
        //    visibleSales.Remove(vale);

        //    List<LineItem> lineitems_toRemove = new List<LineItem>();

        //    foreach (var item in vale.LineItems)
        //    {
        //        lineitems_toRemove.Add(item);
        //    }

        //    foreach (var item in lineitems_toRemove)
        //    {
        //        RemoveLineItem(item);
        //    }

        //    //empty vale & add products to inventory
        //    //while (vale.LineItems.Count > 0)
        //    //{
        //    //    LineItem current = vale.LineItems.First();

        //    //    RemoveLineItem(current);
        //    //}            

        //    //remove from database
        //    appvm.Context.Orders.DeleteObject(vale);

        //    appvm.SaveChanges();                        
        //}

        //void RemoveLineItem(LineItem li)
        //{
        //    Inventory pisoInventory = appvm.InventoriesOC.Single(x => x.Name == "Piso");

        //    inventoryService.ExecuteInventoryOperation(pisoInventory, li.Product, li.Quantity, li.UnitMeasure);            

        //    appvm.Context.LineItems.DeleteObject(li);

        //    //appvm.SaveChanges();
        //}
        

        ObservableCollection<SalePreViewModel> visibleSales;
        public ObservableCollection<SalePreViewModel> Vales
        {
            get 
            {
                if (visibleSales == null)
                {
                    visibleSales = new ObservableCollection<SalePreViewModel>();

                    //appvm.InventoryService.CreateInventorySnapshot(DateTime.Today);

                    FindSales();
                }
                return visibleSales;
            }
        }

        //ICollectionView icvVales;
        //public ICollectionView IcvVales 
        //{
        //    get
        //    {
        //        CollectionViewSource cvs = new CollectionViewSource();
        //        cvs.Source = valeModels;
        //        icvVales = cvs.View;

        //        SortDescription sd = new SortDescription("RealDateTime", ListSortDirection.Ascending);
        //        icvVales.SortDescriptions.Add(sd);

        //        return icvVales; 
        //    }
        //}

        //public string ShortWorkingDate
        //{
        //    get
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(((Dias)workingDate.DayOfWeek).ToString());
        //        sb.Append(", " + workingDate.Day);
        //        sb.Append(" " + ((Meses)workingDate.Month - 1).ToString());
        //        sb.Append("/" + workingDate.Year);

        //        return sb.ToString();
        //    }
        //}

        //DateTime workingDate;
        //public DateTime WorkingDate
        //{
        //    get { return workingDate; }
        //    set
        //    {
        //        workingDate = value;

        //        ChangeDay(workingDate);
        //    }
        //}        

        SalePreViewModel selectedOrder;
        public SalePreViewModel SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        #region Expand Item

        //bool showingSale;
        //public bool ShowingSale 
        //{
        //    get { return showingSale; }
        //    set
        //    {
        //        if (showingSale != value) 
        //        {
        //            showingSale = value;
        //            OnPropertyChanged("ShowingSale");
        //        }
        //    }
        //}

        RelayCommand expandSelectedCommand;
        public ICommand ExpandSelectedCommand
        {
            get
            {
                if (expandSelectedCommand == null)
                    expandSelectedCommand = new RelayCommand(x => ExpandSelected(), x => CanExpandSelected);
                return expandSelectedCommand;
            }
        }

        bool CanExpandSelected 
        {
            get { return selectedOrder != null; }
        }

        void ExpandSelected()
        {
            Expand(selectedOrder);
        }

        void Expand(SalePreViewModel saleToShow) 
        {
            var windowManager = base.GetService<IWindowManager>();

            Predicate<ViewModelBase> predicate = (ViewModelBase wsvm) =>
            {
                if (!(wsvm is OfflineSaleViewModel)) return false;

                OfflineSaleViewModel svm = (OfflineSaleViewModel)wsvm;

                return svm.SaleId == saleToShow.SaleId;
            };

            if (windowManager.Exists(predicate)) windowManager.Activate(predicate);
            else
            {
                OfflineSaleViewModel sovm = new OfflineSaleViewModel(appvm, saleToShow.SaleId, OnRemoved, OnModified);
                windowManager.ShowChildWindow(sovm, appvm);
            }
        }

        

        #endregion


        #region New Sale Command

        RelayCommand newSaleCommand;
        public ICommand NewSaleCommand 
        {
            get 
            {
                if (newSaleCommand == null)
                    newSaleCommand = new RelayCommand(x => DoNewSale());
                return newSaleCommand; 
            }
        }

        void DoNewSale() 
        {
            OfflineSaleViewModel saleVM = new OfflineSaleViewModel(appvm, OnCreated, DateTime.Now, DateTime.Today);

            var windowManager = base.GetService<IWindowManager>();
            windowManager.ShowChildWindow(saleVM, appvm);            

            //var windowManager = base.GetService<IWindowManager>();

            //NewSaleDialogViewModel newSaleDialog = new NewSaleDialogViewModel(appvm);

            //if (windowManager.ShowDialog(newSaleDialog, appvm) == true)
            //{
            //    CreateNewVale(newSaleDialog.SelectedTable, newSaleDialog.SelectedWaiter, newSaleDialog.NumberOfPersons);
            //}
        }

        void OnCreated(Sale s)
        {
            visibleSales.Add(new SalePreViewModel(s));
        }

        void OnRemoved(int saleId)
        {
            //remove from viewmodel list
            var targetSale = visibleSales.FirstOrDefault(x => x.SaleId == saleId);

            if (targetSale != null) visibleSales.Remove(targetSale);
        }

        void OnModified(int saleId)
        {
            var target = visibleSales.FirstOrDefault(x => x.SaleId == saleId);

            if (target != null)
            {
                int index = visibleSales.IndexOf(target);

                visibleSales.RemoveAt(index);

                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    var spvm = new SalePreViewModel(unitOfWork.OrderRepository.GetById(saleId));

                    visibleSales.Insert(index, spvm);
                }               
                
            }            
        }

        #endregion

        #region Load From Register Command

        RelayCommand loadFromRegisterCommand;
        public ICommand LoadFromRegisterCommand
        {
            get
            {
                if (loadFromRegisterCommand == null)
                {
                    loadFromRegisterCommand = new RelayCommand(x => LoadFromRegister());
                }
                return loadFromRegisterCommand;
            }
        }        

        void LoadFromRegister() 
        {
            var windowManager = base.GetService<IWindowManager>();

            LoadFromRegisterDialogViewModel dialog = new LoadFromRegisterDialogViewModel();

            if (windowManager.ShowDialog(dialog, appvm) == true)
            {
                //Process myProcess = new Process();
                ////myProcess.StartInfo.UseShellExecute = false;

                //myProcess.StartInfo.FileName = @"C:\OPTIMA\qdriver.exe";

                //myProcess.StartInfo.Arguments = @"C:\OPTIMA\ethernet1.cmd";

                //myProcess.Start();

                ////wait till qdriver ends
                //myProcess.WaitForExit();

                var unitOfWork = base.GetNewUnitOfWork();

                Misc companyInfo = unitOfWork.MiscRepository.GetById(1);// appvm.Context.Miscs.First();
                GenerateCommandFile(dialog.ZMode, companyInfo.RegisterIP);

                try
                {
                    NativeMethods._qdll("ethernet1.cmd", 0, 0, 0);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    NativeMethods._controlfp(0x9001F, 0xFFFFF); // restore floating-point register to default value required by .NET Framework
                }

                //check if qdriver finished OK
                var fileReader = base.GetService<IFileReaderService>();
                List<string> lines = fileReader.ReadLines(@"QDLL.END");

                if (lines.Count > 0 && lines[0] == "000")
                {
                    //read report
                    if (System.IO.File.Exists("report.csv")) LoadReportAsync("report.csv", dialog.SelectedDate, 1);
                    //save the file if z mode
                    if (dialog.ZMode)
                    {
                        var copySvc = base.GetService<IFileCopyService>();

                        //string reportsFolder = appvm.Context.Miscs.First().ReportsFolder;
                        copySvc.SaveReport("report.csv", dialog.SelectedDate, companyInfo.ReportsFolder);
                    }
                }

                //var copy_svc = base.GetService<IFileCopyService>();
                //copy_svc.SaveReport(@"C:\Users\agu\Desktop\New folder\30 abril turno1 cierre Z.csv", dialog.SelectedDate, companyInfo.ReportsFolder);
            }            

        }

        void GenerateCommandFile(bool zMode, string registerIP)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CLEARLOG=1");
            sb.AppendLine("ENDFILE=1");
            sb.AppendLine("ABORT=1");
            sb.AppendLine("RESULT=1");
            sb.AppendLine("PORT=9999");
            sb.AppendLine("IP=" + registerIP);
            sb.AppendLine("REGISTER=1");

            sb.AppendLine("NEWFILE=report.csv");
            sb.AppendLine(string.Format("COMMAND=RU{0}002001001", zMode ? 'Z' : 'X'));

            var cfs = base.GetService<ICreateFileService>();

            cfs.CreateFile("ethernet1.cmd", sb.ToString());
        }

        /// <summary>
        /// saves the report to a dedicated folder, appends '_n' to the file name, (file count)
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="dateTime"></param>
        

        #endregion

        #region Load Report Command

        RelayCommand loadPLUCmd;
        public ICommand LoadPLUCommand
        {
            get
            {
                if (loadPLUCmd == null)
                    loadPLUCmd = new RelayCommand(x => this.ShowLoadPLUDialog());
                return loadPLUCmd;
            }
        }

        BackgroundWorker loadReportWorker;
        ProgressDialogViewModel pdvm;

        void ShowLoadPLUDialog()
        {
            var windowManager = base.GetService<IWindowManager>();

            LoadReportDialogViewModel lrdvm = new LoadReportDialogViewModel(appvm, "Cargar reporte PLU");

            if (windowManager.ShowDialog(lrdvm, appvm) == true)
            {
                LoadReportAsync(lrdvm.FilePath, lrdvm.SelectedDate, lrdvm.SelectedShift.Id);
            }
        }

        void LoadReportAsync(string file_path, DateTime selected_date, int sltdShiftId) 
        {
            //init worker
            if (loadReportWorker == null)
            {
                loadReportWorker = new BackgroundWorker();

                loadReportWorker.DoWork += new DoWorkEventHandler(loadReportWorker_DoWork);

                loadReportWorker.WorkerReportsProgress = true;
                loadReportWorker.ProgressChanged += new ProgressChangedEventHandler(loadReportWorker_ProgressChanged);

                loadReportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadReportWorker_RunWorkerCompleted);
            }

            pdvm = new ProgressDialogViewModel();
            pdvm.Message = "Cargando Reporte...";
            pdvm.IsBusy = true;

            loadReportWorker.RunWorkerAsync(new object[] { file_path, selected_date, sltdShiftId });

            var windowManager = base.GetService<IWindowManager>();

            windowManager.ShowDialog(pdvm, appvm);
        }

        void loadReportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;

            using (var uow = base.GetNewUnitOfWork())
            {
                var pluReader = base.GetService<IReportPLUReader>();

                string filePath = (string)args[0];
                DateTime workDate = (DateTime)args[1];
                int shiftId = (int)args[2];
                e.Result = pluReader.LoadReport(uow, filePath, workDate, shiftId, loadReportWorker);
            }            
        }

        void loadReportWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pdvm.Progress = e.ProgressPercentage;
        }

        void loadReportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pdvm.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();
            windowManager.Close(pdvm);

            if (e.Error != null)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                msgBox.ShowMessage(e.Error.Message);
            }
            else
            {
                //foreach (var item in productsCreated)
                //{
                //    //appvm.ProductsOC.Add(item);
                //    //appvm.GlobalEventsManager.FireProductCreated(item);
                //}

                int saleResultId = (int)e.Result;

                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    SalePreViewModel pvm = new SalePreViewModel(unitOfWork.OrderRepository.GetById(saleResultId));

                    visibleSales.Add(pvm);
                }

                OfflineSaleViewModel vm = new OfflineSaleViewModel(appvm, saleResultId, OnRemoved, OnModified);
                windowManager.ShowChildWindow(vm, appvm);
                
            }

            loadReportWorker.DoWork -= loadReportWorker_DoWork;
            loadReportWorker.ProgressChanged -= loadReportWorker_ProgressChanged;
            loadReportWorker.RunWorkerCompleted -= loadReportWorker_RunWorkerCompleted;
            loadReportWorker = null;
        }

        //private OfflineSaleViewModel LoadReport(IUnitOfWork unitOfWork, string filepath, DateTime selectedDate, Shift sltdShift)
        //{
        //    var fileReader = base.GetService<IFileReaderService>();
        //    List<string> lines = fileReader.ReadLines(filepath);

        //    //nonuseful lines always there
        //    if (lines.Count <= 5) throw new Exception("El reporte está vacío.");

        //    //make sure first line says "INFORME X CODIGOS"
        //    string firstLine = lines[0];
        //    string lineTitle = firstLine.Split(';')[5];

        //    if (lineTitle != "\"INFORME X CODIGOS\"") throw new Exception("Este no es el informe por códigos.");

        //    //Sale new_sale = CreateSale(selectedDate);
        //    //new_sale.WorkSession = FindOrCreateWorkSession(loadreport_dialog.SelectedDate, loadreport_dialog.SelectedShift);

        //    OfflineSaleViewModel svm = new OfflineSaleViewModel(appvm, OnCreated, DateTime.Now, selectedDate);

        //    svm.ShiftId = sltdShift.Id;

        //    //decimal total_price = 0;
        //    //decimal total_cost = 0;
        //    //skip 1 line, don't read the 5 last lines
        //    for (int i = 1; i < lines.Count - 4; i++)
        //    {
        //        string currentLine = lines[i];

        //        string[] array = currentLine.Split(';');

        //        //#4 PLU #5 Name #7 Qty #8 Price

        //        int plu;
        //        int.TryParse(array[4], out plu);

        //        //skip first and last quotes
        //        string name = array[5].Substring(1, array[5].Length - 2);

        //        if (name == "*TOTAL SECCION*") continue;

        //        double quantity;
        //        double.TryParse(array[7], out quantity);

        //        decimal price;
        //        //decimal.TryParse(array[8], out price);
        //        decimal.TryParse(array[8], NumberStyles.Currency, CultureInfo.CurrentCulture, out price);

        //        Product prod = unitOfWork.ProductRepository.GetFromPLU(plu);

        //        //collectionview does not support updating its source collection from a diff thread
        //        //Dispatcher.CurrentDispatcher.Invoke(new CreateProductDelegate(CreateProduct), plu, name, price / (decimal)quantity);

        //        if (prod == null)
        //        {
        //            prod = new Product();
        //            prod.Code = plu; prod.Name = name; prod.SalePrice = price / (decimal)quantity;
        //            prod.ProductType = ProductType.FinishedGoods;
        //            //prod = appvm.CreateProduct(plu, name, price / (decimal)quantity);                    
        //            unitOfWork.ProductRepository.Add(prod);
        //        }

        //        //Product targetProduct = appvm.ProductsOC.Single(x => x.CodeString == plu.ToString());
        //        //svm.NewLineItem(quantity, prod);
        //        if (quantity > 0)
        //        {
        //            svm.NewLineItem(quantity, prod);

        //            //decimal cost = AddLineItemToSale(new_sale, quantity, prod);

        //            //total_price += (decimal)quantity * prod.SalePrice;
        //            //total_cost += cost;
        //        }

        //        //appvm.SaveChanges();

        //        loadReportWorker.ReportProgress(i * 100 / lines.Count);
        //    }

        //    //new_sale.SubTotal = total_price;
        //    //new_sale.Total = total_price;
        //    //new_sale.TotalCost = total_cost;

        //    //appvm.SaveChanges();

        //    return svm;

        //    //LoadReportDialogViewModel loadreport_dialog = new LoadReportDialogViewModel(appvm);

        //    //if (windowManager.ShowDialog(loadreport_dialog, appvm) == true)
        //    //{                
        //    //}
        //    //SelectingTable = !SelectingTable;

        //    //ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //}

        #endregion

        //#region Load101 Command

        //RelayCommand load101;
        //public ICommand Load101Command
        //{
        //    get
        //    {
        //        if (load101 == null)
        //            load101 = new RelayCommand(x => Load101Execute());
        //        return load101;
        //    }
        //}

        //void Load101Execute()
        //{
        //    var open_file_dialog = base.GetService<IOpenFileDialogService>();

        //    string title = "Buscar fichero de Reporte 101";
        //    string filter = "Reportes|*.csv";

        //    if (open_file_dialog.ShowDialog(title, filter) == true)
        //    {
        //        Load101Async(open_file_dialog.FileName);
        //    }
        //}

        //BackgroundWorker load101Worker;
        //ProgressDialogViewModel progressDialog101;

        //void Load101Async(string file_path)
        //{
        //    //init worker
        //    if (load101Worker == null)
        //    {
        //        load101Worker = new BackgroundWorker();

        //        load101Worker.DoWork += new DoWorkEventHandler(load101Worker_DoWork);

        //        load101Worker.WorkerReportsProgress = true;
        //        load101Worker.ProgressChanged += new ProgressChangedEventHandler(load101Worker_ProgressChanged);

        //        load101Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(load101Worker_RunWorkerCompleted);
        //    }

        //    progressDialog101 = new ProgressDialogViewModel();
        //    progressDialog101.Message = "Cargando Reporte 101...";
        //    progressDialog101.IsBusy = true;

        //    load101Worker.RunWorkerAsync(file_path);

        //    var windowManager = base.GetService<IWindowManager>();

        //    windowManager.ShowDialog(progressDialog101, appvm);
        //}

        //void load101Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    e.Result = base.GetService<IReport101Reader>().Load101Report((string)e.Argument, load101Worker);
        //}

        //void load101Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    progressDialog101.Progress = e.ProgressPercentage;
        //}

        //void load101Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    progressDialog101.IsBusy = false;

        //    var windowManager = base.GetService<IWindowManager>();
        //    windowManager.Close(progressDialog101);

        //    if (e.Error != null)
        //    {
        //        var msgBox = base.GetService<IMessageBoxService>();
        //        msgBox.ShowMessage(e.Error.Message);
        //    }
        //    else
        //    {
        //        Report101InfoViewModel result = (Report101InfoViewModel)e.Result;

        //        windowManager.ShowChildWindow(result, appvm);
        //    }

        //    load101Worker.DoWork -= load101Worker_DoWork;
        //    load101Worker.ProgressChanged -= load101Worker_ProgressChanged;
        //    load101Worker.RunWorkerCompleted -= load101Worker_RunWorkerCompleted;
        //}

        //#endregion

        #region Load103 Command

        RelayCommand load103Command;
        public ICommand Load103Command
        {
            get
            {
                if (load103Command == null)
                    load103Command = new RelayCommand(x => Load103Execute());
                return load103Command;
            }
        }

        string report103FileName;
        string report103FilePath;
        DateTime report103Date;
        int report103ShiftId;

        void Load103Execute()
        {
            //var open_file_dialog = base.GetService<IOpenFileDialogService>();

            //string title = "Buscar fichero de Reporte 103";
            //string filter = "Reportes|*.csv";

            //if (open_file_dialog.ShowDialog(title, filter) == true)
            //{
            //    Load103Async(open_file_dialog.FileName);
            //}

            var windowManager = base.GetService<IWindowManager>();

            LoadReportDialogViewModel lrdvm = new LoadReportDialogViewModel(appvm, "Cargar reporte 103");

            if (windowManager.ShowDialog(lrdvm, appvm) == true)
            {
                report103FileName=lrdvm.FileName;
                report103FilePath = lrdvm.FilePath;
                report103Date = lrdvm.SelectedDate;
                report103ShiftId = lrdvm.SelectedShift.Id;

                Load103Async(report103FileName, report103Date, report103ShiftId);
            }
        }

        BackgroundWorker load103Worker;
        ProgressDialogViewModel progressDialog103;

        void Load103Async(string file_path, DateTime workDate, int shiftId)
        {
            //init worker
            if (load103Worker == null)
            {
                load103Worker = new BackgroundWorker();

                load103Worker.DoWork += new DoWorkEventHandler(load103Worker_DoWork);

                load103Worker.WorkerReportsProgress = true;
                load103Worker.ProgressChanged += new ProgressChangedEventHandler(load103Worker_ProgressChanged);

                load103Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(load103Worker_RunWorkerCompleted);
            }

            progressDialog103 = new ProgressDialogViewModel();
            progressDialog103.Message = "Cargando Reporte 103...";
            progressDialog103.IsBusy = true;

            load103Worker.RunWorkerAsync();

            var windowManager = base.GetService<IWindowManager>();

            windowManager.ShowDialog(progressDialog103, appvm);
        }

        void load103Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var reportReader = base.GetService<IReport103Reader>();

            using (var unitOfWork = base.GetNewUnitOfWork())
            {
                e.Result = reportReader.LoadReport103(unitOfWork, report103FilePath, report103Date, report103ShiftId, load103Worker);            
            }            
        }

        void load103Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressDialog103.Progress = e.ProgressPercentage;
        }

        void load103Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressDialog103.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();
            windowManager.Close(progressDialog103);

            if (e.Error != null)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                msgBox.ShowMessage(e.Error.Message);
            }
            else
            {
                List<int> result = (List<int>)e.Result;

                Report103ResultViewModel resultDialog;

                using (var unitOfWork = base.GetNewUnitOfWork())
                {
                    List<Sale> tempSaleList = new List<Sale>();

                    foreach (var item in result)
                    {
                        Sale saleEntity = unitOfWork.OrderRepository.GetById(item);
                        tempSaleList.Add(saleEntity);
                        visibleSales.Add(new SalePreViewModel(saleEntity));
                    }

                    resultDialog = new Report103ResultViewModel(tempSaleList, report103FileName);                    
                }

                windowManager.ShowDialog(resultDialog, appvm);

                //just to update inventory tree
                //appvm.GlobalEventsManager.FireCategoryCreated(0);                
                
            }

            load103Worker.DoWork -= load103Worker_DoWork;
            load103Worker.ProgressChanged -= load103Worker_ProgressChanged;
            load103Worker.RunWorkerCompleted -= load103Worker_RunWorkerCompleted;
            load103Worker = null;
        }

        #endregion
        
        //bool OpenFileDialog(out string fileName)
        //{
        //    var open_file_dialog = base.GetService<IOpenFileDialogService>();

        //    string title = "Buscar fichero de reporte 101";
        //    string filter = "Reportes (.csv)|*.csv";

        //    if (open_file_dialog.ShowDialog(title, filter) == true)
        //    {
        //        fileName = open_file_dialog.FileName;

        //        return true;
        //    }

        //    fileName = null;
        //    return false;
        //}


        //        private Sale LoadFacturaFromLines(List<string> lines, DateTime selected_date, Shift selected_shift)
//        {

////1;1;1;101;87;"  *** FACTURA ***                   2492"
////1;1;1;101;88;"  MESA #                               5"
////1;1;1;101;89;"  CLIENTE #                            1"
////1;1;1;101;90;"2 Extra huevo                       1.00"
////1;1;1;101;91;"1 Extra Bacon                       0.50"
////1;1;1;101;92;"1 R.BajoCaloria                     1.00"
////1;1;1;101;93;"1 Cafe                              0.60"
////1;1;1;101;94;"1 Tostadas c/mantequilla            1.50"
////1;1;1;101;95;"  DESCONTADO %                     -0.23"
////1;1;1;101;96;"6 TOTAL                             4.35"
////1;1;1;101;97;"YANELYS                                 "
////1;1;1;101;98;"  PAGA CON:                        20.00"
////1;1;1;101;99;"  EFECTIVO CUC                      4.35"
////1;1;1;101;100;"  VUELTO                           15.65"
////1;1;1;101;101;" 3-12-13 2  8:17A                       "
////1;1;1;101;102;" #000011 003 000                        "
////1;1;1;101;103;"----------------------------------------"

//            Sale new_sale = new Sale();

//            //Factura #
//            string firstLine = lines[0];
//            int factura_number = int.Parse(firstLine.Substring(firstLine.Length - 5, 4));
//            new_sale.Number = factura_number;

//            //MESA
//            string secondLine = lines[1];
//            int mesa_number = int.Parse(secondLine.Substring(secondLine.Length - 5, 4));

//            //Personas
//            string thirdLine = lines[2];
//            int client_count = int.Parse(thirdLine.Substring(thirdLine.Length - 5, 4));
//            new_sale.Persons = client_count;

//            //line items
//            string pattern = @"(\d+)(.*)(\d+\.\d+)";

//            Regex rgx = new Regex(pattern);

//            int line_count = 3;
//            decimal subtotal = 0;

//            string last_piece;

//            while (true)
//            {
//                string current_line = lines[line_count++];

//                last_piece = current_line.Split(';').Last();

//                if (current_line.Contains("DESCONTADO %"))
//                {
//                    new_sale.Discount = decimal.Parse(last_piece.Substring(last_piece.Length - 7, 6));
//                }
//                else 
//                {
//                    MatchCollection matches = rgx.Matches(last_piece);

//                    Match m = matches[0];

//                    int qtty = int.Parse(m.Groups[1].Value);

//                    string product_name = m.Groups[2].Value.Trim();

//                    decimal price = decimal.Parse(m.Groups[3].Value);

//                    if (product_name == "TOTAL")
//                    {
//                        //coger total
//                        new_sale.Total = price;
//                        break;
//                    }

//                    Product prod = GetProductFromName(product_name);

//                    if (prod == null) prod = AskWhatToDo(product_name, qtty, price);

//                    subtotal += price;

//                    if (qtty > 0)
//                    {
//                        decimal cost = AddLineItemToSale(new_sale, qtty, prod);
//                        new_sale.TotalCost += cost;
//                    }
//                }                
//            }

//            new_sale.SubTotal = subtotal;

//            //next line comes sales person
//            last_piece = lines[line_count++].Split(';').Last();
//            string sales_person_name = last_piece.Trim('"').Trim();

//            new_sale.Employee = GetEmployeeFromName(sales_person_name);

//            //checkout lines
//            line_count++;
//            line_count++;
//            line_count++;

//            //DATE
//            last_piece = lines[line_count++].Split(';').Last();

//            string date_pattern = @"(\d+)-(\d+)-(\d+)\s\d+\s+(\d+):(\d+)(A|P)";
//            Regex date_rgx = new Regex(date_pattern);

//            MatchCollection date_matches = date_rgx.Matches(last_piece);

//            Match date_m = date_matches[0];

//            int day = int.Parse(date_m.Groups[1].Value);
//            int month = int.Parse(date_m.Groups[2].Value);
//            int year = int.Parse(date_m.Groups[3].Value) + 2000;

//            bool pm = date_m.Groups[6].Value == "P";
//            int hour = int.Parse(date_m.Groups[4].Value) + (pm ? 12 : 0);
//            int minutes = int.Parse(date_m.Groups[5].Value);

//            DateTime date_closed = new DateTime(year, month, day, hour, minutes, 0);

//            new_sale.Date = selected_date;
//            new_sale.Shift = selected_shift;

//            new_sale.DateCreated = date_closed;
//            new_sale.DateClosed = date_closed;

//            return new_sale;

//            //appvm.SaveChanges();
//        }

        //#region Helpers

        //private Sale CreateSale(DateTime workDate)
        //{
        //    //find a new ID for the order
        //    int newID = GenerateId();

        //    //create the new order
        //    Sale newvale = new Sale();
        //    newvale.Number = newID;

        //    newvale.DateCreated = DateTime.Now;
        //    newvale.Date = workDate;

        //    appvm.Context.Orders.AddObject(newvale);

        //    return newvale;
        //}

        //Product GetProductFromPLU(int plu)
        //{
        //    foreach (var item in appvm.ProductsOC)
        //    {
        //        if (item.Code == plu) return item;
        //    }
        //    return null;
        //}       

        //private decimal AddLineItemToSale(OfflineSaleViewModel sale, double qtty_to_add, Product product_to_add)
        //{
        //    sale.NewLineItem(qtty_to_add, product_to_add);

        //    SaleLineItem newLineItem = new SaleLineItem();
        //    newLineItem.Quantity = qtty_to_add;
        //    newLineItem.Product = product_to_add;
        //    newLineItem.UnitMeasure = appvm.UnitMeasureManager.Unit;
        //    newLineItem.Amount = (decimal)qtty_to_add * product_to_add.SalePrice;

        //    sale.LineItems.Add(newLineItem);

        //    var ts = base.GetService<ITransactionService>();

        //    decimal cost = ts.Sell(product_to_add, qtty_to_add, sale.Date);

        //    newLineItem.Cost = cost;

        //    return cost;

        //    //ExecuteSellOperation(sale.Date, product_to_add, -qtty_to_add);
        //}

        //#endregion

        #region Show Conteo Command

        RelayCommand conteoCommand;
        public ICommand ShowConteoCommand
        {
            get
            {
                if (conteoCommand == null)
                {
                    conteoCommand = new RelayCommand(x => this.ShowConteo());
                }
                return conteoCommand;
            }
        }

        private void ShowConteo()
        {
            var windowManager = base.GetService<IWindowManager>();

            ConteoReportViewModel vm = new ConteoReportViewModel(appvm);

            windowManager.Show(vm);

            //if (windowManager.Exists<ConteoViewModel>()) windowManager.Activate<ConteoViewModel>();
            //else
            //{
            //    ConteoViewModel cvm = new ConteoViewModel(appvm);
            //    windowManager.Show(cvm);
            //}
        }

        #endregion

        #region Quorion Config Command

        RelayCommand quorionConfigCmd;

        public RelayCommand QuorionConfigCommand
        {
            get
            {
                if (quorionConfigCmd == null)
                {
                    quorionConfigCmd = new RelayCommand(x => ShowQuorionConfigDialog());
                }
                return quorionConfigCmd;
            }
        }

        private void ShowQuorionConfigDialog()
        {
            QuorionConfigDialogViewModel quorionDialog = new QuorionConfigDialogViewModel(appvm);

            var windowManager = base.GetService<IWindowManager>();

            windowManager.ShowDialog(quorionDialog, appvm);
        }

        #endregion
                
    }
}
