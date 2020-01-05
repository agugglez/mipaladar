USE RestaurantDB;
GO

--add memo to orders and copy title from purchases
ALTER TABLE [Orders] ADD [Memo] nvarchar(max);
GO

UPDATE Orders
SET Orders.Memo = Orders_Purchase.Title
FROM Orders INNER JOIN
	 Orders_Purchase ON Orders.Id = Orders_Purchase.Id;
GO

ALTER TABLE [dbo].[Orders] DROP COLUMN [TheNotes]
GO

--drop purchases.title
ALTER TABLE [dbo].[Orders_Purchase] DROP COLUMN [Title]
GO
--drop notes
ALTER TABLE [dbo].[Orders_Purchase] DROP COLUMN [Notes]
GO

ALTER TABLE [dbo].[Orders_Purchase] DROP CONSTRAINT [PurchasePurchaseType]
GO

ALTER TABLE [dbo].[Orders_Purchase] DROP COLUMN [PurchaseTypeId]
GO

DROP TABLE [dbo].[PurchaseTypes]
GO

--add total to purchases and copy from orders.totalprice
ALTER TABLE [dbo].[Orders_Purchase] ADD [Total] decimal(19,4) NOT NULL DEFAULT 0;
GO

UPDATE Orders_Purchase
SET Orders_Purchase.Total = Orders.TotalPrice
FROM Orders INNER JOIN
	 Orders_Purchase ON Orders.Id = Orders_Purchase.Id;
GO

--add total to sales and copy from orders.totalprice
ALTER TABLE [dbo].[Orders_Sale] ADD [Total] decimal(19,4) NOT NULL DEFAULT 0;
GO

UPDATE [Orders_Sale]
SET [Orders_Sale].Total = Orders.TotalPrice
FROM Orders INNER JOIN
	 [Orders_Sale] ON Orders.Id = [Orders_Sale].Id;
GO

--add subtotal to sales and sum lineitems price
ALTER TABLE [dbo].[Orders_Sale] ADD [SubTotal] decimal(19,4) NOT NULL DEFAULT 0;
GO

UPDATE [Orders_Sale]
SET [Orders_Sale].SubTotal = ISNULL((SELECT CAST(SUM(Price) AS MONEY) 
                                     FROM LineItems 
                                     WHERE LineItems.OrderId = [Orders_Sale].Id),0)
GO

--add closed to sales and copy from orders.isclosed
ALTER TABLE [dbo].[Orders_Sale] ADD [Closed] BIT NOT NULL DEFAULT 0;
GO

UPDATE [Orders_Sale]
SET [Orders_Sale].Closed = Orders.IsClosed
FROM Orders INNER JOIN
	 [Orders_Sale] ON Orders.Id = [Orders_Sale].Id;
GO

ALTER TABLE [dbo].[Orders] DROP COLUMN [TotalPrice]
GO

ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF_Orders_IsClosed]
GO

ALTER TABLE [dbo].[Orders] DROP COLUMN [IsClosed]
GO

--add number to sales and copy from orders.thenumber
ALTER TABLE [dbo].[Orders_Sale] ADD [Number] INT NOT NULL DEFAULT 0;
GO

UPDATE [Orders_Sale]
SET [Orders_Sale].Number = Orders.TheNumber
FROM Orders INNER JOIN
	 [Orders_Sale] ON Orders.Id = [Orders_Sale].Id;
GO

ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [DF_Orders_TheNumber]
GO
ALTER TABLE [dbo].[Orders] DROP COLUMN [TheNumber]
GO
