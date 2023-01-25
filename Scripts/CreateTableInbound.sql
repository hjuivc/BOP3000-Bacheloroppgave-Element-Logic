USE [Element Logic (Web Shop)]
GO

/****** Object:  Table [dbo].[Inbound]    Script Date: 25.01.2023 18:37:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

ALTER TABLE [dbo].[Inbound]  WITH CHECK ADD  CONSTRAINT [FK_Inbound_Products] FOREIGN KEY([ExtProductId])
REFERENCES [dbo].[Products] ([ExtProductId])
GO

ALTER TABLE [dbo].[Inbound] CHECK CONSTRAINT [FK_Inbound_Products]
GO


