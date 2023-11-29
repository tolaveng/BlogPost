using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MongoDB.Bson;
using System.Security.Claims;

namespace WebApp.Auth
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserService userService;

        public AppAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var unAuthState = new AuthenticationState(new ClaimsPrincipal());

            // From cookie
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return unAuthState;
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return unAuthState;
            }

            // Find using in MongoDb
            var id = user.FindFirst(ClaimTypes.NameIdentifier);
            if (id == null || string.IsNullOrEmpty(id.Value) || !ObjectId.TryParse(id.Value, out var userId))
            {
                return unAuthState;
            }

            var userDto = await userService.GetUserByIdAsync(userId);
            if (userDto == null)
            {
                return unAuthState;
            }

            // return new princial
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.Name, userDto.Email),
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            return await Task.FromResult(new AuthenticationState(principal));
        }
    }
}
