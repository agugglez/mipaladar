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
    
    public partial class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
            this.ChildrenCategories = new HashSet<Category>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentCategory_Id { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> ChildrenCategories { get; set; }
        public virtual Category ParentCategory { get; set; }
    }
}