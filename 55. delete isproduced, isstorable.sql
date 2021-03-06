USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [IsStorable]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [IsProduced]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [PrintString]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [IsEntrant]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [CostQuantity]
GO

USE DeCaminoDB;
GO

EXECUTE sp_rename N'dbo.Products.NotInMenu', N'IsInMenu', 'COLUMN' 
GO

UPDATE [dbo].[Products]
   SET [IsInMenu] = 1 - [IsInMenu]
      
GO

USE [DeCaminoDB];
GO

ALTER TABLE [dbo].[Products] ADD [ArbitraryCost] decimal(19,4) NOT NULL DEFAULT 0;
GO

UPDATE [dbo].[Products]
   SET [ArbitraryCost] = [CostPrice]
 WHERE [IsRecipe]=0
GO


