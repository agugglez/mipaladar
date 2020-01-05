USE [DeCaminoDB];
GO

ALTER TABLE [dbo].[Orders_Sale] ADD [Tips] decimal(19,4) NOT NULL DEFAULT 0;
GO

ALTER TABLE [dbo].[Orders_Sale] ADD [TotalCost] decimal(19,4) NOT NULL DEFAULT 0;
GO

UPDATE [dbo].[Orders_Sale] 
SET [TotalCost] = (SELECT ISNULL(SUM(Cost),0)
                   FROM [LineItems_SaleLineItem] AS sli INNER JOIN LineItems AS li ON sli.Id = li.Id
                   WHERE [Orders_Sale].Id = li.Order_Id)
GO