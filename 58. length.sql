INSERT INTO [DeCaminoDB].[dbo].[UMFamilies]
           ([Name])
     VALUES
           ('Length')
GO

INSERT INTO [DeCaminoDB].[dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('mm'
           ,4
           ,1
           ,1
           ,'mm')
GO

INSERT INTO [DeCaminoDB].[dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('cm'
           ,4
           ,0
           ,10
           ,'cm')
GO

INSERT INTO [DeCaminoDB].[dbo].[UnitMeasures]
           ([Caption]
           ,[UMFamilyId]
           ,[IsFamilyBase]
           ,[ToBaseConversion]
           ,[Name])
     VALUES
           ('m'
           ,4
           ,0
           ,1000
           ,'m')
GO




