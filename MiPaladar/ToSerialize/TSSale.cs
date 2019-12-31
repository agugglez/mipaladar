using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSSale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }        
        public DateTime DateCreated { get; set; }
        public string Memo { get; set; }
        public int? WaiterId { get; set; }

        public DateTime? DateClosed { get; set; }
        public DateTime? DatePrinted { get; set; }
        public decimal Discount { get; set; }
        public bool DiscountInPercent { get; set; }
        public decimal Cash { get; set; }
        public int Prints { get; set; }
        public int Persons { get; set; }
        public bool Paid { get; set; }
        public int? Number { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public bool Closed { get; set; }
        public decimal Tax { get; set; }
        public bool TaxInPercent { get; set; }
        public decimal Tips { get; set; }
        public decimal TotalCost { get; set; }
        public int? TableId { get; set; }
        
        //public int? AreaId { get; set; }
    }
}
