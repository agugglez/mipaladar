USE [DeCaminoDB];
GO

DELETE FROM [dbo].[LineItems_SaleLineItem]
WHERE Id IN 
    (SELECT [LineItems_SaleLineItem].Id FROM [dbo].[LineItems_SaleLineItem] INNER JOIN
              LineItems ON [LineItems_SaleLineItem].Id = LineItems.Id INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] >= '07.25.2015');
GO

DELETE FROM [dbo].[LineItems]
WHERE Id IN 
    (SELECT [LineItems].Id FROM [dbo].[LineItems] INNER JOIN
              Orders ON LineItems.Order_Id = Orders.Id
              WHERE Orders.[Date] >= '07.25.2015');
GO

DELETE FROM [dbo].[Orders_Sale]
WHERE Id IN 
    (SELECT [Orders_Sale].Id FROM [dbo].[Orders_Sale] INNER JOIN
              Orders ON [Orders_Sale].Id = Orders.Id
              WHERE Orders.[Date] >= '07.25.2015');
GO

DELETE FROM [dbo].[Orders]
WHERE Orders.[Date] >= '07.25.2015'
GO



