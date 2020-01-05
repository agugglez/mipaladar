USE [LaDivinaPastora]
GO

--Unit Measurre Families
INSERT INTO [dbo].[UMFamilies] ([Name])
     VALUES (N'Quantity')
GO

INSERT INTO [dbo].[UMFamilies] ([Name])
     VALUES (N'Weight')
GO

INSERT INTO [dbo].[UMFamilies]
           ([Name])
     VALUES
           ('Volume')
GO

INSERT INTO [dbo].[UMFamilies]
           ([Name])
     VALUES
           ('Length')
GO

-- Inserting values into 'UnitMeasures'
INSERT INTO [dbo].[UnitMeasures]
           ([Caption],[UMFamilyId],[IsFamilyBase],[ToBaseConversion],[Name])
     VALUES (N'',1,1,1,N'U')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption],[UMFamilyId],[IsFamilyBase],[ToBaseConversion],[Name])
     VALUES
           (N'g',2,1,1,N'Gr')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption],[UMFamilyId],[IsFamilyBase],[ToBaseConversion],[Name])
     VALUES (N'kg',2,0,1000,N'Kg')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('lb'
           ,2
           ,0
           ,453.59229
           ,'Lb')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('ml'
           ,3
           ,1
           ,1
           ,'ml')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('L'
           ,3
           ,0
           ,1000
           ,'L')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('mm'
           ,4
           ,1
           ,1
           ,'mm')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('cm'
           ,4
           ,0
           ,10
           ,'cm')
GO

INSERT INTO [dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('m'
           ,4
           ,0
           ,1000
           ,'m')
GO

--admin role
INSERT INTO [dbo].[Roles]           
     VALUES
           ('Administrador',1,1,1,1,1,1,1,1,1,1,1,1,1,1,1)
GO

--cashier role
INSERT INTO [dbo].[Roles]           
     VALUES
           (N'Cajero',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0)
GO

INSERT INTO [dbo].[Employees]
           ([Name]
           ,[IsActive]
           ,[CanSell]
           ,[Role_Id])
     VALUES
           ('admin'
           ,1
           ,1
           ,1)
GO

-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO