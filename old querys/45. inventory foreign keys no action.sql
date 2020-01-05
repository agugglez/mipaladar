USE [DeCaminoDB];
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT

--productions
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Orders_Production
	DROP CONSTRAINT FK_ProductionInventory
GO
ALTER TABLE dbo.Inventories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Orders_Production ADD CONSTRAINT
	FK_ProductionInventory FOREIGN KEY
	(
	Inventory_Id
	) REFERENCES dbo.Inventories
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Orders_Production SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

--transfers
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Orders_Transfer
	DROP CONSTRAINT FK_TransferFromInventory
GO
ALTER TABLE dbo.Inventories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Orders_Transfer ADD CONSTRAINT
	FK_TransferFromInventory FOREIGN KEY
	(
	InventoryFrom_Id
	) REFERENCES dbo.Inventories
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Orders_Transfer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

--adjustments
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Orders_Adjustment
	DROP CONSTRAINT FK_AdjustmentInventory
GO
ALTER TABLE dbo.Inventories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/****** Object:  Index [IX_FK_AdjustmentInventory]    Script Date: 09/23/2013 22:09:31 ******/
DROP INDEX [IX_FK_AdjustmentInventory] ON [dbo].[Orders_Adjustment] WITH ( ONLINE = OFF )
GO

--not null inventory in adjustments
ALTER TABLE [dbo].[Orders_Adjustment] ALTER COLUMN [Inventory_Id] INT NOT NULL
GO

BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Orders_Adjustment] ADD CONSTRAINT
	FK_AdjustmentInventory FOREIGN KEY
	(
	[Inventory_Id]
	) REFERENCES dbo.Inventories
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.[Orders_Adjustment] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

CREATE INDEX [IX_FK_AdjustmentInventory]
ON [dbo].[Orders_Adjustment]
    ([Inventory_Id]);
GO
