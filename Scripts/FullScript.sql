-- Create Database
-- CREATE DATABASE [Element Logic (Web Shop)]

-- Using the new database
USE [elementlogicwebshop]

-- Create table Inbound
CREATE TABLE [dbo].[Inbound](
	[PurchaseOrderId] [varchar](50) NOT NULL,
	[PurchaseOrderLineId] [int] NOT NULL,
	[ExtProductId] [varchar](50) NOT NULL,
	[Quantity] [decimal](18, 3) NOT NULL,
	[Status] [bit] NOT NULL,
	[InboundId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
 CONSTRAINT [PK_Inbound] PRIMARY KEY CLUSTERED 
(
	[InboundId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
	[TransactionId] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ExtOrderId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


-- Create table Users
CREATE TABLE [dbo].[Users](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nvarchar](100) NOT NULL,
	[password] [nvarchar](200) NOT NULL,
	[admin] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
-- CREATE LOGIN [admin] WITH PASSWORD=N'JNI+b4ISHVuyAhbqns+ewlDx+5GV9cZaVbR4PtYGsaQ=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
-- GO

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
	INSERT INTO dbo.Stock(Quantity, ExtProductId)
	VALUES (0, @ExtProductId)
	SELECT SCOPE_IDENTITY() AS Id;
	
	SELECT @ExtProductId as ExtProductId;
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

-- Creating Stored Procedure for User
CREATE PROCEDURE [dbo].[spAddUsers]
                 @userName nvarchar(100),
                 @password nvarchar(200),
                 @admin bit
AS
BEGIN
    INSERT INTO dbo.Users
    VALUES (@userName, @password, @admin)
    SELECT SCOPE_IDENTITY() AS userId;
END
GO
	

-- Test- data for the Products table
INSERT INTO dbo.Products(ExtProductId,ProductName,ProductDesc,ImageId)
VALUES ('1', 'Rab Torque Pant', 'Hiking pants', ''),
       ('2', 'Rab Momentum Shorts', 'Hiking shorts', ''),
       ('3', 'Rab Outpost Jacket', 'Fleece jacket', ''),
       ('4', 'Rab Firewall Jacket', 'Hiking jacket', ''),
       ('5', 'Rab Firewall Pant', 'Hiking jacket', '')
       
GO

-- Updated 02.04.2023



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
GO

-- Stored procedure for adding an inbound
CREATE PROCEDURE [dbo].[spAddInbound]
(
    @PurchaseOrderId varchar(50),
    @ExtProductId varchar(50),
    @Quantity decimal (18,3)

)
AS
BEGIN
	DECLARE @TransactionId INT;
	SET @TransactionId = NEXT VALUE FOR dbo.TransactionIdSequence;

    INSERT INTO dbo.Inbound(PurchaseOrderId, PurchaseOrderLineId, ExtProductId, Quantity, Status, TransactionId)
    VALUES (@PurchaseOrderId, 1, @ExtProductId, @Quantity, 0, @TransactionId)
	SELECT SCOPE_IDENTITY() AS InboudId;
END
GO

-- Stored procedure for deleting a user
CREATE PROCEDURE [dbo].[spDeleteUser]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Users]
    WHERE userId = @Id;
END
GO

-- Stored procedure for updating a user
CREATE PROCEDURE [dbo].[spUpdateUsers]
 @userId int,
 @userName nvarchar(100),
 @password nvarchar(200),
 @admin bit
AS
UPDATE Users
SET 
 [userName] = @userName,
 [password] = @password,
 [admin] = @admin
WHERE 
 [userId] = @userId
GO

-- Sequence for TransactionId
CREATE SEQUENCE [dbo].[TransactionIdSequence] 
 AS [int]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO

.. Stored procedure for updating stock to 0
USE [WebShop]
GO

/****** Object:  StoredProcedure [dbo].[spUpdateStock]    Script Date: 19-May-23 17:02:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- Creating Stored Procedure for Update stock to 0
CREATE PROCEDURE [dbo].[spUpdateStock]
 @Id Int
AS
UPDATE Stock
SET [Quantity]			= 0
WHERE [Id]		= @Id
GO


-- Stored procedure for deleting all of the content in the database
USE [WebShop]
GO

/****** Object:  StoredProcedure [dbo].[spDeleteAll]    Script Date: 19-May-23 17:04:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

USE [WebShop]
GO

/****** Object:  StoredProcedure [dbo].[spDeleteAll]    Script Date: 19-May-23 19:08:30 ******/
DROP PROCEDURE [dbo].[spDeleteAll]
GO

/****** Object:  StoredProcedure [dbo].[spDeleteAll]    Script Date: 19-May-23 19:08:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spDeleteAll]
AS
BEGIN
    DECLARE @ErrorMessage NVARCHAR(4000);
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Delete data from tables in specific order
        
        -- Table 1
        DELETE FROM [Order];
        
        -- Table 2
        DELETE FROM [Inbound];
        
        -- Table 3
        DELETE FROM [Stock];

	-- Table 4
	DELETE FROM [Products];
	
	-- Resetting the sequence
	ALTER SEQUENCE dbo.TransactionIdSequence RESTART WITH 1;

        -- Commit the transaction
        COMMIT;
    END TRY
    
    BEGIN CATCH
        -- Rollback the transaction in case of any error
        ROLLBACK;
        
        -- Get the error message
        SET @ErrorMessage = ERROR_MESSAGE();
        
        -- Optionally, log or handle the error
        
        -- Throw an error or return the error message
        -- THROW @ErrorMessage; -- Uncomment this line to throw the error
        
        -- Alternatively, return the error message
        SELECT @ErrorMessage AS ErrorMessage;
    END CATCH;
END;
GO



END;
GO








