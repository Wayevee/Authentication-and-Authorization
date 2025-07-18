using System.ComponentModel.DataAnnotations;

namespace AuthenticationandAuthorization.DTO
{
    public class LoginCredentials
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    };
}
