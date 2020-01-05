USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[CurrentExistenceProduct]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryItems]'))
ALTER TABLE [dbo].[InventoryItems] DROP CONSTRAINT [CurrentExistenceProduct]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryInventoryItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryItems]'))
ALTER TABLE [dbo].[InventoryItems] DROP CONSTRAINT [FK_InventoryInventoryItem]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Inventory__Inven__6EF57B66]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InventoryItems] DROP CONSTRAINT [DF__Inventory__Inven__6EF57B66]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__InventoryI__Cost__5224328E]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InventoryItems] DROP CONSTRAINT [DF__InventoryI__Cost__5224328E]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[InventoryItems]    Script Date: 01/04/2015 20:49:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryItems]') AND type in (N'U'))
DROP TABLE [dbo].[InventoryItems]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryTraceInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryTraces]'))
ALTER TABLE [dbo].[InventoryTraces] DROP CONSTRAINT [FK_InventoryTraceInventory]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[ProductExistence]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryTraces]'))
ALTER TABLE [dbo].[InventoryTraces] DROP CONSTRAINT [ProductExistence]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Inventory__Inven__6FE99F9F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[InventoryTraces] DROP CONSTRAINT [DF__Inventory__Inven__6FE99F9F]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[InventoryTraces]    Script Date: 01/04/2015 20:50:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryTraces]') AND type in (N'U'))
DROP TABLE [dbo].[InventoryTraces]
GO


USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Inventori__IsFlo__489AC854]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Inventories] DROP CONSTRAINT [DF__Inventori__IsFlo__489AC854]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Inventories]    Script Date: 01/04/2015 20:50:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Inventories]') AND type in (N'U'))
DROP TABLE [dbo].[Inventories]
GO