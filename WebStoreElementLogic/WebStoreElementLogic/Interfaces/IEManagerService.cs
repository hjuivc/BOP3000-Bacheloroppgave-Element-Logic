using WebStoreElementLogic.Entities;
using System.Net.Http.Headers;
using static EManagerService;

public interface IEManagerService
{
    public Task<bool> ProductInformation(Product product);
    public Task<bool> GoodsReceipt(Product product, double qty);
    public Task<bool> ImportPicklist(List<PicklistLine> picklistLines);

}