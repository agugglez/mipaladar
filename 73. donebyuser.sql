USE DeCaminoDB;
GO

ALTER TABLE dbo.Products ADD
	DoneByUser bit NOT NULL DEFAULT 0
GO