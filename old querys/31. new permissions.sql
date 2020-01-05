USE [DeCaminoDB]
GO

/****** Object:  Table [dbo].[Permissions]    Script Date: 07/15/2013 16:47:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
DROP TABLE [dbo].[Permissions]
GO

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
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
    [CanDesignRestaurant] bit  NOT NULL,
    [CanSeeSalesReport] bit  NOT NULL,
    [CanSeeSalesByItemReport] bit  NOT NULL,
    [CanSeeFollowProductReport] bit  NOT NULL,
    [CanSeeDayAveragesReport] bit  NOT NULL,
    [Employee_Id] int  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [Employee_Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [FK_PermissionsEmployee]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PermissionsEmployee'
CREATE INDEX [IX_FK_PermissionsEmployee]
ON [dbo].[Permissions]
    ([Employee_Id]);
GO

USE [DeCaminoDB]
GO

INSERT INTO [dbo].[Permissions]           
     SELECT 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Id
     FROM [dbo].[Employees]
     WHERE Name != N'admin'
GO

INSERT INTO [dbo].[Permissions]           
     SELECT 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,Id
     FROM [dbo].[Employees]
     WHERE Name = N'admin'
GO