using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IInboundService
    {
        public Task<int> Create(Product product, double qty);
        //Task<int> Update(Product product);
        public Task<int> GetNextID();
        //Task<List<Product>> GetProduct(int Id);
    }
}
