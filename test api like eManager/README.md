# Test API like eManager

This is a test api to simulate eManager while we wait for access.

## Setup

### Requierments
- uvicorn
- fastapi
- xmltodict
> Needed libraries can be installed with:
> ```shell CMD 
> pip install "fastapi[all]"
> pip install xmltodict
> ```
> Installing fast api with "fastapi[all]" installs uvicorn in additiont to fastapi

### Run the API
you can use either  
```shell 
uvicorn main:app --reload
```
or
```shell
python main.py
```

## Usage

### Endpoints
- POST /api/products/import
- POST /api/goodsreceivals/import
- POST /api/picklists/import

### PI (Product Information)
> POST /api/products/import


A typical request will look like this:
```XML
<?xml version="1.0" encoding="UTF-8"?>
<ImportOperation>
  <Lines>
    <ProductLine>
      <TransactionId>1506189</TransactionId>
      <ExtProductId>214522</ExtProductId>
      <ProductName>Rab Torque Capris Whale/ Quince 30</ProductName>
      <ProductDesc>Small</ProductDesc>
      <ImageId>ProductImageId</ImageId>
    </ProductLine>
  </Lines>
</ImportOperation>
```
The response from the above message will look like this:
```XML
<?xml version="1.0" encoding="utf-8"?>
<ExportOperation>
  <Lines>
    <ProductCreatedLine>
      <TransactionId>1506189</TransactionId>
      <ProductId>214522</ProductId>
      <ProductName>Rab Torque Capris Whale/ Quince 30</ProductName>
    </ProductCreatedLine>
  </Lines>
</ExportOperation>
```

### GR (Goods Receival)
> POST /api/goodsreceivals/import

A typical request will look like this:
```XML
<?xml version="1.0" encoding="utf-8"?>
<ImportOperation>
  <Lines>
    <GoodsReceivalLine>
      <TransactionId>1646794</TransactionId>
      <PurchaseOrderId>PO-02311</PurchaseOrderId>
      <PurchaseOrderLineId>1</PurchaseOrderLineId>
      <ExtProductId>6505093</ExtProductId>
      <Quantity>3.00</Quantity>
    </GoodsReceivalLine>
  </Lines>
</ImportOperation>
```
The response from the above message will look like this:
```XML
<?xml version="1.0" encoding="utf-8"?>
<ExportOperation>
  <Lines>
    <PlacedGoodsLine>
      <TransactionId>1646794</TransactionId>
      <PurchaseOrderId>PO-02311</PurchaseOrderId>
      <PurchaseOrderLineId>1</PurchaseOrderLineId>
      <ExtProductId>6505093</ExtProductId>
      <ActQuantity>2.0</ActQuantity>
    </PlacedGoodsLine>
  </Lines>
</ExportOperation>
```

### GR (Goods Receival)
> POST /api/goodsreceivals/import

A typical request will look like this:
```XML
<?xml version="1.0" encoding="UTF-8"?>
<ImportOperation>
  <Lines>
    <PicklistLine>
      <ExtOrderlineId>34589</ExtOrderlineId>
      <TransactionId>1506265</TransactionId>
      <ExtPicklistId>800025089</ExtPicklistId>
      <ExtOrderId>4087</ExtOrderId>
      <ExtProductId>323413</ExtProductId>
      <Quantity>2.000</Quantity>
    </PicklistLine>
    <PicklistLine>
      <ExtOrderlineId>34599</ExtOrderlineId>
      <TransactionId>1506265</TransactionId>
      <ExtPicklistId>800025089</ExtPicklistId>
      <ExtOrderId>4087</ExtOrderId>
      <ExtProductId>323412</ExtProductId>
      <Quantity>4.000</Quantity>
    </PicklistLine>
  </Lines>
</ImportOperation>
```
The response from the above message will look like this:
```XML
<?xml version="1.0" encoding="utf-8"?>
<ExportOperation>
  <Lines>
    <ConfirmedPickLine>
      <TransactionId>1506265</TransactionId>
      <ExtOrderId>4087</ExtOrderId>
      <ExtOrderlineId>34589</ExtOrderlineId>
      <ExtProductId>323413</ExtProductId>
      <Quantity>2.000</Quantity>
    </ConfirmedPickLine>
    <ConfirmedPickLine>
      <TransactionId>1506265</TransactionId>
      <ExtOrderId>4087</ExtOrderId>
      <ExtOrderlineId>34599</ExtOrderlineId>
      <ExtProductId>323412</ExtProductId>
      <Quantity>4.000</Quantity>
    </ConfirmedPickLine>
  </Lines>
</ExportOperation>
```
