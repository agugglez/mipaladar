﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RestaurantDBEntities : DbContext
    {
        public RestaurantDBEntities()
            : base("name=RestaurantDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Product> Products { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UMFamily> UMFamilies { get; set; }
        public DbSet<UnitMeasure> UnitMeasures { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Misc> Miscs { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
