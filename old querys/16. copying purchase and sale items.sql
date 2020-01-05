USE RestaurantDB;
GO

--purchase items
INSERT INTO [dbo].[LineItems_PurchaseLineItem] 
(
    [Id],[Amount],[UnitPrice]
)
	SELECT li.Id,li.Price,li.Price/li.Quantity
	FROM [dbo].[LineItems] li
	     INNER JOIN [dbo].[Orders_Purchase] OP
	     ON li.[OrderId] = OP.[Id]
	WHERE li.Quantity != 0;
GO

INSERT INTO [dbo].[LineItems_PurchaseLineItem] 
(
    [Id],[Amount],[UnitPrice]
)
	SELECT li.Id,li.Price,0
	FROM [dbo].[LineItems] li
		INNER JOIN [dbo].[Orders_Purchase] OP
		ON li.[OrderId] = OP.[Id]
	WHERE li.Quantity = 0;
GO

--sale items
INSERT INTO [dbo].[LineItems_SaleLineItem] 
(
    [Id],[Amount],[UnitPrice],[Printed]
)
	SELECT li.Id,li.Price,li.Price/li.Quantity,ISNULL(li.Printed,0)
	FROM [dbo].[LineItems] li
		INNER JOIN [dbo].[Orders_Sale] OP
		ON li.[OrderId] = OP.[Id]
	WHERE li.Quantity != 0;
GO

INSERT INTO [dbo].[LineItems_SaleLineItem] 
(
    [Id],[Amount],[UnitPrice],[Printed]
)
	SELECT li.Id,li.Price,0,ISNULL(li.Printed,0)
	FROM [dbo].[LineItems] li
		INNER JOIN [dbo].[Orders_Sale] OP
		ON li.[OrderId] = OP.[Id]
	WHERE li.Quantity = 0;
GO

ALTER TABLE [dbo].[LineItems] DROP COLUMN [Printed]
GO

ALTER TABLE [dbo].[LineItems] DROP COLUMN [Price]
GO




