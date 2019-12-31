using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiPaladar.Entities;
using MiPaladar.ViewModels;

namespace MiPaladar.Services
{
    public interface IInventoryService 
    {
        void CreateInventorySnapshot(DateTime workingDate);
        //void ExecuteSellOperation(DateTime date, Product product, float quantity, UnitMeasure um);
        void ExecuteInventoryOperation(DateTime date, Inventory inventory,
            Product product, double quantity, UnitMeasure um, decimal cost);
        InventoryItem GetInventoryItem(Inventory inventoryArea, Product product);
        /// <summary>
        /// Checks if there is enough existence in inventory of the product. In case of a recipe
        /// checks existence for all the ingredients and returns a list with what is missing.
        /// </summary>
        List<InventoryItem> CheckAvailability(Inventory inventory, Product product, double quantity);
    }
    public class InventoryService : IInventoryService
    {
        MainWindowViewModel appvm;
        public InventoryService(MainWindowViewModel appvm)         
        {
            this.appvm = appvm;
        }

        #region Snapshot

        public void CreateInventorySnapshot(DateTime workingDate)
        {
            if (workingDate != DateTime.Today) return;

            var query = from e in appvm.Context.InventoryTraces
                        where e.Date == workingDate
                        select e;

            if (query.Count() > 0) return; //it was initialized

            foreach (var item in appvm.Context.InventoryItems)
            {
                if (!item.Product.IsStorable) continue;

                InventoryTrace newE = new InventoryTrace();
                newE.Product = item.Product;
                newE.Quantity = item.Quantity;
                newE.Date = workingDate;
                newE.Inventory = item.Inventory;

                appvm.Context.InventoryTraces.AddObject(newE);
            }

            appvm.SaveChanges();
        }

        #endregion        

        #region Inventory Operations        

        public void ExecuteInventoryOperation(DateTime date, Inventory inventory, 
            Product product, double quantity, UnitMeasure um, decimal cost)
        {
            if (product == null || quantity == 0 || !product.IsStorable || inventory == null) return;

            //if (product.IsStorable)
            //{
            //work with current existence
            //if (date == DateTime.Today)
            {
                var query = from e in appvm.InventoryItemsOC
                            where e.Product.Id == product.Id && e.Inventory.Id == inventory.Id
                            select e;

                InventoryItem inventoryItem;// = currentExistence.First();
                if (query.Count() > 0)
                {
                    inventoryItem = query.First();
                }
                else
                {
                    inventoryItem = new InventoryItem();
                    inventoryItem.Product = product;
                    inventoryItem.Inventory = inventory;

                    appvm.Context.InventoryItems.AddObject(inventoryItem);
                    appvm.InventoryItemsOC.Add(inventoryItem);
                }

                inventoryItem.Quantity += quantity * um.ToBaseConversion;
                inventoryItem.Cost += cost;
            }

            DateTime tomorrow = date.Date.Date.AddDays(1);

            //work with historic existence
            var queryResult = from e in appvm.Context.InventoryTraces
                              where e.Date >= tomorrow && e.Product.Id == product.Id && e.Inventory.Id == inventory.Id
                              select e;

            foreach (var existence in queryResult)
            {
                existence.Quantity += quantity * um.ToBaseConversion;
            }

            //appvm.SaveChanges();
        }

        #endregion        

        #region Availability

        public List<InventoryItem> CheckAvailability(Inventory inventory, Product product, double quantity)
        {
            List<InventoryItem> answer = new List<InventoryItem>();

            CheckAvailability(inventory, product, quantity, answer);

            return answer;
        }
        private void CheckAvailability(Inventory inventory, Product product, double quantity, List<InventoryItem> unavailableProducts)
        {
            if (product.IsStorable)
            {
                InventoryItem inventoryItem = GetInventoryItem(inventory, product);
                    //add to unavailable products
                if (inventoryItem != null && inventoryItem.Quantity < quantity) unavailableProducts.Add(inventoryItem);
            }
            else if (product.IsRecipe)
            {
                foreach (var item in product.Ingredients)
                {
                    if (item.IngredientProduct.IsStorable)
                        CheckAvailability(inventory, item.IngredientProduct, item.Quantity * quantity, unavailableProducts);
                }
            }
        }

        public InventoryItem GetInventoryItem(Inventory inventoryArea, Product product)
        {
            if (inventoryArea == null) return null;

            var query = from inventoryItem in product.CurrentExistence
                        where inventoryItem.Inventory.Id == inventoryArea.Id
                        select inventoryItem;
            
            if (query.Count() == 0) return null;

            return query.First();
        }

        #endregion
    }
}
