USE [RestaurantDB]
GO

ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF_Products_SalePrice]
GO
ALTER TABLE [dbo].[Products] ALTER COLUMN SalePrice DECIMAL (19, 4) NOT NULL;
GO

ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF_Products_PurchasePrice]
GO
ALTER TABLE [dbo].[Products] ALTER COLUMN PurchasePrice DECIMAL (19, 4) NOT NULL;
GO

ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__CostPr__75A278F5]
GO
ALTER TABLE [dbo].[Products] ALTER COLUMN CostPrice DECIMAL (19, 4) NOT NULL;
GO

ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__CostQu__76969D2E]
GO
ALTER TABLE [dbo].[Products] ALTER COLUMN [CostQuantity] FLOAT NOT NULL;
GO

ALTER TABLE [dbo].[Ingredients] ALTER COLUMN [Quantity] FLOAT NOT NULL ;
GO

ALTER TABLE [dbo].[UnitMeasures] ALTER COLUMN [ToBaseConversion] FLOAT NOT NULL ;
GO

ALTER TABLE [dbo].[InventoryItems] ALTER COLUMN [Quantity] FLOAT NOT NULL ;
GO
ALTER TABLE [dbo].[InventoryItems] ALTER COLUMN [MinimumQuantity] FLOAT NOT NULL ;
GO

ALTER TABLE [dbo].[InventoryTraces] ALTER COLUMN [Quantity] FLOAT NOT NULL ;
GO

ALTER TABLE [dbo].[LineItems] ALTER COLUMN [Quantity] FLOAT NOT NULL ;
GO

ALTER TABLE [dbo].[Orders_Sale] ALTER COLUMN [Discount] DECIMAL (19, 4) NOT NULL ;
GO

ALTER TABLE [dbo].[Orders_Sale] ALTER COLUMN [Cash] DECIMAL (19, 4) NOT NULL ;
GO