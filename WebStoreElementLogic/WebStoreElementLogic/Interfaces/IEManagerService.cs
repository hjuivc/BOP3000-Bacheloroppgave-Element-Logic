using WebStoreElementLogic.Entities;
using System.Net.Http.Headers;
public interface IEManagerService
{
    public Task<bool> ProductInformation(Product product);
}