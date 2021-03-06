USE [RestaurantDB]
GO

--produced product
ALTER TABLE [dbo].[Productions] DROP COLUMN [ProducedQuantity]
GO

DROP INDEX [IX_FK_ProductionProduct] ON [dbo].[Productions] WITH ( ONLINE = OFF )
GO

DROP INDEX [IX_FK_ProductionUnitMeasure] ON [dbo].[Productions] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[Productions] DROP CONSTRAINT [FK_ProductionUnitMeasure]
GO

ALTER TABLE [dbo].[Productions] DROP CONSTRAINT [FK_ProductionProduct]
GO

ALTER TABLE [dbo].[Productions] DROP COLUMN [ProducedUnitMeasure_Id]
GO

ALTER TABLE [dbo].[Productions] DROP COLUMN [ProducedProduct_Id]
GO

--destination inventory
DROP INDEX [IX_FK_ProductionDestinationInventory] ON [dbo].[Productions] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[Productions] DROP CONSTRAINT [FK_ProductionDestinationInventory]
GO

ALTER TABLE [dbo].[Productions] DROP COLUMN [DestinationInventory_Id]
GO

--recipe quantity
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__Recipe__797309D9]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [RecipeQuantity]
GO

DROP INDEX [IX_FK_RecipeUnitMeasure] ON [dbo].[Products] WITH ( ONLINE = OFF )
GO
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_RecipeUnitMeasure]
GO
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__Recipe__7A672E12]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [RecipeUnitMeasure_Id]
GO

