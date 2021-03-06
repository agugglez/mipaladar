/****** Script for SelectTopNRows command from SSMS  ******/
USE [DeCaminoDB];
GO

SELECT [Name]
FROM [dbo].[Products] AS prods
WHERE (SELECT COUNT(*)
              FROM ProductIndexes AS prodIndx 
              WHERE prodIndx.ProductId = prods.Id) = 0
  