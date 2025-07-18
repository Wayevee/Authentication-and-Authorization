using AuthenticationandAuthorizationIdentityCore.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AuthenticationandAuthorizationIdentityCore.Model.Accounts;

namespace AuthenticationandAuthorizationIdentityCore.Pages.Account
{
    public class LoginMFAModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IEmail email;
        private readonly SignInManager<User> signInManager;
        private readonly EmailMFAVerification mfaVerification;

        [BindProperty]
        public EmailMFAVerification MFAVerification { get; set; } = new EmailMFAVerification();

        public LoginMFAModel(UserManager<User> userManager, IEmail email, SignInManager<User> signInManager, EmailMFAVerification mFAVerification)
        {
            this.userManager = userManager;
            this.email = email;
            this.signInManager = signInManager;
            mfaVerification = mFAVerification;
        }
        public async Task OnGetAsync(string Email, bool remeberMe)
        {
           mfaVerification.VerificationCode = string.Empty;
            mfaVerification.RememberMe = remeberMe;
            var testuser = Email;
            var user = await userManager.FindByEmailAsync(Email);

            //Generate Security Code
            var securityCode = await userManager.GenerateTwoFactorTokenAsync(user!, "Email");
            //Send to User
            await email.SendAsync(Email, "Two Factor Authenticator OPT", $"Please use this OTP {securityCode} to login");
        }

        public async Task<IActionResult> OnPostAsync(string Email, bool remeberMe)
        {
            if (!ModelState.IsValid) return Page();
            var user = await userManager.FindByEmailAsync(Email);
            var response = await signInManager.TwoFactorSignInAsync("Email", this.MFAVerification.VerificationCode!, this.MFAVerification.RememberMe, false);

            if (!response.Succeeded) 
            {
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
