using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;

namespace MiPaladar.Extensions
{
    public static class MyExtensions
    {
        public static decimal DiscountToMoney(this Sale sale)
        {
            decimal temp = sale.DiscountInPercent ? sale.SubTotal * sale.Discount / 100 : sale.Discount;

            return temp - temp % 0.05m;
        }

        public static decimal TaxToMoney(this Sale sale)
        {
            decimal temp = sale.TaxInPercent ? sale.SubTotal * sale.Tax / 100 : sale.Tax;

            decimal mod = temp % 0.05m;

            if (mod > 0) { temp += 0.05m - mod; }

            return temp;
        }
    }
}
