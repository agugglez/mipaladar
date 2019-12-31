USE RestaurantDB;
GO

UPDATE [dbo].[InventoryItems]
SET [Cost] = ii.Quantity * Products.CostPrice / (Products.CostQuantity * um.ToBaseConversion)
FROM InventoryItems as ii INNER JOIN Products ON 
	 ii.Product_Id = Products.Id INNER JOIN UnitMeasures as um ON
	 Products.CostUMId = um.Id
WHERE IsRecipe = 0;
GO

UPDATE [dbo].[InventoryItems]
SET [Cost] = [InventoryItems].Quantity * (SELECT ISNULL(SUM(Ingredients.Quantity * ingUM.ToBaseConversion * Products.CostPrice / (Products.CostQuantity * costUM.ToBaseConversion)),0)
              FROM Ingredients INNER JOIN Products ON Products.Id = Ingredients.IngredientProductId 
				INNER JOIN UnitMeasures AS ingUM ON Ingredients.UnitMeasureId = ingUM.Id
				INNER JOIN UnitMeasures AS costUM ON Products.CostUMId = costUM.Id
              WHERE BaseProductId = ii.Product_Id)
FROM InventoryItems as ii INNER JOIN Products ON ii.Product_Id = Products.Id 
WHERE IsRecipe = 1;
GO

ALTER TABLE [dbo].[Products] DROP COLUMN [PurchasePrice]
GO

--put some default cost in sale line items

UPDATE [dbo].[LineItems_SaleLineItem]
   SET [Cost] = li.Quantity * prod.CostPrice / (prod.CostQuantity * um.ToBaseConversion)
  FROM [dbo].[LineItems_SaleLineItem] as sli 
       INNER JOIN LineItems AS li ON sli.Id = li.Id
	   INNER JOIN Products AS prod ON li.Product_Id = prod.Id 
	   INNER JOIN UnitMeasures AS um ON prod.CostUMId = um.Id
 WHERE prod.IsRecipe = 0
GO

UPDATE [dbo].[LineItems_SaleLineItem]
SET [Cost] = li.Quantity * (SELECT ISNULL(SUM(Ingredients.Quantity * ingUM.ToBaseConversion * Products.CostPrice / (Products.CostQuantity * costUM.ToBaseConversion)),0)
                FROM Ingredients INNER JOIN Products ON Products.Id = Ingredients.IngredientProductId 
				     INNER JOIN UnitMeasures AS ingUM ON Ingredients.UnitMeasureId = ingUM.Id
				     INNER JOIN UnitMeasures AS costUM ON Products.CostUMId = costUM.Id
               WHERE BaseProductId = li.Product_Id)
FROM [dbo].[LineItems_SaleLineItem] as sli 
     INNER JOIN LineItems AS li ON sli.Id = li.Id
	 INNER JOIN Products AS prod ON li.Product_Id = prod.Id 
WHERE prod.IsRecipe = 1;
GO
