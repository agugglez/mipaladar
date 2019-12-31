USE RestaurantDB;
GO

-- Creating table 'Orders_Transfer'
CREATE TABLE [dbo].[Orders_Transfer] (
    [Id] int  NOT NULL,
    [InventoryFrom_Id] int  NOT NULL,
    [InventoryTo_Id] int  NOT NULL
);
GO

-- Creating table 'Orders_Adjustment'
CREATE TABLE [dbo].[Orders_Adjustment] (
    [Id] int  NOT NULL,
    [Inventory_Id] int  NULL
);
GO

-- Creating table 'Orders_Production'
CREATE TABLE [dbo].[Orders_Production] (
    [Id] int  NOT NULL,
    [Inventory_Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_AdjustmentItem'
CREATE TABLE [dbo].[LineItems_AdjustmentItem] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_TransferItem'
CREATE TABLE [dbo].[LineItems_TransferItem] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_ProductionItem'
CREATE TABLE [dbo].[LineItems_ProductionItem] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_SaleLineItem'
CREATE TABLE [dbo].[LineItems_SaleLineItem] (
    [UnitPrice] decimal(19,4)  NOT NULL,
    [Amount] decimal(19,4)  NOT NULL,
    [Printed] bit  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_PurchaseLineItem'
CREATE TABLE [dbo].[LineItems_PurchaseLineItem] (
    [UnitPrice] decimal(19,4)  NOT NULL,
    [Amount] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- primary keys
-- Creating primary key on [Id] in table 'Orders_Transfer'
ALTER TABLE [dbo].[Orders_Transfer]
ADD CONSTRAINT [PK_Orders_Transfer]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders_Adjustment'
ALTER TABLE [dbo].[Orders_Adjustment]
ADD CONSTRAINT [PK_Orders_Adjustment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders_Production'
ALTER TABLE [dbo].[Orders_Production]
ADD CONSTRAINT [PK_Orders_Production]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems_AdjustmentItem'
ALTER TABLE [dbo].[LineItems_AdjustmentItem]
ADD CONSTRAINT [PK_LineItems_AdjustmentItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems_TransferItem'
ALTER TABLE [dbo].[LineItems_TransferItem]
ADD CONSTRAINT [PK_LineItems_TransferItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems_ProductionItem'
ALTER TABLE [dbo].[LineItems_ProductionItem]
ADD CONSTRAINT [PK_LineItems_ProductionItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems_SaleLineItem'
ALTER TABLE [dbo].[LineItems_SaleLineItem]
ADD CONSTRAINT [PK_LineItems_SaleLineItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems_PurchaseLineItem'
ALTER TABLE [dbo].[LineItems_PurchaseLineItem]
ADD CONSTRAINT [PK_LineItems_PurchaseLineItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

--drop old foreign keys
ALTER TABLE [dbo].[Transfers] DROP CONSTRAINT [FK_TransferFromInventory]
GO

ALTER TABLE [dbo].[Transfers] DROP CONSTRAINT [FK_TransferToInventory]
GO

ALTER TABLE [dbo].[Productions] DROP CONSTRAINT [FK_ProductionInventory]
GO

ALTER TABLE [dbo].[Adjustments] DROP CONSTRAINT [FK_AdjustmentInventory]
GO

-- foreign keys
-- Creating foreign key on [InventoryFrom_Id] in table 'Orders_Transfer'
ALTER TABLE [dbo].[Orders_Transfer]
ADD CONSTRAINT [FK_TransferFromInventory]
    FOREIGN KEY ([InventoryFrom_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferFromInventory'
CREATE INDEX [IX_FK_TransferFromInventory]
ON [dbo].[Orders_Transfer]
    ([InventoryFrom_Id]);
GO

-- Creating foreign key on [InventoryTo_Id] in table 'Orders_Transfer'
ALTER TABLE [dbo].[Orders_Transfer]
ADD CONSTRAINT [FK_TransferToInventory]
    FOREIGN KEY ([InventoryTo_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferToInventory'
CREATE INDEX [IX_FK_TransferToInventory]
ON [dbo].[Orders_Transfer]
    ([InventoryTo_Id]);
GO

-- Creating foreign key on [Inventory_Id] in table 'Orders_Adjustment'
ALTER TABLE [dbo].[Orders_Adjustment]
ADD CONSTRAINT [FK_AdjustmentInventory]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdjustmentInventory'
CREATE INDEX [IX_FK_AdjustmentInventory]
ON [dbo].[Orders_Adjustment]
    ([Inventory_Id]);
GO

-- Creating foreign key on [Inventory_Id] in table 'Orders_Production'
ALTER TABLE [dbo].[Orders_Production]
ADD CONSTRAINT [FK_ProductionInventory]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionInventory'
CREATE INDEX [IX_FK_ProductionInventory]
ON [dbo].[Orders_Production]
    ([Inventory_Id]);
GO

-- foreign keys on primary key
-- Creating foreign key on [Id] in table 'Orders_Transfer'
ALTER TABLE [dbo].[Orders_Transfer]
ADD CONSTRAINT [FK_Transfer_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Orders_Adjustment'
ALTER TABLE [dbo].[Orders_Adjustment]
ADD CONSTRAINT [FK_Adjustment_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Orders_Production'
ALTER TABLE [dbo].[Orders_Production]
ADD CONSTRAINT [FK_Production_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_AdjustmentItem'
ALTER TABLE [dbo].[LineItems_AdjustmentItem]
ADD CONSTRAINT [FK_AdjustmentItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_TransferItem'
ALTER TABLE [dbo].[LineItems_TransferItem]
ADD CONSTRAINT [FK_TransferItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_ProductionItem'
ALTER TABLE [dbo].[LineItems_ProductionItem]
ADD CONSTRAINT [FK_ProductionItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_SaleLineItem'
ALTER TABLE [dbo].[LineItems_SaleLineItem]
ADD CONSTRAINT [FK_SaleLineItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_PurchaseLineItem'
ALTER TABLE [dbo].[LineItems_PurchaseLineItem]
ADD CONSTRAINT [FK_PurchaseLineItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO