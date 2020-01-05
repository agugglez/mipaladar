//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MiPaladar.Entities;
//using MiPaladar.ViewModels;

//namespace MiPaladar.Services
//{
//    public interface ITransactionService
//    {
//        //decimal Sell(Product prod, double quantity, DateTime workingDate);
//        //void UndoSell(Product prod, double quantity, decimal cost, DateTime workingDate);

//        //void Purchase(Product prod, double quantity, UnitMeasure um, decimal price, DateTime workingDate, Inventory inventoryArea);
//        //void UndoPurchase(Product prod, double quantity, UnitMeasure um, decimal price, DateTime workingDate, Inventory inventoryArea);

//        //decimal Adjust(Product prod, double quantity, UnitMeasure um, DateTime workingDate, Inventory inventoryArea);
//        //void UndoAdjust(Product prod, double quantity, UnitMeasure um, decimal cost, DateTime workingDate, Inventory inventoryArea);
//        //void RemoveAdjustment(Adjustment adj, bool undoChildAdjustmentItems);

//        //decimal Transfer(Product prod, double quantity, UnitMeasure um, DateTime workingDate, Inventory inv_Source, Inventory inv_Destination);
//        //void UndoTransfer(Product prod, double quantity, UnitMeasure um, decimal cost, DateTime workingDate, Inventory inv_Source, Inventory inv_Destination);
//        //void RemoveTransfer(Transfer trans, bool undoChildTransferItems);

//        //decimal Produce(Product prod, double quantity, UnitMeasure um, DateTime workingDate, Inventory inventoryArea);
//        //void UndoProduce(Product prod, double quantity, UnitMeasure um, decimal cost, DateTime workingDate, Inventory inventoryArea);
//        //void RemoveProduction(Production p, bool undoChildProductionItems);
//    }

//    public class TransactionService : ITransactionService
//    {
//        MainWindowViewModel appvm;
//        public TransactionService(MainWindowViewModel appvm) 
//        {
//            this.appvm = appvm;
//        }

//        #region Sell

//        //public decimal Sell(Product prod, double quantity, DateTime workingDate)
//        //{
//        //    return Sell(prod, quantity, appvm.UnitMeasureManager.Unit, workingDate, 0);
//        //}
//        //private decimal Sell(Product prod, double quantity, UnitMeasure um, DateTime workingDate, int level)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be less or equal than zero");

//        //    decimal associated_cost = 0;

//        //    //remove ingredients from inventory, just consider first level ingredients
//        //    if (prod.IsRecipe && level == 0)
//        //    {
//        //        foreach (var ing in prod.Ingredients)
//        //        {
//        //            //Product ing_prod = ing.IngredientProduct;
//        //            //double ing_qtty = ing.Quantity * quantity;
//        //            //UnitMeasure ing_um = ing.UnitMeasure;

//        //            //decimal ing_cost = RemoveOldestCostTracesToReachQuantity(ing_prod, ing_qtty, ing_um, floorInventory);

//        //            //inventoryService.ExecuteInventoryOperation(workingDate, floorInventory, ing_prod, -ing_qtty, ing_um, ing_cost);

//        //            associated_cost += Sell(ing.IngredientProduct, ing.Quantity * quantity, ing.UnitMeasure, workingDate, level + 1);
//        //        }
//        //    }
//        //    else 
//        //    {
//        //        //Inventory floorInventory = appvm.InventoryAreasOC.Where(x => x.IsFloor).FirstOrDefault();
//        //        //if (floorInventory != null)
//        //        //    associated_cost = GetAverageCost(prod, quantity, um, floorInventory);
//        //    }

//        //    //-quantity
//        //    //if (prod.IsStorable)
//        //    //{
//        //    //    var inventoryService = appvm.InventoryService;

//        //    //    Inventory floorInventory = appvm.InventoryAreasOC.Where(x => x.IsFloor).FirstOrDefault();
//        //    //    if (floorInventory != null)
//        //    //        inventoryService.ExecuteInventoryOperation(workingDate, floorInventory, prod, -quantity, um, -associated_cost);
//        //    //}

//        //    return associated_cost;
//        //}

//        //public void UndoSell(Product prod, double quantity, decimal cost, DateTime workingDate)
//        //{
//        //    UndoSell(prod, quantity, appvm.UnitMeasureManager.Unit, workingDate, 0);
//        //}
//        //private void UndoSell(Product prod, double quantity, UnitMeasure um, DateTime workingDate, int level)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be less of equal than zero");

//        //    //var inventoryService = appvm.InventoryService;

//        //    //Inventory floorInventory = appvm.InventoryAreasOC.Where(x => x.IsFloor).FirstOrDefault();

//        //    //if (prod.IsStorable)
//        //    //{
//        //        //decimal cost = prod.PurchasePrice * (decimal)quantity;
//        //        //CreateCostTrace(prod, quantity, um, cost, workingDate, targetInventory);

                

//        //        //inventoryService.ExecuteInventoryOperation(workingDate, targetInventory,
//        //        //    prod, quantity, appvm.UnitMeasureManager.Unit, cost);
//        //    //}
            
//        //    //just consider first level ingredients
//        //    if (prod.IsRecipe && level == 0)
//        //    {
//        //        foreach (var ing in prod.Ingredients)
//        //        {
//        //            Product ing_prod = ing.IngredientProduct;
//        //            double ing_qtty = ing.Quantity * quantity;
//        //            UnitMeasure ing_um = ing.UnitMeasure;

//        //            //we are no keeping real cost of ingredients in a sell operation, just use the current cost
//        //            //decimal ing_cost = GetAverageCost(ing_prod, ing_qtty, ing_um, floorInventory);
//        //            //ing_cost = tbp.IngredientProduct.PurchasePrice * (decimal)(quantity * tbp.Quantity * tbp.UnitMeasure.ToBaseConversion);
//        //            UndoSell(ing_prod, ing_qtty, ing_um, workingDate, level + 1);
//        //        }
//        //    }
//        //    //+quantity

//        //    //if (prod.IsStorable)
//        //    //{
//        //    //    if (floorInventory != null)
//        //    //        InventoryIn(prod, quantity, um, cost, workingDate, floorInventory);
//        //    //}
            
//        //}

//        //decimal CalculateCost(Product product, double quantity, UnitMeasure um) 
//        //{
//        //    return (decimal)(quantity * um.ToBaseConversion) * product.CostPrice / (decimal)(product.CostQuantity * product.CostUnitMeasure.ToBaseConversion);
//        //}

//        #endregion

//        #region Purchase

//        //public void Purchase(Product prod, double quantity, UnitMeasure um, decimal price, 
//        //    DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be <= 0", "quantity");

//        //    //var inventoryService = appvm.InventoryService;

//        //    //create cost trace
//        //    //CreateCostTrace(prod, quantity, um, price, workingDate, inventoryArea);

//        //    //save last cost
//        //    //decimal last_cost = price / (decimal)(quantity * um.ToBaseConversion);

//        //    if (CostChanged(prod.CostQuantity, prod.CostUnitMeasure, prod.CostPrice, quantity, um, price)) 
//        //    {
//        //        prod.CostPrice = price;
//        //        prod.CostQuantity = quantity;
//        //        prod.CostUnitMeasure = um;

//        //        //if (prod.IsIngredient) 
//        //            RefreshRecipesCost(prod);
//        //    }

//        //    //if (last_cost != prod.PurchasePrice) prod.PurchasePrice = last_cost;

//        //    InventoryIn(prod, quantity, um, price, workingDate, inventoryArea);
                        
//        //    //if (prod.IsStorable)
//        //    //{
//        //    //    InventoryItem ii = inventoryService.GetInventoryItem(inventoryArea, prod);

//        //    //    decimal final_cost = price;

//        //    //    //adjust real cost, negative indicates product's lastcost was used
//        //    //    if (ii.Quantity < 0)                 
//        //    //    {
//        //    //        final_cost = CalculateCostWhenNegative(quantity, um, price, ii.Quantity, ii.Cost);
//        //    //    }

//        //    //    inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, quantity, um, final_cost);
//        //    //}
//        //}

//        bool CostChanged(double old_qtty, UnitMeasure old_um, decimal old_cost, 
//            double new_qtty, UnitMeasure new_um, decimal new_cost)
//        {
//            return (new_cost * (decimal)(old_qtty * old_um.ToBaseConversion)) !=
//                (old_cost * (decimal)(new_qtty * new_um.ToBaseConversion));
//        }

//        //void RefreshRecipesCost(Product product)
//        //{
            
//        //    foreach (var recipe_item in product.Outgredients)
//        //    {
//        //        Product recipe_product = recipe_item.BaseProduct;

//        //        if (!IsRecipe(recipe_product)) continue;

//        //        decimal total_cost = 0;

//        //        foreach (var ingredient_item in recipe_product.Ingredients)
//        //        {
//        //            //if (ingredient_item.IngredientProduct == null) continue;

//        //            total_cost += ProductManager.GetCurrentCost(ingredient_item.IngredientProduct, ingredient_item.Quantity, ingredient_item.UnitMeasure);

//        //            //total_cost += ingredient_item.IngredientProduct.PurchasePrice * (decimal)(item.Quantity * item.UnitMeasure.ToBaseConversion);
//        //        }

//        //        //recipe_product.CostQuantity = 1;
//        //        recipe_product.CostUnitMeasure = appvm.UnitMeasureManager.Unit;
//        //        recipe_product.CostPrice = total_cost;
//        //    }           

//        //    //recipe.CostQuantity = 1;
//        //    //recipe.CostUnitMeasure = appvm.UnitMeasureManager.Unit;//recipe.UMFamily.UnitMeasures.Single(x => x.IsFamilyBase);
//        //    //recipe.CostPrice = recipe.PurchasePrice;
//        //}

//        //public void UndoPurchase(Product prod, double quantity, UnitMeasure um, decimal price,
//        //    DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be <= 0", "quantity");

//        //    if (inventoryArea == null) return;

//        //    var inventoryService = appvm.InventoryService;

//        //    //RemoveMostRecentCostTracesToReachQuantity(prod, quantity, um, inventoryArea);

//        //    //-quantity, -price
//        //    if (prod.IsStorable)
//        //    {
//        //        inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, -quantity, um, -price);
//        //    }
//        //}        

//        #endregion

//        //#region Adjust

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <returns>the cost of the adjustment</returns>
//        //public decimal Adjust(Product prod, double quantity, UnitMeasure um, DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity == 0) throw new ArgumentException("Quantity can't be zero", "quantity");

//        //    var inventoryService = appvm.InventoryService;

//        //    decimal cost = 0;            
            
//        //    if (quantity > 0)
//        //    {
//        //        //use last product cost
                
//        //        //cost = prod.PurchasePrice * (decimal)(quantity * um.ToBaseConversion);
//        //        cost = GetAverageCost(prod, quantity, um, inventoryArea);

//        //        //CreateCostTrace(prod, quantity, um, cost, workingDate, inventoryArea);

//        //        InventoryIn(prod, quantity, um, cost, workingDate, inventoryArea);

//        //        //InventoryItem ii = inventoryService.GetInventoryItem(inventoryArea, prod);

//        //        //decimal adjusted_cost = cost;

//        //        ////adjust real cost, negative indicates product's lastcost was used
//        //        //if (ii.Quantity < 0)
//        //        //{
//        //        //    adjusted_cost = CalculateCostWhenNegative(quantity, um, cost, ii.Quantity, ii.Cost);
//        //        //}

//        //        //if (prod.IsStorable)
//        //        //{
//        //        //    inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, quantity, um, adjusted_cost);
//        //        //}
//        //    }
//        //    else 
//        //    {
//        //        //quantity < 0, search in cost traces

//        //        cost = GetAverageCost(prod, -quantity, um, inventoryArea);

//        //        if (prod.IsStorable)
//        //        {
//        //            inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, quantity, um, -cost);
//        //        }
//        //    }

//        //    return cost;
//        //}

//        //public void UndoAdjust(Product prod, double quantity, UnitMeasure um, decimal cost, DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity == 0) throw new ArgumentException("Quantity can't be zero", "quantity");

//        //    var inventoryService = appvm.InventoryService;

//        //    if (quantity < 0)
//        //    {
//        //        //CreateCostTrace(prod, -quantity, um, -cost, workingDate, inventoryArea);

//        //        InventoryIn(prod, -quantity, um, -cost, workingDate, inventoryArea);
//        //    }
//        //    else
//        //    {
//        //        //quantity > 0, search in cost traces
//        //        //RemoveMostRecentCostTracesToReachQuantity(prod, quantity, um, inventoryArea);
//        //        //RemoveOldestCostTracesToReachQuantity(prod, quantity, um, inventoryArea);

//        //        if (prod.IsStorable)
//        //        {
//        //            inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, -quantity, um, -cost);
//        //        }
//        //    }
//        //}

//        //public void RemoveAdjustment(Adjustment adj, bool undoChildAdjustmentItems)
//        //{
//        //    List<AdjustmentItem> items_toRemove = new List<AdjustmentItem>();

//        //    foreach (AdjustmentItem item in adj.LineItems)
//        //    {
//        //        items_toRemove.Add(item);
//        //    }

//        //    foreach (var item in items_toRemove)
//        //    {
//        //        if (undoChildAdjustmentItems) UndoAdjust(item.Product, item.Quantity, item.UnitMeasure, item.Cost, adj.Date, adj.Inventory);

//        //        appvm.Context.LineItems.DeleteObject(item);
//        //    }

//        //    //remove from database
//        //    appvm.Context.Orders.DeleteObject(adj);
//        //}

//        //#endregion

//        //#region Transfer

//        //public decimal Transfer(Product prod, double quantity, UnitMeasure um, DateTime workingDate, Inventory inv_Source, Inventory inv_Destination)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be <= 0", "quantity");

//        //    var inventoryService = appvm.InventoryService;

//        //    //take from source inventory area
//        //    decimal cost = GetAverageCost(prod, quantity, um, inv_Source);
            
//        //    if (prod.IsStorable)
//        //    {
//        //        //-quantity, -cost
//        //        inventoryService.ExecuteInventoryOperation(workingDate, inv_Source, prod, -quantity, um, -cost);
//        //    }

//        //    //add to target inventory area
//        //    //CreateCostTrace(prod, quantity, um, cost, workingDate, inv_Destination);

//        //    InventoryIn(prod, quantity, um, cost, workingDate, inv_Destination);

//        //    return cost;
//        //}

//        //public void UndoTransfer(Product prod, double quantity, UnitMeasure um, decimal cost,
//        //    DateTime workingDate, Inventory inv_Source, Inventory inv_Destination)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be <= 0", "quantity");

//        //    var inventoryService = appvm.InventoryService;

//        //    //take from destination inventory area
//        //    //decimal cost = GetAverageCost(prod, quantity, um, inv_Destination);

//        //    if (prod.IsStorable)
//        //    {
//        //        //-quantity, -cost
//        //        inventoryService.ExecuteInventoryOperation(workingDate, inv_Destination, prod, -quantity, um, -cost);
//        //    }

//        //    //add to target inventory area
//        //    //CreateCostTrace(prod, quantity, um, cost, workingDate, inv_Source);

//        //    InventoryIn(prod, quantity, um, cost, workingDate, inv_Source);
//        //}

//        //public void RemoveTransfer(Transfer trans, bool undoChildTransferItems)
//        //{
//        //    List<TransferItem> items_toRemove = new List<TransferItem>();

//        //    foreach (TransferItem item in trans.LineItems)
//        //    {
//        //        items_toRemove.Add(item);
//        //    }

//        //    foreach (var item in items_toRemove)
//        //    {
//        //        if (undoChildTransferItems) UndoTransfer(item.Product, item.Quantity, item.UnitMeasure, item.Cost,
//        //            trans.Date, trans.InventoryFrom, trans.InventoryTo);

//        //        appvm.Context.LineItems.DeleteObject(item);
//        //    }

//        //    //remove from database
//        //    appvm.Context.Orders.DeleteObject(trans);
//        //}

//        //#endregion

//        //#region Production

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <returns>the cost of production</returns>
//        //public decimal Produce(Product prod, double quantity, UnitMeasure um, DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be <= 0", "quantity");

//        //    var inventoryService = appvm.InventoryService;

//        //    decimal cost = 0;

//        //    //use last product cost
//        //    cost = ProductManager.GetCurrentCost(prod, quantity, um);
//        //    //cost = prod.PurchasePrice * (decimal)(quantity * um.ToBaseConversion);

//        //    //CreateCostTrace(prod, quantity, um, cost, workingDate, inventoryArea);

//        //    InventoryIn(prod, quantity, um, cost, workingDate, inventoryArea);

//        //    //if (prod.IsStorable)
//        //    //{
//        //    //    InventoryItem ii = inventoryService.GetInventoryItem(inventoryArea, prod);

//        //    //    decimal final_cost = cost;

//        //    //    //adjust real cost, negative indicates product's lastcost was used
//        //    //    if (ii.Quantity < 0)
//        //    //    {
//        //    //        final_cost = CalculateCostWhenNegative(quantity, um, cost, ii.Quantity, ii.Cost);
//        //    //    }

//        //    //    inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, quantity, um, final_cost);

//        //    //    //inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, quantity, um, cost);
//        //    //}

//        //    return cost;
//        //}

//        //public void UndoProduce(Product prod, double quantity, UnitMeasure um, decimal cost, DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("Product can't be null", "prod");

//        //    if (quantity <= 0) throw new ArgumentException("Quantity can't be <= 0", "quantity");

//        //    var inventoryService = appvm.InventoryService;
                        
//        //    //RemoveMostRecentCostTracesToReachQuantity(prod, quantity, um, inventoryArea);         

//        //    if (prod.IsStorable)
//        //    {
//        //        inventoryService.ExecuteInventoryOperation(workingDate, inventoryArea, prod, -quantity, um, -cost);
//        //    }
//        //}

//        //public void RemoveProduction(Production p, bool undoChildProductionItems)
//        //{
//        //    //remove production items (add them back to inventory)
//        //    List<ProductionItem> items_toRemove = new List<ProductionItem>();

//        //    foreach (ProductionItem item in p.LineItems)
//        //    {
//        //        items_toRemove.Add(item);
//        //    }

//        //    foreach (var item in items_toRemove)
//        //    {
//        //        if (undoChildProductionItems) UndoProduce(item.Product, item.Quantity, item.UnitMeasure, item.Cost, p.Date, p.Inventory);

//        //        appvm.Context.LineItems.DeleteObject(item);
//        //    }

//        //    //remove from database
//        //    appvm.Context.Orders.DeleteObject(p);
//        //}

//        //#endregion

//        //void InventoryIn(Product prod, double quantity, UnitMeasure um, decimal price, DateTime date, Inventory inventoryArea)
//        //{
//        //    if (!prod.IsStorable) return;

//        //    var inventoryService = appvm.InventoryService;            

//        //    decimal final_cost = price;

//        //    //double cost_trace_qtty = quantity;
//        //    //decimal cost_trace_price = price;

//        //    InventoryItem ii = inventoryService.GetInventoryItem(inventoryArea, prod);

//        //    //adjust real cost, negative indicates product's lastcost was used
//        //    if (ii != null && ii.Quantity < 0)
//        //    {
//        //        //price = CalculateCostWhenNegative(quantity, um, price, ii.Quantity, ii.Cost);
//        //        //since quantity in inventory is negative, make it positive
//        //        //double positive_qtty = -ii.Quantity;

//        //        //express quantity in base unit measure, which inventory uses
//        //        double qtty_in_baseUM = quantity * um.ToBaseConversion;

//        //        //don't take more than -ii.Quantity at old_unit_price
//        //        double quantity_at_old_unit_price = Math.Min(-ii.Quantity, qtty_in_baseUM);
//        //        //use cost in inventory, cost and qtty are < 0
//        //        final_cost = (decimal)quantity_at_old_unit_price * ii.Cost / (decimal)ii.Quantity;

//        //        //what's left
//        //        double quantity_at_new_unit_price = qtty_in_baseUM - quantity_at_old_unit_price;

//        //        //qtty to create cost trace, expressed in um
//        //        //cost_trace_qtty = quantity_at_new_unit_price / um.ToBaseConversion;

//        //        if (quantity_at_new_unit_price > 0)
//        //        {
//        //            //use price for the rest
//        //            //cost_trace_price = (decimal)quantity_at_new_unit_price * price / (decimal)qtty_in_baseUM;

//        //            final_cost += (decimal)quantity_at_new_unit_price * price / (decimal)qtty_in_baseUM;
//        //        }
//        //    }

//        //    inventoryService.ExecuteInventoryOperation(date, inventoryArea, prod, quantity, um, final_cost);

//        //    //if (cost_trace_qtty > 0)
//        //    //{
//        //    //    CreateCostTrace(prod, cost_trace_qtty, um, cost_trace_price, date, inventoryArea);
//        //    //}            
//        //}

//        //private void CreateCostTrace(Product prod, double quantity, UnitMeasure um, decimal price, 
//        //    DateTime workingDate, Inventory inventoryArea)
//        //{
//        //    CostTrace ct = new CostTrace();
//        //    //most of the time date will be now
//        //    ct.Date = workingDate.Date == DateTime.Today ? DateTime.Now : workingDate;
//        //    ct.Product = prod;
//        //    ct.Quantity = quantity;
//        //    ct.UnitMeasure = um;
//        //    ct.Cost = price;
//        //    ct.Inventory = inventoryArea;

//        //    appvm.Context.CostTraces.AddObject(ct);
//        //}        

//        //private decimal GetCostForSellOperation(Product prod, double qtty, UnitMeasure um, Inventory inventoryArea) 
//        //{
//        //    var oldest_cost_traces = from ct in appvm.Context.CostTraces
//        //                             where ct.Product.Id == prod.Id && ct.Inventory.Id == inventoryArea.Id
//        //                             orderby ct.Date
//        //                             select ct;
//        //    double qtty_left;

//        //    decimal total_cost = RemoveOldestCostTracesToReachQuantity_Sell(prod, qtty, um, inventoryArea, out qtty_left);

//        //    //use last cost when there are no more cost traces
//        //    if (qtty_left > 0)
//        //    {
//        //        if (!prod.IsRecipe)
//        //        {
//        //            total_cost += ProductManager.GetCurrentCost(prod, qtty_left, um);
//        //        }
//        //            //sum costs of ingredients
//        //        else
//        //        {
//        //            foreach (var tbp in prod.Ingredients)
//        //            {
//        //                total_cost += GetCostForSellOperation(tbp.IngredientProduct, tbp.Quantity * qtty_left, tbp.UnitMeasure, inventoryArea);
//        //            }
//        //        }
                
//        //        //total_cost += (decimal)(qtty_left * um.ToBaseConversion) * prod.PurchasePrice;
//        //    }

//        //    return total_cost;
//        //}


//        //decimal RemoveOldestCostTracesToReachQuantity_Sell(Product prod, double quantity, UnitMeasure um, Inventory inventoryArea, out double qtty_left)         
//        //{
//        //    var oldest_cost_traces = from ct in appvm.Context.CostTraces
//        //                             where ct.Product.Id == prod.Id && ct.Inventory.Id == inventoryArea.Id
//        //                             orderby ct.Date
//        //                             select ct;

//        //    qtty_left = quantity;//quantity to remove from cost trace
//        //    List<CostTrace> to_remove = new List<CostTrace>();

//        //    decimal total_cost = 0;
//        //    //decimal last_cost = prod.PurchasePrice;

//        //    foreach (var item in oldest_cost_traces)
//        //    {
//        //        if (item.EntityState == System.Data.EntityState.Deleted) continue;

//        //        if (qtty_left == 0) break;

//        //        //express qtty left in cost trace unit measure                
//        //        //double temp_qtty = qtty_left * um.ToBaseConversion / item.UnitMeasure.ToBaseConversion;
//        //        double temp_qtty = Convert(qtty_left, um, item.UnitMeasure);
//        //        decimal this_cost = (decimal)Math.Min(item.Quantity, temp_qtty) * item.Cost / (decimal)item.Quantity;
//        //        total_cost += this_cost;

//        //        //last_cost = item.Cost / (decimal)(item.Quantity * item.UnitMeasure.ToBaseConversion);

//        //        item.Quantity -= temp_qtty;
//        //        item.Cost -= this_cost;

//        //        //if qtty > 0 there was enough to take from
//        //        if (item.Quantity > 0) { qtty_left = 0; break; }

//        //        //quantity <= 0
//        //        to_remove.Add(item);
//        //        //qtty_left = Math.Abs(item.Quantity) * item.UnitMeasure.ToBaseConversion / um.ToBaseConversion;
//        //        qtty_left = Convert(Math.Abs(item.Quantity), item.UnitMeasure, um);
//        //    }

//        //    foreach (var item in to_remove)
//        //    {
//        //        appvm.Context.CostTraces.DeleteObject(item);
//        //    }

//        //    return total_cost;
//        //}

//        //private decimal GetAverageCost(Product prod, double quantity, UnitMeasure um, Inventory inventoryArea)
//        //{
//        //    if (prod == null) throw new ArgumentException("product can't be null, average cost");

//        //    if (inventoryArea != null)
//        //    {
//        //        var inventoryItem = (from ii in prod.CurrentExistence
//        //                             where ii.Inventory.Id == inventoryArea.Id
//        //                             select ii).FirstOrDefault();

//        //        if (inventoryItem != null && inventoryItem.Quantity > 0)
//        //        {
//        //            return inventoryItem.Cost * (decimal)quantity * (decimal)um.ToBaseConversion / (decimal)inventoryItem.Quantity;
//        //        }
//        //    }            

//        //    //if there is nothing in inventory, use purchase price
//        //    return ProductManager.GetCurrentCost(prod, quantity, um);
//        //}

//        /// <summary>
//        /// returns the total cost of selling product
//        /// </summary>
//        //private decimal RemoveOldestCostTracesToReachQuantity(Product prod, double quantity, UnitMeasure um, Inventory inventoryArea)
//        //{
//        //    var oldest_cost_traces = from ct in appvm.Context.CostTraces
//        //                             where ct.Product.Id == prod.Id && ct.Inventory.Id == inventoryArea.Id
//        //                             orderby ct.Date
//        //                             select ct;

//        //    double qtty_left = quantity;//quantity to remove from cost trace
//        //    List<CostTrace> to_remove = new List<CostTrace>();

//        //    decimal total_cost = 0;
//        //    decimal last_cost = prod.PurchasePrice;

//        //    foreach (var item in oldest_cost_traces)
//        //    {
//        //        if (item.EntityState == System.Data.EntityState.Deleted) continue;

//        //        if (qtty_left == 0) break;

//        //        express qtty left in cost trace unit measure                
//        //        double temp_qtty = qtty_left * um.ToBaseConversion / item.UnitMeasure.ToBaseConversion;
//        //        double temp_qtty = Convert(qtty_left, um, item.UnitMeasure);
//        //        decimal this_cost = (decimal)Math.Min(item.Quantity, temp_qtty) * item.Cost / (decimal)item.Quantity;
//        //        total_cost += this_cost;

//        //        last_cost = item.Cost / (decimal)(item.Quantity * item.UnitMeasure.ToBaseConversion);

//        //        item.Quantity -= temp_qtty;
//        //        item.Cost -= this_cost;

//        //        if qtty > 0 there was enough to take from
//        //        if (item.Quantity > 0) { qtty_left = 0; break; }

//        //        quantity <= 0
//        //        to_remove.Add(item);                
//        //        qtty_left = Math.Abs(item.Quantity) * item.UnitMeasure.ToBaseConversion / um.ToBaseConversion;
//        //        qtty_left = Convert(Math.Abs(item.Quantity), item.UnitMeasure, um);
//        //    }

//        //    foreach (var item in to_remove)
//        //    {
//        //        appvm.Context.CostTraces.DeleteObject(item);
//        //    }

//        //    update last used cost
//        //    if (last_cost != prod.PurchasePrice) prod.PurchasePrice = last_cost;

//        //    use current cost when there are no more cost traces
//        //    if (qtty_left > 0) 
//        //    {
//        //        total_cost += ProductManager.GetCurrentCost(prod, qtty_left, um);
//        //        total_cost += (decimal)(qtty_left * um.ToBaseConversion) * prod.PurchasePrice;
//        //    }

//        //    return total_cost;
//        //}

//        //private decimal RemoveMostRecentCostTracesToReachQuantity(Product prod, double quantity, UnitMeasure um, Inventory inventoryArea)
//        //{
//        //    var most_recent_cost_traces = from ct in appvm.Context.CostTraces
//        //                                  where ct.Product.Id == prod.Id && ct.Inventory.Id == inventoryArea.Id
//        //                                  orderby ct.Date descending
//        //                                  select ct;

//        //    double qtty_left = quantity;//quantity to remove from cost trace
//        //    List<CostTrace> to_remove = new List<CostTrace>();

//        //    decimal total_cost = 0;

//        //    foreach (var item in most_recent_cost_traces)
//        //    {
//        //        if (item.EntityState == System.Data.EntityState.Deleted) continue;

//        //        if (qtty_left == 0) break;

//        //        //express qtty left in cost trace unit measure
//        //        //double temp_qtty = qtty_left * um.ToBaseConversion / item.UnitMeasure.ToBaseConversion;
//        //        double temp_qtty = Convert(qtty_left, um, item.UnitMeasure);

//        //        decimal this_cost = (decimal)Math.Min(item.Quantity, temp_qtty) * item.Cost / (decimal)item.Quantity;
//        //        total_cost += this_cost;

//        //        item.Quantity -= temp_qtty;
//        //        item.Cost -= this_cost;

//        //        //if qtty > 0 there was enough to take from
//        //        if (item.Quantity > 0) { qtty_left = 0; break; }

//        //        //quantity <= 0
//        //        to_remove.Add(item);
//        //        qtty_left = Convert(Math.Abs(item.Quantity), item.UnitMeasure, um);
//        //        //qtty_left = Math.Abs(item.Quantity) * item.UnitMeasure.ToBaseConversion / um.ToBaseConversion;
//        //    }

//        //    foreach (var item in to_remove)
//        //    {
//        //        appvm.Context.CostTraces.DeleteObject(item);
//        //    }

//        //    if (qtty_left > 0)
//        //    {
//        //        total_cost += ProductManager.GetCurrentCost(prod, qtty_left, um);
//        //        //total_cost += (decimal)(qtty_left * um.ToBaseConversion) * prod.PurchasePrice;
//        //    }

//        //    return total_cost;
//        //}

//        //double Convert(double qtty, UnitMeasure sourceUM, UnitMeasure targetUM) 
//        //{
//        //    return qtty * sourceUM.ToBaseConversion / targetUM.ToBaseConversion;
//        //}
//        /// <summary>
//        /// adjusts cost of the operation when quantity in inventory is negative, meaning an asumption in cost was made
//        /// </summary>
//        /// <param name="qtty_to_add">quantity to add to inventory</param>
//        /// <param name="um_to_add">the unit of measure</param>
//        /// <param name="original_cost">original cost of the operation</param>
//        /// <param name="qtty_inventory">quantity in inventory</param>
//        /// <param name="cost_inventory">cost in inventory</param>
//        /// <returns></returns>
//        //private decimal CalculateCostWhenNegative(double qtty_to_add, UnitMeasure um_to_add, decimal original_cost, double qtty_inventory, decimal cost_inventory)
//        //{
//        //    if (qtty_inventory >= 0) throw new ArgumentException("Quantity in inventory must be < 0", "qtty_inventory");

//        //    decimal final_cost = 0;

//        //    double qtty_in_baseUM = qtty_to_add * um_to_add.ToBaseConversion;

//        //    //decimal old_unit_price = cost_inventory / (decimal)qtty_inventory;
//        //    //decimal new_unit_price = original_cost / (decimal)qtty_in_baseUM;

//        //    //don't take more than ii.Quantity at old_unit_price
//        //    double quantity_at_old_unit_price = Math.Min(-qtty_inventory, qtty_in_baseUM);
//        //    final_cost = (decimal)quantity_at_old_unit_price * cost_inventory / (decimal)qtty_inventory;

//        //    double quantity_at_new_unit_price = qtty_in_baseUM - quantity_at_old_unit_price;

//        //    if (quantity_at_new_unit_price > 0)
//        //    {
//        //        final_cost += (decimal)quantity_at_new_unit_price * original_cost / (decimal)qtty_in_baseUM;
//        //    }

//        //    return final_cost;
//        //}  
//    }
//}
