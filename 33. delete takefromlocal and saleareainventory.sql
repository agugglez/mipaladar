USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [DF__Products__TakeFr__47A6A41B]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [TakeFromLocal]
GO
