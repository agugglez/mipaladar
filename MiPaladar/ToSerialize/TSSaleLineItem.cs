using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSSaleLineItem
    {
        public int OrderId { get; set; }
        
        public double Quantity { get; set; }

        public int UnitMeasureId { get; set; }
        
        public int ProductId { get; set; }
        
        public decimal Amount { get; set; }

        public bool Printed { get; set; }

        public decimal Cost { get; set; }
    }
}
