/*
   miércoles, 28 de junio de 201716:31:08
   User: 
   Server: DESKTOP-I96V8O9
   Database: DeCaminoDB
   Application: 
*/

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
ALTER TABLE dbo.Products ADD
	PrintString nvarchar(MAX) NULL
GO
ALTER TABLE dbo.Products SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

UPDATE [dbo].[Products]
   SET [PrintString] = [Name]
GO