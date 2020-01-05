USE [DeCaminoDB];
GO

ALTER TABLE [Orders_Sale] ADD [Tax] decimal(19,4) NOT NULL DEFAULT 0;
GO

ALTER TABLE [Orders_Sale] ADD [TaxInPercent] BIT NOT NULL DEFAULT 0;
GO

UPDATE [dbo].[Orders_Sale]
   SET [Tax] = -[Discount]
      ,[TaxInPercent] = [DiscountInPercent]
      ,[Discount] = 0
      ,[DiscountInPercent]= 0
 WHERE [Discount] < 0
GO

