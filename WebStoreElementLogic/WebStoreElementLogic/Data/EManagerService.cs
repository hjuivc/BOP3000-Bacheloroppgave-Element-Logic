using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using WebStoreElementLogic.Entities;
using WebStoreElementLogic.Data;

public class EManagerService : ApiServiceBase, IEManagerService
{

    public EManagerService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
    {
        try
        {
            BaseUrl = _configuration["Api:EManager:BaseUrl"];
            string username = _configuration["Api:EManager:Username"];
            string password = _configuration["Api:EManager:Password"];

            _httpClient.DefaultRequestHeaders.Authorization = CreateAuthHeader(username, password);
        }
        catch(Exception e)
        { 
            Console.WriteLine($"Could not create EManagerService: {e.Message}");
            throw; 
        }
    }

    public async Task<bool> ProductInformation(Product product)
    {
        string xml = $@"
            <ImportOperation>
                <Lines>
                    <ProductLine>
                        <TransactionId>{product.Id}</TransactionId>
                        <ExtProductId>{product.Id}</ExtProductId>
                        <ProductName>{product.Name}</ProductName>
                        <ProductDesc>{product.Descr}</ProductDesc>
                        <ImageId>{product.URL}</ImageId>
                    </ProductLine>
                </Lines>
            </ImportOperation>            
        ";

        HttpResponseMessage? req = await Post("/api/products/import", xml);

        if (req != null)
        {
            int status = ((int)req.StatusCode);
            return status < 300 && status >= 200;
        }
        else
        {
            return false;
        }
    }

    private static AuthenticationHeaderValue CreateAuthHeader(string username, string password)
    {
        string credentials = $"{username}:{password}";
        string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
        return new AuthenticationHeaderValue("Basic", encodedCredentials);
    }
}