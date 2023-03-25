using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using WebStoreElementLogic.Entities;

public class EManagerService : iEManagerService
{
    private readonly HttpClient _httpClient;
    private string BaseUrl = "193.69.50.119";
    private string Username = "apiuser";
    private string Password = "1994";

    public void addAuth()
    {
        _httpClient.DefaultRequestHeaders.Authorization = CreateAuthHeader(Username, Password);
    }

    public async Task<HttpResponseMessage> Post(string endpoint, string xml)
    {
        return await _httpClient.PostAsync(
            BaseUrl + endpoint, 
            new StringContent(xml, Encoding.UTF8, "application/xml"
        ));
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
        int status = ((int)req.StatusCode);

        return status < 300 && status >= 200;
    }

    private static AuthenticationHeaderValue CreateAuthHeader(string username, string password)
    {
        string credentials = $"{username}:{password}";
        string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
        return new AuthenticationHeaderValue("Basic", encodedCredentials);
    }
}