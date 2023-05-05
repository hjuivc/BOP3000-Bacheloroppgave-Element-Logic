-- Create Database
CREATE DATABASE [Element Logic (Web Shop)]

-- Using the new database
USE [Element Logic (Web Shop)]

-- Create table Inbound
CREATE TABLE [dbo].[Inbound](
	[InboundId] [int] NOT NULL,
	[TransactionId] [int] NOT NULL,
	[PurchaseOrderId] [varchar](50) NOT NULL,
	[PurchaseOrderLineId] [int] NOT NULL,
	[ExtProductId] [varchar](50) NOT NULL,
	[Quantity] [decimal](18, 3) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Inbound] PRIMARY KEY CLUSTERED 
 (
	[InboundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Create table Order
CREATE TABLE [dbo].[Order](
	[ExtPickListId] [varchar](50) NOT NULL,
	[ExtOrderId] [int] IDENTITY(1,1) NOT NULL,
	[ExtOrderLineId] [int] NOT NULL,
	[ExtProductId] [varchar](50) NOT NULL,
	[Quantity] [decimal](18, 3) NOT NULL,
	[Status] [bit] NOT NULL,
	[TransactionId] [int] NOT NULL
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ExtOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Create table Products
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

-- Create table Stock
CREATE TABLE [dbo].[Stock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [decimal](18, 3) NULL,
	[ExtProductId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Adding relationship between Inbound and Products "ExtProductId"
ALTER TABLE [dbo].[Inbound]  WITH CHECK ADD  CONSTRAINT [FK_Inbound_Products] FOREIGN KEY([ExtProductId])
REFERENCES [dbo].[Products] ([ExtProductId])
GO
--
ALTER TABLE [dbo].[Inbound] CHECK CONSTRAINT [FK_Inbound_Products]
GO

-- Adding relationship between Order and Products "ExtProductId"
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Products] FOREIGN KEY([ExtProductId])
REFERENCES [dbo].[Products] ([ExtProductId])
GO

-- Veryfying the data in Order against Products "ExtProductId"
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Products]
GO

-- Adding relationship between Stock and Products
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Products] FOREIGN KEY([ExtProductId])
REFERENCES [dbo].[Products] ([ExtProductId])
GO

-- Veryfiyng the data in Stock against Products "ExtProductId"
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Products]
GO

-- Creating a user for this database. Password is encrypted.
CREATE LOGIN [admin] WITH PASSWORD=N'JNI+b4ISHVuyAhbqns+ewlDx+5GV9cZaVbR4PtYGsaQ=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

-- Creating Stored Procedures for Add Products
CREATE PROCEDURE [dbo].[spAddProducts]
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
GO

-- Creating Stored Procedure for Update Products
CREATE PROCEDURE [dbo].[spUpdateProducts]
 @ExtProductId varchar(50),
 @ProductName varchar(50),
 @ProductDesc varchar(500),
 @ImageId varchar(250)
AS
UPDATE Products
SET [ProductName]			= @ProductName,
    [ProductDesc]			= @ProductDesc,
	[ImageId]				= @ImageId
WHERE [ExtProductId]		= @ExtProductId
GO

-- Test- data for the Products table
INSERT INTO dbo.Products(ExtProductId,ProductName,ProductDesc,ImageId)
VALUES ('1', 'Rab Torque Pant', 'Hiking pants', ''),
       ('2', 'Rab Momentum Shorts', 'Hiking shorts', ''),
       ('3', 'Rab Outpost Jacket', 'Fleece jacket', ''),
       ('4', 'Rab Firewall Jacket', 'Hiking jacket', ''),
       ('5', 'Rab Firewall Pant', 'Hiking jacket', '')
       
GO

-- Updated 18.02.2023

-- Stored procedure for addOrder
CREATE PROCEDURE [dbo].[spAddOrder]
(
	@TransactionId int,
	@ExtPickListId varchar(50),
	@ExtOrderLineId int,
	@ExtProductId varchar(50),
	@Quantity decimal (18,3)
)
AS
BEGIN

    INSERT INTO [dbo].[Order](ExtPickListId, ExtOrderLineId, ExtProductId, Quantity, Status, TransactionId)
    VALUES (@ExtPickListId, @ExtOrderLineId, @ExtProductId, @Quantity, 0, @TransactionId)
	SELECT SCOPE_IDENTITY() AS ExtOrderId;
END
