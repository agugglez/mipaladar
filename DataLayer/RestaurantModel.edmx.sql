
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/20/2017 21:30:10
-- Generated from EDMX file: D:\LO QUE ESTABA EN C\Documents\Visual Studio 2012\Projects\MiPaladar DeCamino\DataLayer\RestaurantModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DeCaminoDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ProductIngredient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients] DROP CONSTRAINT [FK_ProductIngredient];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductIngredient1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients] DROP CONSTRAINT [FK_ProductIngredient1];
GO
IF OBJECT_ID(N'[dbo].[FK_Ingredients_UnitMeasures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients] DROP CONSTRAINT [FK_Ingredients_UnitMeasures];
GO
IF OBJECT_ID(N'[dbo].[FK_UMFamilyProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_UMFamilyProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitMeasures_UMFamilies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitMeasures] DROP CONSTRAINT [FK_UnitMeasures_UMFamilies];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitMeasureProductCost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_UnitMeasureProductCost];
GO
IF OBJECT_ID(N'[dbo].[FK_LineItemUnitMeasure]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT [FK_LineItemUnitMeasure];
GO
IF OBJECT_ID(N'[dbo].[FK_LineItemOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT [FK_LineItemOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeesRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_EmployeesRole];
GO
IF OBJECT_ID(N'[dbo].[FK_ShiftSale]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Sale] DROP CONSTRAINT [FK_ShiftSale];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductTag_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductTag] DROP CONSTRAINT [FK_ProductTag_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductTag_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductTag] DROP CONSTRAINT [FK_ProductTag_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleLineItemSale]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_SaleLineItem] DROP CONSTRAINT [FK_SaleLineItemSale];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_ProductCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryParentCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [FK_CategoryParentCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductRecipeUnitMeasure]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_ProductRecipeUnitMeasure];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductLineItem1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT [FK_ProductLineItem1];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_Sale_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Sale] DROP CONSTRAINT [FK_Sale_inherits_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleLineItem_inherits_LineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_SaleLineItem] DROP CONSTRAINT [FK_SaleLineItem_inherits_LineItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[LineItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Ingredients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingredients];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[UMFamilies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMFamilies];
GO
IF OBJECT_ID(N'[dbo].[UnitMeasures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitMeasures];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Shifts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Shifts];
GO
IF OBJECT_ID(N'[dbo].[Miscs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Miscs];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Orders_Sale]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders_Sale];
GO
IF OBJECT_ID(N'[dbo].[LineItems_SaleLineItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems_SaleLineItem];
GO
IF OBJECT_ID(N'[dbo].[ProductTag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductTag];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [SalePrice] decimal(19,4)  NOT NULL,
    [UMFamilyId] int  NOT NULL,
    [ImageFileName] nvarchar(max)  NULL,
    [CostPrice] decimal(19,4)  NOT NULL,
    [CostUMId] int  NOT NULL,
    [Code] int  NOT NULL,
    [ArbitraryCost] decimal(19,4)  NOT NULL,
    [ProductType] int  NOT NULL,
    [RecipeQuantity] float  NOT NULL,
    [EdiblePart] float  NOT NULL,
    [CategoryId] int  NULL,
    [ProductionProcess] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [RecipeUnitMeasure_Id] int  NOT NULL,
    [DoneByUser] bit  NOT NULL
);
GO

-- Creating table 'LineItems'
CREATE TABLE [dbo].[LineItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] float  NOT NULL,
    [Product_Id] int  NOT NULL,
    [UnitMeasure_Id] int  NOT NULL,
    [Order_Id] int  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ParentCategory_Id] int  NULL
);
GO

-- Creating table 'Ingredients'
CREATE TABLE [dbo].[Ingredients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] float  NOT NULL,
    [IngredientProductId] int  NOT NULL,
    [BaseProductId] int  NOT NULL,
    [UnitMeasureId] int  NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CanSell] bit  NOT NULL,
    [ImageFileName] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [Role_Id] int  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [Memo] nvarchar(max)  NULL,
    [Employee_Id] int  NULL
);
GO

-- Creating table 'UMFamilies'
CREATE TABLE [dbo].[UMFamilies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UnitMeasures'
CREATE TABLE [dbo].[UnitMeasures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Caption] nvarchar(max)  NOT NULL,
    [UMFamilyId] int  NOT NULL,
    [IsFamilyBase] bit  NOT NULL,
    [ToBaseConversion] float  NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CanLogin] bit  NOT NULL,
    [CanSeeSales] bit  NOT NULL,
    [CanRemoveSales] bit  NOT NULL,
    [CanSeeInventory] bit  NOT NULL,
    [CanCreateProducts] bit  NOT NULL,
    [CanEditProducts] bit  NOT NULL,
    [CanRemoveProducts] bit  NOT NULL,
    [CanSeeEmployees] bit  NOT NULL,
    [CanCreateEmployees] bit  NOT NULL,
    [CanEditEmployees] bit  NOT NULL,
    [CanRemoveEmployees] bit  NOT NULL,
    [CanExportImport] bit  NOT NULL,
    [CanSeeReports] bit  NOT NULL,
    [CanSeeRoles] bit  NOT NULL,
    [CanSeeDashboard] bit  NOT NULL
);
GO

-- Creating table 'Shifts'
CREATE TABLE [dbo].[Shifts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Miscs'
CREATE TABLE [dbo].[Miscs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [CompanyImage] nvarchar(max)  NULL,
    [ReportsFolder] nvarchar(max)  NULL,
    [RegisterIP] nvarchar(max)  NULL,
    [Capacity] int  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Orders_Sale'
CREATE TABLE [dbo].[Orders_Sale] (
    [DateClosed] datetime  NULL,
    [DatePrinted] datetime  NULL,
    [Discount] decimal(19,4)  NOT NULL,
    [DiscountInPercent] bit  NOT NULL,
    [Cash] decimal(19,4)  NOT NULL,
    [Prints] int  NOT NULL,
    [Persons] int  NOT NULL,
    [Paid] bit  NOT NULL,
    [Number] int  NULL,
    [SubTotal] decimal(19,4)  NOT NULL,
    [Total] decimal(19,4)  NOT NULL,
    [Closed] bit  NOT NULL,
    [Tax] decimal(19,4)  NOT NULL,
    [TaxInPercent] bit  NOT NULL,
    [Tips] decimal(19,4)  NOT NULL,
    [TotalCost] decimal(19,4)  NOT NULL,
    [ShiftId] int  NULL,
    [Voids] int  NOT NULL,
    [VoidsAfterReceipt] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_SaleLineItem'
CREATE TABLE [dbo].[LineItems_SaleLineItem] (
    [Amount] decimal(19,4)  NOT NULL,
    [Printed] bit  NOT NULL,
    [Cost] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL,
    [Sale_Id] int  NOT NULL
);
GO

-- Creating table 'ProductTag'
CREATE TABLE [dbo].[ProductTag] (
    [Products_Id] int  NOT NULL,
    [Tags_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems'
ALTER TABLE [dbo].[LineItems]
ADD CONSTRAINT [PK_LineItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ingredients'
ALTER TABLE [dbo].[Ingredients]
ADD CONSTRAINT [PK_Ingredients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UMFamilies'
ALTER TABLE [dbo].[UMFamilies]
ADD CONSTRAINT [PK_UMFamilies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitMeasures'
ALTER TABLE [dbo].[UnitMeasures]
ADD CONSTRAINT [PK_UnitMeasures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [PK_Shifts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Miscs'
ALTER TABLE [dbo].[Miscs]
ADD CONSTRAINT [PK_Miscs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [PK_Orders_Sale]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineItems_SaleLineItem'
ALTER TABLE [dbo].[LineItems_SaleLineItem]
ADD CONSTRAINT [PK_LineItems_SaleLineItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Products_Id], [Tags_Id] in table 'ProductTag'
ALTER TABLE [dbo].[ProductTag]
ADD CONSTRAINT [PK_ProductTag]
    PRIMARY KEY NONCLUSTERED ([Products_Id], [Tags_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IngredientProductId] in table 'Ingredients'
ALTER TABLE [dbo].[Ingredients]
ADD CONSTRAINT [FK_ProductIngredient]
    FOREIGN KEY ([IngredientProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductIngredient'
CREATE INDEX [IX_FK_ProductIngredient]
ON [dbo].[Ingredients]
    ([IngredientProductId]);
GO

-- Creating foreign key on [BaseProductId] in table 'Ingredients'
ALTER TABLE [dbo].[Ingredients]
ADD CONSTRAINT [FK_ProductIngredient1]
    FOREIGN KEY ([BaseProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductIngredient1'
CREATE INDEX [IX_FK_ProductIngredient1]
ON [dbo].[Ingredients]
    ([BaseProductId]);
GO

-- Creating foreign key on [UnitMeasureId] in table 'Ingredients'
ALTER TABLE [dbo].[Ingredients]
ADD CONSTRAINT [FK_Ingredients_UnitMeasures]
    FOREIGN KEY ([UnitMeasureId])
    REFERENCES [dbo].[UnitMeasures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Ingredients_UnitMeasures'
CREATE INDEX [IX_FK_Ingredients_UnitMeasures]
ON [dbo].[Ingredients]
    ([UnitMeasureId]);
GO

-- Creating foreign key on [UMFamilyId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_UMFamilyProduct]
    FOREIGN KEY ([UMFamilyId])
    REFERENCES [dbo].[UMFamilies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UMFamilyProduct'
CREATE INDEX [IX_FK_UMFamilyProduct]
ON [dbo].[Products]
    ([UMFamilyId]);
GO

-- Creating foreign key on [UMFamilyId] in table 'UnitMeasures'
ALTER TABLE [dbo].[UnitMeasures]
ADD CONSTRAINT [FK_UnitMeasures_UMFamilies]
    FOREIGN KEY ([UMFamilyId])
    REFERENCES [dbo].[UMFamilies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitMeasures_UMFamilies'
CREATE INDEX [IX_FK_UnitMeasures_UMFamilies]
ON [dbo].[UnitMeasures]
    ([UMFamilyId]);
GO

-- Creating foreign key on [CostUMId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_UnitMeasureProductCost]
    FOREIGN KEY ([CostUMId])
    REFERENCES [dbo].[UnitMeasures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitMeasureProductCost'
CREATE INDEX [IX_FK_UnitMeasureProductCost]
ON [dbo].[Products]
    ([CostUMId]);
GO

-- Creating foreign key on [UnitMeasure_Id] in table 'LineItems'
ALTER TABLE [dbo].[LineItems]
ADD CONSTRAINT [FK_LineItemUnitMeasure]
    FOREIGN KEY ([UnitMeasure_Id])
    REFERENCES [dbo].[UnitMeasures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineItemUnitMeasure'
CREATE INDEX [IX_FK_LineItemUnitMeasure]
ON [dbo].[LineItems]
    ([UnitMeasure_Id]);
GO

-- Creating foreign key on [Order_Id] in table 'LineItems'
ALTER TABLE [dbo].[LineItems]
ADD CONSTRAINT [FK_LineItemOrder]
    FOREIGN KEY ([Order_Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineItemOrder'
CREATE INDEX [IX_FK_LineItemOrder]
ON [dbo].[LineItems]
    ([Order_Id]);
GO

-- Creating foreign key on [Role_Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_EmployeesRole]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeesRole'
CREATE INDEX [IX_FK_EmployeesRole]
ON [dbo].[Employees]
    ([Role_Id]);
GO

-- Creating foreign key on [ShiftId] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [FK_ShiftSale]
    FOREIGN KEY ([ShiftId])
    REFERENCES [dbo].[Shifts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShiftSale'
CREATE INDEX [IX_FK_ShiftSale]
ON [dbo].[Orders_Sale]
    ([ShiftId]);
GO

-- Creating foreign key on [Products_Id] in table 'ProductTag'
ALTER TABLE [dbo].[ProductTag]
ADD CONSTRAINT [FK_ProductTag_Product]
    FOREIGN KEY ([Products_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tags_Id] in table 'ProductTag'
ALTER TABLE [dbo].[ProductTag]
ADD CONSTRAINT [FK_ProductTag_Tag]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductTag_Tag'
CREATE INDEX [IX_FK_ProductTag_Tag]
ON [dbo].[ProductTag]
    ([Tags_Id]);
GO

-- Creating foreign key on [Sale_Id] in table 'LineItems_SaleLineItem'
ALTER TABLE [dbo].[LineItems_SaleLineItem]
ADD CONSTRAINT [FK_SaleLineItemSale]
    FOREIGN KEY ([Sale_Id])
    REFERENCES [dbo].[Orders_Sale]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleLineItemSale'
CREATE INDEX [IX_FK_SaleLineItemSale]
ON [dbo].[LineItems_SaleLineItem]
    ([Sale_Id]);
GO

-- Creating foreign key on [CategoryId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProductCategory]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategory'
CREATE INDEX [IX_FK_ProductCategory]
ON [dbo].[Products]
    ([CategoryId]);
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

-- Creating foreign key on [RecipeUnitMeasure_Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_ProductRecipeUnitMeasure]
    FOREIGN KEY ([RecipeUnitMeasure_Id])
    REFERENCES [dbo].[UnitMeasures]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductRecipeUnitMeasure'
CREATE INDEX [IX_FK_ProductRecipeUnitMeasure]
ON [dbo].[Products]
    ([RecipeUnitMeasure_Id]);
GO

-- Creating foreign key on [Product_Id] in table 'LineItems'
ALTER TABLE [dbo].[LineItems]
ADD CONSTRAINT [FK_ProductLineItem1]
    FOREIGN KEY ([Product_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductLineItem1'
CREATE INDEX [IX_FK_ProductLineItem1]
ON [dbo].[LineItems]
    ([Product_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Employees]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Employees'
CREATE INDEX [IX_FK_Orders_Employees]
ON [dbo].[Orders]
    ([Employee_Id]);
GO

-- Creating foreign key on [Id] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [FK_Sale_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_SaleLineItem'
ALTER TABLE [dbo].[LineItems_SaleLineItem]
ADD CONSTRAINT [FK_SaleLineItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------