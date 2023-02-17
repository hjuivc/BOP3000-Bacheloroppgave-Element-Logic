ALTER TABLE dbo."Order"
  ADD CONSTRAINT FK_Order_Products
    FOREIGN KEY(ExtProductId) REFERENCES dbo.Products(ExtProductId)