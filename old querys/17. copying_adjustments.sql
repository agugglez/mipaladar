USE RestaurantDB;
GO

EXEC sp_rename 'Orders.EmployeeId', 'Employee_Id', 'COLUMN';
GO

EXEC sp_rename 'Orders_Sale.TableId', 'Table_Id', 'COLUMN';
GO

--adjustments
ALTER TABLE [dbo].[Orders] ADD [OldAdjustmentId] INT;
GO

INSERT INTO [dbo].[Orders]
		([OldAdjustmentId],[Date],[DateCreated],[Memo],[Employee_Id])
	SELECT [Id] AS [OldAdjustmentId],[Date],[Date],[Memo],[Responsible_Id]
	FROM [dbo].[Adjustments];
GO	

INSERT INTO [dbo].[Orders_Adjustment] 
(
    [Id],
    [Inventory_Id]
)
	SELECT O.Id, T.[Inventory_Id]
	FROM [dbo].[Orders] O
		INNER JOIN [dbo].[Adjustments] T
		ON O.[OldAdjustmentId] = T.[Id];
GO

EXEC sp_rename 'LineItems.ProductId', 'Product_Id', 'COLUMN';
GO

EXEC sp_rename 'LineItems.UnitMeasureId', 'UnitMeasure_Id', 'COLUMN';
GO

EXEC sp_rename 'LineItems.OrderId', 'Order_Id', 'COLUMN';
GO

--adjustment items
ALTER TABLE [dbo].[LineItems] ADD [OldId] INT;
GO

INSERT INTO [dbo].[LineItems]
		([OldId],Quantity,[Product_Id],[UnitMeasure_Id],[Order_Id])
	SELECT AI.[Id] AS [OldId],Quantity,[Product_Id],[UnitMeasure_Id],O.Id
	FROM [dbo].[AdjustmentItems] AI
		 INNER JOIN [dbo].[Orders] O
		 ON AI.[Adjustment_Id] = O.[OldAdjustmentId];
GO	

INSERT INTO [dbo].[LineItems_AdjustmentItem] 
(
    [Id]
)
	SELECT O.Id
	FROM [dbo].[LineItems] O
		INNER JOIN [dbo].[AdjustmentItems] T
		ON O.[OldId] = T.[Id];
GO
	
ALTER TABLE [dbo].[LineItems] DROP COLUMN [OldId];
GO

ALTER TABLE [dbo].[Orders] DROP COLUMN [OldAdjustmentId];
GO

DROP TABLE [dbo].[AdjustmentItems]
GO

DROP TABLE [dbo].[Adjustments]
GO
