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
    
    public partial class UnitMeasure
    {
        public UnitMeasure()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int Id { get; set; }
        public string Caption { get; set; }
        public int UMFamilyId { get; set; }
        public bool IsFamilyBase { get; set; }
        public double ToBaseConversion { get; set; }
        public string Name { get; set; }
    
        public virtual UMFamily UMFamily { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
