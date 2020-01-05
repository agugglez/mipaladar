USE DeCaminoDB;
GO

EXECUTE sp_rename N'dbo.Misc', N'Miscs', 'OBJECT' 
GO

USE DeCaminoDB;
GO

ALTER TABLE dbo.LineItems
	DROP CONSTRAINT ProductLineItem
GO

ALTER TABLE dbo.LineItems ADD CONSTRAINT
	ProductLineItem FOREIGN KEY
	(
	Product_Id
	) REFERENCES dbo.Products
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Employees] DROP COLUMN [CanPurchase]
GO

USE DeCaminoDB;
GO

ALTER TABLE dbo.Miscs ADD Capacity INT NOT NULL DEFAULT 0
GO

USE DeCaminoDB;
GO

ALTER TABLE dbo.Orders_Sale ADD Voids INT NOT NULL DEFAULT 0
GO

ALTER TABLE dbo.Orders_Sale ADD VoidsAfterReceipt INT NOT NULL DEFAULT 0
GO