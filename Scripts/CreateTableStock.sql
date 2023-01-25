USE [Element Logic (Web Shop)]
GO

/****** Object:  Table [dbo].[Stock]    Script Date: 25.01.2023 18:23:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Products] FOREIGN KEY([ExtProductId])
REFERENCES [dbo].[Products] ([ExtProductId])
GO

ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Products]
GO


