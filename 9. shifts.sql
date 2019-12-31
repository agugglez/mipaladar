USE [RestaurantDB];
GO

-- Creating table 'Shifts'
CREATE TABLE [dbo].[Shifts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WorkSessions'
CREATE TABLE [dbo].[WorkSessions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Shift_Id] int  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [PK_Shifts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WorkSessions'
ALTER TABLE [dbo].[WorkSessions]
ADD CONSTRAINT [PK_WorkSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [Shift_Id] in table 'WorkSessions'
ALTER TABLE [dbo].[WorkSessions]
ADD CONSTRAINT [FK_WorkSessionShift]
    FOREIGN KEY ([Shift_Id])
    REFERENCES [dbo].[Shifts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkSessionShift'
CREATE INDEX [IX_FK_WorkSessionShift]
ON [dbo].[WorkSessions]
    ([Shift_Id]);
GO

-- Sale -> WorkSession
ALTER TABLE dbo.Orders_Sale ADD [WorkSessionId] int  NULL
GO

-- Creating foreign key on [WorkSessionId] in table 'Orders_Sale'
ALTER TABLE [dbo].[Orders_Sale]
ADD CONSTRAINT [FK_WorkSessionSale]
    FOREIGN KEY ([WorkSessionId])
    REFERENCES [dbo].[WorkSessions]
        ([Id])
    ON DELETE SET NULL ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkSessionSale'
CREATE INDEX [IX_FK_WorkSessionSale]
ON [dbo].[Orders_Sale]
    ([WorkSessionId]);
GO