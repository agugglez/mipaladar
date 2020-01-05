USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeOldSales]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeePurchases]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanRemovePurchases]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeOldPurchases]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeMiPaladar]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeSalesReport]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeSalesByItemReport]
GO

USE [DeCaminoDB]
GO
ALTER TABLE dbo.Roles ADD CanSeeDashboard bit NOT NULL DEFAULT 1
GO
