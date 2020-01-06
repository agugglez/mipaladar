USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Products_ProductionAreas]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_ProductionAreas]
GO

USE [DeCaminoDB]
GO

/****** Object:  Index [IX_FK_Products_ProductionAreas]    Script Date: 01/09/2015 10:38:12 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = N'IX_FK_Products_ProductionAreas')
DROP INDEX [IX_FK_Products_ProductionAreas] ON [dbo].[Products] WITH ( ONLINE = OFF )
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [ProductionAreaId]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[ProductionAreas]    Script Date: 01/09/2015 10:37:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductionAreas]') AND type in (N'U'))
DROP TABLE [dbo].[ProductionAreas]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[TableSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Sale]'))
ALTER TABLE [dbo].[Orders_Sale] DROP CONSTRAINT [TableSale]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Orders_Sale] DROP COLUMN [Table_Id]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tables_PriceLists]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tables]'))
ALTER TABLE [dbo].[Tables] DROP CONSTRAINT [FK_Tables_PriceLists]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Tables]    Script Date: 01/09/2015 10:40:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tables]') AND type in (N'U'))
DROP TABLE [dbo].[Tables]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[PriceLists]    Script Date: 01/09/2015 10:42:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PriceLists]') AND type in (N'U'))
DROP TABLE [dbo].[PriceLists]
GO




