USE [DeCaminoDB]
GO

--drop permissions
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK_PermissionsEmployee]
GO

DROP TABLE [dbo].[Permissions]
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
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
    [CanSeeDayAveragesReport] bit  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

--admin role
INSERT INTO [dbo].[Roles]           
     VALUES
           (N'Administrador',1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1)
GO

--cashier role
INSERT INTO [dbo].[Roles]           
     VALUES
           (N'Cajero',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
GO

ALTER TABLE [dbo].[Employees] ADD [Role_Id] INT NOT NULL DEFAULT 2
GO

UPDATE [dbo].[Employees]
   SET [Role_Id] = 1
 WHERE [Name] = 'admin'
GO

-- Creating foreign key on [Role_Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_EmployeesRole]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PermissionsRole'
CREATE INDEX [IX_FK_EmployeesRole]
ON [dbo].[Employees]
    ([Role_Id]);
GO

