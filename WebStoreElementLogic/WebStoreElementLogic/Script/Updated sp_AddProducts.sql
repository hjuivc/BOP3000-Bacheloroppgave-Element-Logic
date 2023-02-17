USE [Element Logic (Web Shop)]
GO
/****** Object:  StoredProcedure [dbo].[spAddProducts]    Script Date: 17.02.2023 13:31:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spAddProducts]
                 @ExtProductId varchar(50),
                 @ProductName varchar(50),
                 @ProductDesc varchar(500),
				 @ImageId varchar(250)
     
AS
BEGIN
    INSERT INTO dbo.Products(ExtProductId,ProductName,ProductDesc, ImageId)
    VALUES (@ExtProductId, @ProductName, @ProductDesc, @ImageId)
    SELECT @ExtProductId AS ExtProductId;
END
