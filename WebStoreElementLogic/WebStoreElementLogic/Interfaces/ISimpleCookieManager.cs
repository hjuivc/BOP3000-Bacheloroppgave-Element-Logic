namespace WebStoreElementLogic.Interfaces
{
    public interface ISimpleCookieManager
    {
        public Task<string[]?> GetAuthCookie();
        public Task PlaceAuthCookie(string username, string password);
        public Task PlaceCookie(string cookieName, string cookieData);
        public Task<string?> GetCookie(string cookieName);
    }
}
