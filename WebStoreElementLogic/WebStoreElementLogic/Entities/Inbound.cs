using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebStoreElementLogic.Entities
{
    public class Inbound
    {
        public int InboundId { get; set; }
        public int TransactionId { get; set; }
        public string PurchaseOrderId { get; set; }
        public int PurchaseOrderLineId { get; set; }
        public string ExtProductId { get; set; }
        public decimal Quantity { get; set; }
        public bool Status { get; set; }
        public string? Name { get; set; }
        public int DisplayQuantity => (int)Quantity;

        public Inbound(int inboundId, int transactionId, string purchaseOrderId, int purchaseOrderLineId, string extproductId, decimal quantity, bool status) 
        {
            InboundId = inboundId;
            TransactionId = transactionId;
            PurchaseOrderId = purchaseOrderId;
            PurchaseOrderLineId= purchaseOrderLineId;
            ExtProductId = extproductId;
            Quantity = quantity;
            Status = status;

        }

        public Inbound()
        {
            
        }
    }
}
