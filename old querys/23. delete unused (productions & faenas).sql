USE [RestaurantDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaFaenaItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[FaenaItems]'))
ALTER TABLE [dbo].[FaenaItems] DROP CONSTRAINT [FK_FaenaFaenaItem]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaItemProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[FaenaItems]'))
ALTER TABLE [dbo].[FaenaItems] DROP CONSTRAINT [FK_FaenaItemProduct]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaItemUnitMeasure]') AND parent_object_id = OBJECT_ID(N'[dbo].[FaenaItems]'))
ALTER TABLE [dbo].[FaenaItems] DROP CONSTRAINT [FK_FaenaItemUnitMeasure]
GO

USE [RestaurantDB]
GO

/****** Object:  Table [dbo].[FaenaItems]    Script Date: 06/23/2013 15:49:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FaenaItems]') AND type in (N'U'))
DROP TABLE [dbo].[FaenaItems]
GO

USE [RestaurantDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaDestinationInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Faenas]'))
ALTER TABLE [dbo].[Faenas] DROP CONSTRAINT [FK_FaenaDestinationInventory]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaEmployee]') AND parent_object_id = OBJECT_ID(N'[dbo].[Faenas]'))
ALTER TABLE [dbo].[Faenas] DROP CONSTRAINT [FK_FaenaEmployee]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Faenas]'))
ALTER TABLE [dbo].[Faenas] DROP CONSTRAINT [FK_FaenaInventory]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[Faenas]'))
ALTER TABLE [dbo].[Faenas] DROP CONSTRAINT [FK_FaenaProduct]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FaenaUnitMeasure]') AND parent_object_id = OBJECT_ID(N'[dbo].[Faenas]'))
ALTER TABLE [dbo].[Faenas] DROP CONSTRAINT [FK_FaenaUnitMeasure]
GO

USE [RestaurantDB]
GO

/****** Object:  Table [dbo].[Faenas]    Script Date: 06/23/2013 15:49:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Faenas]') AND type in (N'U'))
DROP TABLE [dbo].[Faenas]
GO

USE [RestaurantDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductionItemProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductionItems]'))
ALTER TABLE [dbo].[ProductionItems] DROP CONSTRAINT [FK_ProductionItemProduct]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductionItemProduction]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductionItems]'))
ALTER TABLE [dbo].[ProductionItems] DROP CONSTRAINT [FK_ProductionItemProduction]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductionItemUnitMeasure]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductionItems]'))
ALTER TABLE [dbo].[ProductionItems] DROP CONSTRAINT [FK_ProductionItemUnitMeasure]
GO

USE [RestaurantDB]
GO

/****** Object:  Table [dbo].[ProductionItems]    Script Date: 06/23/2013 15:50:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductionItems]') AND type in (N'U'))
DROP TABLE [dbo].[ProductionItems]
GO

USE [RestaurantDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductionEmployee]') AND parent_object_id = OBJECT_ID(N'[dbo].[Productions]'))
ALTER TABLE [dbo].[Productions] DROP CONSTRAINT [FK_ProductionEmployee]
GO

USE [RestaurantDB]
GO

/****** Object:  Table [dbo].[Productions]    Script Date: 06/23/2013 15:49:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Productions]') AND type in (N'U'))
DROP TABLE [dbo].[Productions]
GO

USE [RestaurantDB]
GO

/****** Object:  Table [dbo].[UserPasswords]    Script Date: 06/23/2013 15:50:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPasswords]') AND type in (N'U'))
DROP TABLE [dbo].[UserPasswords]
GO







