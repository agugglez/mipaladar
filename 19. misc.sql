UPDATE [dbo].[InventoryItems]
   SET [Quantity] = ROUND([Quantity],2)
GO

ALTER TABLE dbo.PriceLists ADD [InventoryArea_Id] int  NULL;
GO

-- Creating foreign key on [InventoryArea_Id] in table 'PriceLists'
ALTER TABLE [dbo].[PriceLists]
ADD CONSTRAINT [FK_SaleAreaInventoryArea]
    FOREIGN KEY ([InventoryArea_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE SET NULL ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleAreaInventoryArea'
CREATE INDEX [IX_FK_SaleAreaInventoryArea]
ON [dbo].[PriceLists]
    ([InventoryArea_Id]);
GO

ALTER TABLE dbo.Products ADD [TakeFromLocal] BIT NOT NULL DEFAULT 0;
GO

ALTER TABLE dbo.Inventories ADD [IsFloor] BIT NOT NULL DEFAULT 0;
GO

UPDATE [dbo].[Inventories]
   SET [IsFloor] = 1
 WHERE Name = 'Piso'
GO