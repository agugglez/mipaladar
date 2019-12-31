using System;

namespace MiPaladar.ToSerialize
{
    public class TSTransfer
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public string Memo { get; set; }
        public int InventoryAreaFromId { get; set; }
        public int InventoryAreaToId { get; set; }
        public int ResponsibleId { get; set; }
    }
}
