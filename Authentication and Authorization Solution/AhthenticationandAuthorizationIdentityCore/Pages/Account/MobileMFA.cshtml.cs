using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AuthenticationandAuthorizationIdentityCore.Model.Accounts;

namespace AuthenticationandAuthorizationIdentityCore.Pages.Account
{
    public class MobileMFAModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public SetupMobileMFAVerification MobileMFA { get; set; }
        public MobileMFAModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.MobileMFA = new SetupMobileMFAVerification();
        }
        public async Task OnGet()
        {
            var user = await userManager.GetUserAsync(base.User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return;
            }
            await userManager.ResetAuthenticatorKeyAsync(user);
            string? key = await userManager.GetAuthenticatorKeyAsync(user);

            MobileMFA.Key = key ?? string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var user = await userManager.GetUserAsync(base.User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return Page();
            }
            await userManager.VerifyTwoFactorTokenAsync(user, "Mobile", MobileMFA.Key);
        }
    }
}
