using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Text;
using Talabat.Core.Entities.Email;
using Talabat.Core.Entities.Identity;
using Talabat.Core.IServices;

namespace Talabat.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmailService> _logger;

        public EmailService(UserManager<ApplicationUser> userManager, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _userManager = userManager;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body, // HTML email content
            };
            email.Body = bodyBuilder.ToMessageBody();
            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

        public async Task SendVerificationEmailAsync(ApplicationUser user)
        {
            var Codetoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(Codetoken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            var link = urlHelper.Action("VerifyEmail", "Account", new { email = user.Email, token = validEmailToken }, _httpContextAccessor.HttpContext.Request.Scheme);

            string htmlBody = $@"
                        <html>

                        <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f3f4f6;'>
                            <div
                                style='max-width: 600px; margin: 30px auto; background: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
                                <div style='background-color: #4CAF50; padding: 20px; text-align: center;'>
                                    <h1 style='color: white; margin: 0; font-size: 24px;'>Welcome to Talabat.E-Commerce</h1>
                                </div>
                                <div style='padding: 20px; color: #333333;'>
                                    <h2 style='margin-top: 0;'>Hello, {user.DisplayName}!</h2>
                                    <p style='font-size: 16px; line-height: 1.6;'>
                                        Thank you for signing up with <strong>Talabat.E-Commerce</strong>. We’re thrilled to have you
                                        onboard!
                                    </p>
                                    <p style='font-size: 16px; line-height: 1.6;'>
                                        To complete your registration, please verify your email address by clicking the button below:
                                    </p>
                                    <div style='text-align: center; margin: 30px 0;'>
                                        <a href='{link}'
                                            style='background-color: #4CAF50; color: white; padding: 12px 30px; font-size: 16px; font-weight: bold; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                            Verify Email
                                        </a>
                                    </div>

                                </div>
                                <div style='background-color: #f9f9f9; padding: 15px; text-align: center; font-size: 12px; color: #999999;'>
                                    <p style='margin: 5px 0 0;'>Need help? <a href='mailto:michealghobriall@gmail.com'
                                            style='color: #4CAF50;'>Contact
                                            Us</a></p>
                                </div>
                            </div>
                        </body>

                        </html>";
            try
            {
                await SendEmailAsync(user.Email, "Email Verification", htmlBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the email.", ex.Message);
            }
        }
    }
}
