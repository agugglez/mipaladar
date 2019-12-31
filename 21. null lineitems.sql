USE RestaurantDB;
GO

DELETE dbo.[LineItems_PurchaseLineItem]
	FROM dbo.[LineItems_PurchaseLineItem] AS sli, dbo.LineItems AS li
	WHERE sli.Id = li.Id AND li.Product_Id IS NULL;
GO

DELETE dbo.[LineItems_SaleLineItem]
	FROM dbo.[LineItems_SaleLineItem] AS sli, dbo.LineItems AS li
	WHERE sli.Id = li.Id AND li.Product_Id IS NULL;
GO

DELETE dbo.[LineItems_ProductionItem]
	FROM dbo.[LineItems_ProductionItem] AS sli, dbo.LineItems AS li
	WHERE sli.Id = li.Id AND li.Product_Id IS NULL;
GO

DELETE FROM dbo.LineItems 
WHERE Product_Id IS NULL;

ALTER TABLE dbo.LineItems ALTER COLUMN Product_Id INT NOT NULL
GO

ALTER TABLE dbo.Orders_Sale ALTER COLUMN Number INT NULL
GO



