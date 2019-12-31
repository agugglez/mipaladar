
namespace MiPaladar.ToSerialize
{
    public class TSRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool CanLogin { get; set; }
        public bool CanSeeSales { get; set; }
        public bool CanRemoveSales { get; set; }
        public bool CanSeeOldSales { get; set; }
        public bool CanSeePurchases { get; set; }
        public bool CanRemovePurchases { get; set; }
        public bool CanSeeOldPurchases { get; set; }
        public bool CanSeeInventory { get; set; }
        public bool CanCreateProducts { get; set; }
        public bool CanEditProducts { get; set; }
        public bool CanRemoveProducts { get; set; }
        public bool CanSeeEmployees { get; set; }
        public bool CanCreateEmployees { get; set; }
        public bool CanEditEmployees { get; set; }
        public bool CanRemoveEmployees { get; set; }
        public bool CanSeeMiPaladar { get; set; }
        public bool CanExportImport { get; set; }
        public bool CanSeeReports { get; set; }     
        public bool CanSeeSalesReport { get; set; }
        public bool CanSeeSalesByItemReport { get; set; }
    }
}
