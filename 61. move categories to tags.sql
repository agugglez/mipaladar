USE DeCaminoDB;
GO

SET IDENTITY_INSERT [dbo].[Tags] ON
GO

INSERT INTO [dbo].[Tags]
           ([Id],[Name])
     
       SELECT [Id],[Name]
       FROM [dbo].[Categories]

GO

SET IDENTITY_INSERT [dbo].[Tags] OFF
GO

INSERT INTO [dbo].[ProductTag]
           ([Products_Id],[Tags_Id])
     
  SELECT [ProductId], [CategoryId]
  FROM [dbo].[ProductIndexes]
GO


USE DeCaminoDB;
GO

DELETE FROM [dbo].[ProductIndexes]
GO

DELETE FROM [dbo].[Categories]
GO

USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[CategoryProductIndex]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductIndexes]'))
ALTER TABLE [dbo].[ProductIndexes] DROP CONSTRAINT [CategoryProductIndex]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[ProductProductIndex]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductIndexes]'))
ALTER TABLE [dbo].[ProductIndexes] DROP CONSTRAINT [ProductProductIndex]
GO

USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[ProductIndexes]    Script Date: 01/21/2015 12:01:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductIndexes]') AND type in (N'U'))
DROP TABLE [dbo].[ProductIndexes]
GO


