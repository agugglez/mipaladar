USE RestaurantDB;
GO

UPDATE RestaurantDB.[dbo].[Ingredients]
   SET [Quantity] = ROUND([Quantity],2)
GO

-- Creating table 'CostTraces'
CREATE TABLE [dbo].[CostTraces] (
    [Date] datetime  NOT NULL,
    [Quantity] float  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cost] decimal(19,4)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Inventory_Id] int  NOT NULL,
    [UnitMeasure_Id] int  NOT NULL
);
GO
-- Creating primary key on [Id] in table 'CostTraces'
ALTER TABLE [dbo].[CostTraces]
ADD CONSTRAINT [PK_CostTraces]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [Product_Id] in table 'CostTraces'
ALTER TABLE [dbo].[CostTraces]
ADD CONSTRAINT [FK_CostTraceProduct]
    FOREIGN KEY ([Product_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CostTraceProduct'
CREATE INDEX [IX_FK_CostTraceProduct]
ON [dbo].[CostTraces]
    ([Product_Id]);
GO

-- Creating foreign key on [Inventory_Id] in table 'CostTraces'
ALTER TABLE [dbo].[CostTraces]
ADD CONSTRAINT [FK_CostTraceInventory]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CostTraceInventory'
CREATE INDEX [IX_FK_CostTraceInventory]
ON [dbo].[CostTraces]
    ([Inventory_Id]);
GO

-- Creating foreign key on [UnitMeasure_Id] in table 'CostTraces'
ALTER TABLE [dbo].[CostTraces]
ADD CONSTRAINT [FK_CostTraceUnitMeasure]
    FOREIGN KEY ([UnitMeasure_Id])
    REFERENCES [dbo].[UnitMeasures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CostTraceUnitMeasure'
CREATE INDEX [IX_FK_CostTraceUnitMeasure]
ON [dbo].[CostTraces]
    ([UnitMeasure_Id]);
GO