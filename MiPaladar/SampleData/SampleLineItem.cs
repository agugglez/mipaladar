
using MiPaladar.Entities;

namespace MiPaladar.SampleData
{
    public class SampleLineItem
    {
        public double Quantity { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public string QuantityExpression { get { return Quantity + UnitMeasure.Caption; } }
        public Product Product { get; set; }
        public decimal Price { get; set; }
        public bool Printed { get; set; }
        public bool IsEntrant { get; set; }
        public bool ToSpecify { get; set; }
    }
}
