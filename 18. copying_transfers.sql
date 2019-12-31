USE RestaurantDB;
GO

--transfers
ALTER TABLE [dbo].[Orders] ADD [OldTransferId] INT;
GO

INSERT INTO [dbo].[Orders]
		([OldTransferId],[Date],[DateCreated],[Memo],[Employee_Id])
	SELECT [Id] AS [OldTransferId],[Date],[Date],[Memo],[Responsible_Id]
	FROM [dbo].[Transfers];
GO	

INSERT INTO [dbo].[Orders_Transfer] 
(
    [Id],
    [InventoryFrom_Id],
    [InventoryTo_Id]
)
	SELECT O.Id, T.[InventoryFrom_Id], T.[InventoryTo_Id]
	FROM [dbo].[Orders] O
		INNER JOIN [dbo].[Transfers] T
		ON O.[OldTransferId] = T.[Id];
GO

--transfer items
ALTER TABLE [dbo].[LineItems] ADD [OldId] INT;
GO

INSERT INTO [dbo].[LineItems]
		([OldId],Quantity,[Product_Id],[UnitMeasure_Id],[Order_Id])
	SELECT AI.[Id] AS [OldId],Quantity,[Product_Id],[UnitMeasure_Id],O.Id
	FROM [dbo].[TransferItems] AI
		 INNER JOIN [dbo].[Orders] O
		 ON AI.[Transfer_Id] = O.[OldTransferId];
GO	

INSERT INTO [dbo].[LineItems_TransferItem] 
(
    [Id]
)
	SELECT O.Id
	FROM [dbo].[LineItems] O
		INNER JOIN [dbo].[TransferItems] T
		ON O.[OldId] = T.[Id];
GO
	
ALTER TABLE [dbo].[LineItems] DROP COLUMN [OldId];
GO

ALTER TABLE [dbo].[Orders] DROP COLUMN [OldTransferId];
GO

DROP TABLE [dbo].[TransferItems]
GO

DROP TABLE [dbo].[Transfers]
GO

