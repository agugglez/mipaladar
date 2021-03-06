/*
   Saturday, July 04, 201510:27:01 AM
   User: 
   Server: AGOSTINO-PC
   Database: LaDivinaPastora
   Application: 
*/

USE [LaDivinaPastora];
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
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Products
	DROP CONSTRAINT FK_ProductCategory
GO
ALTER TABLE dbo.Categories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Products ADD CONSTRAINT
	FK_ProductCategory FOREIGN KEY
	(
	CategoryId
	) REFERENCES dbo.Categories
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  SET NULL 
	
GO
ALTER TABLE dbo.Products SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
