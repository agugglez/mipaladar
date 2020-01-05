using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

namespace MiPaladar.Classes
{
    public class TotalByWaiterByProduct
    {
        Product product;
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
            }
        }

        Employee waiter;
        public Employee Waiter 
        {
            get { return waiter; }
            set { waiter = value; }
        }

        double total;
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
    }
}
