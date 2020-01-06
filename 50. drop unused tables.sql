--drop adjustments
USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Adjustment_inherits_Order]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Adjustment]'))
ALTER TABLE [dbo].[Orders_Adjustment] DROP CONSTRAINT [FK_Adjustment_inherits_Order]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AdjustmentInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Adjustment]'))
ALTER TABLE [dbo].[Orders_Adjustment] DROP CONSTRAINT [FK_AdjustmentInventory]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Orders_Adjustment]    Script Date: 01/04/2015 17:33:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders_Adjustment]') AND type in (N'U'))
DROP TABLE [dbo].[Orders_Adjustment]
GO

--drop productions
USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Production_inherits_Order]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Production]'))
ALTER TABLE [dbo].[Orders_Production] DROP CONSTRAINT [FK_Production_inherits_Order]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductionInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Production]'))
ALTER TABLE [dbo].[Orders_Production] DROP CONSTRAINT [FK_ProductionInventory]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Orders_Production]    Script Date: 01/04/2015 18:04:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders_Production]') AND type in (N'U'))
DROP TABLE [dbo].[Orders_Production]
GO

--drop transfer
USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transfer_inherits_Order]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Transfer]'))
ALTER TABLE [dbo].[Orders_Transfer] DROP CONSTRAINT [FK_Transfer_inherits_Order]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TransferFromInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Transfer]'))
ALTER TABLE [dbo].[Orders_Transfer] DROP CONSTRAINT [FK_TransferFromInventory]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TransferToInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Transfer]'))
ALTER TABLE [dbo].[Orders_Transfer] DROP CONSTRAINT [FK_TransferToInventory]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Orders_Transfer]    Script Date: 01/04/2015 18:05:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders_Transfer]') AND type in (N'U'))
DROP TABLE [dbo].[Orders_Transfer]
GO


--drop purchases
USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Purchase_inherits_Order]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Purchase]'))
ALTER TABLE [dbo].[Orders_Purchase] DROP CONSTRAINT [FK_Purchase_inherits_Order]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PurchaseInventory]') AND parent_object_id = OBJECT_ID(N'[dbo].[Orders_Purchase]'))
ALTER TABLE [dbo].[Orders_Purchase] DROP CONSTRAINT [FK_PurchaseInventory]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Orders_Pu__Total__41EDCAC5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Orders_Purchase] DROP CONSTRAINT [DF__Orders_Pu__Total__41EDCAC5]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Orders_Purchase]    Script Date: 01/04/2015 18:06:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders_Purchase]') AND type in (N'U'))
DROP TABLE [dbo].[Orders_Purchase]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AdjustmentItem_inherits_LineItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[LineItems_AdjustmentItem]'))
ALTER TABLE [dbo].[LineItems_AdjustmentItem] DROP CONSTRAINT [FK_AdjustmentItem_inherits_LineItem]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__LineItems___Cost__503BEA1C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LineItems_AdjustmentItem] DROP CONSTRAINT [DF__LineItems___Cost__503BEA1C]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[LineItems_AdjustmentItem]    Script Date: 01/04/2015 18:27:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LineItems_AdjustmentItem]') AND type in (N'U'))
DROP TABLE [dbo].[LineItems_AdjustmentItem]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductionItem_inherits_LineItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[LineItems_ProductionItem]'))
ALTER TABLE [dbo].[LineItems_ProductionItem] DROP CONSTRAINT [FK_ProductionItem_inherits_LineItem]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__LineItems___Cost__51300E55]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LineItems_ProductionItem] DROP CONSTRAINT [DF__LineItems___Cost__51300E55]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[LineItems_ProductionItem]    Script Date: 01/04/2015 18:27:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LineItems_ProductionItem]') AND type in (N'U'))
DROP TABLE [dbo].[LineItems_ProductionItem]
GO


USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TransferItem_inherits_LineItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[LineItems_TransferItem]'))
ALTER TABLE [dbo].[LineItems_TransferItem] DROP CONSTRAINT [FK_TransferItem_inherits_LineItem]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__LineItems___Cost__671F4F74]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[LineItems_TransferItem] DROP CONSTRAINT [DF__LineItems___Cost__671F4F74]
END

GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[LineItems_TransferItem]    Script Date: 01/04/2015 18:28:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LineItems_TransferItem]') AND type in (N'U'))
DROP TABLE [dbo].[LineItems_TransferItem]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PurchaseLineItem_inherits_LineItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[LineItems_PurchaseLineItem]'))
ALTER TABLE [dbo].[LineItems_PurchaseLineItem] DROP CONSTRAINT [FK_PurchaseLineItem_inherits_LineItem]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[LineItems_PurchaseLineItem]    Script Date: 01/04/2015 18:28:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LineItems_PurchaseLineItem]') AND type in (N'U'))
DROP TABLE [dbo].[LineItems_PurchaseLineItem]
GO

