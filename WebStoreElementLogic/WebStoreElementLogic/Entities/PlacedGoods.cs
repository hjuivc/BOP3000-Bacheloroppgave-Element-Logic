using System.Xml.Linq;

namespace WebStoreElementLogic.Entities
{
    public class PlacedGoodsBody
    {
        public int TransactionId { get; set; }
        public string PurchaseOrderId { get; set; }
        public int PurchaseOrderLineId { get; set; }
        public int ExtProductId { get; set; }
        public decimal Quantity { get; set; }

        public override string ToString()
        {
            return $"PrdouctId: {ExtProductId}\nQuantity: {Quantity}";
        }

        public static PlacedGoodsBody[] FromXml(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            var transactionId = int.Parse(doc.Root.Element("TRANSACTIONID").Value);
            var putaways = doc.Root.Elements("PUTAWAY");
            List<PlacedGoodsBody> pgBodies = new List<PlacedGoodsBody>();

            foreach (var putaway in putaways)
            {
                pgBodies.Add(new PlacedGoodsBody
                {
                    TransactionId = transactionId,
                    PurchaseOrderId = putaway.Element("PURCHASEORDERID").Value,
                    PurchaseOrderLineId = int.Parse(putaway.Element("PURCHASEORDERLINEID").Value),
                    ExtProductId = int.Parse(putaway.Element("EXTPRODUCTID").Value),
                    Quantity = decimal.Parse(
                        putaway.Element("ACTQUANTITY").Value,
                        System.Globalization.CultureInfo.InvariantCulture
                    )
                });
            }

            return pgBodies.ToArray();
        }
    }
}
