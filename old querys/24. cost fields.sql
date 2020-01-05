USE RestaurantDB;
GO

ALTER TABLE [LineItems_SaleLineItem] ADD [Cost] decimal(19,4) NOT NULL DEFAULT 0;
GO

ALTER TABLE [LineItems_AdjustmentItem] ADD [Cost] decimal(19,4) NOT NULL DEFAULT 0;
GO

ALTER TABLE [LineItems_ProductionItem] ADD [Cost] decimal(19,4) NOT NULL DEFAULT 0;
GO

ALTER TABLE [InventoryItems] ADD [Cost] decimal(19,4) NOT NULL DEFAULT 0;
GO

UPDATE [dbo].[UnitMeasures]
   SET [ToBaseConversion] = 453.592
 WHERE Caption = 'lb'
GO