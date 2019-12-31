USE [DeCaminoDB];
GO

ALTER TABLE [dbo].[LineItems_TransferItem] ADD [Cost] decimal(19,4) NOT NULL DEFAULT 0;
GO

BEGIN TRANSACTION

	UPDATE [dbo].[LineItems_TransferItem]
	   SET [Cost] = li.Quantity * um.ToBaseConversion * prod.CostPrice / (prod.CostQuantity * costUM.ToBaseConversion)
	  FROM [dbo].[LineItems_TransferItem] as sli 
		   INNER JOIN LineItems AS li ON sli.Id = li.Id
		   INNER JOIN Products AS prod ON li.Product_Id = prod.Id 
		   INNER JOIN UnitMeasures AS costUM ON prod.CostUMId = costUM.Id
		   INNER JOIN UnitMeasures AS um ON li.UnitMeasure_Id = um.Id
	GO

COMMIT