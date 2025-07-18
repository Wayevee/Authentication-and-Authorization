
namespace AuthenticationandAuthorizationIdentityCore.Utils
{
    public interface IEmail
    {
        Task SendAsync(string toEmail, string subject, string body);
    }
}