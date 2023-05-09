using Microsoft.Extensions.FileProviders;

namespace WebStoreElementLogic.Interfaces
{
    public interface ICustomWebHostEnvironment
    {
        string EnvironmentName { get; set; }
        string BaseUrl { get; set; }
        string ApplicationName { get; set; }
        string ContentRootPath { get; set; }
        IFileProvider ContentRootFileProvider { get; set; }
        string WebRootPath { get; set; }
        IFileProvider WebRootFileProvider { get; set; }
    }
}
