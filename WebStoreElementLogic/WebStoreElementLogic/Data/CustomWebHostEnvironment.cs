using Microsoft.Extensions.FileProviders;
using WebStoreElementLogic.Interfaces;

namespace WebStoreElementLogic.Data
{
    public class CustomWebHostEnvironment : ICustomWebHostEnvironment
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomWebHostEnvironment(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;

            ApplicationName = "WebStoreElementLogic";
        }

        public string EnvironmentName
        {
            get => _webHostEnvironment.EnvironmentName;
            set => _webHostEnvironment.EnvironmentName = value;
        }

        public string BaseUrl { get; set; }

        public string ApplicationName
        {
            get => _webHostEnvironment?.ApplicationName ?? "";
            set => _webHostEnvironment.ApplicationName = value;
        }

        public string ContentRootPath
        {
            get => _webHostEnvironment.ContentRootPath;
            set => _webHostEnvironment.ContentRootPath = value;
        }

        public IFileProvider ContentRootFileProvider
        {
            get => _webHostEnvironment.ContentRootFileProvider;
            set => _webHostEnvironment.ContentRootFileProvider = value;
        }

        public string WebRootPath
        {
            get => _webHostEnvironment.WebRootPath;
            set => _webHostEnvironment.WebRootPath = value;
        }

        public IFileProvider WebRootFileProvider
        {
            get => _webHostEnvironment.WebRootFileProvider;
            set => _webHostEnvironment.WebRootFileProvider = value;
        }
    }
}
