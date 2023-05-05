namespace WebStoreElementLogic.Entities
{
    public class PicklistLine
    {
        public int TransactionId { get; set; }
        public string ExtPicklistId { get; set; }
        public int ExtOrderId { get; set; }
        public string ExtProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
