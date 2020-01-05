using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Services;
using MiPaladar.MVVM;
using MiPaladar.Extensions;

namespace MiPaladar.ViewModels
{
    public class ReportLineViewModel : ViewModelBase
    {
        public ReportLineViewModel() { }

        #region LINEITEMS CONSTRUCTORS

        public ReportLineViewModel(Product prod, double qtty, UnitMeasure um, 
            decimal amount, DateTime date, Shift shift, Employee salesPerson)
        {
            Quantity = qtty;
            UnitMeasure = um;            

            Product = prod;
            ProductId = prod.Id;

            //AMOUNT
            Amount = amount;
            //COST
            var invSVC = base.GetService<IInventoryService>();
            Cost = invSVC.GetProductCost(prod, qtty, um, false);
            //Cost = BasicCost / (decimal)prod.EdiblePart;

            //get this from Order
            Date = date;
            //DateCreated = li.Order.DateCreated;
            //MondayDate = GetWeekMonday(date);
            DayOfWeek = date.DayOfWeek;

            //PRODUCT TYPE
            ProductType = prod.ProductType;
            //CATEGORY            
            if (prod.Category != null)
            {
                //CategoryName = invSVC.GetFullCategoryName(prod.Category);
                Category = prod.Category;
            }
            //SALESPERSON
            if (salesPerson != null)
            {
                SalesPersonName = salesPerson.Name;
                SalesPerson = salesPerson;
            }
            //SHIFT
            if (shift != null)
            {
                ShiftName = shift.Name;
                ShiftId = shift.Id;
            }

            //TAGS
            foreach (var tag in prod.Tags)
            {
                tagIds.Add(tag.Id);
            }
        }

        public ReportLineViewModel(SaleLineItem li)
            : this(li.Product, li.Quantity, li.UnitMeasure, li.Amount, li.Order.Date, li.Sale.Shift, li.Sale.Employee)
        {
            //get this from Order
            //Date = li.Order.Date.Date;
            //DateCreated = li.Order.DateCreated;
            //MondayDate = GetWeekMonday(li.Order.Date);
            //DayOfWeek = li.Order.Date.Date.DayOfWeek;

            //Sale = li.Sale;
            //OrderType = li.Order is Sale ? OrderType.Sale : OrderType.Purchase;
        }

        #endregion        

        #region SALE CONSTRUCTORS

        public ReportLineViewModel(Sale sale, bool loadCost)
        {
            LineId = sale.Id;
            Date = sale.Date;
            DateCreated = sale.DateCreated;
            DatePrinted = sale.DatePrinted;
            DateClosed = sale.DateClosed;

            DayOfWeek = Date.DayOfWeek;

            if (sale.Shift != null)
            {
                ShiftName = sale.Shift.Name;
                ShiftId = sale.Shift.Id;
            }
            if (sale.Employee != null)
            {
                SalesPersonName = sale.Employee.Name;
                SalesPerson = sale.Employee;
            }

            InvoiceNumber = sale.Number;

            Clients = sale.Persons;

            Discount = sale.DiscountToMoney();
            Tax = sale.TaxToMoney();
            Tips = sale.Tips;

            Amount = sale.Total;

            if (loadCost) LoadTotalCost(sale);
        }

        private void LoadTotalCost(Sale sale)
        {
            var invSVC = ServiceContainer.GetService<IInventoryService>();

            Cost = 0;

            foreach (var lineitem in sale.SaleLineItems)
            {
                Cost += invSVC.GetProductCost(lineitem.Product, lineitem.Quantity, lineitem.UnitMeasure, false);
            }
        }

        #endregion

        #region LINEITEMS FIELDS

        //this quantity considers edible part
        public double Quantity { get; set; }
        public UnitMeasure UnitMeasure { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public UMFamily UMFamily { get; set; }

        public ProductType ProductType { get; set; }

        public string CategoryName { get; set; }
        //public int CategoryId { get; set; }
        //public bool IsRootCategory { get; set; }
        public Category Category { get; set; }

        //TAGS
        List<int> tagIds = new List<int>();
        public List<int> TagIds
        {
            get { return tagIds; }
        }

        #endregion

        #region DATE FIELDS

        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DatePrinted { get; set; }
        public DateTime? DateClosed { get; set; }

        public int ServiceTime
        {
            get
            {
                if (InvoiceNumber == 6382)
                { Console.WriteLine(); }
                DateTime dateToUse;

                if (DatePrinted != null)
                {
                    dateToUse = DatePrinted.Value;
                }
                else if (DateClosed != null)
                {
                    dateToUse = DateClosed.Value;
                }
                else
                {
                    dateToUse = DateCreated;
                }

                return (int)(dateToUse - DateCreated).TotalMinutes;
                
                //if (PrintTicketDate > StartDate) dateToUse = PrintTicketDate.Value;
                //else if (FacturaDate > StartDate) dateToUse = FacturaDate.Value;
                //else return 0;

                //return (int)(dateToUse - StartDate).TotalMinutes;
            }
        }

        public DayOfWeek DayOfWeek { get; set; }
        public Dias DiaEnEspanol { get { return (Dias)DayOfWeek; } }

        public DateTime MondayDate { get { return GetWeekMonday(Date); } }

        public string WeekString
        {
            get { return MondayDate.ToString("d MMM") + " - " + MondayDate.AddDays(6).ToString("d MMM"); }
        }

        public string DateSpanString { get; set; }

        #endregion

        #region SALE FIELDS

        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Tips { get; set; }

        public int Clients { get; set; }

        public int? InvoiceNumber { get; set; }        

        public string SalesPersonName { get; set; }
        public Employee SalesPerson { get; set; }
        //public int SalesPersonId { get; set; }

        public string ShiftName { get; set; }
        public int ShiftId { get; set; }

        public decimal SalesByClient
        {
            get
            {
                if (Clients == 0) return 0;

                return Amount / Clients;
            }
        }

        #endregion        

        public decimal Amount { get; set; }

        public decimal Cost { get; set; }
        public decimal RealCost { get; set; }
        public decimal Profit { get { return Amount - Cost; } }

        public decimal CostToPriceRatio
        {
            get { return Amount != 0 ? Cost / Amount : 0; }
        }
        public decimal ProfitMargin
        {
            get { return Amount != 0 ? Profit / Amount : 0; }
        }

        //public Sale Sale { get; set; }

        public decimal ChildrenProductsTotalSale { get; set; }             

        public static DateTime GetWeekMonday(DateTime dt)
        {
            var days_from_monday = dt.DayOfWeek - DayOfWeek.Monday;

            //monday
            if (days_from_monday == -1)
                days_from_monday = 6;

            return dt.Date.AddDays(-days_from_monday);
        }
                
        //public decimal BasicCost { get; set; }
        public double RealQuantity { get; set; }
        public UnitMeasure RealUnitMeasure { get; set; }

        #region Projections Members

        ////Original DoW Quantities in base UM
        //double[] origQtties = new double[7];

        //public double[] OriginalQuantities
        //{
        //    get { return origQtties; }
        //}

        //DAY OF WEEK QUANTITIES
        //double[] projectedQtties = new double[7];

        //public double[] ProjQuantities
        //{
        //    get { return projectedQtties; }
        //}

        //UnitMeasure[] projectedUMs = new UnitMeasure[7];
        //public UnitMeasure[] ProjUnitOfMeasures
        //{
        //    get { return projectedUMs; }
        //}

        //public void ReportQuantitiesChanged()
        //{
        //    OnPropertyChanged("Quantities");
        //    OnPropertyChanged("UnitOfMeasures");
        //}

        public double MinimumQuantity { get; set; }
        public UnitMeasure MinimumUnitMeasure { get; set; }

        public double MaximumQuantity { get; set; }
        public UnitMeasure MaximumUnitMeasure { get; set; }

        public double AverageQuantity { get; set; }
        public UnitMeasure AverageUnitMeasure { get; set; }

        //double projQtty;
        //public double ProjectedQuantity
        //{
        //    get { return projQtty; }
        //    set
        //    {
        //        projQtty = value;
        //        OnPropertyChanged("ProjectedQuantity");
        //    }
        //}
        //public UnitMeasure ProjectedUnitMeasure { get; set; }

        double changePct;
        public double ChangePercent
        {
            get { return changePct; }
            set
            {
                changePct = value;
                OnPropertyChanged("ChangePercent");
            }
        }

        //minimum sold quantity by day of week
        //double[] minQtties = new double[7];

        //public double[] MinQuantities
        //{
        //    get { return minQtties; }
        //}

        //UnitMeasure[] minUMs = new UnitMeasure[7];
        //public UnitMeasure[] MinUnitOfMeasures
        //{
        //    get { return minUMs; }
        //}

        //maximun sold quantity by day of week
        //double[] maxQtties = new double[7];

        //public double[] MaxQuantities
        //{
        //    get { return maxQtties; }
        //}

        //UnitMeasure[] maxUMs = new UnitMeasure[7];
        //public UnitMeasure[] MaxUnitOfMeasures
        //{
        //    get { return maxUMs; }
        //}

        //aerage sold quantity by day of week
        //double[] aveQtties = new double[7];

        //public double[] AveQuantities
        //{
        //    get { return aveQtties; }
        //}

        //UnitMeasure[] aveUMs = new UnitMeasure[7];
        //public UnitMeasure[] AverageUnitOfMeasures
        //{
        //    get { return aveUMs; }
        //}

        //public double WeekdayQtty { get; set; }
        //public UnitMeasure WeekdayUM { get; set; }

        //public double WeekendQtty { get; set; }
        //public UnitMeasure WeekendUM { get; set; }

        #endregion

        //PRODUCT RANKINGS
        public double Preference { get; set; }
        public ProductClass ProductClass { get; set; }

        //saleId
        public int LineId { get; set; }
        
    }
    public enum ProductClass { A, B, C, D }
}