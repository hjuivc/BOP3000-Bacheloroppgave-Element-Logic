using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStoreElementLogic.Entities;
using WebStoreElementLogic.Interfaces;

namespace WebStoreElementLogic.Account
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public CustomAuthenticationStateProvider(
            IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var user = httpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            var claims = new List<Claim>(user.Claims);

            var dbUser = await _userService.GetUser(user.Identity.Name);
            if (dbUser != null && dbUser.admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var name = httpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Name, name));
            }

            var roles = httpContext.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }


        public async Task LogoutAsync()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = new AuthenticationState(anonymousUser);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }


        public void NotifyAuthenticationStateChanged(AuthenticationState state)
        {
            Task.Run(() => NotifyAuthenticationStateChangedAsync(state));
        }

        public async Task NotifyAuthenticationStateChangedAsync(AuthenticationState state)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }
    }
}
