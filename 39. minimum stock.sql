USE [DeCaminoDB]
GO

ALTER TABLE [dbo].[Products] ADD [MinimumStock] FLOAT NOT NULL DEFAULT 0
GO

UPDATE [dbo].[Products]
   SET [MinimumStock] = MinimumQuantity
   FROM Products AS prod INNER JOIN
		InventoryItems AS ii ON prod.Id = ii.Product_Id
 WHERE MinimumQuantity != 0
GO

ALTER TABLE [dbo].[Products] ADD [MinimumStockUMId] INT NOT NULL DEFAULT 1
GO

UPDATE [dbo].[Products]
   SET [MinimumStockUMId] = CostUMId
GO

-- Creating foreign key on [MinimumStockUMId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProductsMinimumStockUnitMeasure]
    FOREIGN KEY ([MinimumStockUMId])
    REFERENCES [dbo].[UnitMeasures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductsMinimumStockUnitMeasure'
CREATE INDEX [IX_FK_ProductsMinimumStockUnitMeasure]
ON [dbo].[Products]
    ([MinimumStockUMId]);
GO

