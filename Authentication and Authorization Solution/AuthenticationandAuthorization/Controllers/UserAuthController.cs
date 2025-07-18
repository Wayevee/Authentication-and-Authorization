using AuthenticationandAuthorization.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationandAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public UserAuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginCredentials loginCredentials)
        {
            //Verify the credentials
            if (loginCredentials.Username == "admin" && loginCredentials.Password == "password")
            {
                //List Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginCredentials.Username),
                    //new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Email, "admin@gmail.com"),
                    new Claim("Department", "HR"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2020-01-01")

                };
                //Set Expiry Period
                var expiresAt = DateTime.UtcNow.AddHours(10);
                // Generate Token
                var token = GenerateJwtToken(claims, expiresAt);
                //return Token
                return Ok(new
                {
                    access_token = token,
                    expires_at = expiresAt

                });
            }
            ModelState.AddModelError("LoginError", "Invalid username or password");
                return Unauthorized(ModelState);
             
        }
        private string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            //Convert Secret Key to array of bytes
            var secretKey = Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"] ?? "");
            //Set JWT Token Claims
            var jwtClaims = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                // Set siging Credentials
                signingCredentials: new SigningCredentials( new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
               
            );
           
            return new JwtSecurityTokenHandler().WriteToken(jwtClaims);
        }
    }
}
