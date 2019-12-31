USE [RestaurantDB]
GO
SELECT DATEPART(Year ,Date) Año, DATENAME(Month,Date) Mes, ROUND(SUM(Total),2) Venta 
FROM ([dbo].[Orders] AS o
	 INNER JOIN [dbo].[Orders_Sale] as s
	 ON o.Id = s.Id)
GROUP BY DATEPART(Year ,Date), DATEPART(Month,Date), DATENAME(Month,Date)
ORDER BY DATEPART(Year ,Date), DATEPART(Month,Date)
