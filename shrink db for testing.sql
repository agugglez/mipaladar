USE [RestaurantDB];
GO

DELETE FROM [dbo].[LineItems_SaleLineItem]
WHERE Id IN 
    (SELECT [LineItems_SaleLineItem].Id FROM [dbo].[LineItems_SaleLineItem] INNER JOIN
              LineItems ON [LineItems_SaleLineItem].Id = LineItems.Id INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[LineItems_PurchaseLineItem]
WHERE Id IN 
    (SELECT [LineItems_PurchaseLineItem].Id FROM [dbo].[LineItems_PurchaseLineItem] INNER JOIN
              LineItems ON [LineItems_PurchaseLineItem].Id = LineItems.Id INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[LineItems_AdjustmentItem]
WHERE Id IN 
    (SELECT [LineItems_AdjustmentItem].Id FROM [dbo].[LineItems_AdjustmentItem] INNER JOIN
              LineItems ON [LineItems_AdjustmentItem].Id = LineItems.Id INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[LineItems_ProductionItem]
WHERE Id IN 
    (SELECT [LineItems_ProductionItem].Id FROM [dbo].[LineItems_ProductionItem] INNER JOIN
              LineItems ON [LineItems_ProductionItem].Id = LineItems.Id INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[LineItems_TransferItem]
WHERE Id IN 
    (SELECT [LineItems_TransferItem].Id FROM [dbo].[LineItems_TransferItem] INNER JOIN
              LineItems ON [LineItems_TransferItem].Id = LineItems.Id INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[LineItems]
WHERE Id IN 
    (SELECT [LineItems].Id FROM [dbo].[LineItems] INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[Orders_Sale]
WHERE Id IN 
    (SELECT [Orders_Sale].Id FROM [dbo].[Orders_Sale] INNER JOIN
              Orders ON [Orders_Sale].Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[Orders_Purchase]
WHERE Id IN 
    (SELECT [Orders_Purchase].Id FROM [dbo].[Orders_Purchase] INNER JOIN
              Orders ON [Orders_Purchase].Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[Orders_Adjustment]
WHERE Id IN 
    (SELECT [Orders_Adjustment].Id FROM [dbo].[Orders_Adjustment] INNER JOIN
              Orders ON [Orders_Adjustment].Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[Orders_Production]
WHERE Id IN 
    (SELECT [Orders_Production].Id FROM [dbo].[Orders_Production] INNER JOIN
              Orders ON [Orders_Production].Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[Orders_Transfer]
WHERE Id IN 
    (SELECT [Orders_Transfer].Id FROM [dbo].[Orders_Transfer] INNER JOIN
              Orders ON [Orders_Transfer].Id = Orders.Id
              WHERE Orders.[Date] < '09.01.2013');
GO

DELETE FROM [dbo].[Orders]
WHERE Orders.[Date] < '09.01.2013'
GO

DELETE FROM [dbo].[InventoryTraces]
      WHERE [Date] < '09.01.2013'
GO



