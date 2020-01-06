USE DeCaminoDB;
GO

ALTER TABLE dbo.Products ADD [ProductType] INT NOT NULL DEFAULT 0
GO

ALTER TABLE dbo.Products ADD [RecipeQuantity] FLOAT NOT NULL DEFAULT 1
GO

ALTER TABLE dbo.Products ADD [RecipeUnitMeasure_Id] INT NOT NULL DEFAULT 1
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

UPDATE [dbo].[Products]
   SET [RecipeUnitMeasure_Id] = 1
GO

ALTER TABLE dbo.Products ADD [EdiblePart] FLOAT NOT NULL DEFAULT 1
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [IsRecipe]
GO

USE [DeCaminoDB]
GO
ALTER TABLE [dbo].[Products] DROP COLUMN [IsInMenu]
GO


ALTER TABLE dbo.LineItems_SaleLineItem
	DROP CONSTRAINT FK_SaleLineItem_inherits_LineItem
GO

ALTER TABLE dbo.LineItems_SaleLineItem ADD CONSTRAINT
	FK_SaleLineItem_inherits_LineItem FOREIGN KEY
	(
	Id
	) REFERENCES dbo.LineItems
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
