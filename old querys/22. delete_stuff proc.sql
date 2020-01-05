USE RestaurantDB;
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
