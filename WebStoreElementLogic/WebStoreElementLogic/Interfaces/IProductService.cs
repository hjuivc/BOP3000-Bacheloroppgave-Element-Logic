using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Interfaces
{
    public interface IProductService
    {
        Task<int> Create(Product product);
        Task<int> Delete(int id);
        Task<int> Count(string search);
        Task<int> Update(Product product);
        Task<Product> GetByID(int id);
        Task<List<Product>> ListAll(int page, int pageSize, string sortColumnName, string sortDirection, string searchTerm);
        Task<List<Product>> ListAllRefresh(string searchTerm, int page, int pageSize, string sortColumnName, string sortDirection);
        Task<List<Product>> GetProduct(int Id);
        Task<List<Product>> GetNextID(int Id);
        Task<List<Product>> GetProductNames(string Name);
        Task<List<Product>> GetProducts(int? id, string name, string descr);
    }
}
