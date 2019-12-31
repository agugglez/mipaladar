using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSProductionItem
    {
        public double Quantity { get; set; }
        public int UnitMeasureId { get; set; }
        public int ProductId { get; set; }
        public int ProductionId { get; set; }

        public decimal Cost { get; set; }
    }
}
