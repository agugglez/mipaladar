USE DeCaminoDB;
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


UPDATE [dbo].[Orders_Sale] 
SET [TotalCost] = (SELECT ISNULL(SUM(Cost),0)
                   FROM [LineItems_SaleLineItem] AS sli INNER JOIN LineItems AS li ON sli.Id = li.Id
                   WHERE [Orders_Sale].Id = li.Order_Id)
GO