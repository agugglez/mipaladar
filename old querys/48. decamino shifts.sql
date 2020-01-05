USE [DeCaminoDB];
GO

-- Creating table 'Shifts'
CREATE TABLE [dbo].[Shifts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [PK_Shifts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Sale -> Shift
ALTER TABLE dbo.Orders_Sale ADD [ShiftId] INT NULL
GO

-- Creating foreign key on [ShiftId] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [FK_ShiftSale]
    FOREIGN KEY ([ShiftId])
    REFERENCES [dbo].[Shifts]
        ([Id])
    ON DELETE SET NULL ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShiftSale'
CREATE INDEX [IX_FK_ShiftSale]
ON [dbo].[Orders_Sale]
    ([ShiftId]);
GO

INSERT INTO [dbo].[Shifts]
           ([Name])
     VALUES
           ('Turno 1')
GO

INSERT INTO [dbo].[Shifts]
           ([Name])
     VALUES
           ('Turno 2')
GO

--update sales
UPDATE [dbo].[Orders_Sale]
   SET [ShiftId] = 1
  FROM [dbo].[Orders_Sale] AS os INNER JOIN 
		Orders AS o ON o.Id = os.Id INNER JOIN Employees AS e ON [Employee_Id] = e.Id
 WHERE e.Name = 'Turno 1'
GO

UPDATE [dbo].[Orders_Sale]
   SET [ShiftId] = 2
  FROM [dbo].[Orders_Sale] AS os INNER JOIN 
		Orders AS o ON o.Id = os.Id INNER JOIN Employees AS e ON [Employee_Id] = e.Id
 WHERE e.Name = 'Turno 2'
GO

