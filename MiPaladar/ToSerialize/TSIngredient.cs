using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSIngredient
    {
        public double Quantity { get; set; }
        public int BaseProductId { get; set; }
        public int IngredientProductId { get; set; }
        public int UnitMeasureId { get; set; }
    }
}
