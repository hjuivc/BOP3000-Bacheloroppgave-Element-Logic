using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using WebStoreElementLogic.Entities;

public class EManagerService : IEManagerService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    private string BaseUrl;


    public EManagerService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        BaseUrl = _configuration["Api:EManager:BaseUrl"];
        string username = _configuration["Api:EManager:Username"];
        string password = _configuration["Api:EManager:Password"];

        _httpClient.DefaultRequestHeaders.Authorization = CreateAuthHeader(username, password);
    }

    public async Task<HttpResponseMessage> Post(string endpoint, string xml)
    {
        try
        {
            return await _httpClient.PostAsync(
                BaseUrl + endpoint,
                new StringContent(xml, Encoding.UTF8, "application/xml")
            );
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Problem posting request: {ex.Message}");
        }
        // Crashes application
        return null;
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

        var req = await Post("/api/products/import", xml);

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