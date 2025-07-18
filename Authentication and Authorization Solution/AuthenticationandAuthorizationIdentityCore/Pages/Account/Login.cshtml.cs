using AuthenticationandAuthorizationIdentityCore.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AuthenticationandAuthorizationIdentityCore.Model.Accounts;

namespace AuthenticationandAuthorizationIdentityCore.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManger;

        public LoginModel(SignInManager<User> signInManger)
        {
            _signInManger = signInManger;
        }
        [BindProperty]
        public Login Credential { get; set; } = new Login();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var response = await _signInManger.PasswordSignInAsync(
                Credential.Email, 
                Credential.Password,
                Credential.RememberMe, 
                false);
           
            if (!response.Succeeded)
            {
                if (response.RequiresTwoFactor)
                {
                    return RedirectToPage("/Account/LoginMFA", new
                    {
                       Credential.Email,
                       Credential.RememberMe
                    });
                }
                if (response.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "Account is locked out. Please try again later.");
                }
                else
                {
                    ModelState.AddModelError("Login", "Invalid login attempt. Please check your credentials.");
                }
                return Page();
            }

            return RedirectToPage("/Index");



        }
    }
}

