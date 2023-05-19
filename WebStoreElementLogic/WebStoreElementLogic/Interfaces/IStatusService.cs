using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IStatusService
    {
        Task<int> DeleteInbound(int id);
        Task<int> DeleteOrder(int id);
        public Task<List<Inbound>> GetUnfinishedInbounds();
        public Task<List<Order>> GetUnfinishedOrders();

    }
}
