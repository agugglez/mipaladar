USE [RestaurantDB]
GO
ALTER TABLE [dbo].[LineItems_SaleLineItem] DROP COLUMN [UnitPrice]
GO
ALTER TABLE [dbo].[LineItems_PurchaseLineItem] DROP COLUMN [UnitPrice]
GO
