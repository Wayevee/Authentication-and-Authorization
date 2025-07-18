using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationandAuthorizationIdentityCore.Model
{
    public class Accounts
    {
        public class Registration
        {
            [Required]
            [EmailAddress(ErrorMessage = "Inavlid Email Address")]
            public string Email { get; set; } = string.Empty;
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            public string Department { get; set; } = string.Empty;
            public string Position { get; set; } = string.Empty;
        }
        public class Login
        {
            [Required]
            [EmailAddress(ErrorMessage = "Inavlid Email Address")]
            public string Email { get; set; } = string.Empty;
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }

        public class User: IdentityUser
        {
            public string Department { get; set; } = string.Empty;
            public string Position { get; set; } = string.Empty;
        }

        public class UserProfile
        {
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Department { get; set; } = string.Empty;
            [Required]
            public string Position { get; set; } = string.Empty;

        }

        public class EmailMFAVerification
        {
            [Required]
            [Display(Name = "Verification Code")]
            public string? VerificationCode { get; set; }
            public bool RememberMe { get; set; } = false;
          
        }
        public class SetupMobileMFAVerification
        {
            [Required]
            public string? Key { get; set; }
            [Required]
            [Display(Name = "Verification Code")]
            public string VerificationCode { get; set; } = string.Empty;
        }
    }
   
}
