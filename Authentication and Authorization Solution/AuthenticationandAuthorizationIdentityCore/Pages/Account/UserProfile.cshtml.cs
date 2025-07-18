using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static AuthenticationandAuthorizationIdentityCore.Model.Accounts;

namespace AuthenticationandAuthorizationIdentityCore.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public UserProfile? UserProfile { get; set; }

        public string? SuccessMessage { get; set; }
        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.UserProfile = new UserProfile();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            SuccessMessage = string.Empty;
           var (user, departmentClaim, positionClaim) = await GetUserInfo();
            if (user == null)
            {
                return Redirect("/Index");
            }
            var claims = await userManager.GetClaimsAsync(user!);

            var email = User.Identity;

            UserProfile.Email = User.Identity?.Name ?? string.Empty;
            UserProfile.Department = departmentClaim?.Value ?? string.Empty;
            UserProfile.Position = positionClaim?.Value ?? string.Empty;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var (user, departmentClaim, positionClaim) = await GetUserInfo();
                if (user != null && departmentClaim != null)
                {
                    await userManager.ReplaceClaimAsync(user!, departmentClaim!, new Claim(departmentClaim?.Type, UserProfile.Department));
                }
                if (user != null && positionClaim != null)
                {
                    await userManager.ReplaceClaimAsync(user!, positionClaim!, new Claim(positionClaim?.Type, UserProfile.Position));
                }
                SuccessMessage = "User profile updated successfully.";
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while updating user profile. Please try again.");
            }
           
            return Page();

        }
        private async Task<(User? user, Claim? departmantClaim, Claim? positionClaim)> GetUserInfo()
        {
            var user = await userManager.FindByNameAsync(User?.Identity.Name ?? string.Empty);
            if (user == null)
            {
                return (null, null, null);
            }
            var claims = await userManager.GetClaimsAsync(user!);

            var email = User.Identity;

            UserProfile.Email = User.Identity?.Name ?? string.Empty;
            Claim? departmantClaim = claims.FirstOrDefault(c => c.Type == "Department");
            Claim? positionClaim = claims.FirstOrDefault(c => c.Type == "Position");
            return (user, departmantClaim, positionClaim);
        }
    }
}
