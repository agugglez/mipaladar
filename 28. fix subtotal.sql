USE RestaurantDB;
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


UPDATE [Orders_Sale]
SET SubTotal = Total
WHERE SubTotal != Total AND Discount = 0
GO

UPDATE [Orders_Sale]
SET [Orders_Sale].SubTotal = ISNULL((SELECT CAST(SUM(sli.Amount) AS MONEY) 
                                     FROM LineItems_SaleLineItem AS sli INNER JOIN LineItems AS li ON sli.Id = li.Id
                                     WHERE li.Order_Id = [Orders_Sale].Id),0)
WHERE Discount != 0
GO