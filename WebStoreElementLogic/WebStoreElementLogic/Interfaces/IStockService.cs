using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IStockService
    {
        Task<int> EmptyStockAsync(int productId);
    }
}