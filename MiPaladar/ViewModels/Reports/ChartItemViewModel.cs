using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Services;

using MiPaladar.MVVM;

namespace MiPaladar.ViewModels
{
    public class ChartItemViewModel : ViewModelBase
    {
        public ChartItemViewModel() { }

        //public SalesChartItemViewModel(Purchase order)
        //{
        //    Date = order.Date;
        //    TotalPurchase = order.Total;
        //}

        public ChartItemViewModel(Sale order)
        {
            Date = order.Date;
            DayOfWeek = order.Date.DayOfWeek;
            TotalSale = order.Total;

            var invSVC = base.GetService<IInventoryService>();

            decimal temp = 0;
            foreach (SaleLineItem li in order.LineItems)
            {
                temp += invSVC.GetProductCost(li.Product, li.Quantity, li.UnitMeasure);
            }
            TotalCost = temp;

            Discount = order.DiscountInPercent ? order.SubTotal * order.Discount / 100 : order.Discount;
            Clients = order.Persons;
            //IsSale = true;
        }

        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public string WeekString
        {
            get { return Date.ToString("d MMM") + " - " + Date.AddDays(6).ToString("d MMM"); }
        }

        public static DateTime GetWeekMonday(DateTime dt)
        {
            var days_from_monday = dt.DayOfWeek - DayOfWeek.Monday;

            //monday
            if (days_from_monday == -1)
                days_from_monday = 6;

            return dt.Date.Date.AddDays(-days_from_monday);
        }

        public string X { get; set; }
        public double Quantity { get; set; }
        //public decimal Y1 { get; set; }
        //public decimal Y2 { get; set; }

        public int Clients { get; set; }
        public decimal TotalSale { get; set; }        
        //public decimal TotalPurchase { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Profit { get; set; }
        public decimal Discount { get; set; }
        public int Count { get; set; }
        public int NumberOfDays { get; set; }

        //public bool IsSale { get; set; }//either sale or purchase

        public decimal SalesByClient
        {
            get
            {
                if (Clients == 0) return 0;

                return TotalSale / Clients;
            }
        }

        
    }
}
