using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using WebStoreElementLogic.Entities;

namespace WebStoreElementLogic.Data
{
    public abstract class ApiServiceBase
    {

        protected readonly HttpClient _httpClient;
        protected readonly IConfiguration _configuration;

        protected string? BaseUrl;

        public ApiServiceBase(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage?> Post(string endpoint, string xml)
        {
            try
            {
                return await _httpClient.PostAsync(
                    BaseUrl ?? "" + endpoint,
                    new StringContent(xml, Encoding.UTF8, "application/xml")
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem posting request: {ex.Message}");
            }
            // Check for null response when using this method.
            // might need change later
            return null;
        }

        public async Task<HttpResponseMessage?> PostForm(string endpoint, MultipartFormDataContent formData)
        {
            try
            {
                return await _httpClient.PostAsync(
                    BaseUrl ?? "" + endpoint,
                    content: formData
                );
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Problem posting request: {ex.Message}");
            }
            return null;
        }

    }
}
