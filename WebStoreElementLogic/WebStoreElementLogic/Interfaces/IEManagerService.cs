using WebStoreElementLogic.Entities;
public interface iEManagerService
{
    public Task<HttpResponseMessage> Post(string endpoint, string xml);
    public Task<bool> ProductInformation(Product product);
}