//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiPaladar.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.LineItems = new HashSet<LineItem>();
        }
    
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Memo { get; set; }
        public Nullable<int> Employee_Id { get; set; }
    
        public virtual ICollection<LineItem> LineItems { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
