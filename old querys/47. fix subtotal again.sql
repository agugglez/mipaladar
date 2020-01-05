USE DeCaminoDB;
GO

UPDATE [Orders_Sale]
SET SubTotal = Total
WHERE SubTotal != Total AND Discount = 0
GO

UPDATE [Orders_Sale]
SET [Orders_Sale].SubTotal = ISNULL((SELECT CAST(SUM(sli.Amount) AS MONEY) 
                                     FROM LineItems_SaleLineItem AS sli INNER JOIN LineItems AS li ON sli.Id = li.Id
                                     WHERE li.Order_Id = [Orders_Sale].Id),0)
WHERE Discount != 0
GO