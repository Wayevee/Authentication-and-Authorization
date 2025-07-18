using AuthenticationandAuthorizationIdentityCore.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static AuthenticationandAuthorizationIdentityCore.Model.Accounts;


namespace AuthenticationandAuthorizationIdentityCore.Pages.Accounts
{
   
    public class RegistrationModel(UserManager<User> userManager, IEmail? email) : PageModel
    {
        private readonly UserManager<User> userManager = userManager;
        private readonly IEmail? email = email;

        [BindProperty]
        public Registration Registration { get; set; } = new Registration();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Validate Email Address (Already Implemented in Program.cs)

            //Create User Account
            var user = new User
            {
                UserName = Registration.Email,
                Email = Registration.Email,

            };

            var departmentclaim = new Claim("Department", Registration.Department);
            var positionclaim = new Claim("Position", Registration.Position);

            var response =  await userManager.CreateAsync(user, Registration.Password);

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }
            await this.userManager.AddClaimAsync(user, departmentclaim);
            await this.userManager.AddClaimAsync(user, positionclaim);
            var emailConfirmation = await userManager.GenerateEmailConfirmationTokenAsync(user);
            return  Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userId = user.Id, token = emailConfirmation }) ?? "");


            // To implement the email confirmation logic, use implementation below

            //var subject = "Confirm your Email";
            //var body = $"Please confirm your email by clicking this link: {confirmationLink}";

            //await email.SendAsync(Registration.Email, subject, body);
            //return RedirectToPage("/Account/Login");
        }
    };
 
}
