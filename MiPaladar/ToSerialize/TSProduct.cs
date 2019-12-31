namespace MiPaladar.ToSerialize
{
    public class TSProduct
    {
        public int Id { get; set; }
        //public int Code { get; set; }
        public string Name { get; set; }
        //public bool IsIngredient { get; set; }
        public bool IsRecipe { get; set; }
        public bool IsStorable { get; set; }
        public bool NotInMenu { get; set; }
        public decimal SalePrice { get; set; }        
        //public bool IsPurchasable { get; set; }
        public int? ProductionAreaId { get; set; }
        public bool IsProduced { get; set; }
        public bool IsEntrant { get; set; }
        public int UMFamilyId { get; set; }
        public string Description { get; set; }
        public string PrintString { get; set; }
        public string ImageFileName { get; set; }
        //public decimal PurchasePrice { get; set; }
        public decimal CostPrice { get; set; }
        public double CostQuantity { get; set; }
        public int CostUMId { get; set; }

        public double MinimumStock { get; set; }
        public int MinimumStockUMId { get; set; }
    }
}
