USE [DeCaminoDB]
GO

--remove unused permissions
ALTER TABLE [dbo].[Roles] DROP COLUMN [CanDesignRestaurant]
GO

ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeFollowProductReport]
GO

ALTER TABLE [dbo].[Roles] DROP COLUMN [CanSeeDayAveragesReport]
GO

--new permissions

ALTER TABLE [dbo].[Roles] ADD [CanSeeReports] BIT NOT NULL DEFAULT 0
GO

ALTER TABLE [dbo].[Roles] ADD [CanSeeRoles] BIT NOT NULL DEFAULT 0
GO

UPDATE [dbo].[Roles]
   SET [CanSeeReports] = 1
      ,[CanSeeRoles] = 1
 WHERE [Name]='Administrador'
GO