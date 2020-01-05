USE DeCaminoDB;
GO

ALTER TABLE dbo.Products ADD Category_Id int NULL
GO

-- Creating foreign key on [Category_Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProductCategory]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE SET NULL ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategory'
CREATE INDEX [IX_FK_ProductCategory]
ON [dbo].[Products]
    ([Category_Id]);
GO



ALTER TABLE dbo.Categories ADD [ParentCategory_Id] int NULL
GO

-- Creating foreign key on [ParentCategory_Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [FK_CategoryParentCategory]
    FOREIGN KEY ([ParentCategory_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryParentCategory'
CREATE INDEX [IX_FK_CategoryParentCategory]
ON [dbo].[Categories]
    ([ParentCategory_Id]);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProductTag'
CREATE TABLE [dbo].[ProductTag] (
    [Products_Id] int  NOT NULL,
    [Tags_Id] int  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Products_Id], [Tags_Id] in table 'ProductTag'
ALTER TABLE [dbo].[ProductTag]
ADD CONSTRAINT [PK_ProductTag]
    PRIMARY KEY NONCLUSTERED ([Products_Id], [Tags_Id] ASC);
GO

-- Creating foreign key on [Products_Id] in table 'ProductTag'
ALTER TABLE [dbo].[ProductTag]
ADD CONSTRAINT [FK_ProductTag_Product]
    FOREIGN KEY ([Products_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tags_Id] in table 'ProductTag'
ALTER TABLE [dbo].[ProductTag]
ADD CONSTRAINT [FK_ProductTag_Tag]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductTag_Tag'
CREATE INDEX [IX_FK_ProductTag_Tag]
ON [dbo].[ProductTag]
    ([Tags_Id]);
GO