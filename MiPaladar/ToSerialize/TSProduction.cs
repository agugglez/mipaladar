using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiPaladar.ToSerialize
{
    public class TSProduction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public string Memo { get; set; }
        public int InventoryAreaId { get; set; }
        public int ResponsibleId { get; set; }
    }
}
