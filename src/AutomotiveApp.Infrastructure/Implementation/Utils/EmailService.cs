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
            var appBaseUrl = _configuration["AppBaseUrl"] ?? "http://localhost:5160";
            var confirmationLink = $"{appBaseUrl}/confirm-email?userId={userId}&token={encodedToken}";

            var subject = "Confirm Your Email - Otomobil";
            var body = GenerateStyledEmail(
                title: "Welcome to Otomobil!",
                message: "Please confirm your email address by clicking the button below.",
                buttonText: "Confirm Email",
                link: confirmationLink
            );

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            var appBaseUrl = _configuration["AppBaseUrl"] ?? "http://localhost:5160";
            var resetLink = $"{appBaseUrl}/new-password?email={email}&token={encodedToken}";

            var subject = "Reset Your Password - Otomobil";
            var body = GenerateStyledEmail(
                title: "Password Reset Request",
                message: "Click the button below to reset your password. This link will expire in 1 hour.",
                buttonText: "Reset Password",
                link: resetLink
            );

            await SendEmailAsync(email, subject, body);
        }

        private string GenerateStyledEmail(string title, string message, string buttonText, string link)
        {
            const string logoUrl = "https://i.ibb.co.com/NnFpf1Zr/27b4c045de3c5777298487dcd1e1a033fcf47569.png";

            return $"""
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                </head>
                <body style="margin:0; padding:0; background-color:#f4f6f8; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">

                    <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f6f8; padding:30px 0;">
                        <tr>
                            <td align="center">
                                <table role="presentation" width="600" cellpadding="0" cellspacing="0" style="background-color:#ffffff; border-radius:10px; overflow:hidden; box-shadow:0 4px 10px rgba(0,0,0,0.1);">
                                    <tr>
                                        <td style="background-color:#790B0A; padding:20px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <img src="{logoUrl}" style="height:140px;">
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="padding:30px; padding-top:0px; text-align:center; color:#333333;">
                                            <h2 style="margin-bottom:10px; color:#790B0A; font-size:24px;">{title}</h2>
                                            <p style="font-size:16px; line-height:1.5; margin:0 0 20px;">{message}</p>

                                            <a href="{link}" 
                                               style="display:inline-block; margin-top:10px; padding:12px 24px; background-color:#790B0A; 
                                                      color:#ffffff; text-decoration:none; border-radius:6px; font-weight:bold;">
                                                {buttonText}
                                            </a>

                                            <p style="margin-top:25px; font-size:14px; color:#555;">Or copy this link:</p>
                                            <p style="word-break:break-all; color:#555; font-size:13px;">{link}</p>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="center" style="background-color:#f4f6f8; padding:20px; font-size:13px; color:#888888;">
                                            <p style="margin:5px 0 0;">&copy; {DateTime.Now.Year} Otomobil. All rights reserved.</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </body>
                </html>
                """;
        }
    }
}
