USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductsMinimumStockUnitMeasure]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_ProductsMinimumStockUnitMeasure]
GO

ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__Minimu__5E8A0973]
GO

/****** Object:  Index [IX_FK_ProductsMinimumStockUnitMeasure]    Script Date: 01/13/2015 15:05:51 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = N'IX_FK_ProductsMinimumStockUnitMeasure')
DROP INDEX [IX_FK_ProductsMinimumStockUnitMeasure] ON [dbo].[Products] WITH ( ONLINE = OFF )
GO

ALTER TABLE [dbo].[Products] DROP COLUMN [MinimumStockUMId]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__Minimu__5D95E53A]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [MinimumStock]
GO

