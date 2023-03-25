from fastapi import FastAPI, Request, HTTPException, Response
from fastapi.middleware import Middleware
from starlette.middleware.base import BaseHTTPMiddleware

import xmltodict
import json

def isXML(req):
    return req.headers.get("Content-Type") == "application/xml"
    
def checkForField(data: dict, field: str):
    try:
        return data[field]
    except:
        raise HTTPException(status_code=400, detail=f"No {field} was given")

    
class LoggingMiddleware(BaseHTTPMiddleware):
    async def dispatch(self, request: Request, call_next) -> Response:
        print(f"Request: {request.method} | {request.url.path}")
        print(f"Headers: {json.dumps(dict(request.headers), indent=4)}")


        response = await call_next(request)

        return response


app = FastAPI(middleware=[Middleware(LoggingMiddleware)])


@app.post("/api/products/import")
async def post_products_information(req: Request):
    if not isXML(req):
        raise HTTPException(status_code=400, detail="Not XML")
    
    try:
        body = await req.body()
        data = xmltodict.parse(body)
        product = data["ImportOperation"]["Lines"]["ProductLine"]
    except:
        raise HTTPException(status_code=400, detail="Wrong XML format")

    checkForField(product, "ProductDesc")
    checkForField(product, "ImageId")

    productId = checkForField(product, "ExtProductId")
    productName = checkForField(product, "ProductName")
    transactionId = checkForField(product, "TransactionId")

    xmlResponse = xmltodict.unparse({
        "ExportOperation": {
            "Lines": {
                "ProductCreatedLine": {
                    "TransactionId": transactionId,
                    "ProductId": productId,
                    "ProductName": productName
                }
            }
        }
    }, pretty=True)

    return Response(content=xmlResponse, headers={ "Content-Type": "application/xml" })


@app.post("/api/goodsreceivals/import")
async def post_goods_receivals(req: Request):
    if not isXML(req):
        raise HTTPException(status_code=400, detail="Not XML")
    
    try:
        body = await req.body()
        data = xmltodict.parse(body)
        product = data["ImportOperation"]["Lines"]["GoodsReceivalLine"]
    except:
        raise HTTPException(status_code=400, detail="Wrong XML format")

    transactionId = checkForField(product, "TransactionId")
    purchaseOrderId = checkForField(product, "PurchaseOrderId")
    purchaseOrderLineId = checkForField(product, "PurchaseOrderLineId")
    extProductId = checkForField(product, "ExtProductId")
    quantity = float(checkForField(product, "Quantity"))

    xmlResponse = xmltodict.unparse({
        "ExportOperation": {
            "Lines": {
                "PlacedGoodsLine": {
                    "TransactionId": transactionId,
                    "PurchaseOrderId": purchaseOrderId,
                    "PurchaseOrderLineId": purchaseOrderLineId,
                    "ExtProductId": extProductId,
                    # Simulate products not always coming in
                    "ActQuantity": quantity - 1 if quantity > 0 else quantity
                }
            }
        }
    }, pretty=True)

    return Response(content=xmlResponse, headers={ "Content-Type": "application/xml" })


@app.post("/api/picklists/import")
async def post_pick_lists(req: Request):
    if not isXML(req):
        raise HTTPException(status_code=400, detail="Not XML")
    
    try:
        body = await req.body()
        data = xmltodict.parse(body)
        products = data["ImportOperation"]["Lines"]["PicklistLine"]
    except:
        raise HTTPException(status_code=400, detail="Wrong XML format")

    def checkProduct(product: dict):
        checkForField(product, "ExtPicklistId")

        transactionId = checkForField(product, "TransactionId")
        extOrderId = checkForField(product, "ExtOrderId")
        extOrderlineId = checkForField(product, "ExtOrderlineId")
        extProductId = checkForField(product, "ExtProductId")
        quantity = checkForField(product, "Quantity")

        return {
            "TransactionId": transactionId,
            "ExtOrderId": extOrderId,
            "ExtOrderlineId": extOrderlineId,
            "ExtProductId": extProductId,
            "Quantity": quantity
        }

    confirmedPickLines = None

    if isinstance(products, list):
        confirmedPickLines = []
        for product in products:
            confirmedPickLines.append(checkProduct(product=product))
    else:
        confirmedPickLines = checkProduct(product=products)

    xmlResponse = xmltodict.unparse({
        "ExportOperation": {
            "Lines": {
                "ConfirmedPickLine": confirmedPickLines
            }
        }
    }, pretty=True)

    return Response(content=xmlResponse, headers={ "Content-Type": "application/xml" })


if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="127.0.0.1", port=8000)