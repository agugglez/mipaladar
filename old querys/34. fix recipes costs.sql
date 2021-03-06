USE [DeCaminoDB];
GO

/****** Script for SelectTopNRows command from SSMS  ******/
/*SELECT [Name]
      ,[IsRecipe]
      ,(SELECT ISNULL(CAST(SUM(Ingredients.Quantity * ingUM.ToBaseConversion * prods.CostPrice / (prods.CostQuantity * costUM.ToBaseConversion)) AS MONEY),0)
        FROM Ingredients INNER JOIN Products as prods ON prods.Id = Ingredients.IngredientProductId 
				     INNER JOIN UnitMeasures AS ingUM ON Ingredients.UnitMeasureId = ingUM.Id
				     INNER JOIN UnitMeasures AS costUM ON prods.CostUMId = costUM.Id
        WHERE BaseProductId = [Products].Id)
               
  FROM [dbo].[Products]
  WHERE IsRecipe = 1
  ORDER BY Name;*/
  
  
DELETE FROM [dbo].[InventoryItems]
WHERE Product_Id IN (SELECT Id FROM Products WHERE IsStorable = 0);
GO

DELETE FROM [dbo].[InventoryTraces]
WHERE ProductId IN (SELECT Id FROM Products WHERE IsStorable = 0);
GO

UPDATE [dbo].[Products]
SET [CostPrice] = (SELECT ISNULL(SUM(Ingredients.Quantity * ingUM.ToBaseConversion * prods.CostPrice / (prods.CostQuantity * costUM.ToBaseConversion)),0)
                   FROM Ingredients INNER JOIN Products as prods ON prods.Id = Ingredients.IngredientProductId 
                   			        INNER JOIN UnitMeasures AS ingUM ON Ingredients.UnitMeasureId = ingUM.Id
                   			        INNER JOIN UnitMeasures AS costUM ON prods.CostUMId = costUM.Id
                   WHERE BaseProductId = [Products].Id)
WHERE IsRecipe = 1;
GO

UPDATE [dbo].[InventoryItems]
SET [Cost] = ii.Quantity * (SELECT ISNULL(SUM(Ingredients.Quantity * ingUM.ToBaseConversion * Products.CostPrice / (Products.CostQuantity * costUM.ToBaseConversion)),0)
              FROM Ingredients INNER JOIN Products ON Products.Id = Ingredients.IngredientProductId 
				INNER JOIN UnitMeasures AS ingUM ON Ingredients.UnitMeasureId = ingUM.Id
				INNER JOIN UnitMeasures AS costUM ON Products.CostUMId = costUM.Id
              WHERE BaseProductId = ii.Product_Id)
FROM InventoryItems as ii INNER JOIN Products ON ii.Product_Id = Products.Id 
WHERE IsRecipe = 1;
GO