using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IInboundService
    {
        public Task<int> Create(Product product, double qty);
        //Task<int> Update(Product product);
        public Task<int> GetNextID();
        public Task<Inbound> GetInbound(int transactionId);
        public Task<int> GetTransactionId(int nextPurchaseOrderId);

        public Task<List<Inbound>> GetUnfinishedInbounds();
    }
}
