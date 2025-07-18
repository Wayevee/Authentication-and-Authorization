using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace AuthenticationAPI.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuration;

        public LoginModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [BindProperty]
        public Models.Credentials Credential { get; set; } = new Models.Credentials();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            // Verify Credntials and Create Security Context
            if (Credential.UserName == "admin" && Credential.Password == "password")
            {
                //Creating Claims

                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@gmail.com"),
                new Claim("Department", "HR"),
                new Claim("Role", "Admin"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2025-02-01")
                };
                //Add Claims to Identity
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //Assign identity to CalimsPrincipal

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                // Implementing Persistent cookie
                var authorizationPrperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Redirect("/index");
            }
            return Page();
        }
    }

}
