namespace WebStoreElementLogic.Entities
{
    public class Order
    {
        public int ExtPickListId { get; set; }
        public int ExtOrderId { get; set; }
        public string ExtProductId { get; set; }
        public decimal Quantity { get; set; }
        public bool Status { get; set; }
        public int TransactionId { get; set; }
        public string? Name { get; set; }

        public Order(int extPickListId, int extOrderId, string extProductId, decimal quantity, bool status, int transactionId)
        {
            ExtPickListId = extPickListId;
            ExtOrderId = extOrderId;
            ExtProductId = extProductId;
            Quantity = quantity;
            Status = status;
            TransactionId = transactionId;
        }

    }
}
