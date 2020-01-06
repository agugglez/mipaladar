USE DeCaminoDB;
GO

ALTER TABLE dbo.LineItems_SaleLineItem ADD [Sale_Id] int NOT NULL DEFAULT 1
GO

UPDATE [dbo].[LineItems_SaleLineItem]  
   SET [Sale_Id] = LineItems.Order_Id
  FROM [dbo].[LineItems_SaleLineItem] INNER JOIN LineItems ON [dbo].[LineItems_SaleLineItem].Id = LineItems.Id
GO

-- Creating foreign key on [Sale_Id] in table 'LineItems_SaleLineItem'
ALTER TABLE [dbo].[LineItems_SaleLineItem]
ADD CONSTRAINT [FK_SaleLineItemSale]
    FOREIGN KEY ([Sale_Id])
    REFERENCES [dbo].[Orders_Sale]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleLineItemSale'
CREATE INDEX [IX_FK_SaleLineItemSale]
ON [dbo].[LineItems_SaleLineItem]
    ([Sale_Id]);
GO

