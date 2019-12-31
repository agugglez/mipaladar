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
using System.Windows;

namespace MiPaladar.ViewModels
{
    public class OfflineSaleViewModel : ViewModelBase, IScreen
    {
        MainWindowViewModel appvm;

        Action<Sale> onCreated;
        Action<Sale> onAssociationChanged;
        Action<Sale> onRemoved;

        bool creating;

        public OfflineSaleViewModel(MainWindowViewModel appvm, Action<Sale> onCreated,
            Action<Sale> onRemoved, Action<Sale> onAssociationChanged)
        {
            this.appvm = appvm;
            this.onCreated = onCreated;
            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = DateTime.Now;
            workingDate = DateTime.Today;

            creating = true;

            HasPendingChanges = true;
        }

        public OfflineSaleViewModel(MainWindowViewModel appvm, Sale sale,
            Action<Sale> onRemoved, Action<Sale> onAssociationChanged)
        {
            this.appvm = appvm;
            this.sale = sale;

            this.onRemoved = onRemoved;
            this.onAssociationChanged = onAssociationChanged;

            dateCreated = sale.DateCreated;

            CopyFromSale();

            HasPendingChanges = false;

            //subTotal = sale.LineItems.Sum(x => ((SaleLineItem)x).Amount);
        }

        Sale sale;
        public Sale WrappedSale
        {
            get { return sale; }
        }

        public MainWindowViewModel AppVM { get { return appvm; } }

        #region Copy Methods

        void CopyFromSale()
        {
            if (workingDate != sale.Date) WorkingDate = sale.Date;
            if (memo != sale.Memo) Memo = sale.Memo;
            if (waiter != sale.Employee) Waiter = sale.Employee;
            if (shift != sale.Shift) Shift = sale.Shift;

            //if (dateClosed != sale.DateClosed) DateClosed = sale.DateClosed;
            //if (datePrinted != sale.DatePrinted) DatePrinted = sale.DatePrinted;

            if (discount != sale.Discount) Discount = sale.Discount;
            if (discountInPercent != sale.DiscountInPercent) DiscountInPercent = sale.DiscountInPercent;
            if (tax != sale.Tax) Tax = sale.Tax;
            if (taxInPercent != sale.TaxInPercent) TaxInPercent = sale.TaxInPercent;

            //if (cash != sale.Cash) Cash = sale.Cash;
            if (persons != sale.Persons) Persons = sale.Persons;
            //if (paid != sale.Paid) Paid = sale.Paid;
            //if (number != sale.Number) Number = sale.Number;

            if (subTotal != sale.SubTotal) RawTotal = sale.SubTotal;
            if (totalPrice != sale.Total) TotalPrice = sale.Total;

            //if (closed != sale.Closed) Closed = sale.Closed;
            //if (tips != sale.Tips) Tips = sale.Tips;
            //if (table != sale.Table) Table = sale.Table;

            lineitems.Clear();

            foreach (SaleLineItem item in sale.LineItems)
            {
                OfflineLineItemViewModel ol = new OfflineLineItemViewModel(item, OnQuantityChanged, OnPriceChanged);
                lineitems.Add(ol);
            }
        }

        void CopyToSale()
        {
            if (workingDate != sale.Date) sale.Date = workingDate;
            if (memo != sale.Memo) sale.Memo = memo;
            if (waiter != sale.Employee) sale.Employee = waiter;
            if (shift != sale.Shift) sale.Shift = shift;

            //if (dateClosed != sale.DateClosed) sale.DateClosed = dateClosed;
            //if (datePrinted != sale.DatePrinted) sale.DatePrinted = datePrinted;

            if (discount != sale.Discount) sale.Discount = discount;
            if (discountInPercent != sale.DiscountInPercent) sale.DiscountInPercent = discountInPercent;
            if (tax != sale.Tax) sale.Tax = tax;
            if (taxInPercent != sale.TaxInPercent) sale.TaxInPercent = taxInPercent;

            //if (cash != sale.Cash) sale.Cash = Cash;
            if (persons != sale.Persons) sale.Persons = persons;
            //if (paid != sale.Paid) sale.Paid = paid;
            //if (number != sale.Number) sale.Number = number;

            if (subTotal != sale.SubTotal) sale.SubTotal = subTotal;
            if (totalPrice != sale.Total) sale.Total = totalPrice;

            //if (closed != sale.Closed) sale.Closed = closed;
            //if (tips != sale.Tips) sale.Tips = tips;
            //if (table != sale.Table) sale.Table = table;

            //when date changes we have to undo all original salelineitems
            SyncLineItems(workingDate != sale.Date);
        }

        /// <summary>
        /// needsUndo is for when date is modified, then we have to undo all original salelineitems
        /// </summary>
        /// <param name="needsUndo"></param>
        void SyncLineItems(bool needsUndo)
        {
            //check for removed/modified lineitems
            var ts = base.GetService<ITransactionService>();

            List<SaleLineItem> toRemove = new List<SaleLineItem>();

            foreach (SaleLineItem sli in sale.LineItems)
            {
                bool contains = lineitems.Any(x => x.WrappedLineItem == sli);
                //lineitem was removed
                if (!contains)
                {
                    toRemove.Add(sli);
                }
            }

            foreach (var item in toRemove)
            {
                ts.UndoSell(item.Product, item.Quantity, item.Cost, sale.Date);

                appvm.Context.LineItems.DeleteObject(item);
            }

            decimal total_cost = 0;

            //check for added lineitems
            foreach (var item in lineitems)
            {
                //it is new
                if (item.WrappedLineItem == null)
                {
                    SaleLineItem new_sli = new SaleLineItem();
                    new_sli.Quantity = item.Quantity;
                    new_sli.UnitMeasure = appvm.UnitMeasureManager.Unit;
                    new_sli.Product = item.Product;
                    new_sli.Amount = item.Price;

                    sale.LineItems.Add(new_sli);
                    item.WrappedLineItem = new_sli;

                    decimal cost = ts.Sell(item.Product, item.Quantity, workingDate);

                    new_sli.Cost = cost;

                    total_cost += cost;
                }
                //it is not new
                else
                {
                    SaleLineItem original = item.WrappedLineItem;

                    if (needsUndo)
                    {
                        ts.UndoSell(original.Product, original.Quantity, original.Cost, sale.Date);

                        decimal cost = ts.Sell(item.Product, item.Quantity, workingDate);

                        original.Cost = cost;

                        original.Quantity = item.Quantity;
                        original.Amount = item.Price;
                    }
                    else
                    {
                        //check if the lineitem was modified
                        if (item.Quantity != original.Quantity)
                        {
                            double diff = item.Quantity - original.Quantity;

                            if (diff > 0)
                            {
                                decimal cost = ts.Sell(item.Product, diff, workingDate);

                                original.Cost += cost;

                                total_cost += cost;
                            }
                            else
                            //diff < 0, cant be zero
                            {
                                diff = -diff;
                                //-diff
                                decimal diff_cost = original.Cost * (decimal)diff / (decimal)original.Quantity;

                                ts.UndoSell(original.Product, diff, diff_cost, sale.Date);

                                original.Cost -= diff_cost;

                                total_cost -= diff_cost;
                            }

                            original.Quantity = item.Quantity;
                            original.Amount = item.Price;
                        }
                    }
                }
            }

        }

        #endregion

        public ObservableCollection<Employee> Waiters
        {
            get { return appvm.CanSellEmployees; }
        }
        public ObservableCollection<Shift> Shifts
        {
            get { return appvm.ShiftsOC; }
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
            get { return appvm.ProductManager.VentaItems; }
        }

        bool hasPendingChanges;
        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
            set
            {
                hasPendingChanges = value;
                OnPropertyChanged("HasPendingChanges");
            }
        }

        #region Properties

        DateTime workingDate;
        public DateTime WorkingDate
        {
            get { return workingDate; }
            set
            {
                workingDate = value;
                OnPropertyChanged("WorkingDate");

                HasPendingChanges = true;
            }
        }

        DateTime dateCreated;
        public DateTime RealDateTime
        {
            get { return dateCreated; }
        }

        //DateTime? datePrinted;
        //public DateTime? DatePrinted
        //{
        //    get { return datePrinted; }
        //    set 
        //    {
        //        datePrinted = value;
        //        OnPropertyChanged("DatePrinted");
        //    }
        //}

        //DateTime? dateClosed;
        //public DateTime? DateClosed
        //{
        //    get { return dateClosed; }
        //    set
        //    {
        //        dateClosed = value;
        //        OnPropertyChanged("DateClosed");
        //    }
        //}

        decimal subTotal;
        public decimal RawTotal
        {
            get { return subTotal; }
            set
            {
                subTotal = value;
                OnPropertyChanged("RawTotal");
            }
        }

        decimal totalPrice;
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        string memo;
        public string Memo
        {
            get { return memo; }
            set
            {
                memo = value;
                OnPropertyChanged("Memo");
            }
        }

        //public bool HasNotes
        //{
        //    get { return !string.IsNullOrWhiteSpace(memo); }
        //}

        //bool closed;
        //public bool Closed
        //{
        //    get { return closed; }
        //    set
        //    {
        //        if (closed != value)
        //        {
        //            closed = value;

        //            OnPropertyChanged("Closed");
        //            OnPropertyChanged("OpenOrCloseContent");
        //        }
        //    }
        //}

        //bool paid;
        //public bool Paid
        //{
        //    get { return paid; }
        //    set
        //    {
        //        if (paid != value)
        //        {
        //            paid = value;

        //            OnPropertyChanged("Paid");
        //        }
        //    }
        //}

        int persons;
        public int Persons
        {
            get { return persons; }
            set
            {
                persons = value;
                OnPropertyChanged("Persons");

                HasPendingChanges = true;
            }
        }

        //decimal cash;
        //public decimal Cash
        //{
        //    get { return cash; }
        //    set
        //    {
        //        cash = value;
        //        OnPropertyChanged("Cash");
        //    }
        //}        

        //decimal tips;
        //public decimal Tips
        //{
        //    get { return tips; }
        //    set
        //    {
        //        tips = value;
        //        OnPropertyChanged("Tips");
        //    }
        //}

        decimal discount;
        public decimal Discount
        {
            get { return discount; }
            set
            {
                discount = value;

                OnPropertyChanged("Discount");
                OnPropertyChanged("TieneDescuento");
                OnPropertyChanged("DiscountToMoney");

                UpdateTotalPrice();
            }
        }

        bool discountInPercent;
        public bool DiscountInPercent
        {
            get { return discountInPercent; }
            set
            {
                discountInPercent = value;
                OnPropertyChanged("DiscountInPercent");

                UpdateTotalPrice();
            }
        }

        decimal tax;
        public decimal Tax
        {
            get { return tax; }
            set
            {
                tax = value;
                OnPropertyChanged("Tax");
                OnPropertyChanged("TieneGravamen");
                OnPropertyChanged("TaxToMoney");

                UpdateTotalPrice();
            }
        }

        bool taxInPercent;
        public bool TaxInPercent
        {
            get { return taxInPercent; }
            set
            {
                taxInPercent = value;
                OnPropertyChanged("TaxInPercent");

                UpdateTotalPrice();
            }
        }

        Employee waiter;
        public Employee Waiter
        {
            get { return waiter; }
            set
            {
                waiter = value;
                OnPropertyChanged("Waiter");

                HasPendingChanges = true;
            }
        }

        Shift shift;
        public Shift Shift
        {
            get { return shift; }
            set
            {
                shift = value;
                OnPropertyChanged("Shift");

                HasPendingChanges = true;
            }
        }

        //int? number;
        //public int? Number
        //{
        //    get { return number; }
        //    set
        //    {
        //        number = value;
        //        OnPropertyChanged("Number");
        //    }
        //}

        //Table table;
        //public Table Table
        //{
        //    get { return table; }
        //    set
        //    {
        //        table = value;
        //        OnPropertyChanged("Table");
        //    }
        //}

        #endregion

        #region Show Charge Dialog Command

        //RelayCommand showChargeDialogCommand;
        //public ICommand ShowChargeDialogCommand
        //{
        //    get
        //    {
        //        if (showChargeDialogCommand == null)
        //            showChargeDialogCommand = new RelayCommand(x => ShowChargeDialog());
        //        return showChargeDialogCommand;
        //    }
        //}

        //void ShowChargeDialog() 
        //{
        //    var windowManager = base.GetService<IWindowManager>();

        //    ChargeDialogViewModel chargeDialog = new ChargeDialogViewModel(subTotal, DiscountToMoney, TaxToMoney, totalPrice, cash, tips);

        //    if (windowManager.ShowDialog(chargeDialog, this) == true)
        //    {
        //        Cash = chargeDialog.Cash;
        //        Tips = chargeDialog.Tips;                

        //        Paid = true;
        //        Closed = true;
        //    }
        //}

        #endregion

        #region Print Command

        RelayCommand printCommand;
        public ICommand PrintCommand
        {
            get
            {
                if (printCommand == null)
                    printCommand = new RelayCommand(x => this.Print());
                return printCommand;
            }
        }

        void Print()
        {
            Printer.PrintVale(this);
            //if ()
            //{
            //    //save first date only
            //    if (datePrinted == null) DatePrinted = DateTime.Now;

            //    Close();
            //}
        }

        #endregion

        #region Print Document

        //RelayCommand printDocument;
        //public ICommand PrintDocumentCommand
        //{
        //    get
        //    {
        //        if (printDocument == null)
        //            printDocument = new RelayCommand(x => this.PrintDocument());
        //        return printDocument;
        //    }
        //}

        //void PrintDocument()
        //{
        //    Printer.PrintValeDocument(this);
        //    //if (Printer.PrintValeDocument(this))
        //    //{
        //    //    sale.DatePrinted = DateTime.Now;

        //    //    sale.Prints++;

        //    //    Close();

        //    //    appvm.SaveChanges();
        //    //}
        //}

        #endregion

        #region Send to Kitchen Command

        //RelayCommand sendToKitchenCommand;
        //public ICommand PrintProductionCommand
        //{
        //    get
        //    {
        //        if (sendToKitchenCommand == null)
        //            sendToKitchenCommand = new RelayCommand(x => this.SendToKitchen(),
        //                x => this.CanPrintProduction);
        //        return sendToKitchenCommand;
        //    }
        //}

        //bool CanPrintProduction { get { return !paid; } }

        //void SendToKitchen()
        //{
        //    if (Printer.PrintProductionVale(this, Number))
        //    {
        //        foreach (var item in lineitems)
        //        {
        //            item.Printed = true;
        //        }
        //    }
        //}

        #endregion

        #region Toggle Close command

        //public string OpenOrCloseContent
        //{
        //    get { return Closed ? "Abrir" : "Cerrar"; }
        //}

        //RelayCommand toggleCloseCommand;
        //public ICommand ToggleCloseCommand
        //{
        //    get
        //    {
        //        if (toggleCloseCommand == null)
        //            toggleCloseCommand = new RelayCommand(x => this.ToggleClose());
        //        return toggleCloseCommand;
        //    }
        //}

        //void ToggleClose()
        //{
        //    if (closed)
        //    {
        //        //Paid = false;
        //        Closed = false;
        //    }
        //    else
        //    {
        //        var msgBox = base.GetService<IMessageBoxService>();

        //        if (msgBox.ShowYesNoDialog("¿Está seguro que desea cerrar el vale?") == true)
        //            Close();                
        //    }
        //}

        //void Close()
        //{
        //    Closed = true;

        //    if (dateClosed == null)
        //    {
        //        DateClosed = DateTime.Now;
        //    }
        //}

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

        bool CanRemove { get { return !creating; } }

        void Remove()
        {
            var msgBox = base.GetService<IMessageBoxService>();

            string message = "¿Está seguro que desea eliminar esta venta?";

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

        public OfflineLineItemViewModel SelectedLineItem { get; set; }

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
                return SelectedLineItem != null /*&& !Closed &&
                    (!SelectedLineItem.Printed || appvm.PersonnelManager.UserCanRemoveCalledLineItem)*/;
            }
        }

        public void DoRemoveLineItemCommand(OfflineLineItemViewModel livm)
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
            lineitems.Remove(livm);

            ReCalculate();

            HasPendingChanges = true;
        }

        #endregion

        #region New LineItem Command

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
            //SaleLineItem newLineItem = new SaleLineItem();
            //newLineItem.Quantity = qtty_to_add;
            //newLineItem.Product = product_to_add;
            //newLineItem.UnitMeasure = appvm.UnitMeasureManager.Unit;
            UnitMeasure um = appvm.UnitMeasureManager.Unit;
            decimal amount = (decimal)qtty_to_add * product_to_add.SalePrice;

            //Action onQuantityUpdated = () => RefreshTotal();

            OfflineLineItemViewModel newLineItemViewModel =
                new OfflineLineItemViewModel(product_to_add, quantityToAdd, um, amount, OnQuantityChanged, OnPriceChanged);

            lineitems.Add(newLineItemViewModel);

            //sale.LineItems.Add(newLineItem);

            CheckProductIsAvailable(product_to_add, qtty_to_add);

            //var ts = base.GetService<ITransactionService>();
            //decimal cost = ts.Sell(product_to_add, qtty_to_add, WorkingDate);

            //newLineItem.Cost = cost;

            //sale.TotalCost += cost;
            //ExecuteSellOperation(product_to_add, -qtty_to_add);

            //appvm.SaveChanges();

            ReCalculate();

            HasPendingChanges = true;

            //clear input fields          
            //UnitMeasureToAdd = null;
            QuantityToAdd = 1;
            ProductToAdd = null;
            //SearchText = string.Empty;

            OnLineItemAdded();
        }

        public void OnQuantityChanged()
        {
            HasPendingChanges = true;
        }

        public void OnPriceChanged()
        {
            ReCalculate();

            HasPendingChanges = true;
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

        #region Is Product Available Message

        void CheckProductIsAvailable(Product prod, double quantity)
        {
            if (appvm.InventoryAreasOC.Count > 0)
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

        #endregion

        #endregion

        private void ReCalculate()
        {
            RawTotal = lineitems.Sum(x => x.Price);

            UpdateTotalPrice();
        }

        #region ApplyDiscount Command

        RelayCommand applyDiscountCommand;
        public ICommand ApplyDiscountCommand
        {
            get
            {
                if (applyDiscountCommand == null)
                    applyDiscountCommand = new RelayCommand(x => ApplyDiscountExecute());
                return applyDiscountCommand;
            }
        }

        void ApplyDiscountExecute()
        {
            var windowManager = base.GetService<IWindowManager>();

            DiscountOrTaxDialogViewModel dialog =
                new DiscountOrTaxDialogViewModel("Descuento", "Ingrese la cantidad que quiere descontar:", discount, discountInPercent);

            if (windowManager.ShowDialog(dialog, this) == true)
            {
                if (discount != dialog.Quantity) Discount = dialog.Quantity;
                if (discountInPercent != dialog.InPercent) DiscountInPercent = dialog.InPercent;
            }
        }

        #endregion

        #region ApplyTax Command

        RelayCommand applyTaxCommand;
        public ICommand ApplyTaxCommand
        {
            get
            {
                if (applyTaxCommand == null)
                    applyTaxCommand = new RelayCommand(x => ApplyTaxExecute());
                return applyTaxCommand;
            }
        }

        void ApplyTaxExecute()
        {
            var windowManager = base.GetService<IWindowManager>();

            DiscountOrTaxDialogViewModel dialog =
                new DiscountOrTaxDialogViewModel("Recargo", "Ingrese la cantidad que quiere recargar:", tax, taxInPercent);

            if (windowManager.ShowDialog(dialog, this) == true)
            {
                if (tax != dialog.Quantity) Tax = dialog.Quantity;
                if (taxInPercent != dialog.InPercent) TaxInPercent = dialog.InPercent;
            }
        }

        #endregion

        //void UpdateSubTotal() 
        //{
        //    decimal temp = 0;

        //    foreach (OfflineLineItemViewModel vi in lineitems)
        //    {
        //        temp += vi.Price;
        //    }

        //    RawTotal = temp;
        //}

        //void UpdateDiscountToMoney()
        //{
        //    decimal temp = discountInPercent ? subTotal * discount / 100 : discount;

        //    DiscountToMoney = temp - temp % 0.05m;
        //}

        //void UpdateTaxToMoney()
        //{
        //    decimal temp = taxInPercent ? subTotal * tax / 100 : tax;

        //    decimal mod = temp % 0.05m;

        //    if (mod > 0) { temp += 0.05m - mod; }

        //    TaxToMoney = temp;
        //}

        void UpdateTotalPrice()
        {
            TotalPrice = subTotal + TaxToMoney - DiscountToMoney;

            HasPendingChanges = true;
        }

        //public void RefreshTotal()
        //{
        //    decimal total = 0;
        //    //refresh total price
        //    foreach (OfflineLineItemViewModel vi in lineitems)
        //    {
        //        total += vi.Price;
        //    }

        //    RawTotal = total;

        //    if (!DiscountInPercent)
        //        discount = Discount;
        //    else discount = total * Discount / 100;

        //    //round to cents
        //    discount = discount - discount % 0.05m;

        //    decimal tax = 0;

        //    if (!TaxInPercent)
        //        tax = Tax;
        //    else tax = total * Tax / 100;

        //    decimal mod = tax % 0.05m;
        //    //round to cents
        //    if (mod > 0) { tax += 0.05m - mod; }

        //    TotalPrice = total + tax - discount;
        //}        

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

        #region Print Helpers

        public string WaiterPrefix
        {
            get
            {
                if (waiter == null) return string.Empty;

                return waiter.Name.Length > 3 ? Waiter.Name.Substring(0, 3) : waiter.Name;
            }
        }

        //public string ShortID
        //{
        //    get 
        //    {
        //        string saleAreaPart = ""; string numberPart = "";

        //        if (table != null) 
        //        {
        //            string saleAreaName = table.PriceList.Name;
        //            saleAreaPart = saleAreaName.Length > 3 ? saleAreaName.Substring(0, 3) : saleAreaName;
        //        }

        //        if (number != null) 
        //        {
        //            numberPart = Number.ToString().PadLeft(3, '0');
        //        }

        //        return saleAreaPart + numberPart;
        //    }
        //}

        //public string MonthSpanish
        //{
        //    get { return ((Meses)(sale.Date.Month - 1)).ToString(); }
        //}        

        public bool TieneDescuento
        {
            get { return Discount != 0; }
        }

        public bool TieneGravamen
        {
            get { return Tax != 0; }
        }

        public decimal DiscountToMoney
        {
            get
            {
                decimal temp = discountInPercent ? subTotal * discount / 100 : discount;

                return temp - temp % 0.05m;
            }
        }

        public decimal TaxToMoney
        {
            get
            {
                decimal temp = taxInPercent ? subTotal * tax / 100 : tax;

                decimal mod = temp % 0.05m;

                if (mod > 0) { temp += 0.05m - mod; }

                return temp;
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

        #endregion

        ObservableCollection<OfflineLineItemViewModel> lineitems =
            new ObservableCollection<OfflineLineItemViewModel>();
        public ObservableCollection<OfflineLineItemViewModel> OrderItems
        {
            get
            {
                //if (lineitems == null)
                //{
                //    //orderItemsFirstTime = false;

                //    lineitems = new ObservableCollection<OfflineLineItemViewModel>();

                //    foreach (SaleLineItem li in sale.LineItems)
                //    {
                //        OfflineLineItemViewModel livm = new OfflineLineItemViewModel(li, OnQuantityChanged, ReCalculate);
                //        lineitems.Add(livm);
                //        //AddOrderItemEventHandlers(li);
                //    }

                //    //lineitems.CollectionChanged += new NotifyCollectionChangedEventHandler(lineitems_CollectionChanged);
                //        //new CollectionChangeEventHandler(OrderItems_AssociationChanged);
                //}
                return lineitems;
            }
        }

        //#region Production Items

        //ICollectionView icvProductionItems;
        //public ICollectionView IcvProductionItems
        //{
        //    get
        //    {
        //        CollectionViewSource cvs = new CollectionViewSource();
        //        cvs.Source = OrderItems;
        //        icvProductionItems = cvs.View;

        //        icvProductionItems.Filter = IsProducedProduct;

        //        PropertyGroupDescription pgd = new PropertyGroupDescription("Product.ProductionArea.Name");
        //        icvProductionItems.GroupDescriptions.Add(pgd);

        //        SortDescription sd = new SortDescription("IsEntrant", ListSortDirection.Descending);
        //        icvProductionItems.SortDescriptions.Add(sd);

        //        return icvProductionItems;
        //    }
        //}

        //bool IsProducedProduct(object o)
        //{
        //    LineItemViewModel lineitem = (LineItemViewModel)o;

        //    if (lineitem.Product == null) return false;

        //    bool? nullableProduced = lineitem.Product.IsProduced;
        //    bool isproduced = nullableProduced.HasValue ? nullableProduced.Value : false;

        //    return isproduced;
        //}

        //public ICollectionView IcvFinalToPrint
        //{
        //    get
        //    {
        //        CollectionViewSource cvs = new CollectionViewSource();
        //        cvs.Source = OrderItems;
        //        ICollectionView icvFinalToPrint = cvs.View;

        //        icvFinalToPrint.Filter = IsFinalToPrintProduct;

        //        PropertyGroupDescription pgd = new PropertyGroupDescription("Product.ProductionArea.Name");
        //        icvFinalToPrint.GroupDescriptions.Add(pgd);

        //        SortDescription sd = new SortDescription("IsEntrant", ListSortDirection.Descending);
        //        icvFinalToPrint.SortDescriptions.Add(sd);

        //        return icvFinalToPrint;
        //    }
        //}

        //bool IsFinalToPrintProduct(object o)
        //{
        //    LineItemViewModel lineitem = (LineItemViewModel)o;

        //    if (lineitem.Product == null) return false;

        //    bool? nullableProduced = lineitem.Product.IsProduced;
        //    bool isproduced = nullableProduced.HasValue ? nullableProduced.Value : false;

        //    return isproduced && !lineitem.Printed;
        //}

        //#endregion

        #region Cancel Command

        RelayCommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(x => this.Cancel());
                return cancelCommand;
            }
        }

        void Cancel()
        {
            if (creating)
            {
                //close this window
                var windowManager = base.GetService<IWindowManager>();

                windowManager.Close(this);
            }
            //saving changes
            else
            {
                CopyFromSale();
                HasPendingChanges = false;
            }
        }

        #endregion

        #region Save Command

        RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(x => this.Save());
                return saveCommand;
            }
        }

        void Save()
        {
            if (creating)
            {
                sale = new Sale();
                sale.Date = workingDate;
                sale.DateCreated = dateCreated;
                appvm.Context.Orders.AddObject(sale);
            }
            else if (onAssociationChanged != null) onAssociationChanged(sale);

            CopyToSale();

            appvm.SaveChanges();

            HasPendingChanges = false;

            if (creating)
            {
                onCreated(sale);
                creating = false;
            }
        }

        #endregion

        bool IScreen.TryToClose()
        {
            if (hasPendingChanges)
            {
                var msgBox = base.GetService<IMessageBoxService>();
                if (msgBox != null)
                {
                    var result = msgBox.Show("¿Desea guardar los cambios antes de salir?",
                        "Guardar cambios",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                        return false;

                    if (result == MessageBoxResult.Yes)
                        this.Save();
                }
            }
            return true;
        }
    }
}