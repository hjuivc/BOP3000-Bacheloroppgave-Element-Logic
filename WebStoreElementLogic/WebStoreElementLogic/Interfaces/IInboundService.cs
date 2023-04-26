using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IInboundService
    {
        public Task<int> Create(Product product, double qty);
        public Task<int> Update(int transactionId);
        public Task<int> GetNextID();
        public Task<Inbound> GetInbound(int transactionId);
        public Task<int> GetTransactionId(int nextPurchaseOrderId);

        public Task<List<Inbound>> GetUnfinishedInbounds();
    }
}
