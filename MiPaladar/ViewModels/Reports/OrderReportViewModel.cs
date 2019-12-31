using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Extensions;

namespace MiPaladar.ViewModels
{
    public class OrdersReportItemViewModel
    {
        public OrdersReportItemViewModel() { }

        public OrdersReportItemViewModel(Sale order)
        {
            Date = order.Date;
            DateCreated = order.DateCreated;
            Employee = order.Employee;
            //Notes = order.Memo;
            Order = order;
            MondayDate = GetWeekMonday(order.Date);
            DayOfWeek = order.Date.DayOfWeek;

            //if (order is Sale)
            {
                TotalPrice = order.Total;

                Discount = order.DiscountToMoney();
                Tax = order.TaxToMoney();
                Tips = order.Tips;

                int? n = order.Number;
                Number = n.HasValue ? n.Value : 0;

                //Number = sale.Number;
                if (order.Table != null) PriceList = order.Table.PriceList;
                //Waiter = sale.Waiter;
                Table = order.Table;
                Clients = order.Persons;
                //Discount = CalculateDiscount(sale);
                //OrderType = OrderType.Sale;
                //LineItems = sale.LineItems;
            }
            //else if (order is Purchase)
            //{
            //    Purchase purchase = (Purchase)order;

            //    TotalPrice = purchase.Total;
            //    OrderType = OrderType.Purchase;
            //    //PurchaseType = purchase.PurchaseType;
            //    Title = purchase.Memo;
            //    //int? number = ((Purchase)order).Number;
            //    //Number = number.HasValue ? number.Value : 0;
            //}
        }

        //private decimal CalculateDiscount(Sale sale)
        //{
        //    if (!sale.DiscountInPercent) return sale.Discount;

        //    if (sale.Discount == 100) return sale.TotalPrice;

        //    float total = sale.TotalPrice * 100 / (100 - sale.Discount);
        //    return total - sale.TotalPrice;
        //    decimal total = 0;
        //    foreach (var item in sale.LineItems)
        //    {
        //        total += ((SaleLineItem)item).Amount;
        //    }

        //    return total * sale.Discount / 100;
        //}

        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }

        public DayOfWeek DayOfWeek { get; set; }        
        public DateTime MondayDate { get; set; }
        public string WeekString 
        {
            get { return MondayDate.ToString("d MMM") + " - " + MondayDate.AddDays(6).ToString("d MMM"); }
        }
        //public float DayOfWeekPercent { get; set; }
        public int Number { get; set; }
        public PriceList PriceList { get; set; }
        public Employee Employee { get; set; }
        public Table Table { get; set; }
        public Sale Order { get; set; }
        
        public int Clients { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DayAverage { get; set; }
        //public decimal TotalPurchase { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Tips { get; set; }
        //public IEnumerable<LineItem> LineItems { get; set; }

        public int Count { get; set; }
        //public OrderType OrderType { get; set; }
        //public string Title { get; set; }

        public decimal SalesByClient 
        {
            get 
            {
                if (Clients == 0) return 0;

                return TotalPrice / Clients;
            } 
        }

        //public string Notes { get; set; }

        public static DateTime GetWeekMonday(DateTime dt)
        {
            var days_from_monday = dt.DayOfWeek - DayOfWeek.Monday;
            
            //monday
            if (days_from_monday == -1) 
                days_from_monday = 6;

            return dt.Date.Date.AddDays(-days_from_monday);
        }
    }
}
