using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.ToSerialize;

using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Data.Objects;

namespace MiPaladar.Services
{
    public interface IXmlSerializationSvc
    {
        void Serialize(string newFolder, BackgroundWorker worker);
        void Deserialize(string folder, BackgroundWorker worker);
        bool CheckFilesForImport(string folder);
    }
    public class XmlSerializerSvc : IXmlSerializationSvc
    {
        RestaurantDBEntities context;

        int operations_total;
        int operations_completed;

        const string productsFile = "\\products.xml";
        const string ingredientsFile = "\\ingredients.xml";
        const string categoriesFile = "\\categories.xml";
        const string productionAreasFile = "\\productionareas.xml";
        const string productindexesFile = "\\productindexes.xml";
        const string umFamiliesFile = "\\umfamilies.xml";
        const string unitMeasuresFile = "\\unitMeasures.xml";
        const string employeesFile = "\\employees.xml";
        const string rolesFile = "\\roles.xml";
        
        const string saleareasFile = "\\saleareas.xml";
        const string tablesFile = "\\tables.xml";
        const string inventoryAreasFile = "\\inventoryareas.xml";
        const string inventoryFile = "\\inventory.xml";
        const string inventoryhistoryFile = "\\inventoryhistory.xml";

        const string salesFile = "\\sales.xml";
        const string purchasesFile = "\\purchases.xml";

        const string adjustmentsFile = "\\adjustments.xml";
        const string transfersFile = "\\transfers.xml";
        const string productionsFile = "\\productions.xml";

        const string salelineitemsFile = "\\salelineitems.xml";
        const string purchaselineitemsFile = "\\purchaselineitems.xml";
        
        const string adjustmentItemsFile = "\\adjustmentitems.xml";
        const string transferItemsFile = "\\transferitems.xml";
        const string productionItemsFile = "\\productionitems.xml";

        const string companyInfoFile = "\\companyInfo.xml";    

        #region Serialization

        public void Serialize(string newFolder, BackgroundWorker worker)
        {
            context = new RestaurantDBEntities();

            operations_completed = 0;

            int productCount = context.Products.Count();
            int ingredientCount = context.Ingredients.Count();
            int categoryCount = context.Categories.Count();
            int productIndexCount = context.ProductIndexes.Count();
            int umFamiliesCount = context.UMFamilies.Count();
            int unitMeasureCount = context.UnitMeasures.Count();
            int roleCount = context.Roles.Count();
            int employeeCount = context.Employees.Count();

            int productionAreaCount = context.ProductionAreas.Count();
            int saleAreaCount = context.PriceLists.Count();
            int tableCount = context.Tables.Count();
            int inventoryAreasCount = context.Inventories.Count();
            //int costTracesCount = context.CostTraces.Count();
            int inventoryCount = context.InventoryItems.Count();
            int inventoryHistoryCount = context.InventoryTraces.Count();

            int orderCount = context.Orders.Count();

            int lineitemCount = context.LineItems.Count();

            operations_total = productCount + ingredientCount + categoryCount + productIndexCount + umFamiliesCount + unitMeasureCount +
                roleCount + employeeCount + productionAreaCount + saleAreaCount + tableCount + 
                inventoryAreasCount + inventoryCount + inventoryHistoryCount +
                orderCount + lineitemCount;
                //salesCount + purchasesCount 
                //+ adjustmentsCount + transfersCount + productionsCount +
                //adjustmentItemsCount + transferItemsCount + productionItemsCount

            operations_completed = 0;

            //products
            SerializeProducts(newFolder);
            operations_completed += productCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //ingredients
            SerializeIngredients(newFolder);
            operations_completed += ingredientCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //categories
            SerializeCategories(newFolder);
            operations_completed += categoryCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //product categories
            SerializeProductIndexes(newFolder);
            operations_completed += productIndexCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeUMFamilies(newFolder);
            operations_completed += umFamiliesCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeUnitMeasures(newFolder);
            operations_completed += unitMeasureCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //roles
            SerializeRoles(newFolder);

            //employees
            SerializeEmployees(newFolder);
            operations_completed += employeeCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //production areas
            SerializeProductionAreas(newFolder);
            operations_completed += productionAreaCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //sale areas
            SerializeSaleAreas(newFolder);
            operations_completed += saleAreaCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //tables
            SerializeTables(newFolder);
            operations_completed += tableCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeInventoryAreas(newFolder);
            operations_completed += inventoryAreasCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            
            //SerializeCostTraces(newFolder, worker);
            //operations_completed += costTracesCount;
            //worker.ReportProgress(operations_completed * 100 / operations_total);

            //inventory
            SerializeInventoryItems(newFolder, worker);
            operations_completed += inventoryCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //inventory history
            SerializeInventoryHistory(newFolder, worker);

            //sales
            SerializeSales(newFolder, worker);

            //purchases
            SerializePurchases(newFolder);
            //operations_completed += purchasesCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeAdjustments(newFolder);
            //operations_completed += adjustmentsCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeTransfers(newFolder);
            //operations_completed += transfersCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeProductions(newFolder);
            //operations_completed += productionsCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);
            
            SerializeAdjustmentItems(newFolder);
            //operations_completed += adjustmentItemsCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeTransferItems(newFolder);
            //operations_completed += transferItemsCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeProductionItems(newFolder);
            //operations_completed += productionItemsCount;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //purchaselineitems
            SerializePurchaseItems(newFolder);
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //salelineitems
            SerializeSaleItems(newFolder, worker);
            //operations_completed += lineitemCount;
            //worker.ReportProgress(operations_completed * 100 / operations_total);

            SerializeCompanyInfo(newFolder);

            context = null;
        }        

        private void SerializeProductionAreas(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSProductionArea[]));

            List<TSProductionArea> productionAreas = new List<TSProductionArea>();

            foreach (var item in context.ProductionAreas)
            {
                TSProductionArea productionAreaCopy = new TSProductionArea();
                productionAreaCopy.Id = item.Id;
                productionAreaCopy.Name = item.Name;

                productionAreas.Add(productionAreaCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + productionAreasFile))
            {
                ser.Serialize(sw, productionAreas.ToArray());
            }
        } 

        private void SerializeProducts(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSProduct[]));

            List<TSProduct> products = new List<TSProduct>();

            foreach (var item in context.Products)
            {
                TSProduct productCopy = new TSProduct();

                productCopy.Id = item.Id;
                //productCopy.Code = item.Code;
                productCopy.Name = item.Name;
                //productCopy.IsIngredient = item.IsIngredient;
                productCopy.IsRecipe = item.IsRecipe;
                productCopy.IsStorable = item.IsStorable;                
                productCopy.NotInMenu = item.NotInMenu;
                productCopy.SalePrice = item.SalePrice;
                //productCopy.IsPurchasable = item.IsPurchasable;
                productCopy.ProductionAreaId = item.ProductionAreaId;
                productCopy.IsProduced = item.IsProduced;
                productCopy.IsEntrant = item.IsEntrant;
                productCopy.UMFamilyId = item.UMFamily.Id;
                productCopy.Description = item.Description;
                productCopy.PrintString = item.PrintString;
                productCopy.ImageFileName = item.ImageFileName;
                //productCopy.PurchasePrice = item.PurchasePrice;
                productCopy.CostPrice = item.CostPrice;
                productCopy.CostQuantity = item.CostQuantity;
                productCopy.CostUMId = item.CostUMId;
                productCopy.MinimumStock = item.MinimumStock;
                productCopy.MinimumStockUMId = item.MinimumStockUMId;

                products.Add(productCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + productsFile))
            {
                ser.Serialize(sw, products.ToArray());
            }
        }
        
        private void SerializeIngredients(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSIngredient[]));

            List<TSIngredient> ingredients = new List<TSIngredient>();

            foreach (var item in context.Ingredients)
            {
                TSIngredient ingredientCopy = new TSIngredient();
                ingredientCopy.Quantity = item.Quantity;
                ingredientCopy.BaseProductId = item.BaseProductId;
                ingredientCopy.IngredientProductId = item.IngredientProductId;
                ingredientCopy.UnitMeasureId = item.UnitMeasure.Id;

                ingredients.Add(ingredientCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + ingredientsFile))
            {
                ser.Serialize(sw, ingredients.ToArray());
            }
        }

        private void SerializeCategories(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSCategory[]));

            List<TSCategory> categories = new List<TSCategory>();

            foreach (var item in context.Categories)
            {
                TSCategory categoryCopy = new TSCategory();
                categoryCopy.Id = item.Id;
                categoryCopy.Name = item.Name;

                categories.Add(categoryCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + categoriesFile))
            {
                ser.Serialize(sw, categories.ToArray());
            }
        }        
        private void SerializeProductIndexes(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSProductIndex[]));

            List<TSProductIndex> productCategories = new List<TSProductIndex>();

            foreach (var item in context.ProductIndexes)
            {
                TSProductIndex itemCopy = new TSProductIndex();
                itemCopy.ProductId = item.ProductId;
                itemCopy.CategoryId = item.CategoryId;
                itemCopy.IsMain = item.IsMain;

                productCategories.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + productindexesFile))
            {
                ser.Serialize(sw, productCategories.ToArray());
            }
        }
        private void SerializeUMFamilies(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSUMFamily[]));

            List<TSUMFamily> umFamilies = new List<TSUMFamily>();

            foreach (var item in context.UMFamilies)
            {
                TSUMFamily itemCopy = new TSUMFamily();
                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;

                umFamilies.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + umFamiliesFile))
            {
                ser.Serialize(sw, umFamilies.ToArray());
            }
        }
        private void SerializeUnitMeasures(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSUMnitMeasure[]));

            List<TSUMnitMeasure> unitMeasures = new List<TSUMnitMeasure>();

            foreach (var item in context.UnitMeasures)
            {
                TSUMnitMeasure itemCopy = new TSUMnitMeasure();
                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;
                itemCopy.Caption = item.Caption;
                itemCopy.IsFamilyBase = item.IsFamilyBase;
                itemCopy.ToBaseConversion = item.ToBaseConversion;
                itemCopy.UMFamilyId = item.UMFamily.Id;

                unitMeasures.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + unitMeasuresFile))
            {
                ser.Serialize(sw, unitMeasures.ToArray());
            }
        }
        private void SerializeEmployees(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSEmployee[]));

            List<TSEmployee> employees = new List<TSEmployee>();

            foreach (var item in context.Employees)
            {
                TSEmployee itemCopy = new TSEmployee();
                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;
                itemCopy.IsActive = item.IsActive;
                itemCopy.CanSell = item.CanSell;
                itemCopy.CanPurchase = item.CanPurchase;
                itemCopy.ImageFileName = item.ImageFileName;
                itemCopy.Password = item.Password;
                itemCopy.RoleId = item.Role_Id;

                employees.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + employeesFile))
            {
                ser.Serialize(sw, employees.ToArray());
            }
        }
        private void SerializeRoles(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSRole[]));

            List<TSRole> roles = new List<TSRole>();

            foreach (var item in context.Roles)
            {
                TSRole itemCopy = new TSRole();
                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;
                itemCopy.CanLogin = item.CanLogin;
                itemCopy.CanSeeSales = item.CanSeeSales;
                itemCopy.CanRemoveSales = item.CanRemoveSales;
                itemCopy.CanSeeOldSales = item.CanSeeOldSales;
                itemCopy.CanSeePurchases = item.CanSeePurchases;
                itemCopy.CanRemovePurchases = item.CanRemovePurchases;
                itemCopy.CanSeeOldPurchases = item.CanSeePurchases;
                itemCopy.CanSeeInventory = item.CanSeeInventory;
                itemCopy.CanCreateProducts = item.CanCreateProducts;
                itemCopy.CanEditProducts = item.CanEditProducts;
                itemCopy.CanRemoveProducts = item.CanRemoveProducts;
                itemCopy.CanSeeEmployees = item.CanSeeEmployees;
                itemCopy.CanCreateEmployees = item.CanCreateEmployees;
                itemCopy.CanEditEmployees = item.CanEditEmployees;
                itemCopy.CanRemoveEmployees = item.CanRemoveEmployees;
                itemCopy.CanSeeMiPaladar = item.CanSeeMiPaladar;
                itemCopy.CanExportImport = item.CanExportImport;
                itemCopy.CanSeeReports = item.CanSeeReports;
                itemCopy.CanSeeSalesReport = item.CanSeeSalesReport;
                itemCopy.CanSeeSalesByItemReport = item.CanSeeSalesByItemReport;

                roles.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + rolesFile))
            {
                ser.Serialize(sw, roles.ToArray());
            }
        }        

        private void SerializeSaleAreas(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSSaleArea[]));

            List<TSSaleArea> areas = new List<TSSaleArea>();

            foreach (var item in context.PriceLists)
            {
                TSSaleArea itemCopy = new TSSaleArea();
                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;

                areas.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + saleareasFile))
            {
                ser.Serialize(sw, areas.ToArray());
            }
        }
        private void SerializeTables(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSTable[]));

            List<TSTable> tables = new List<TSTable>();

            foreach (var item in context.Tables)
            {
                TSTable itemCopy = new TSTable();
                itemCopy.Id = item.Id;
                itemCopy.Number = item.Number;
                itemCopy.Capacity = item.Capacity;
                itemCopy.Description = item.Description;
                itemCopy.AreaId = item.AreaId;
                itemCopy.IsBar = item.IsBar;

                tables.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + tablesFile))
            {
                ser.Serialize(sw, tables.ToArray());
            }
        }

        private void SerializeInventoryAreas(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSInventoryArea[]));

            List<TSInventoryArea> inventoryAreas = new List<TSInventoryArea>();

            foreach (var item in context.Inventories)
            {
                TSInventoryArea itemCopy = new TSInventoryArea();
                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;
                itemCopy.IsFloor = item.IsFloor;

                inventoryAreas.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + inventoryAreasFile))
            {
                ser.Serialize(sw, inventoryAreas.ToArray());
            }
        }
        //private void SerializeCostTraces(string newFolder, BackgroundWorker worker)
        //{
        //    XmlSerializer ser = new XmlSerializer(typeof(TSCostTrace[]));

        //    List<TSCostTrace> cost_traces = new List<TSCostTrace>();

        //    foreach (var item in context.CostTraces)
        //    {
        //        TSCostTrace itemCopy = new TSCostTrace();
        //        itemCopy.Date = item.Date;
        //        itemCopy.ProductId = item.Product.Id;
        //        itemCopy.Quantity = item.Quantity;
        //        itemCopy.UnitMeasureId = item.UnitMeasure.Id;                
        //        itemCopy.InventoryAreaId = item.Inventory.Id;
        //        itemCopy.Cost = item.Cost;

        //        cost_traces.Add(itemCopy);
        //    }

        //    using (StreamWriter sw = new StreamWriter(newFolder + costTracesFile))
        //    {
        //        ser.Serialize(sw, cost_traces.ToArray());
        //    }

        //    worker.ReportProgress(operations_completed * 100 / operations_total);
        //}        

        private void SerializeInventoryItems(string newFolder, BackgroundWorker worker)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSInventoryItem[]));

            List<TSInventoryItem> inventory = new List<TSInventoryItem>();

            foreach (var item in context.InventoryItems)
            {
                TSInventoryItem itemCopy = new TSInventoryItem();
                itemCopy.Quantity = item.Quantity;
                itemCopy.ProductId = item.Product.Id;
                itemCopy.MinimumQuantity = item.MinimumQuantity;
                itemCopy.InventoryAreaId = item.Inventory.Id;
                itemCopy.Cost = item.Cost;

                inventory.Add(itemCopy);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + inventoryFile))
            {
                ser.Serialize(sw, inventory.ToArray());
            }

            worker.ReportProgress(operations_completed * 100 / operations_total);
        }
        private void SerializeInventoryHistory(string newFolder, BackgroundWorker worker)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSInventoryTrace[]));

            List<TSInventoryTrace> inventoryHistory = new List<TSInventoryTrace>();

            foreach (var item in context.InventoryTraces)
            {
                TSInventoryTrace itemCopy = new TSInventoryTrace();
                itemCopy.Quantity = item.Quantity;
                itemCopy.ProductId = item.ProductId;
                itemCopy.Date = item.Date;
                itemCopy.InventoryAreaId = item.Inventory.Id;

                inventoryHistory.Add(itemCopy);

                if (operations_completed++ % 100 == 0)
                    worker.ReportProgress(operations_completed * 100 / operations_total);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + inventoryhistoryFile))
            {
                ser.Serialize(sw, inventoryHistory.ToArray());
            }

            worker.ReportProgress(operations_completed * 100 / operations_total);
        }
        private void SerializeSales(string newFolder, BackgroundWorker worker)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSSale[]));

            List<TSSale> sales = new List<TSSale>();

            foreach (var item in context.Orders.OfType<Sale>())
            {
                TSSale itemCopy = new TSSale();

                itemCopy.Id = item.Id;
                itemCopy.Date = item.Date;
                itemCopy.DateCreated = item.DateCreated;
                itemCopy.Memo = item.Memo;
                if (item.Employee != null)
                    itemCopy.WaiterId = item.Employee.Id;               

                itemCopy.DateClosed = item.DateClosed;
                itemCopy.DatePrinted = item.DatePrinted;
                itemCopy.Discount = item.Discount;
                itemCopy.DiscountInPercent = item.DiscountInPercent;
                itemCopy.Cash = item.Cash;
                itemCopy.Prints = item.Prints;
                itemCopy.Persons = item.Persons;
                itemCopy.Paid = item.Paid;
                itemCopy.Number = item.Number;
                itemCopy.SubTotal = item.SubTotal;
                itemCopy.Total = item.Total;
                itemCopy.Closed = item.Closed;                
                itemCopy.Tax = item.Tax;
                itemCopy.TaxInPercent = item.TaxInPercent;
                itemCopy.Tips = item.Tips;
                itemCopy.TotalCost = item.TotalCost;

                if (item.Table != null) itemCopy.TableId = item.Table.Id;               
                
                //itemCopy.AreaId = item.PriceListId;                

                sales.Add(itemCopy);

                if (operations_completed++ % 100 == 0)
                    worker.ReportProgress(operations_completed * 100 / operations_total);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + salesFile))
            {
                ser.Serialize(sw, sales.ToArray());
            }

            worker.ReportProgress(operations_completed * 100 / operations_total);
        }
        private void SerializePurchases(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSPurchase[]));

            List<TSPurchase> purchases = new List<TSPurchase>();

            foreach (var item in context.Orders.OfType<Purchase>())
            {
                TSPurchase itemCopy = new TSPurchase();
                itemCopy.Id = item.Id;                
                itemCopy.Date = item.Date;
                itemCopy.DateCreated = item.DateCreated;
                //itemCopy.Number = item.TheNumber;
                itemCopy.Total = item.Total;
                //itemCopy.Closed = item.IsClosed;
                //itemCopy.PurchaseTypeId = item.PurchaseTypeId;
                itemCopy.Memo = item.Memo;
                if (item.Employee != null)
                    itemCopy.ResponsibleId = item.Employee.Id;

                //itemCopy.Title = item.Memo;
                if (item.Inventory != null) itemCopy.InventoryId = item.Inventory.Id;

                purchases.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + purchasesFile))
            {
                ser.Serialize(sw, purchases.ToArray());
            }
        }
        private void SerializeAdjustments(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSAdjustment[]));

            List<TSAdjustment> adjustments = new List<TSAdjustment>();

            foreach (var item in context.Orders.OfType<Adjustment>())
            {
                TSAdjustment itemCopy = new TSAdjustment();
                itemCopy.Id = item.Id;
                itemCopy.Date = item.Date;
                itemCopy.DateCreated = item.DateCreated;
                itemCopy.Memo = item.Memo;
                if (item.Employee != null) itemCopy.ResponsibleId = item.Employee.Id;
                itemCopy.InventoryAreaId = item.Inventory.Id;

                adjustments.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + adjustmentsFile))
            {
                ser.Serialize(sw, adjustments.ToArray());
            }
        }
        private void SerializeTransfers(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSTransfer[]));

            List<TSTransfer> purchases = new List<TSTransfer>();

            foreach (var item in context.Orders.OfType<Transfer>())
            {
                TSTransfer itemCopy = new TSTransfer();
                itemCopy.Id = item.Id;
                itemCopy.Date = item.Date;
                itemCopy.DateCreated = item.DateCreated;
                itemCopy.Memo = item.Memo;
                if (item.Employee != null) itemCopy.ResponsibleId = item.Employee.Id;
                itemCopy.InventoryAreaFromId = item.InventoryFrom.Id;
                itemCopy.InventoryAreaToId = item.InventoryTo.Id;

                purchases.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + transfersFile))
            {
                ser.Serialize(sw, purchases.ToArray());
            }
        }
        private void SerializeProductions(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSProduction[]));

            List<TSProduction> purchases = new List<TSProduction>();

            foreach (var item in context.Orders.OfType<Production>())
            {
                TSProduction itemCopy = new TSProduction();
                itemCopy.Id = item.Id;
                itemCopy.Date = item.Date;
                itemCopy.DateCreated = item.DateCreated;
                itemCopy.Memo = item.Memo;
                if (item.Employee != null) itemCopy.ResponsibleId = item.Employee.Id;
                itemCopy.InventoryAreaId = item.Inventory.Id;

                purchases.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + productionsFile))
            {
                ser.Serialize(sw, purchases.ToArray());
            }
        }
        private void SerializeSaleItems(string newFolder, BackgroundWorker worker)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSSaleLineItem[]));

            List<TSSaleLineItem> saleitems = new List<TSSaleLineItem>();

            foreach (var item in context.LineItems.OfType<SaleLineItem>())
            {
                TSSaleLineItem itemCopy = new TSSaleLineItem();
                itemCopy.ProductId = item.Product.Id;
                itemCopy.Quantity = item.Quantity;
                itemCopy.UnitMeasureId = item.UnitMeasure.Id;                
                itemCopy.OrderId = item.Order.Id;
                itemCopy.Amount = item.Amount;
                itemCopy.Printed = item.Printed;                
                itemCopy.Cost = item.Cost;

                saleitems.Add(itemCopy);

                if (operations_completed++ % 100 == 0)
                    worker.ReportProgress(operations_completed * 100 / operations_total);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + salelineitemsFile))
            {
                ser.Serialize(sw, saleitems.ToArray());
            }

            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void SerializePurchaseItems(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSPurchaseLineItem[]));

            List<TSPurchaseLineItem> purchaseitems = new List<TSPurchaseLineItem>();

            foreach (var item in context.LineItems.OfType<PurchaseLineItem>())
            {
                TSPurchaseLineItem itemCopy = new TSPurchaseLineItem();
                itemCopy.ProductId = item.Product.Id;
                itemCopy.Quantity = item.Quantity;
                itemCopy.UnitMeasureId = item.UnitMeasure.Id;
                itemCopy.PurchaseId = item.Order.Id;
                itemCopy.Amount = item.Amount;

                purchaseitems.Add(itemCopy);

                operations_completed++;
                //if (operations_completed++ % 100 == 0)
                //    worker.ReportProgress(operations_completed * 100 / operations_total);
            }

            using (StreamWriter sw = new StreamWriter(newFolder + purchaselineitemsFile))
            {
                ser.Serialize(sw, purchaseitems.ToArray());
            }
        }

        private void SerializeAdjustmentItems(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSAdjustmentItem[]));

            List<TSAdjustmentItem> adjustmentItems = new List<TSAdjustmentItem>();

            foreach (var item in context.LineItems.OfType<AdjustmentItem>())
            {
                TSAdjustmentItem itemCopy = new TSAdjustmentItem();
                itemCopy.Quantity = item.Quantity;
                itemCopy.ProductId = item.Product.Id;
                itemCopy.UnitMeasureId = item.UnitMeasure.Id;
                itemCopy.AdjustmentId = item.Order.Id;
                itemCopy.Cost = item.Cost;

                adjustmentItems.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + adjustmentItemsFile))
            {
                ser.Serialize(sw, adjustmentItems.ToArray());
            }
        }
        private void SerializeTransferItems(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSTransferItem[]));

            List<TSTransferItem> transferItems = new List<TSTransferItem>();

            foreach (var item in context.LineItems.OfType<TransferItem>())
            {
                TSTransferItem itemCopy = new TSTransferItem();
                itemCopy.Quantity = item.Quantity;
                itemCopy.ProductId = item.Product.Id;
                itemCopy.UnitMeasureId = item.UnitMeasure.Id;
                itemCopy.TransferId = item.Order.Id;

                transferItems.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + transferItemsFile))
            {
                ser.Serialize(sw, transferItems.ToArray());
            }
        }
        private void SerializeProductionItems(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSProductionItem[]));

            List<TSProductionItem> productionItems = new List<TSProductionItem>();

            foreach (var item in context.LineItems.OfType<ProductionItem>())
            {
                TSProductionItem itemCopy = new TSProductionItem();
                itemCopy.Quantity = item.Quantity;
                itemCopy.ProductId = item.Product.Id;
                itemCopy.UnitMeasureId = item.UnitMeasure.Id;
                itemCopy.ProductionId = item.Order.Id;
                itemCopy.Cost = item.Cost;

                productionItems.Add(itemCopy);

                operations_completed++;
            }

            using (StreamWriter sw = new StreamWriter(newFolder + productionItemsFile))
            {
                ser.Serialize(sw, productionItems.ToArray());
            }
        }

        private void SerializeCompanyInfo(string newFolder)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSCompanyInfo));

            TSCompanyInfo companyInfo = new TSCompanyInfo();

            Misc source_info = context.Miscs.First();

            companyInfo.CompanyName = source_info.CompanyName;
            companyInfo.CompanyImage = source_info.CompanyImage;
            companyInfo.DefaultTax = source_info.DefaultTax;
            companyInfo.StartingShiftAmount = source_info.StartingShiftAmount;

            using (StreamWriter sw = new StreamWriter(newFolder + companyInfoFile))
            {
                ser.Serialize(sw, companyInfo);
            }
        }

        #endregion        

        #region Deserialization

        /// <summary>
        /// checks if the folder to import data from has all the files required
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool CheckFilesForImport(string folder) 
        {
            if (!File.Exists(folder + productsFile)) return false;
            if (!File.Exists(folder + ingredientsFile)) return false;
            if (!File.Exists(folder + categoriesFile)) return false;
            if (!File.Exists(folder + productindexesFile)) return false;

            if (!File.Exists(folder + umFamiliesFile)) return false;
            if (!File.Exists(folder + unitMeasuresFile)) return false;

            if (!File.Exists(folder + employeesFile)) return false;
            if (!File.Exists(folder + rolesFile)) return false;
            
            if (!File.Exists(folder + productionAreasFile)) return false;
            if (!File.Exists(folder + saleareasFile)) return false;
            if (!File.Exists(folder + tablesFile)) return false;
            if (!File.Exists(folder + inventoryAreasFile)) return false;

            if (!File.Exists(folder + inventoryFile)) return false;
            if (!File.Exists(folder + inventoryhistoryFile)) return false;

            if (!File.Exists(folder + salesFile)) return false;
            if (!File.Exists(folder + purchasesFile)) return false;
            if (!File.Exists(folder + adjustmentsFile)) return false;
            if (!File.Exists(folder + transfersFile)) return false;
            if (!File.Exists(folder + productionsFile)) return false;

            if (!File.Exists(folder + adjustmentItemsFile)) return false;
            if (!File.Exists(folder + transferItemsFile)) return false;
            if (!File.Exists(folder + productionItemsFile)) return false;
            if (!File.Exists(folder + salelineitemsFile)) return false;
            if (!File.Exists(folder + purchaselineitemsFile)) return false;

            return true;
        }

        public void Deserialize(string folder, BackgroundWorker worker)
        {
            context = new RestaurantDBEntities();

            operations_completed = 0;

            //total operations to clear database
            operations_total = context.Products.Count() + 
                context.Ingredients.Count() +
                context.Categories.Count() +
                context.ProductIndexes.Count() +
                context.Roles.Count() +
                context.Employees.Count() + 
                context.PriceLists.Count() +
                context.Tables.Count() +
                //context.CostTraces.Count() +
                context.ProductionAreas.Count() +
                context.Inventories.Count() +
                context.InventoryItems.Count() +
                context.InventoryTraces.Count() +
                context.Orders.Count() +
                context.LineItems.Count();

            //deserialize entities from file

            TSProduct[] products = DeserializeEntities<TSProduct>(folder + productsFile);
            TSIngredient[] ingredients = DeserializeEntities<TSIngredient>(folder + ingredientsFile);
            TSCategory[] categories = DeserializeEntities<TSCategory>(folder + categoriesFile);            
            TSProductIndex[] productindexes = DeserializeEntities<TSProductIndex>(folder + productindexesFile);
            TSUMFamily[] umfamilies = DeserializeEntities<TSUMFamily>(folder + umFamiliesFile);
            TSUMnitMeasure[] unitmeasures = DeserializeEntities<TSUMnitMeasure>(folder + unitMeasuresFile);

            TSEmployee[] employees = DeserializeEntities<TSEmployee>(folder + employeesFile);            
            TSRole[] roles = DeserializeEntities<TSRole>(folder + rolesFile);
            

            TSProductionArea[] productionAreas = DeserializeEntities<TSProductionArea>(folder + productionAreasFile);
            TSSaleArea[] saleAreas = DeserializeEntities<TSSaleArea>(folder + saleareasFile);
            
            TSTable[] tables = DeserializeEntities<TSTable>(folder + tablesFile);
            TSInventoryArea[] inventoryAreas = DeserializeEntities<TSInventoryArea>(folder + inventoryAreasFile);
            //TSCostTrace[] cost_traces = DeserializeEntities<TSCostTrace>(folder + costTracesFile);
            TSInventoryItem[] inventory = DeserializeEntities<TSInventoryItem>(folder + inventoryFile);
            TSInventoryTrace[] inventoryHistory = DeserializeEntities<TSInventoryTrace>(folder + inventoryhistoryFile);

            TSSale[] sales = DeserializeEntities<TSSale>(folder + salesFile);
            TSPurchase[] purchases = DeserializeEntities<TSPurchase>(folder + purchasesFile);
            TSAdjustment[] adjustments = DeserializeEntities<TSAdjustment>(folder + adjustmentsFile);
            TSTransfer[] transfers = DeserializeEntities<TSTransfer>(folder + transfersFile);
            TSProduction[] productions = DeserializeEntities<TSProduction>(folder + productionsFile);

            TSAdjustmentItem[] adjustmentItems = DeserializeEntities<TSAdjustmentItem>(folder + adjustmentItemsFile);
            TSTransferItem[] transferItems = DeserializeEntities<TSTransferItem>(folder + transferItemsFile);
            TSProductionItem[] productionItems = DeserializeEntities<TSProductionItem>(folder + productionItemsFile);
            TSSaleLineItem[] salelineitems = DeserializeEntities<TSSaleLineItem>(folder + salelineitemsFile);
            TSPurchaseLineItem[] purchaselineitems = DeserializeEntities<TSPurchaseLineItem>(folder + purchaselineitemsFile);
            
            TSCompanyInfo companyInfo = DeserializeEntity<TSCompanyInfo>(folder + companyInfoFile);

            int lineitems_count = context.LineItems.Count();
            int orders_count = context.Orders.Count();
            int inventoryTrace_count = context.InventoryTraces.Count();

            //my own function
            context.DeleteStuff();

            operations_completed += lineitems_count + orders_count + inventoryTrace_count;
            worker.ReportProgress(operations_completed * 100 / operations_total);

            //ClearObjectSet(context.LineItems, context.LineItems.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.ProductionItems, context.ProductionItems.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.TransferItems, context.TransferItems.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.AdjustmentItems, context.AdjustmentItems.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.Productions, context.Productions.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.Transfers, context.Transfers.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.Adjustments, context.Adjustments.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.Orders, context.Orders.Where(x => x.Id == x.Id), worker);

            context = new RestaurantDBEntities();

            ClearObjectSet(context.Tables, context.Tables.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.PriceLists, context.PriceLists.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.ProductionAreas, context.ProductionAreas.Where(x => x.Id == x.Id), worker);

            ClearObjectSet(context.Employees, context.Employees.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.Roles, context.Roles.Where(x => x.Id == x.Id), worker);
            //DeleteAllEmployees();
            //ClearObjectSet(context.Roles, context.Roles.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.Permissions, context.Permissions.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.RoleDefinitions, context.RoleDefinitions.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.Prices, context.Prices.Where(x => x.Id == x.Id), worker);
            
            //ClearObjectSet(context.PurchaseTypes, context.PurchaseTypes.Where(x => x.Id == x.Id), worker);            
            //ClearObjectSet(context.InventoryTraces, context.InventoryTraces.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.CostTraces, context.CostTraces.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.InventoryItems, context.InventoryItems.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.Inventories, context.Inventories.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.JobPositions, context.JobPositions.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.ProductInTemplates, context.ProductInTemplates.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.ProductTemplates, context.ProductTemplates.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.ProductIndexes, context.ProductIndexes.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.Categories, context.Categories.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.Ingredients, context.Ingredients.Where(x => x.Id == x.Id), worker);
            ClearObjectSet(context.Products, context.Products.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.UnitMeasures, context.UnitMeasures.Where(x => x.Id == x.Id), worker);
            //ClearObjectSet(context.UMFamilies, context.UMFamilies.Where(x => x.Id == x.Id), worker);

            Dictionary<int, Product> productDict = new Dictionary<int, Product>();            
            Dictionary<int, Category> categoryDict = new Dictionary<int, Category>();
            Dictionary<int, UMFamily> umFamiliesDict = new Dictionary<int, UMFamily>();
            Dictionary<int, UnitMeasure> unitMeasuresDict = new Dictionary<int, UnitMeasure>();
            
            Dictionary<int, Employee> employeeDict = new Dictionary<int, Employee>();
            Dictionary<int, Role> rolesDict = new Dictionary<int, Role>();

            Dictionary<int, ProductionArea> productionAreaDict = new Dictionary<int, ProductionArea>();
            Dictionary<int, PriceList> saleAreaDict = new Dictionary<int, PriceList>();
            Dictionary<int, Inventory> inventoryAreaDict = new Dictionary<int, Inventory>();

            Dictionary<int, Table> tableDict = new Dictionary<int, Table>();
            Dictionary<int, Order> orderDict = new Dictionary<int, Order>();
            Dictionary<int, Adjustment> adjustmentDict = new Dictionary<int, Adjustment>();
            Dictionary<int, Transfer> transferDict = new Dictionary<int, Transfer>();
            Dictionary<int, Production> productionDict = new Dictionary<int, Production>();

            CreateCategories(categories, categoryDict);

            CreateProductionAreas(productionAreas, productionAreaDict);

            CreateUMFamilies(umfamilies, umFamiliesDict);

            CreateUnitMeasures(unitmeasures, unitMeasuresDict, umFamiliesDict);

            CreateProducts(worker, products, productDict, productionAreaDict, umFamiliesDict, unitMeasuresDict);

            CreateIngredients(worker, ingredients, productDict, unitMeasuresDict);            

            CreateProductIndexes(worker, productindexes, productDict, categoryDict);

            CreateRoles(roles, rolesDict);

            CreateEmployees(employees, rolesDict, employeeDict);

            CreateSaleAreas(saleAreas, saleAreaDict);

            CreateTables(tables, tableDict, saleAreaDict);

            CreateInventoryAreas(inventoryAreas, inventoryAreaDict);

            //CreateCostTraces(worker, cost_traces, productDict, unitMeasuresDict, inventoryAreaDict);

            CreateInventory(worker, inventory, productDict, inventoryAreaDict);

            CreateInventoryHistory(worker, inventoryHistory, productDict, inventoryAreaDict);

            CreateSales(worker, sales, employeeDict, saleAreaDict, tableDict, orderDict);

            CreatePurchases(worker, purchases,employeeDict, orderDict, inventoryAreaDict);

            CreateAdjustments(adjustments, inventoryAreaDict, employeeDict, adjustmentDict);

            CreateTransfers(transfers, inventoryAreaDict, employeeDict, transferDict);

            CreateProductions(productions, inventoryAreaDict, employeeDict, productionDict);

            CreateSaleLineItems(worker, salelineitems, productDict, orderDict, unitMeasuresDict);

            CreatePurchaseLineItems(worker, purchaselineitems, productDict, orderDict, unitMeasuresDict);

            CreateAdjustmentItems(worker, adjustmentItems, productDict, adjustmentDict, unitMeasuresDict);

            CreateTransferItems(worker, transferItems, productDict, transferDict,unitMeasuresDict);

            CreateProductionItems(worker, productionItems, productDict, productionDict, unitMeasuresDict);

            CreateCompanyInfo(companyInfo);

            //done
            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);

            context = null;
        }

        private void CreateProductionItems(BackgroundWorker worker, TSProductionItem[] productionItems,
            Dictionary<int, Product> productDict, Dictionary<int, Production> productionDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in productionItems)
            {
                ProductionItem pi = new ProductionItem();
                pi.Quantity = item.Quantity;
                pi.UnitMeasure = unitMeasureDict[item.UnitMeasureId];
                pi.Product = productDict[item.ProductId];
                pi.Order = productionDict[item.ProductionId];
                pi.Cost = item.Cost;

                context.LineItems.AddObject(pi);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateTransferItems(BackgroundWorker worker, TSTransferItem[] transferItems,
            Dictionary<int, Product> productDict, Dictionary<int, Transfer> transferDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in transferItems)
            {
                TransferItem ti = new TransferItem();
                ti.Quantity = item.Quantity;
                ti.UnitMeasure = unitMeasureDict[item.UnitMeasureId];
                ti.Product = productDict[item.ProductId];
                ti.Order = transferDict[item.TransferId];

                context.LineItems.AddObject(ti);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateAdjustmentItems(BackgroundWorker worker, TSAdjustmentItem[] adjustmentItems,
            Dictionary<int, Product> productDict, Dictionary<int, Adjustment> adjustmentDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in adjustmentItems)
            {
                AdjustmentItem ai = new AdjustmentItem();
                ai.Quantity = item.Quantity;
                ai.UnitMeasure = unitMeasureDict[item.UnitMeasureId];
                ai.Product = productDict[item.ProductId];
                ai.Order = adjustmentDict[item.AdjustmentId];
                ai.Cost = item.Cost;

                context.LineItems.AddObject(ai);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreatePurchaseLineItems(BackgroundWorker worker, TSPurchaseLineItem[] lineitems,
            Dictionary<int, Product> productDict, Dictionary<int, Order> orderDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in lineitems)
            {
                PurchaseLineItem lineitem = new PurchaseLineItem();
                lineitem.Quantity = item.Quantity;
                lineitem.UnitMeasure = unitMeasureDict[item.UnitMeasureId];
                lineitem.Product = productDict[item.ProductId];
                lineitem.Amount = item.Amount;
                lineitem.Order = orderDict[item.PurchaseId];

                context.LineItems.AddObject(lineitem);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }
        private void CreateSaleLineItems(BackgroundWorker worker, TSSaleLineItem[] lineitems,
            Dictionary<int, Product> productDict, Dictionary<int, Order> orderDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in lineitems)
            {
                SaleLineItem lineitem = new SaleLineItem();
                lineitem.Quantity = item.Quantity;
                lineitem.UnitMeasure = unitMeasureDict[item.UnitMeasureId];
                lineitem.Product = productDict[item.ProductId];
                lineitem.Amount = item.Amount;
                lineitem.Order = orderDict[item.OrderId];
                lineitem.Printed = item.Printed;
                lineitem.Cost = item.Cost;

                context.LineItems.AddObject(lineitem);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateProductions(TSProduction[] productions, Dictionary<int, Inventory> inventoryAreaDict,
            Dictionary<int, Employee> employeeDict, Dictionary<int, Production> productionDict)
        {
            foreach (var item in productions)
            {
                Production production = new Production();
                production.Date = item.Date;
                production.DateCreated = item.DateCreated;
                production.Memo = item.Memo;
                production.Inventory = inventoryAreaDict[item.InventoryAreaId];
                
                if (employeeDict.ContainsKey(item.ResponsibleId))
                    production.Employee = employeeDict[item.ResponsibleId];

                productionDict[item.Id] = production;

                context.Orders.AddObject(production);
            }
            context.SaveChanges();
        }

        private void CreateTransfers(TSTransfer[] transfers, Dictionary<int, Inventory> inventoryAreaDict,
            Dictionary<int, Employee> employeeDict, Dictionary<int, Transfer> transferDict)
        {
            foreach (var item in transfers)
            {
                Transfer transfer = new Transfer();
                transfer.Date = item.Date;
                transfer.DateCreated = item.DateCreated;
                transfer.Memo = item.Memo;
                transfer.InventoryFrom = inventoryAreaDict[item.InventoryAreaFromId]; 
                transfer.InventoryTo = inventoryAreaDict[item.InventoryAreaToId];

                if (employeeDict.ContainsKey(item.ResponsibleId))
                    transfer.Employee = employeeDict[item.ResponsibleId];

                transferDict[item.Id] = transfer;

                context.Orders.AddObject(transfer);
            }
            context.SaveChanges();
        }

        private void CreateAdjustments(TSAdjustment[] adjustments, Dictionary<int, Inventory> inventoryAreaDict, 
            Dictionary<int, Employee> employeeDict, Dictionary<int, Adjustment> adjustmentDict)
        {
            foreach (var item in adjustments)
            {
                Adjustment adjustment = new Adjustment();
                adjustment.Date = item.Date;
                adjustment.DateCreated = item.DateCreated;
                adjustment.Memo = item.Memo;
                adjustment.Inventory = inventoryAreaDict[item.InventoryAreaId];

                if (item.ResponsibleId.HasValue) 
                    adjustment.Employee = employeeDict[item.ResponsibleId.Value];

                adjustmentDict[item.Id] = adjustment;

                context.Orders.AddObject(adjustment);
            }
            context.SaveChanges();
        }

        private void CreatePurchases(BackgroundWorker worker, TSPurchase[] purchases, Dictionary<int, Employee> employeeDict,
            Dictionary<int, Order> orderDict, Dictionary<int, Inventory> inventoryAreaDict)
        {
            foreach (var item in purchases)
            {
                Purchase purchase = new Purchase();
                //purchase.TheNumber = item.Number;
                purchase.Date = item.Date;
                purchase.DateCreated = item.DateCreated;
                purchase.Total = item.Total;
                //purchase.IsClosed = item.Closed;
                //purchase.PurchaseType = item.PurchaseTypeId.HasValue ? purchaseTypeDict[item.PurchaseTypeId.Value] : null;
                purchase.Memo = item.Memo;
                //purchase.Memo = item.Title;
                if (item.ResponsibleId.HasValue) purchase.Employee = employeeDict[item.ResponsibleId.Value];
                if (item.InventoryId.HasValue) purchase.Inventory = inventoryAreaDict[item.InventoryId.Value];

                orderDict[item.Id] = purchase;

                context.Orders.AddObject(purchase);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateSales(BackgroundWorker worker, TSSale[] sales, Dictionary<int, Employee> employeeDict,
            Dictionary<int, PriceList> areaDict, Dictionary<int, Table> tableDict, Dictionary<int, Order> orderDict)
        {
            foreach (var item in sales)
            {
                Sale sale = new Sale();

                sale.Date = item.Date;
                sale.DateCreated = item.DateCreated;
                sale.Memo = item.Memo;
                sale.Employee = item.WaiterId.HasValue ? employeeDict[item.WaiterId.Value] : null;
                
                sale.DateClosed = item.DateClosed;
                sale.DatePrinted = item.DatePrinted;
                sale.Discount = item.Discount;
                sale.DiscountInPercent = item.DiscountInPercent;
                sale.Cash = item.Cash;
                sale.Prints = item.Prints;
                sale.Persons = item.Persons;
                sale.Paid = item.Paid;
                sale.Number = item.Number;
                sale.SubTotal = item.SubTotal;
                sale.Total = item.Total;
                sale.Closed = item.Closed;
                sale.Tax = item.Tax;
                sale.TaxInPercent = item.TaxInPercent;
                sale.Tips = item.Tips;
                sale.TotalCost = item.TotalCost;                
                //sale.PriceList = item.AreaId.HasValue ? areaDict[item.AreaId.Value] : null;
                sale.Table = item.TableId.HasValue ? tableDict[item.TableId.Value] : null;

                orderDict[item.Id] = sale;

                context.Orders.AddObject(sale);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateInventoryHistory(BackgroundWorker worker, TSInventoryTrace[] inventoryHistory,
            Dictionary<int, Product> productDict, Dictionary<int, Inventory> inventoryAreaDict)
        {
            foreach (var item in inventoryHistory)
            {
                InventoryTrace it = new InventoryTrace();
                it.Quantity = item.Quantity;
                it.Product = productDict[item.ProductId];
                it.Date = item.Date;
                it.Inventory = inventoryAreaDict[item.InventoryAreaId];

                context.InventoryTraces.AddObject(it);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        //private void CreateCostTraces(BackgroundWorker worker, TSCostTrace[] cost_traces, Dictionary<int, Product> productDict,
        //    Dictionary<int, UnitMeasure> umDict, Dictionary<int, Inventory> inventoryAreaDict)
        //{
        //    foreach (var item in cost_traces)
        //    {
        //        CostTrace ct = new CostTrace();
        //        ct.Date = item.Date;
        //        ct.Product = productDict[item.ProductId];
        //        ct.Quantity = item.Quantity;
        //        ct.UnitMeasure = umDict[item.UnitMeasureId];
        //        ct.Inventory = inventoryAreaDict[item.InventoryAreaId];
        //        ct.Cost = item.Cost;

        //        context.CostTraces.AddObject(ct);

        //        if (operations_completed++ % 100 == 0)
        //        {
        //            context.SaveChanges();
        //            worker.ReportProgress(operations_completed * 100 / operations_total);
        //        }
        //    }

        //    context.SaveChanges();
        //    worker.ReportProgress(operations_completed * 100 / operations_total);
        //}

        private void CreateInventory(BackgroundWorker worker, TSInventoryItem[] inventory, Dictionary<int, Product> productDict,
            Dictionary<int, Inventory> inventoryAreaDict)
        {
            foreach (var item in inventory)
            {
                InventoryItem ii = new InventoryItem();
                ii.Quantity = item.Quantity;
                ii.Product = productDict[item.ProductId];
                ii.MinimumQuantity = item.MinimumQuantity;
                ii.Inventory = inventoryAreaDict[item.InventoryAreaId];
                ii.Cost = item.Cost;

                context.InventoryItems.AddObject(ii);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }
        private void CreateInventoryAreas( TSInventoryArea[] inventoryAreas, Dictionary<int, Inventory> inventoryAreaDict)
        {
            foreach (var item in inventoryAreas)
            {
                Inventory inv = new Inventory();
                inv.Name = item.Name;
                inv.IsFloor = item.IsFloor;

                inventoryAreaDict[item.Id] = inv;

                context.Inventories.AddObject(inv);
            }

            context.SaveChanges();
        }

        private void CreateTables(TSTable[] tables, Dictionary<int, Table> tableDict, Dictionary<int, PriceList> areaDict)
        {
            foreach (var item in tables)
            {
                Table table = new Table();
                table.Number = item.Number;
                table.Capacity = item.Capacity;
                table.Description = item.Description;
                table.IsBar = item.IsBar;
                if (item.AreaId.HasValue) table.PriceList = areaDict[item.AreaId.Value];

                tableDict[item.Id] = table;

                context.Tables.AddObject(table);
            }

            context.SaveChanges();
        }

        private void CreateSaleAreas(TSSaleArea[] areas, Dictionary<int, PriceList> areaDict)
        {
            foreach (var item in areas)
            {
                PriceList area = new PriceList();
                area.Name = item.Name;

                areaDict[item.Id] = area;

                context.PriceLists.AddObject(area);
            }

            context.SaveChanges();
        }

        private void CreateRoles(TSRole[] roles, Dictionary<int, Role> rolesDict)
        {
            foreach (var item in roles)
            {
                Role role = new Role();
                role.Name = item.Name;

                role.CanLogin = item.CanLogin;
                role.CanSeeSales = item.CanSeeSales;
                role.CanRemoveSales = item.CanRemoveSales;
                role.CanSeeOldSales = item.CanSeeOldSales;
                role.CanSeePurchases = item.CanSeePurchases;
                role.CanRemovePurchases = item.CanRemovePurchases;
                role.CanSeeOldPurchases = item.CanSeePurchases;
                role.CanSeeInventory = item.CanSeeInventory;
                role.CanCreateProducts = item.CanCreateProducts;
                role.CanEditProducts = item.CanEditProducts;
                role.CanRemoveProducts = item.CanRemoveProducts;
                role.CanSeeEmployees = item.CanSeeEmployees;
                role.CanCreateEmployees = item.CanCreateEmployees;
                role.CanEditEmployees = item.CanEditEmployees;
                role.CanRemoveEmployees = item.CanRemoveEmployees;
                role.CanSeeMiPaladar = item.CanSeeMiPaladar;
                role.CanExportImport = item.CanExportImport;
                role.CanSeeReports = item.CanSeeReports;
                role.CanSeeSalesReport = item.CanSeeSalesReport;
                role.CanSeeSalesByItemReport = item.CanSeeSalesByItemReport;

                rolesDict[item.Id] = role;

                context.Roles.AddObject(role);
            }

            context.SaveChanges();
        }

        private void CreateEmployees(TSEmployee[] employees, Dictionary<int, Role> rolesDict, Dictionary<int, Employee> employeeDict)
        {
            foreach (var item in employees)
            {
                Employee emp = new Employee();
                emp.Name = item.Name;
                emp.IsActive = item.IsActive;
                emp.CanSell = item.CanSell;
                emp.CanPurchase = item.CanPurchase;
                emp.ImageFileName = item.ImageFileName;
                emp.Password = item.Password;
                emp.Role = rolesDict[item.RoleId];

                employeeDict[item.Id] = emp;

                context.Employees.AddObject(emp);
            }

            //context.SaveChanges();
        }
        private void CreateProductIndexes(BackgroundWorker worker, TSProductIndex[] productcategories, 
            Dictionary<int, Product> productDict, Dictionary<int, Category> categoryDict)
        {
            foreach (var item in productcategories)
            {
                //if (!item.ProductId.HasValue || !item.CategoryId.HasValue) continue;

                ProductIndex pi = new ProductIndex();
                pi.Product = productDict[item.ProductId];
                pi.Category = categoryDict[item.CategoryId];
                pi.IsMain = item.IsMain;

                context.ProductIndexes.AddObject(pi);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateCategories(TSCategory[] categories, Dictionary<int, Category> categoryDict)
        {
            foreach (var item in categories)
            {
                Category cat = new Category();
                cat.Name = item.Name;
                categoryDict[item.Id] = cat;

                context.Categories.AddObject(cat);
            }

            context.SaveChanges();
        }
        private void CreateIngredients(BackgroundWorker worker, TSIngredient[] ingredients,
            Dictionary<int, Product> productDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in ingredients)
            {
                Ingredient ing = new Ingredient();
                ing.BaseProduct = productDict[item.BaseProductId];
                ing.IngredientProduct = productDict[item.IngredientProductId];
                ing.Quantity = item.Quantity;
                ing.UnitMeasure = unitMeasureDict[item.UnitMeasureId];

                context.Ingredients.AddObject(ing);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        private void CreateProducts(BackgroundWorker worker, TSProduct[] products, Dictionary<int, Product> productDict,
            Dictionary<int, ProductionArea> productionAreaDict, Dictionary<int, UMFamily> umFamilyDict, Dictionary<int, UnitMeasure> unitMeasureDict)
        {
            foreach (var item in products)
            {
                Product p = new Product();
                //p.Code = item.Code;
                p.Name = item.Name;
                p.IsRecipe = item.IsRecipe;
                //p.IsIngredient = item.IsIngredient;
                p.IsStorable = item.IsStorable;
                p.NotInMenu = item.NotInMenu;
                //p.IsPurchasable = item.IsPurchasable;
                p.SalePrice = item.SalePrice;
                p.UMFamily = umFamilyDict[item.UMFamilyId];
                p.Description=item.Description;
                p.PrintString=item.PrintString;
                p.ImageFileName=item.ImageFileName;
                p.CostPrice=item.CostPrice;
                p.CostQuantity = item.CostQuantity;
                p.CostUnitMeasure=unitMeasureDict[item.CostUMId];
                //p.PurchasePrice = item.PurchasePrice;
                p.IsProduced = item.IsProduced;
                p.ProductionArea = item.ProductionAreaId.HasValue ? productionAreaDict[item.ProductionAreaId.Value] : null;
                p.IsEntrant = item.IsEntrant;
                p.MinimumStock = item.MinimumStock;
                p.MinimumStockUM = unitMeasureDict[item.MinimumStockUMId];

                productDict[item.Id] = p;

                context.Products.AddObject(p);

                if (operations_completed++ % 100 == 0)
                {
                    context.SaveChanges();
                    worker.ReportProgress(operations_completed * 100 / operations_total);
                }
            }

            context.SaveChanges();
            worker.ReportProgress(operations_completed * 100 / operations_total);
        }
        private void CreateUnitMeasures(TSUMnitMeasure[] unitMeasures, 
            Dictionary<int, UnitMeasure> unitMeasureDict, Dictionary<int,UMFamily> umFamiliesDict)
        {
            foreach (var item in unitMeasures)
            {
                //search for um with the same name
                foreach (var um in context.UnitMeasures)
                {
                    if (um.Name == item.Name)
                    {
                        unitMeasureDict[item.Id] = um;
                        break;
                    }
                }
                //UnitMeasure um = new UnitMeasure();
                //um.Name = item.Name;
                //um.Caption = item.Caption;
                //um.IsFamilyBase = item.IsFamilyBase;
                //um.ToBaseConversion = item.ToBaseConversion;
                //um.UMFamily = umFamiliesDict[item.UMFamilyId];

                

                //context.UnitMeasures.AddObject(um);
            }

            //context.SaveChanges();
        }
        private void CreateUMFamilies(TSUMFamily[] umFamilies, Dictionary<int, UMFamily> umFamiliesDict)
        {
            foreach (var item in umFamilies)
            {
                //UMFamily umf = new UMFamily();
                //umf.Name = item.Name;

                //search for UMF with the same name
                foreach (var umf in context.UMFamilies)
                {
                    if (umf.Name == item.Name)
                    {
                        umFamiliesDict[item.Id] = umf;
                        break;
                    }
                }                

                //context.UMFamilies.AddObject(umf);
            }

            //context.SaveChanges();
        }

        private void CreateProductionAreas(TSProductionArea[] productionAreas, Dictionary<int, ProductionArea> productionAreaDict)
        {
            foreach (var item in productionAreas)
            {
                foreach (var pa in context.ProductionAreas)
                {
                    if (pa.Name == item.Name)
                    {
                        productionAreaDict[item.Id] = pa;
                        break;
                    }
                }
            }
            
            context.SaveChanges();
        }

        private void CreateCompanyInfo(TSCompanyInfo source_info)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TSCompanyInfo));

            Misc target_info = context.Miscs.First();

            target_info.CompanyName = source_info.CompanyName;
            target_info.CompanyImage = source_info.CompanyImage;
            target_info.DefaultTax = source_info.DefaultTax;
            target_info.StartingShiftAmount = source_info.StartingShiftAmount;
        }

        private T[] DeserializeEntities<T>(string filepath) where T : class
        {
            T[] items = null;
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T[]));
                    items = (T[])serializer.Deserialize(sr);

                    operations_total += items.Length;
                }
            }
            else items = new T[0];

            return items;
        }

        private T DeserializeEntity<T>(string filepath) where T : class
        {
            T item = null;
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    item = (T)serializer.Deserialize(sr);
                }
            }

            return item;
        }

        private void ClearObjectSet<T>(ObjectSet<T> set, IEnumerable<T> data, BackgroundWorker worker) where T : class
        {
            foreach (var item in data)
            {
                set.DeleteObject(item);

                operations_completed++;

                //if (operations_completed++ % 100 == 0)
                //{

                //}
            }

            context.SaveChanges();

            worker.ReportProgress(operations_completed * 100 / operations_total);
        }

        #endregion        
    }
}
