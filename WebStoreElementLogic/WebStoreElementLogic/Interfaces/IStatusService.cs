using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IStatusService
    {
        public Task<List<Inbound>> GetUnfinishedInbounds();
        public Task<List<Order>> GetUnfinishedOrders();

    }
}
