using WebStoreElementLogic.Entities;
using System.Net.Http.Headers;
using static EManagerService;

public interface IEManagerService
{
    public Task<bool> ProductInformation(Product product);
    public Task<bool> GoodsReceival(Product product, double qty, int purchaseOrderId, int transactionId);
    public Task<bool> ImportPicklist(List<PicklistLine> picklistLines);

}