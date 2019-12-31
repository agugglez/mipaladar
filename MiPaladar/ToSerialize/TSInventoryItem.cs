using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSInventoryItem
    {
        public double Quantity { get; set; }
        public int ProductId { get; set; }
        public double MinimumQuantity { get; set; }
        public int InventoryAreaId { get; set; }
        public decimal Cost { get; set; }
    }
}
