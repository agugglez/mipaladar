USE [DeCaminoDB]
GO

-- Creating table 'Misc'
CREATE TABLE [dbo].[Misc] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyName] nvarchar(max) NOT NULL,
    [CompanyImage] nvarchar(max) NULL,
    [DefaultTax] DECIMAL (19,4)  NOT NULL,
    [StartingShiftAmount] DECIMAL (19,4)  NOT NULL,
    [ReportsFolder] nvarchar(max) NULL,
    [RegisterIP] nvarchar(max) NULL
);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Misc]
ADD CONSTRAINT [PK_Misc]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


INSERT INTO [dbo].[Misc]           
     VALUES
           ('D''Camino',NULL,0,0,'C:\Reportes','192.168.2.1')
GO