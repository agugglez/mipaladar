USE [DeCaminoDB];
GO
/****** Script for SelectTopNRows command from SSMS  
SELECT [Quantity]
      ,[InventoryId]
      ,[Cost]
      ,[Name]
  FROM [dbo].[InventoryItems] JOIN
		Products ON Product_Id = Products.Id
  WHERE Cost < 0 AND Quantity >= 0
  ORDER BY Name******/
  
UPDATE [dbo].[InventoryItems]
SET [Cost] = ii.Quantity * Products.CostPrice / (Products.CostQuantity * um.ToBaseConversion)
FROM InventoryItems AS ii INNER JOIN Products ON 
	 ii.Product_Id = Products.Id INNER JOIN UnitMeasures AS um ON
	 Products.CostUMId = um.Id
WHERE Cost < 0 AND Quantity >= 0
GO