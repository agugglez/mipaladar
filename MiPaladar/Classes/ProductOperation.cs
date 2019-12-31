using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

namespace MiPaladar.Classes
{
    public class ProductOperation
    {
        public string Type
        {
            get 
            {
                if (Vale is Sale) return "Venta";
                else if (Vale is Purchase) return "Compra";

                throw new Exception("product orperation type");
            }
        }

        public double Total
        {
            get;
            set;
        }

        public Product Product
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public string ShortDate 
        {
            get { return Date.ToShortDateString(); }
        }

        public Order Vale { get; set; }

        public Employee Waiter { get; set; }
    }    
}
