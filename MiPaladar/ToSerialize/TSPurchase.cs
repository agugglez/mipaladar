using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSPurchase
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Total { get; set; }
        public string Memo { get; set; }
        //public int Number { get; set; }
        //public bool Closed { get; set; }
        public int? ResponsibleId { get; set; }

        //public string Title { get; set; }        
        //public int? PurchaseTypeId { get; set; }        
        public int? InventoryId { get; set; }
    }
}
