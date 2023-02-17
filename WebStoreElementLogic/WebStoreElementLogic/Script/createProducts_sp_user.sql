CREATE TABLE [dbo].[Products](
	[ExtProductId] [varchar](50) NOT NULL,
	[ProductName] [varchar](50) NOT NULL,
	[ProductDesc] [varchar](500) NULL,
	[ImageId] [varchar](250) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ExtProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/* Stored Procedure for creating products */
CREATE PROCEDURE [dbo].[spAddProducts]
                 @ExtProductId varchar(50),
                 @ProductName varchar(50),
                 @ProductDesc varchar(500),
				 @ImageId varchar(250)
     
AS
BEGIN
    INSERT INTO dbo.Products(ExtProductId,ProductName,ProductDesc, ImageId)
    VALUES (@ExtProductId, @ProductName, @ProductDesc, @ImageId)
    SELECT @ExtProductId AS ExtProductID;
END
GO

/* Stored Procedure for updating products */
CREATE PROCEDURE [dbo].[spUpdateProducts]
 @ExtProductID varchar(50),
 @ProductName varchar(50),
 @ProductDesc varchar(500),
 @ImageId varchar(250)
AS
UPDATE Products
SET [ProductName]			= @ProductName,
    [ProductDesc]			= @ProductDesc,
	[ImageId]				= @ImageId
WHERE [ExtProductID]		= @ExtProductID
GO

/* Inserting some values */
INSERT INTO dbo.Products(ExtProductID,ProductName,ProductDesc,ImageId)
VALUES ('1', 'Rab Torque Pant', 'Hiking pants', ''),
       ('2', 'Rab Momentum Shorts', 'Hiking shorts', ''),
       ('3', 'Rab Outpost Jacket', 'Fleece jacket', ''),
       ('4', 'Rab Firewall Jacket', 'Hiking jacket', ''),
       ('5', 'Rab Firewall Pant', 'Hiking jacket', '')
       
GO

SELECT *
FROM dbo.Products


/* Creating User- login */
USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [admin]    Script Date: 12.02.2023 19:55:16 ******/
CREATE LOGIN [admin] WITH PASSWORD=N'JNI+b4ISHVuyAhbqns+ewlDx+5GV9cZaVbR4PtYGsaQ=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [admin] DISABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [admin]
GO