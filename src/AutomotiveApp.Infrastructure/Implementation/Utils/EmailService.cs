using AutomotiveApp.Application.Interfaces.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AutomotiveApp.Infrastructure.Implementation.Utils
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IConfiguration _configuration;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IConfiguration configuration)
        {
            _emailSettings = emailSettings.Value;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = _emailSettings.EnableSsl,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw new InvalidOperationException("Failed to send email", ex);
            }
        }
        public async Task SendConfirmationEmailAsync(string email, string userId, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            // TODO: should be frontend url for POST request to work
            var appBaseUrl = _configuration["AppBaseUrl"] ?? "http://localhost:5001/api/auth";
            var confirmationLink = $"{appBaseUrl}/confirm-email?userId={userId}&token={encodedToken}";

            var subject = "Confirm Your Email - Otomobil";
            var body = $"""
            <!DOCTYPE html>
            <html>
            <head>
            </head>
            <body>
                <div>
                    <h2>Welcome to Otomobil!</h2>
                    <p>Click the link below to confirm your email:</p>
                    <p>
                        <a href="{confirmationLink}">Confirm Email</a>
                    </p>
                    <p>Or copy and paste this link in your browser:</p>
                    <p>{confirmationLink}</p>
                    <div>
                        <p>If you didn't create an account, please ignore this email.</p>
                    </div>
                </div>
            </body>
            </html>
            """;

            await SendEmailAsync(email, subject, body);

            await Task.CompletedTask;
        }

        public async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            // TODO: should be frontend url for POST request to work
            var appBaseUrl = _configuration["AppBaseUrl"] ?? "http://localhost:5001/api/auth";
            var resetLink = $"{appBaseUrl}/reset-password?email={email}&token={encodedToken}";

            var subject = "Reset Your Password - Otomobil";
            var body = $"""
            <!DOCTYPE html>
            <html>
            <head>
            </head>
            <body>
                <div>
                    <h2>Password Reset Request</h2>
                    <p>Click the link below to set a new password:</p>
                    <p>
                        <a href="{resetLink}">Reset Password</a>
                    </p>
                    <p>Or copy and paste this link in your browser:</p>
                    <p>{resetLink}</p>
                    <div>
                        <p>This link will expire in 1 hour. If you didn't request a password reset, please ignore this email.</p>
                    </div>
                </div>
            </body>
            </html>
            """;

            await SendEmailAsync(email, subject, body);
        }
    }
}
