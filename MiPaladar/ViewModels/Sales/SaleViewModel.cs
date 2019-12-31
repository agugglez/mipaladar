using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Services;
using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Classes;
using MiPaladar.Extensions;

using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Collections;

namespace MiPaladar.ViewModels
{
    public class SaleViewModel : ViewModelBase
    {
        MainWindowViewModel appvm;        
        Sale sale;
        Action<Sale> onRemoved;

        ProductManager productManager;

        Action onCheckOut, onClientsChanged, onWaiterChanged;

        public SaleViewModel(MainWindowViewModel appvm, Sale salesOrder, Action<Sale> onRemoved)
        {
            this.appvm = appvm;
            this.onRemoved = onRemoved;
            this.sale = salesOrder;

            this.productManager = appvm.ProductManager;

            RawTotal = sale.LineItems.Sum(x => ((SaleLineItem)x).Amount);
        }

        public SaleViewModel(MainWindowViewModel appvm, Sale salesOrder, Action<Sale> onRemoved, 
            Action onCheckOut, Action onClientsChanged, Action onWaiterChanged)
        {
            this.appvm = appvm;            
            this.sale = salesOrder;

            this.onCheckOut = onCheckOut;
            this.onClientsChanged = onClientsChanged;
            this.onWaiterChanged = onWaiterChanged;
            this.onRemoved = onRemoved;

            this.productManager = appvm.ProductManager;

            RawTotal = sale.LineItems.Sum(x => ((SaleLineItem)x).Amount);
        }

        //public override string DisplayName
        //{
        //    get
        //    {
        //        return string.Format("Vale {0}", Number);
        //    }
        //    protected set
        //    {
        //        base.DisplayName = value;
        //    }
        //}

        public MainWindowViewModel AppVM { get { return appvm; } }        

        #region Show Charge Dialog Command

        RelayCommand showChargeDialogCommand;
        public ICommand ShowChargeDialogCommand
        {
            get
            {
                if (showChargeDialogCommand == null)
                    showChargeDialogCommand = new RelayCommand(x => ShowChargeDialog());
                return showChargeDialogCommand;
            }
        }

        void ShowChargeDialog() 
        {
            var windowManager = base.GetService<IWindowManager>();

            ChargeDialogViewModel chargeDialog = new ChargeDialogViewModel(rawTotal, AjusteToMoney, TaxToMoney, TotalPrice, Cash, Tips);

            if (windowManager.ShowDialog(chargeDialog, this) == true)
            {
                Cash = chargeDialog.Cash;                
                Tips = chargeDialog.Tips;

                Paid = true;
                Closed = true;

                if (onCheckOut != null) onCheckOut();

                //CreateNewVale(newSaleDialog.SelectedTable, newSaleDialog.SelectedWaiter, newSaleDialog.NumberOfPersons);

                //ShowingSale = true;
            }
        }

        #endregion        

        #region Print Command

        RelayCommand printCommand;
        public ICommand PrintCommand 
        {
            get 
            {
                if (printCommand == null)
                    printCommand = new RelayCommand(x => this.Print(), x => this.CanPrint);
                return printCommand; 
            }
        }

        bool CanPrint { get { return !Closed; } }

        void Print()
        {
            if (Printer.PrintVale(this))
            {
                sale.DatePrinted = DateTime.Now;

                sale.Prints++;

                Close();

                appvm.SaveChanges();
            }
        }

        #endregion

        #region Print Document

        RelayCommand printDocument;
        public ICommand PrintDocumentCommand
        {
            get
            {
                if (printDocument == null)
                    printDocument = new RelayCommand(x => this.PrintDocument());
                return printDocument;
            }
        }

        void PrintDocument()
        {
            Printer.PrintValeDocument(this);
            //if (Printer.PrintValeDocument(this))
            //{
            //    sale.DatePrinted = DateTime.Now;

            //    sale.Prints++;

            //    Close();

            //    appvm.SaveChanges();
            //}
        }

        #endregion

        #region Print Production Command

        RelayCommand printProductionCommand;
        public ICommand PrintProductionCommand
        {
            get
            {
                if (printProductionCommand == null)
                    printProductionCommand = new RelayCommand(x => this.PrintProduction(), 
                        x => this.CanPrintProduction);
                return printProductionCommand;
            }
        }

        bool CanPrintProduction { get { return !Closed; } }

        void PrintProduction()
        {

            if (Printer.PrintProductionVale(this, Number))
            {
                foreach (var item in lineitems)
                {
                    if (item.IsEntrant)
                    {
                        if (!item.Product.IsEntrant) item.Product.IsEntrant = true;
                    }

                    //item.LineItem.Printed = true;
                    item.Printed = true;

                    appvm.SaveChanges();
                }
            }
        }

        #endregion

        #region Toggle Close command

        RelayCommand toggleCloseCommand;
        public ICommand ToggleCloseCommand
        {
            get
            {
                if (toggleCloseCommand == null)
                    toggleCloseCommand = new RelayCommand(x => this.ToggleClose());
                return toggleCloseCommand;
            }
        }

        //bool CanToggleClose 
        //{
        //    get { return !Closed || appvm.PersonnelManager.UserCanOpenSaleOrders; }
        //}

        void ToggleClose()
        {
            if (!Closed)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox.ShowYesNoDialog("¿Está seguro que desea cerrar el vale?") == true) Close();
                //if (!confirmator.AskForConfirmation("Está seguro que desea cerrar el vale?")) return;

                //Close();
            }
            else 
            {
                //if (!appvm.AdminMode && !passwordAsker.AskForPassword()) return;
                //Paid = false;
                Closed = false;                
            }
        }

        void Close() 
        {
            Closed = true;

            if (sale.DateClosed == null)
            {
                sale.DateClosed = DateTime.Now;
                appvm.SaveChanges();
            }
        }

        #endregion                

        #region Remove Command

        RelayCommand removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new RelayCommand(x => this.Remove());
                return removeCommand;
            }
        }

        BackgroundWorker removeWorker;
        ProgressDialogViewModel pdvm;

        //bool CanRemove { get { return appvm.LoggedInUser.Permissions.CanRemoveSales; } }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar este vale?";

            if (msgBox.ShowYesNoDialog(message) == true)
            {
                if (removeWorker == null) 
                {
                    removeWorker = new BackgroundWorker();

                    removeWorker.DoWork += new DoWorkEventHandler(removeWorker_DoWork);

                    removeWorker.WorkerReportsProgress = true;
                    removeWorker.ProgressChanged += new ProgressChangedEventHandler(removeWorker_ProgressChanged);

                    removeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(removeWorker_RunWorkerCompleted);
                }

                pdvm = new ProgressDialogViewModel();
                pdvm.Message = "Eliminando...";
                pdvm.IsBusy = true;

                var windowManager = base.GetService<IWindowManager>();

                removeWorker.RunWorkerAsync();

                windowManager.ShowDialog(pdvm, this);
            }
        }               

        void removeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RemoveVale();
        }

        void removeWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pdvm.Progress = e.ProgressPercentage;
        }

        void removeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pdvm.IsBusy = false;

            var windowManager = base.GetService<IWindowManager>();

            //close progress dialog
            windowManager.Close(pdvm);

            //let parent remove it
            if (onRemoved != null) onRemoved(sale);

            //close this window
            windowManager.Close(this);
        } 

        public void RemoveVale()
        {
            int total_lines = lineitems.Count;
            int count = 0;

            List<LineItem> lineitems_toRemove = new List<LineItem>(sale.LineItems);
            foreach (SaleLineItem item in lineitems_toRemove)
            {
                var ts = base.GetService<ITransactionService>();

                ts.UndoSell(item.Product, item.Quantity, item.Cost, WorkingDate);
                
                appvm.Context.LineItems.DeleteObject(item);

                appvm.SaveChanges();

                removeWorker.ReportProgress(++count * 100 / total_lines);
            }           

            //remove from database
            appvm.Context.Orders.DeleteObject(sale);

            appvm.SaveChanges();
        }

        #endregion

        #region Remove LineItem Command

        public LineItemViewModel SelectedLineItem { get; set; }

        RelayCommand removeLineItemCommand;
        public ICommand RemoveLineItemCommand
        {
            get
            {
                if (removeLineItemCommand == null)
                    removeLineItemCommand = new RelayCommand(x => this.DoRemoveLineItemCommand(SelectedLineItem), x => this.CanRemoveLineItem);
                return removeLineItemCommand;
            }
        }

        public bool CanRemoveLineItem
        {
            get 
            {
                return SelectedLineItem != null && !Closed/* &&
                    (!SelectedLineItem.Printed || appvm.PersonnelManager.UserCanRemoveCalledLineItem)*/;
            }
        }

        public void DoRemoveLineItemCommand(LineItemViewModel livm)
        {
            //if (livm.Printed && !appvm.AdminMode)
            //{
            //    string message = "No puede eliminar productos después de haberlos marchados, " +
            //        "necesita permisos de Administrador";
            //    var msgBox = base.GetService<IMessageBoxService>();
            //    msgBox.ShowMessage(message);
            //    return;
            //}
            //else
                RemoveLineItem(livm);

                RefreshTotal();
        }
        public void RemoveLineItem(LineItemViewModel livm)
        {
            //if (livm.Printed && !appvm.AdminMode)
            //{
            //    string message = "No puede eliminar productos después de haber imprimido "+
            //        "el vale de cocina, necesita permisos de Administrador";
            //    messageBoxService.ShowMessage(message);
            //}
            //else 
            //{            

            //ExecuteSellOperation(livm.Product, livm.Quantity);

            SaleLineItem li = livm.LineItem;

            var ts = base.GetService<ITransactionService>();

            ts.UndoSell(livm.Product, livm.Quantity, li.Cost, WorkingDate);

            lineitems.Remove(livm);

            //update total cost
            sale.TotalCost -= li.Cost;

            appvm.Context.LineItems.DeleteObject(li);

            appvm.SaveChanges();
                
            //}            
        }

        //bool ProductionOrderPrinted() 
        //{
        //    foreach (var item in lineitems)
        //    {
        //        if (item.Printed) return true;
        //    }
        //    return false;
        //}

        #endregion

        #region New LineItem Command

        //ProductQuantityViewModel itemToAdd;
        //public ProductQuantityViewModel ItemToAdd 
        //{
        //    get 
        //    {
        //        if (itemToAdd == null) itemToAdd = new ProductQuantityViewModel(appvm);
        //        return itemToAdd; 
        //    }
        //    set { itemToAdd = value; }
        //}

        double quantityToAdd;
        public double QuantityToAdd
        {
            get { return quantityToAdd; }
            set
            {
                quantityToAdd = value;
                OnPropertyChanged("QuantityToAdd");
            }
        }

        Product productToAdd;
        public Product ProductToAdd
        {
            get { return productToAdd; }
            set
            {
                productToAdd = value;

                OnPropertyChanged("ProductToAdd");
            }
        }

        //UnitMeasure unitMeasureToAdd;
        //public UnitMeasure UnitMeasureToAdd
        //{
        //    get { return unitMeasureToAdd; }
        //    set
        //    {
        //        unitMeasureToAdd = value;
        //        OnPropertyChanged("UnitMeasureToAdd");
        //    }
        //}

        //string quantityExpression;
        //public string QuantityExpression
        //{
        //    get
        //    {
        //        if (quantityExpression == null)
        //        {
        //            if (quantityToAdd == 0 || unitMeasureToAdd == null) quantityExpression = string.Empty;
        //            else quantityExpression = quantityToAdd + unitMeasureToAdd.Caption;
        //        }
        //        return quantityExpression;
        //    }
        //    set
        //    {
        //        quantityExpression = value;
        //        OnPropertyChanged("QuantityExpression");

        //        if (!string.IsNullOrWhiteSpace(quantityExpression))
        //        {
        //            float float_part; UnitMeasure um_part;
        //            patternMatcher.ParseQuantityString(quantityExpression, out float_part, out um_part);

        //            QuantityToAdd = float_part;
        //            UnitMeasureToAdd = um_part;
        //        }

        //        OnPropertyChanged("QuantityExpression");
        //    }
        //}

        RelayCommand newLineItemCommand;
        public ICommand NewCommand 
        {
            get
            {
                if (newLineItemCommand == null)
                    newLineItemCommand = new RelayCommand(x => this.NewLineItem(quantityToAdd, productToAdd), x => this.CanNew);
                return newLineItemCommand; 
            }
        }

        public bool CanNew
        {
            get
            {
                return /*!itemToAdd.HasErrors &&*/ quantityToAdd > 0 && productToAdd != null;
            }
        }

        public void NewLineItem(double qtty_to_add, Product product_to_add)
        {
            SaleLineItem newLineItem = new SaleLineItem();
            newLineItem.Quantity = qtty_to_add;
            newLineItem.Product = product_to_add;
            newLineItem.UnitMeasure = appvm.UnitMeasureManager.Unit;
            newLineItem.Amount = (decimal)qtty_to_add * product_to_add.SalePrice;

            //Action onQuantityUpdated = () => RefreshTotal();

            LineItemViewModel newLineItemViewModel =
                new LineItemViewModel(newLineItem, this, RefreshTotal, RefreshCost);
            OrderItems.Add(newLineItemViewModel);

            sale.LineItems.Add(newLineItem);

            CheckProductIsAvailable(product_to_add, qtty_to_add);

            var ts = base.GetService<ITransactionService>();
            decimal cost = ts.Sell(product_to_add, qtty_to_add, WorkingDate);

            newLineItem.Cost = cost;

            sale.TotalCost += cost;
            //ExecuteSellOperation(product_to_add, -qtty_to_add);

            appvm.SaveChanges();

            //this saves changes
            RefreshTotal();

            //clear input fields          
            //UnitMeasureToAdd = null;
            QuantityToAdd = 1;
            ProductToAdd = null;
            //SearchText = string.Empty;

            OnLineItemAdded();
        }

        public event EventHandler LineItemAdded;

        protected void OnLineItemAdded()
        {
            EventHandler handler = this.LineItemAdded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
                       
        public void RefreshTotal()
        {
            decimal total = 0;
            //refresh total price
            foreach (SaleLineItem vi in sale.LineItems)
            {
                total += vi.Amount;
            }

            RawTotal = total;
            sale.SubTotal = total;

            decimal discount = 0;

            if (!DiscountInPercent)
                discount = Discount;
            else discount = total * Discount / 100;

            //round to cents
            discount = discount - discount % 0.05m;

            decimal tax = 0;

            if (!TaxInPercent)
                tax = Tax;
            else tax = total * Tax / 100;

            decimal mod = tax % 0.05m;
            //round to cents
            if (mod > 0) { tax += 0.05m - mod; }

            TotalPrice = total + tax - discount;
        }

        public void RefreshCost() 
        {
            sale.TotalCost = lineitems.Sum(x => x.Cost);

            appvm.SaveChanges();
        }

        decimal rawTotal;
        public decimal RawTotal 
        {
            get { return rawTotal; }
            set 
            {
                rawTotal = value;
                OnPropertyChanged("RawTotal");
            }
        }

        public decimal TotalPrice 
        {
            get { return sale.Total; }
            set 
            {
                sale.Total = value;
                appvm.SaveChanges();
                OnPropertyChanged("TotalPrice");
                OnPropertyChanged("Change");
            }
        }

        public ObservableCollection<Employee> Waiters 
        {
            get { return appvm.CanSellEmployees; }
        }
        //public ObservableCollection<PriceList> PriceLists 
        //{
        //    get { return appvm.PriceListsOC; }
        //}
        public ObservableCollection<Table> Tables 
        {
            get { return appvm.TablesOC; }
        }

        //ICollectionView icvVentaItems;
        //ObservableCollection<Product> ventaItems;
        public ObservableCollection<Product> VentaItems 
        {
            get { return productManager.VentaItems; }
        }

        //string searchText;
        //public string SearchText
        //{
        //    get { return searchText; }
        //    set
        //    {
        //        searchText = value;
        //        OnPropertyChanged("SearchText");

        //        //Typing = !string.IsNullOrEmpty(searchText);

        //        //icvVentaItems.Refresh();
        //    }
        //}

        #region Properties
                
        public string Notes
        {
            get { return sale.Memo; }
            set
            {
                sale.Memo = value;
                appvm.SaveChanges();
                OnPropertyChanged("HasNotes");
            }
        }

        public bool HasNotes
        {
            get { return !string.IsNullOrWhiteSpace(Notes); }
        }

        public bool Closed
        {
            get { return sale.Closed; }
            set
            {
                if (sale.Closed != value)
                {
                    sale.Closed = value;
                    appvm.SaveChanges();

                    OnPropertyChanged("Closed");
                    OnPropertyChanged("Opened");
                    OnPropertyChanged("OpenOrCloseContent");
                }
            }
        }

        public bool Opened
        {
            get { return !Closed; }
        }

        public string OpenOrCloseContent
        {
            get { return Closed ? "Abrir" : "Cerrar"; }
        }

        public bool Paid 
        {
            get { return sale.Paid; }
            set
            {
                if (sale.Paid != value)                 
                {
                    sale.Paid = value;
                    appvm.SaveChanges();

                    OnPropertyChanged("Paid");
                    OnPropertyChanged("UnPaid");
                }                
            }
        }

        public bool UnPaid { get { return !sale.Paid; } }

        public int Persons
        {
            get { return sale.Persons; }
            set
            {
                sale.Persons = value;
                appvm.SaveChanges();
                OnPropertyChanged("Persons");
            }
        }

        public decimal Cash
        {
            get { return sale.Cash; }
            set
            {
                sale.Cash = value;
                appvm.SaveChanges();
            }
        }

        public decimal Tips
        {
            get { return sale.Tips; }
            set
            {
                sale.Tips = value;
                appvm.SaveChanges();
            }
        }

        #region Discount

        public decimal Discount
        {
            get { return sale.Discount; }
            set
            {
                sale.Discount = value;
                appvm.SaveChanges();
                //OnPropertyChanged("Discount");
                OnPropertyChanged("AjusteToMoney");
                RefreshTotal();
            }
        }

        public bool DiscountInPercent
        {
            get { return sale.DiscountInPercent; }
            set
            {
                sale.DiscountInPercent = value;
                appvm.SaveChanges();
                OnPropertyChanged("DiscountInPercent");
                OnPropertyChanged("DiscountInMoney");
                OnPropertyChanged("AjusteToMoney");

                RefreshTotal();
            }
        }
        public bool DiscountInMoney
        {
            get { return !sale.DiscountInPercent; }
        }

        public decimal AjusteToMoney
        {
            get
            {
                return sale.DiscountToMoney();
                //if (DiscountInPercent)
                //{
                //    //decimal basictotal = OrderItems.Sum(x => x.Price);

                //    return sale.SubTotal * Discount / 100;
                //}
                //decimal temp = DiscountInPercent ? sale.SubTotal * Discount / 100 : Discount;

                //return temp - temp % 0.05m;
            }
        }

        #endregion

        #region Tax

        public decimal Tax
        {
            get { return sale.Tax; }
            set
            {
                sale.Tax = value;
                appvm.SaveChanges();
                OnPropertyChanged("AjusteToMoney");
                RefreshTotal();
            }
        }

        public bool TaxInPercent
        {
            get { return sale.TaxInPercent; }
            set
            {
                sale.TaxInPercent = value;
                appvm.SaveChanges();
                OnPropertyChanged("TaxInPercent");
                OnPropertyChanged("TaxInMoney");
                OnPropertyChanged("TaxToMoney");

                RefreshTotal();
            }
        }
        public bool TaxInMoney
        {
            get { return !sale.TaxInPercent; }
        }

        public decimal TaxToMoney
        {
            get
            {
                return sale.TaxToMoney();
                //decimal temp = TaxInPercent ? sale.SubTotal * Tax / 100 : Tax;

                //decimal mod = temp % 0.05m;

                //if (mod > 0) { temp += 0.05m - mod; }

                //return temp;
            }
        }

        #endregion

        public Employee Waiter
        {
            get { return sale.Employee; }
            set
            {
                if (sale.Employee != value) 
                {
                    sale.Employee = value;
                    appvm.SaveChanges();

                    if (onWaiterChanged != null) onWaiterChanged();
                }
                
            }
        }

        //public PriceList PriceList
        //{
        //    get
        //    {
        //        return sale.Table != null ? sale.Table.PriceList : null;
        //    }
        //    //set
        //    //{
        //    //    sale.Table.PriceList = value;
        //    //    appvm.SaveChanges();
        //    //}
        //}

        public int? Number
        {
            get { return sale.Number; }
            set
            {
                sale.Number = value;
                OnPropertyChanged("Number");
                appvm.SaveChanges();
            }
        }

        public Table Table
        {
            get { return sale.Table; }
            set
            {
                if (value != null) 
                {
                    //oldValue = sale.Table.PriceList;

                    sale.Table = value;
                    OnPropertyChanged("Table");

                    //onTableChanged();

                    appvm.SaveChanges();
                }                                
            }
        }

        //PriceList oldValue;

        //void onTableChanged() 
        //{
        //    if (oldValue != sale.Table.PriceList) 
        //    {
        //        var ts = base.GetService<ITransactionService>();

        //        foreach (var item in lineitems)
        //        {
        //            ts.UndoSell(item.Product, item.Quantity, item.LineItem.Cost, WorkingDate, oldValue);
        //        }

        //        foreach (var item in lineitems)
        //        {
        //            decimal cost = ts.Sell(item.Product, item.Quantity, WorkingDate);

        //            item.LineItem.Cost = cost;
        //        }
        //    }
        //}

        public DateTime WorkingDate 
        {
            get { return sale.Date; }
        }

        public DateTime RealDateTime
        {
            get
            {
                return sale.DateCreated;
            }
        }       

        public decimal Change
        {
            get
            {
                return Cash - TotalPrice;
            }
        }

        //public WorkSession WorkSession 
        //{
        //    get { return sale.WorkSession; }
        //}

        //public string ShortTime
        //{
        //    get
        //    {
        //        return sale.DateCreated.ToShortTimeString();
        //    }
        //}

        public int Prints 
        {
            get { return sale.Prints; }
            set 
            {
                sale.Prints = value;
                OnPropertyChanged("Prints");
            }
        }

        #endregion        

        #region Print Helpers

        public string WaiterPrefix
        {
            get
            {
                return Waiter != null ? Waiter.Name.Substring(0, 3) : string.Empty;
            }
        }

        public string ShortID
        {
            get 
            {
                return Table.PriceList.Name.Substring(0, 3) + Number.ToString().PadLeft(3, '0');
            }
        }

        //public string MonthSpanish
        //{
        //    get { return ((Meses)(sale.Date.Month - 1)).ToString(); }
        //}

        

        public bool TieneDescuento
        {
            get { return Discount > 0; }
        }

        public bool TieneGravamen
        {
            get { return Tax > 0; }
        }

        #endregion
                
        ObservableCollection<LineItemViewModel> lineitems;
        public ObservableCollection<LineItemViewModel> OrderItems 
        {
            get 
            {
                if (lineitems == null)
                {
                    //orderItemsFirstTime = false;

                    lineitems = new ObservableCollection<LineItemViewModel>();

                    foreach (SaleLineItem li in sale.LineItems)
                    {
                        LineItemViewModel livm = new LineItemViewModel(li, this, RefreshTotal, RefreshCost);
                        lineitems.Add(livm);
                        //AddOrderItemEventHandlers(li);
                    }

                    //lineitems.CollectionChanged += new NotifyCollectionChangedEventHandler(lineitems_CollectionChanged);
                        //new CollectionChangeEventHandler(OrderItems_AssociationChanged);
                }
                return lineitems; 
            }
        }

        public IEnumerable LineItemsTotalized
        {
            get
            {
                var query = from li in lineitems
                            group li by li.Product into groupingByProduct
                            select new
                            {
                                Quantity = groupingByProduct.Sum(x => x.Quantity),
                                UnitMeasure = appvm.UnitMeasureManager.Unit,
                                Product = groupingByProduct.Key,
                                Price = groupingByProduct.Sum(x => x.Price)
                            };

                return query.ToList();
            }
        }

        #region Production Items

        ICollectionView icvProductionItems;
        public ICollectionView IcvProductionItems
        {
            get
            {
                CollectionViewSource cvs = new CollectionViewSource();
                cvs.Source = OrderItems;
                icvProductionItems = cvs.View;

                icvProductionItems.Filter = IsProducedProduct;

                PropertyGroupDescription pgd = new PropertyGroupDescription("Product.ProductionArea.Name");
                icvProductionItems.GroupDescriptions.Add(pgd);

                SortDescription sd = new SortDescription("IsEntrant", ListSortDirection.Descending);
                icvProductionItems.SortDescriptions.Add(sd);

                return icvProductionItems;
            }
        }

        bool IsProducedProduct(object o)
        {
            LineItemViewModel lineitem = (LineItemViewModel)o;

            if (lineitem.Product == null) return false;

            bool? nullableProduced = lineitem.Product.IsProduced;
            bool isproduced = nullableProduced.HasValue ? nullableProduced.Value : false;

            return isproduced;
        }

        public ICollectionView IcvFinalToPrint
        {
            get
            {
                CollectionViewSource cvs = new CollectionViewSource();
                cvs.Source = OrderItems;
                ICollectionView icvFinalToPrint = cvs.View;

                icvFinalToPrint.Filter = IsFinalToPrintProduct;

                PropertyGroupDescription pgd = new PropertyGroupDescription("Product.ProductionArea.Name");
                icvFinalToPrint.GroupDescriptions.Add(pgd);

                SortDescription sd = new SortDescription("IsEntrant", ListSortDirection.Descending);
                icvFinalToPrint.SortDescriptions.Add(sd);

                return icvFinalToPrint;
            }
        }

        bool IsFinalToPrintProduct(object o)
        {
            LineItemViewModel lineitem = (LineItemViewModel)o;

            if (lineitem.Product == null) return false;

            bool? nullableProduced = lineitem.Product.IsProduced;
            bool isproduced = nullableProduced.HasValue ? nullableProduced.Value : false;

            return isproduced && !lineitem.Printed;
        }

        #endregion        
                
        void CheckProductIsAvailable(Product prod, double quantity)
        {
            Inventory pisoInventory = appvm.InventoryAreasOC.Single(x => x.IsFloor);

            //trying to sell quantity items
            List<InventoryItem> missingItems = appvm.InventoryService.CheckAvailability(pisoInventory, prod, quantity);

            //there are items missing
            if (missingItems.Count > 0)
            {
                var msgBox = base.GetService<IMessageBoxService>();

                msgBox.ShowMessage(BuildProductsUnavailableMessage(missingItems));
            }
        }

        string BuildProductsUnavailableMessage(List<InventoryItem> missingItems)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("No tiene suficiente ");
            foreach (var item in missingItems)
            {
                if (item.Product != null)
                {
                    sb.Append(string.Format("{0}({1}), ", item.Product.Name, item.Quantity));
                }
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append(".");

            return sb.ToString();
        }

        //sales work against floor items
        //protected void ExecuteSellOperation(Product prod, double quantity)
        //{
        //    if (prod == null) return;

        //    Inventory pisoInventory = appvm.InventoryAreasOC.Single(x => x.IsFloor);

        //    if (prod.IsStorable)
        //    {
        //        inventoryService.ExecuteInventoryOperation(WorkingDate, pisoInventory, prod, quantity, appvm.UnitMeasureManager.Unit);
        //    }
        //    else if (prod.IsRecipe)
        //    {
        //        double rate = quantity;// prod.RecipeQuantity;
        //        foreach (var tbp in prod.Ingredients)
        //        {
        //            inventoryService.ExecuteInventoryOperation(WorkingDate, pisoInventory, tbp.IngredientProduct, tbp.Quantity * quantity, tbp.UnitMeasure);
        //        }
        //    }
        //}

        public Sale SalesOrder { get { return sale; } }
    }
}