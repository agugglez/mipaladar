using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSInventoryTrace
    {        
        public double Quantity { get; set; }
        
        public int ProductId { get; set; }
        
        public DateTime Date { get; set; }
        public int InventoryAreaId { get; set; }
    }
}
