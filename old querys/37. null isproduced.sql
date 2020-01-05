USE DeCaminoDB;
GO

UPDATE [dbo].[Products]
   SET [IsProduced] = 0
 WHERE [IsProduced] IS NULL
GO

UPDATE [dbo].[Products]
   SET [IsEntrant] = 0
 WHERE [IsEntrant] IS NULL
GO

SELECT * FROM Products
WHERE IsProduced IS NULL

ALTER TABLE dbo.Products ALTER COLUMN IsProduced BIT NOT NULL
GO

ALTER TABLE dbo.Products ALTER COLUMN [IsEntrant] BIT NOT NULL
GO


