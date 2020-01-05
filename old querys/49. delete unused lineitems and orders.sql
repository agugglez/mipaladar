USE DeCaminoDB;
GO

--Delete all adjustment, production, transfer and purchase items
DELETE FROM LineItems_AdjustmentItem
GO

DELETE FROM LineItems_ProductionItem
GO

DELETE FROM LineItems_TransferItem
GO

DELETE FROM LineItems_PurchaseLineItem
GO

--delete lineitems that belong to Adjustments, Productions, Transfers and Purchases
DELETE FROM LineItems
WHERE Order_Id IN 
    (SELECT Orders_Adjustment.Id
     FROM Orders_Adjustment);
GO

DELETE FROM LineItems
WHERE Order_Id IN 
    (SELECT Orders_Production.Id
     FROM Orders_Production);
GO

DELETE FROM LineItems
WHERE Order_Id IN 
    (SELECT Orders_Transfer.Id
     FROM Orders_Transfer);
GO

DELETE FROM LineItems
WHERE Order_Id IN 
    (SELECT Orders_Purchase.Id
     FROM Orders_Purchase);
GO

--Delete adjustments
DECLARE @MyAdjTableVar table (
    ValeId int NOT NULL);
    
DECLARE @MyProdTableVar table (
ValeId int NOT NULL);
    
DECLARE @MyTransTableVar table (
ValeId int NOT NULL);
    
DECLARE @MyPurTableVar table (
ValeId int NOT NULL);

DELETE FROM Orders_Adjustment 
OUTPUT DELETED.Id 
INTO @MyAdjTableVar;

DELETE FROM Orders_Production
OUTPUT DELETED.Id 
INTO @MyProdTableVar;

DELETE FROM Orders_Transfer
OUTPUT DELETED.Id 
INTO @MyTransTableVar;

DELETE FROM Orders_Purchase
OUTPUT DELETED.Id 
INTO @MyPurTableVar;

--Delete orders that were adjustments	  
DELETE FROM Orders
WHERE Orders.Id IN (SELECT * FROM @MyAdjTableVar);

DELETE FROM Orders
WHERE Orders.Id IN (SELECT * FROM @MyProdTableVar);

DELETE FROM Orders
WHERE Orders.Id IN (SELECT * FROM @MyTransTableVar);

DELETE FROM Orders
WHERE Orders.Id IN (SELECT * FROM @MyPurTableVar);