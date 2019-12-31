using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;

using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Collections;

using MiPaladar.ViewModels;
using MiPaladar.Classes;

namespace MiPaladar.SampleData
{
    public class SampleSale
    {
        public bool ShowingNotes { get; set; }
        
        public bool Charging { get; set; }

        public double QuantityToAdd { get; set; }
        
        public Product ProductToAdd { get; set; }        

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                //refresh total price
                foreach (SampleLineItem vi in OrderItems)
                {
                    total += vi.Price;
                }

                return RawTotal - AjusteToMoney + TaxToMoney;
            }
        }

        public decimal RawTotal { get { return OrderItems.Sum(x => x.Price); } }
        
        public string SearchText{ get; set; }

        #region Properties

        public string Notes { get; set; }

        public bool HasNotes { get; set; }

        public bool Closed { get; set; }

        public bool Opened { get; set; }

        public string OpenOrCloseContent
        {
            get { return Closed ? "Abrir" : "Cerrar"; }
        }

        public bool Paid { get; set; }

        public int Persons { get; set; }

        public decimal Cash { get; set; }

        public decimal Discount { get; set; }

        public bool DiscountInPercent { get; set; }

        public bool DiscountInMoney { get; set; }

        public decimal Tax { get; set; }

        public bool TaxInPercent { get; set; }

        public bool TaxInMoney { get; set; }

        public Employee Waiter { get; set; }

        public int Number { get; set; }

        public Table Table { get; set; }

        public DateTime WorkingDate { get; set; }

        public DateTime RealDateTime { get; set; }

        public double Change { get; set; }

        public int Prints { get; set; }

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
        //    get { return ((Meses)(WorkingDate.Month - 1)).ToString(); }
        //}

        public decimal AjusteToMoney
        {
            get
            {
                decimal temp = DiscountInPercent ? RawTotal * Discount / 100 : Discount;

                return temp - temp % 0.05m;
            }
        }

        public decimal TaxToMoney
        {
            get
            {
                decimal temp = TaxInPercent ? RawTotal * Tax / 100 : Tax;

                decimal mod = temp % 0.05m;

                if (mod > 0) { temp += 0.05m - mod; }

                return temp;
            }
        }

        public bool TieneDescuento
        {
            get { return Discount > 0; }
        }

        public bool TieneGravamen
        {
            get { return Tax > 0; }
        }

        #endregion

        ObservableCollection<SampleLineItem> orderItems = new ObservableCollection<SampleLineItem>();
        public ObservableCollection<SampleLineItem> OrderItems { get { return orderItems; } }

        public IEnumerable LineItemsTotalized
        {
            get
            {
                var query = from li in orderItems
                            group li by li.Product into groupingByProduct
                            select new
                            {
                                Quantity = groupingByProduct.Sum(x => x.Quantity),
                                Product = groupingByProduct.Key,
                                Price = groupingByProduct.Sum(x => x.Price)
                            };

                return query.ToList();
            }
        }

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
        //    SampleLineItem lineitem = (SampleLineItem)o;

        //    if (lineitem.Product == null) return false;

        //    bool? nullableProduced = lineitem.Product.IsProduced;
        //    bool isproduced = nullableProduced.HasValue ? nullableProduced.Value : false;

        //    return lineitem.Product != null && isproduced && !lineitem.Printed;
        //}        
    }
}