
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/06/2013 10:39:17
-- Generated from EDMX file: C:\Users\agostino\Documents\Visual Studio 2012\Projects\MiPaladar DeCamino\DataLayer\RestaurantModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_CategoryProductIndex]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductIndexes] DROP CONSTRAINT [FK_CategoryProductIndex];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrentExistenceProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryItems] DROP CONSTRAINT [FK_CurrentExistenceProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_Adjustment_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Adjustment] DROP CONSTRAINT [FK_Adjustment_inherits_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_AdjustmentInventory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Adjustment] DROP CONSTRAINT [FK_AdjustmentInventory];
GO
IF OBJECT_ID(N'[dbo].[FK_AdjustmentItem_inherits_LineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_AdjustmentItem] DROP CONSTRAINT [FK_AdjustmentItem_inherits_LineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeesRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_EmployeesRole];
GO
IF OBJECT_ID(N'[dbo].[FK_Ingredients_UnitMeasures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients] DROP CONSTRAINT [FK_Ingredients_UnitMeasures];
GO
IF OBJECT_ID(N'[dbo].[FK_InventoryInventoryItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryItems] DROP CONSTRAINT [FK_InventoryInventoryItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InventoryTraceInventory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryTraces] DROP CONSTRAINT [FK_InventoryTraceInventory];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_Production_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Production] DROP CONSTRAINT [FK_Production_inherits_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionInventory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Production] DROP CONSTRAINT [FK_ProductionInventory];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionItem_inherits_LineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_ProductionItem] DROP CONSTRAINT [FK_ProductionItem_inherits_LineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_Products_ProductionAreas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_ProductionAreas];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductsMinimumStockUnitMeasure]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_ProductsMinimumStockUnitMeasure];
GO
IF OBJECT_ID(N'[dbo].[FK_Purchase_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Purchase] DROP CONSTRAINT [FK_Purchase_inherits_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseInventory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Purchase] DROP CONSTRAINT [FK_PurchaseInventory];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseLineItem_inherits_LineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_PurchaseLineItem] DROP CONSTRAINT [FK_PurchaseLineItem_inherits_LineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_Sale_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Sale] DROP CONSTRAINT [FK_Sale_inherits_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleLineItem_inherits_LineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_SaleLineItem] DROP CONSTRAINT [FK_SaleLineItem_inherits_LineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ShiftSale]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Sale] DROP CONSTRAINT [FK_ShiftSale];
GO
IF OBJECT_ID(N'[dbo].[FK_Tables_PriceLists]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tables] DROP CONSTRAINT [FK_Tables_PriceLists];
GO
IF OBJECT_ID(N'[dbo].[FK_Transfer_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Transfer] DROP CONSTRAINT [FK_Transfer_inherits_Order];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferFromInventory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Transfer] DROP CONSTRAINT [FK_TransferFromInventory];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferItem_inherits_LineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems_TransferItem] DROP CONSTRAINT [FK_TransferItem_inherits_LineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferToInventory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Transfer] DROP CONSTRAINT [FK_TransferToInventory];
GO
IF OBJECT_ID(N'[dbo].[FK_UMFamilyProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_UMFamilyProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitMeasureProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT [FK_UnitMeasureProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitMeasureProductCost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_UnitMeasureProductCost];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitMeasures_UMFamilies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitMeasures] DROP CONSTRAINT [FK_UnitMeasures_UMFamilies];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderLineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT [FK_OrderLineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductExistence]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryTraces] DROP CONSTRAINT [FK_ProductExistence];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductIngredient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients] DROP CONSTRAINT [FK_ProductIngredient];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductIngredient1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients] DROP CONSTRAINT [FK_ProductIngredient1];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductLineItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineItems] DROP CONSTRAINT [FK_ProductLineItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductIndex]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductIndexes] DROP CONSTRAINT [FK_ProductProductIndex];
GO
IF OBJECT_ID(N'[dbo].[FK_TableSale]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders_Sale] DROP CONSTRAINT [FK_TableSale];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Ingredients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingredients];
GO
IF OBJECT_ID(N'[dbo].[Inventories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inventories];
GO
IF OBJECT_ID(N'[dbo].[InventoryItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryItems];
GO
IF OBJECT_ID(N'[dbo].[InventoryTraces]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryTraces];
GO
IF OBJECT_ID(N'[dbo].[LineItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems];
GO
IF OBJECT_ID(N'[dbo].[LineItems_AdjustmentItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems_AdjustmentItem];
GO
IF OBJECT_ID(N'[dbo].[LineItems_ProductionItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems_ProductionItem];
GO
IF OBJECT_ID(N'[dbo].[LineItems_PurchaseLineItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems_PurchaseLineItem];
GO
IF OBJECT_ID(N'[dbo].[LineItems_SaleLineItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems_SaleLineItem];
GO
IF OBJECT_ID(N'[dbo].[LineItems_TransferItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineItems_TransferItem];
GO
IF OBJECT_ID(N'[dbo].[Misc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Misc];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[Orders_Adjustment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders_Adjustment];
GO
IF OBJECT_ID(N'[dbo].[Orders_Production]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders_Production];
GO
IF OBJECT_ID(N'[dbo].[Orders_Purchase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders_Purchase];
GO
IF OBJECT_ID(N'[dbo].[Orders_Sale]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders_Sale];
GO
IF OBJECT_ID(N'[dbo].[Orders_Transfer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders_Transfer];
GO
IF OBJECT_ID(N'[dbo].[PriceLists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PriceLists];
GO
IF OBJECT_ID(N'[dbo].[ProductIndexes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductIndexes];
GO
IF OBJECT_ID(N'[dbo].[ProductionAreas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionAreas];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Shifts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Shifts];
GO
IF OBJECT_ID(N'[dbo].[Tables]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tables];
GO
IF OBJECT_ID(N'[dbo].[UMFamilies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UMFamilies];
GO
IF OBJECT_ID(N'[dbo].[UnitMeasures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitMeasures];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsRecipe] bit  NOT NULL,
    [IsStorable] bit  NOT NULL,
    [NotInMenu] bit  NOT NULL,
    [SalePrice] decimal(19,4)  NOT NULL,
    [ProductionAreaId] int  NULL,
    [IsProduced] bit  NOT NULL,
    [IsEntrant] bit  NOT NULL,
    [UMFamilyId] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [PrintString] nvarchar(max)  NULL,
    [ImageFileName] nvarchar(max)  NULL,
    [CostPrice] decimal(19,4)  NOT NULL,
    [CostQuantity] float  NOT NULL,
    [CostUMId] int  NOT NULL,
    [MinimumStock] float  NOT NULL,
    [MinimumStockUMId] int  NOT NULL,
    [Code] int  NOT NULL
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
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PriceLists'
CREATE TABLE [dbo].[PriceLists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
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
    [CanPurchase] bit  NOT NULL,
    [CanSell] bit  NOT NULL,
    [ImageFileName] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [Role_Id] int  NOT NULL
);
GO

-- Creating table 'Tables'
CREATE TABLE [dbo].[Tables] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL,
    [Capacity] int  NOT NULL,
    [AreaId] int  NULL,
    [IsBar] bit  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'ProductIndexes'
CREATE TABLE [dbo].[ProductIndexes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsMain] bit  NOT NULL,
    [CategoryId] int  NOT NULL,
    [ProductId] int  NOT NULL
);
GO

-- Creating table 'InventoryItems'
CREATE TABLE [dbo].[InventoryItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] float  NOT NULL,
    [MinimumQuantity] float  NOT NULL,
    [InventoryId] int  NOT NULL,
    [Cost] decimal(19,4)  NOT NULL,
    [Product_Id] int  NOT NULL
);
GO

-- Creating table 'InventoryTraces'
CREATE TABLE [dbo].[InventoryTraces] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] float  NOT NULL,
    [Date] datetime  NOT NULL,
    [ProductId] int  NOT NULL,
    [InventoryId] int  NOT NULL
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

-- Creating table 'ProductionAreas'
CREATE TABLE [dbo].[ProductionAreas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
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

-- Creating table 'Inventories'
CREATE TABLE [dbo].[Inventories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsFloor] bit  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CanLogin] bit  NOT NULL,
    [CanSeeSales] bit  NOT NULL,
    [CanRemoveSales] bit  NOT NULL,
    [CanSeeOldSales] bit  NOT NULL,
    [CanSeePurchases] bit  NOT NULL,
    [CanRemovePurchases] bit  NOT NULL,
    [CanSeeOldPurchases] bit  NOT NULL,
    [CanSeeInventory] bit  NOT NULL,
    [CanCreateProducts] bit  NOT NULL,
    [CanEditProducts] bit  NOT NULL,
    [CanRemoveProducts] bit  NOT NULL,
    [CanSeeEmployees] bit  NOT NULL,
    [CanCreateEmployees] bit  NOT NULL,
    [CanEditEmployees] bit  NOT NULL,
    [CanRemoveEmployees] bit  NOT NULL,
    [CanSeeMiPaladar] bit  NOT NULL,
    [CanExportImport] bit  NOT NULL,
    [CanSeeSalesReport] bit  NOT NULL,
    [CanSeeSalesByItemReport] bit  NOT NULL,
    [CanSeeReports] bit  NOT NULL,
    [CanSeeRoles] bit  NOT NULL
);
GO

-- Creating table 'Miscs'
CREATE TABLE [dbo].[Miscs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [CompanyImage] nvarchar(max)  NULL,
    [DefaultTax] decimal(19,4)  NOT NULL,
    [StartingShiftAmount] decimal(19,4)  NOT NULL,
    [ReportsFolder] nvarchar(max)  NULL,
    [RegisterIP] nvarchar(max)  NULL
);
GO

-- Creating table 'Shifts'
CREATE TABLE [dbo].[Shifts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
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
    [Inventory_Id] int  NOT NULL
);
GO

-- Creating table 'Orders_Purchase'
CREATE TABLE [dbo].[Orders_Purchase] (
    [Total] decimal(19,4)  NOT NULL,
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
    [Id] int  NOT NULL,
    [Table_Id] int  NULL
);
GO

-- Creating table 'LineItems_AdjustmentItem'
CREATE TABLE [dbo].[LineItems_AdjustmentItem] (
    [Cost] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_TransferItem'
CREATE TABLE [dbo].[LineItems_TransferItem] (
    [Cost] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_ProductionItem'
CREATE TABLE [dbo].[LineItems_ProductionItem] (
    [Cost] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_SaleLineItem'
CREATE TABLE [dbo].[LineItems_SaleLineItem] (
    [Amount] decimal(19,4)  NOT NULL,
    [Printed] bit  NOT NULL,
    [Cost] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'LineItems_PurchaseLineItem'
CREATE TABLE [dbo].[LineItems_PurchaseLineItem] (
    [Amount] decimal(19,4)  NOT NULL,
    [Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'PriceLists'
ALTER TABLE [dbo].[PriceLists]
ADD CONSTRAINT [PK_PriceLists]
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

-- Creating primary key on [Id] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [PK_Tables]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductIndexes'
ALTER TABLE [dbo].[ProductIndexes]
ADD CONSTRAINT [PK_ProductIndexes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InventoryItems'
ALTER TABLE [dbo].[InventoryItems]
ADD CONSTRAINT [PK_InventoryItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InventoryTraces'
ALTER TABLE [dbo].[InventoryTraces]
ADD CONSTRAINT [PK_InventoryTraces]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductionAreas'
ALTER TABLE [dbo].[ProductionAreas]
ADD CONSTRAINT [PK_ProductionAreas]
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

-- Creating primary key on [Id] in table 'Inventories'
ALTER TABLE [dbo].[Inventories]
ADD CONSTRAINT [PK_Inventories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Miscs'
ALTER TABLE [dbo].[Miscs]
ADD CONSTRAINT [PK_Miscs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [PK_Shifts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

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

-- Creating primary key on [Id] in table 'Orders_Purchase'
ALTER TABLE [dbo].[Orders_Purchase]
ADD CONSTRAINT [PK_Orders_Purchase]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders_Production'
ALTER TABLE [dbo].[Orders_Production]
ADD CONSTRAINT [PK_Orders_Production]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [PK_Orders_Sale]
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryId] in table 'ProductIndexes'
ALTER TABLE [dbo].[ProductIndexes]
ADD CONSTRAINT [FK_CategoryProductIndex]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryProductIndex'
CREATE INDEX [IX_FK_CategoryProductIndex]
ON [dbo].[ProductIndexes]
    ([CategoryId]);
GO

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

-- Creating foreign key on [ProductId] in table 'ProductIndexes'
ALTER TABLE [dbo].[ProductIndexes]
ADD CONSTRAINT [FK_ProductProductIndex]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductIndex'
CREATE INDEX [IX_FK_ProductProductIndex]
ON [dbo].[ProductIndexes]
    ([ProductId]);
GO

-- Creating foreign key on [Product_Id] in table 'InventoryItems'
ALTER TABLE [dbo].[InventoryItems]
ADD CONSTRAINT [FK_CurrentExistenceProduct]
    FOREIGN KEY ([Product_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrentExistenceProduct'
CREATE INDEX [IX_FK_CurrentExistenceProduct]
ON [dbo].[InventoryItems]
    ([Product_Id]);
GO

-- Creating foreign key on [ProductId] in table 'InventoryTraces'
ALTER TABLE [dbo].[InventoryTraces]
ADD CONSTRAINT [FK_ProductExistence]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductExistence'
CREATE INDEX [IX_FK_ProductExistence]
ON [dbo].[InventoryTraces]
    ([ProductId]);
GO

-- Creating foreign key on [AreaId] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [FK_Tables_PriceLists]
    FOREIGN KEY ([AreaId])
    REFERENCES [dbo].[PriceLists]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Tables_PriceLists'
CREATE INDEX [IX_FK_Tables_PriceLists]
ON [dbo].[Tables]
    ([AreaId]);
GO

-- Creating foreign key on [ProductionAreaId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_ProductionAreas]
    FOREIGN KEY ([ProductionAreaId])
    REFERENCES [dbo].[ProductionAreas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_ProductionAreas'
CREATE INDEX [IX_FK_Products_ProductionAreas]
ON [dbo].[Products]
    ([ProductionAreaId]);
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

-- Creating foreign key on [InventoryId] in table 'InventoryItems'
ALTER TABLE [dbo].[InventoryItems]
ADD CONSTRAINT [FK_InventoryInventoryItem]
    FOREIGN KEY ([InventoryId])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryInventoryItem'
CREATE INDEX [IX_FK_InventoryInventoryItem]
ON [dbo].[InventoryItems]
    ([InventoryId]);
GO

-- Creating foreign key on [InventoryId] in table 'InventoryTraces'
ALTER TABLE [dbo].[InventoryTraces]
ADD CONSTRAINT [FK_InventoryTraceInventory]
    FOREIGN KEY ([InventoryId])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryTraceInventory'
CREATE INDEX [IX_FK_InventoryTraceInventory]
ON [dbo].[InventoryTraces]
    ([InventoryId]);
GO

-- Creating foreign key on [InventoryFrom_Id] in table 'Orders_Transfer'
ALTER TABLE [dbo].[Orders_Transfer]
ADD CONSTRAINT [FK_TransferFromInventory]
    FOREIGN KEY ([InventoryFrom_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

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

-- Creating foreign key on [Inventory_Id] in table 'Orders_Purchase'
ALTER TABLE [dbo].[Orders_Purchase]
ADD CONSTRAINT [FK_PurchaseInventory]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseInventory'
CREATE INDEX [IX_FK_PurchaseInventory]
ON [dbo].[Orders_Purchase]
    ([Inventory_Id]);
GO

-- Creating foreign key on [Inventory_Id] in table 'Orders_Production'
ALTER TABLE [dbo].[Orders_Production]
ADD CONSTRAINT [FK_ProductionInventory]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [dbo].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionInventory'
CREATE INDEX [IX_FK_ProductionInventory]
ON [dbo].[Orders_Production]
    ([Inventory_Id]);
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

-- Creating foreign key on [Product_Id] in table 'LineItems'
ALTER TABLE [dbo].[LineItems]
ADD CONSTRAINT [FK_LineItemProduct]
    FOREIGN KEY ([Product_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineItemProduct'
CREATE INDEX [IX_FK_LineItemProduct]
ON [dbo].[LineItems]
    ([Product_Id]);
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

-- Creating foreign key on [Table_Id] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [FK_SaleTable]
    FOREIGN KEY ([Table_Id])
    REFERENCES [dbo].[Tables]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleTable'
CREATE INDEX [IX_FK_SaleTable]
ON [dbo].[Orders_Sale]
    ([Table_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_OrderEmployee]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderEmployee'
CREATE INDEX [IX_FK_OrderEmployee]
ON [dbo].[Orders]
    ([Employee_Id]);
GO

-- Creating foreign key on [Order_Id] in table 'LineItems'
ALTER TABLE [dbo].[LineItems]
ADD CONSTRAINT [FK_LineItemOrder]
    FOREIGN KEY ([Order_Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineItemOrder'
CREATE INDEX [IX_FK_LineItemOrder]
ON [dbo].[LineItems]
    ([Order_Id]);
GO

-- Creating foreign key on [ProductionAreaId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_ProductionAreas1]
    FOREIGN KEY ([ProductionAreaId])
    REFERENCES [dbo].[ProductionAreas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_ProductionAreas1'
CREATE INDEX [IX_FK_Products_ProductionAreas1]
ON [dbo].[Products]
    ([ProductionAreaId]);
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

-- Creating foreign key on [Id] in table 'Orders_Transfer'
ALTER TABLE [dbo].[Orders_Transfer]
ADD CONSTRAINT [FK_Transfer_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Orders_Adjustment'
ALTER TABLE [dbo].[Orders_Adjustment]
ADD CONSTRAINT [FK_Adjustment_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Orders_Purchase'
ALTER TABLE [dbo].[Orders_Purchase]
ADD CONSTRAINT [FK_Purchase_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Orders_Production'
ALTER TABLE [dbo].[Orders_Production]
ADD CONSTRAINT [FK_Production_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [FK_Sale_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_AdjustmentItem'
ALTER TABLE [dbo].[LineItems_AdjustmentItem]
ADD CONSTRAINT [FK_AdjustmentItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_TransferItem'
ALTER TABLE [dbo].[LineItems_TransferItem]
ADD CONSTRAINT [FK_TransferItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'LineItems_ProductionItem'
ALTER TABLE [dbo].[LineItems_ProductionItem]
ADD CONSTRAINT [FK_ProductionItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
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

-- Creating foreign key on [Id] in table 'LineItems_PurchaseLineItem'
ALTER TABLE [dbo].[LineItems_PurchaseLineItem]
ADD CONSTRAINT [FK_PurchaseLineItem_inherits_LineItem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LineItems]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------