using Talabat.Core.Entities.Identity;

namespace Talabat.Core.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendVerificationEmailAsync(ApplicationUser user);
    }
}
