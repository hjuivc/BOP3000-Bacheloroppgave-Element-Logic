using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebStoreElementLogic.Entities
{
    public class Order
    {
        public int ExtPickListId { get; set; }
        public int ExtOrderId { get; set; }
        public int ExtOrderLineId { get; set; }
        public string ExtProductId { get; set; }
        public decimal Quantity { get; set; }
        public bool Status { get; set; }
        public int TransactionId { get; set; }
        public string? Name { get; set; }
        public int DisplayQuantity => (int)Quantity;

        public Order(int extPickListId, int extOrderId, int extOrderLineId, string extProductId, decimal quantity, bool status, int transactionId)
        {
            ExtPickListId = extPickListId;
            ExtOrderId = extOrderId;
            ExtOrderLineId = extOrderLineId;
            ExtProductId = extProductId;
            Quantity = quantity;
            Status = status;
            TransactionId = transactionId;
        }
        public Order()
        {

        }
    }
}
