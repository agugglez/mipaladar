USE [DeCaminoDB]
GO

--Unit Measurre Families
INSERT INTO [dbo].[UMFamilies] ([Name])
     VALUES (N'Quantity')
GO

INSERT INTO [dbo].[UMFamilies] ([Name])
     VALUES (N'Weight')
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

INSERT INTO [dbo].[UMFamilies]
           ([Name])
     VALUES
           ('Volume')
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

INSERT INTO [dbo].[Inventories]
           ([Name]
           ,[IsFloor])
     VALUES
           (N'Piso'
           ,1)
GO

INSERT INTO [dbo].[Inventories]
           ([Name]
           ,[IsFloor])
     VALUES
           (N'Almacen'
           ,0)
GO

--admin role
INSERT INTO [dbo].[Roles]           
     VALUES
           (N'Administrador',1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1)
GO

--cashier role
INSERT INTO [dbo].[Roles]           
     VALUES
           (N'Cajero',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
GO

INSERT INTO [dbo].[Employees]
           ([Name]
           ,[IsActive]
           ,[CanPurchase]
           ,[CanSell]
           ,[Role_Id])
     VALUES
           ('admin'
           ,1
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
-- =============================================
-- Author:		agugglez
-- Create date: 6-23-2013
-- Description:	
-- =============================================
CREATE PROCEDURE delete_stuff 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	TRUNCATE TABLE [dbo].[InventoryTraces]

    -- Insert statements for procedure here
	TRUNCATE TABLE LineItems_AdjustmentItem
    TRUNCATE TABLE LineItems_TransferItem
    TRUNCATE TABLE LineItems_ProductionItem
    TRUNCATE TABLE LineItems_SaleLineItem
    TRUNCATE TABLE LineItems_PurchaseLineItem
    
    DELETE FROM [dbo].[LineItems]
    
    DELETE FROM [dbo].[Orders_Adjustment]
    DELETE FROM [dbo].[Orders_Transfer]
    DELETE FROM [dbo].[Orders_Production]
    DELETE FROM [dbo].[Orders_Sale]
    DELETE FROM [dbo].[Orders_Purchase]
    
    DELETE FROM [dbo].[Orders]    
END
GO