USE [DeCaminoDB]
GO

ALTER TABLE [dbo].[Products] DROP COLUMN [IsPurchasable]
GO

ALTER TABLE [dbo].[Products] DROP COLUMN [IsIngredient]
GO
