using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace WebApp.Pages.Accounts
{
    public class LoginModel : PageModel
    {

        private readonly IUserService userService;

        [BindProperty]
        public UserDto UserInput { get; set; } = new UserDto();

        [TempData]
        public string ErrorMessage { get; set; } = String.Empty;

        public string ReturnUrl { get; set; } = String.Empty;

        public LoginModel(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task OnGetAsync([FromQuery] string? returnUrl)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl;
            ErrorMessage = string.Empty;

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] string? returnUrl)
        {
            ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return Page();

            // Return Url
            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                if (Uri.IsWellFormedUriString(returnUrl, UriKind.Absolute) && !returnUrl.StartsWith(baseUrl))
                {
                    return Unauthorized();
                }
            }
            else
            {
                returnUrl = baseUrl;
            }
            
            
            if (String.IsNullOrWhiteSpace(UserInput.Username) || String.IsNullOrWhiteSpace(UserInput.Password))
            {
                ErrorMessage = "Invalid Username or Password";
                //ModelState.Clear();
                UserInput.Password = String.Empty;
                return Page();
            }


            // Do sign in
            // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
            var loggedInUser = await userService.AuthenticateUserAsync(UserInput.Username, UserInput.Password);
            if (loggedInUser != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {   
                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                return Redirect(returnUrl);
            }


            ErrorMessage = "Invalid Username or Password";
            UserInput.Password = String.Empty;
            ModelState.AddModelError(string.Empty, "Invalid Username or Password");
            return Page();
        }
    }
}
