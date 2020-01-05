USE [DeCaminoDB]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleDefinitionPermission_Permission]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleDefinitionPermission]'))
ALTER TABLE [dbo].[RoleDefinitionPermission] DROP CONSTRAINT [FK_RoleDefinitionPermission_Permission]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleDefinitionPermission_RoleDefinition]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleDefinitionPermission]'))
ALTER TABLE [dbo].[RoleDefinitionPermission] DROP CONSTRAINT [FK_RoleDefinitionPermission_RoleDefinition]
GO

/****** Object:  Table [dbo].[RoleDefinitionPermission]    Script Date: 07/13/2013 19:59:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleDefinitionPermission]') AND type in (N'U'))
DROP TABLE [dbo].[RoleDefinitionPermission]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermission_Permission]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolePermission]'))
ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [FK_RolePermission_Permission]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermission_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolePermission]'))
ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [FK_RolePermission_Role]
GO

/****** Object:  Table [dbo].[RolePermission]    Script Date: 07/13/2013 19:59:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolePermission]') AND type in (N'U'))
DROP TABLE [dbo].[RolePermission]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleEmployee]') AND parent_object_id = OBJECT_ID(N'[dbo].[Roles]'))
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [FK_RoleEmployee]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleRoleDefinition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Roles]'))
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [FK_RoleRoleDefinition]
GO

/****** Object:  Table [dbo].[Roles]    Script Date: 07/13/2013 19:59:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
DROP TABLE [dbo].[Roles]
GO

/****** Object:  Table [dbo].[RoleDefinitions]    Script Date: 07/13/2013 19:59:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleDefinitions]') AND type in (N'U'))
DROP TABLE [dbo].[RoleDefinitions]
GO