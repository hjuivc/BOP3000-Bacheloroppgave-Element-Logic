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

    public async Task<bool> GoodsReceipt(Product product, double qty)
    {
        string xml = $@"
            <ImportOperation>
                <Lines>
                    <GoodsReceivalLine>
                        <TransactionId>{999999}</TransactionId>
                        <PurchaseOrderId>{999999}</PurchaseOrderId>
                        <PurchaseOrderLineId>{9999}</PurchaseOrderLineId>
                        <ExtProductId>{product.Id}</ExtProductId>
                        <Quantity>{qty}</Quantity>
                    </GoodsReceivalLine>
                </Lines>
            </ImportOperation>         
        ";

        HttpResponseMessage? req = await Post("/api/goodsreceivals/import", xml);

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

    public async Task<bool> ImportPicklist(List<PicklistLine> picklistLines)
    {
        if (picklistLines == null || !picklistLines.Any())
        {
            throw new ArgumentException("Picklist lines cannot be null or empty.");
        }

        StringBuilder xmlLines = new StringBuilder();
        foreach (var line in picklistLines)
        {
            xmlLines.Append($@"
            <PicklistLine>
                <TransactionId>{line.TransactionId}</TransactionId>
                <ExtPicklistId>{line.ExtPicklistId}</ExtPicklistId>
                <ExtOrderId>{line.ExtOrderId}</ExtOrderId>
                <ExtProductId>{line.ExtProductId}</ExtProductId>
                <Quantity>{line.Quantity}</Quantity>
            </PicklistLine>");
        }

        string xml = $@"
        <ImportOperation>
            <Lines>
                {xmlLines}
            </Lines>
        </ImportOperation>";

        HttpResponseMessage? req = await Post("/api/picklists/import", xml);

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

    public class PicklistLine
    {
        public int TransactionId { get; set; }
        public string ExtPicklistId { get; set; }
        public string ExtOrderId { get; set; }
        public string ExtProductId { get; set; }
        public decimal Quantity { get; set; }
    }


    private static AuthenticationHeaderValue CreateAuthHeader(string username, string password)
    {
        string credentials = $"{username}:{password}";
        string encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
        return new AuthenticationHeaderValue("Basic", encodedCredentials);
    }
}