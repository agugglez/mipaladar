USE [RestaurantDB];
GO

DELETE FROM [dbo].[ProductIndexes]
      WHERE [CategoryId] IS NULL OR [ProductId] IS NULL
GO

ALTER TABLE [dbo].[ProductIndexes] ALTER COLUMN [ProductId] INT NOT NULL
GO

ALTER TABLE [dbo].[ProductIndexes] ALTER COLUMN [CategoryId] INT NOT NULL
GO