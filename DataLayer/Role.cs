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
    
    public partial class Role
    {
        public Role()
        {
            this.Employees = new HashSet<Employee>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanLogin { get; set; }
        public bool CanSeeSales { get; set; }
        public bool CanRemoveSales { get; set; }
        public bool CanSeeInventory { get; set; }
        public bool CanCreateProducts { get; set; }
        public bool CanEditProducts { get; set; }
        public bool CanRemoveProducts { get; set; }
        public bool CanSeeEmployees { get; set; }
        public bool CanCreateEmployees { get; set; }
        public bool CanEditEmployees { get; set; }
        public bool CanRemoveEmployees { get; set; }
        public bool CanExportImport { get; set; }
        public bool CanSeeReports { get; set; }
        public bool CanSeeRoles { get; set; }
        public bool CanSeeDashboard { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
    }
}