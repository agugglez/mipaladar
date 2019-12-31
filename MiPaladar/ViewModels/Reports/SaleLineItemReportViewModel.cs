using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.Enums;
using MiPaladar.Services;

namespace MiPaladar.ViewModels
{
    public class SaleLineItemReportViewModel : ViewModelBase
    {
        public SaleLineItemReportViewModel() { }

        public SaleLineItemReportViewModel(SaleLineItem li)
        {
            Quantity = li.Quantity;
            UnitMeasure = li.UnitMeasure;
            Product = li.Product;

            //if (li is SaleLineItem)
            {
                Price = (li).Amount;
            }

            //if (li is SaleLineItem) 
            {
                Cost = ProductManager.GetCurrentCost(li.Product, li.Quantity, li.UnitMeasure); 
                //Cost = li.Cost;
            }
            
            Profit = Price - Cost;

            //get this from Order
            Date = li.Order.Date.Date;
            DateCreated = li.Order.DateCreated;
            MondayDate = GetWeekMonday(li.Order.Date);
            DayOfWeek = li.Order.Date.Date.DayOfWeek;

            Employee = li.Order.Employee;
            Shift = ((Sale)li.Order).Shift;
                        
            Order = (Sale)li.Order;
            //OrderType = li.Order is Sale ? OrderType.Sale : OrderType.Purchase;
        }

        public DateTime Date { get; set; }

        public DateTime DateCreated { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime MondayDate { get; set; }

        public string WeekString
        {
            get { return MondayDate.ToString("d MMMM") + " - " + MondayDate.AddDays(6).ToString("d MMMM"); }
        }

        public Employee Employee { get; set; }
        
        public Shift Shift{ get; set; }

        public Product Product { get; set; }

        public UnitMeasure UnitMeasure { get; set; }

        public double Quantity { get; set; }
        public decimal Price { get; set; }

        public double DayAverage { get; set; }

        //public double QuantitySold { get; set; }
        //public decimal SalePrice { get; set; }

        //public double QuantityPurchased { get; set; }
        //public decimal PurchasePrice { get; set; }

        public decimal Cost { get; set; }
        public decimal Profit { get; set; }

        public decimal CostToPriceRatio 
        {
            get { if (Price != 0)return Cost / Price; return 0; }
        }
        public decimal ProfitToPriceRatio 
        {
            get { if (Price != 0)return Profit / Price; return 0; }
        }

        public Sale Order { get; set; }
        
        //public OrderType OrderType { get; set; }

        //double ingredientQuantity;
        //public double IngredientQuantity 
        //{
        //    get { return ingredientQuantity; }
        //    set 
        //    {
        //        ingredientQuantity = value;
        //        OnPropertyChanged("IngredientQuantity");
        //    }
        //}

        //UnitMeasure ingredientUM;
        //public UnitMeasure IngredientUM 
        //{
        //    get { return ingredientUM; }
        //    set
        //    {
        //        ingredientUM = value;
        //        OnPropertyChanged("IngredientUM");
        //    }
        //}

        Category category;
        public Category Category
        {
            get
            {
                if (category == null && Product != null)
                {
                    ProductIndex pi = Product.RelatedCategories.FirstOrDefault();
                    if (pi != null)
                    {
                        category = pi.Category;
                        foreach (var item in Product.RelatedCategories)
                        {
                            if (item.IsMain)
                            {
                                category = item.Category;
                                break;
                            }
                        }
                    }                    
                }
                
                return category;
            }
            set 
            {
                category = value;
            }
        }

        public static DateTime GetWeekMonday(DateTime dt)
        {
            var days_from_monday = dt.DayOfWeek - DayOfWeek.Monday;

            //monday
            if (days_from_monday == -1)
                days_from_monday = 6;

            return dt.Date.AddDays(-days_from_monday);
        }
    }
}