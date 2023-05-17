using Microsoft.JSInterop;
using System.Net;
using WebStoreElementLogic.Interfaces;

namespace WebStoreElementLogic.Data
{
    public class SimpleCookieManager : ISimpleCookieManager
    {
        private readonly IJSRuntime _runtime;
        private readonly static string _authCookieName = "ELWebShopAuth";

        public SimpleCookieManager(IJSRuntime runtime) 
        {
            _runtime = runtime;
        }

        public async Task<string[]?> GetAuthCookie()
        {
            string? cookie = await GetCookie(_authCookieName);
            if (cookie == null)
            {
                return null;
            }
            return cookie.Split(":");
        }

        public async Task PlaceAuthCookie(string username, string password)
        {
            // TODO: add hashing
            string cookie = $"{username}:{password}";
            await PlaceCookie(_authCookieName, cookie);
        }

        public async Task PlaceCookie(string cookieName, string cookieData)
        {
            string cookieString = $"document.cookie = '{cookieName}={cookieData}';";
            await _runtime.InvokeVoidAsync("eval", cookieString);
        }

        public async Task<string?> GetCookie(string cookieName)
        {
            var allCookies = await _runtime.InvokeAsync<string>("eval", new string[] { "document.cookie" });
            return allCookies
                .Split(';')
                .FirstOrDefault(part => part.StartsWith($"{cookieName}=") || part.StartsWith($" {cookieName}="))?
                .Split('=')[1];
        }

    }
}
