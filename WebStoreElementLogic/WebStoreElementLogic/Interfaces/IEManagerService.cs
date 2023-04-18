using WebStoreElementLogic.Entities;
using System.Net.Http.Headers;
public interface IEManagerService
{
    public Task<bool> ProductInformation(Product product);
    public Task<bool> GoodsReceival(Product product, double qty);
}