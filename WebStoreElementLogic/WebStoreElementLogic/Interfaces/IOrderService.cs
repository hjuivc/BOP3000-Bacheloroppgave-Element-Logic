using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IOrderService
    {
        public Task<int[]> Create(Dictionary<Product, int> CartDict);
        public Task<string> Update(string ExtPickListId);
        public Task<int> GetNextID();
        //public Task<Inbound> GetOrder(int transactionId);

        //public Task<List<Inbound>> GetUnfinishedOrders();
    }
}
