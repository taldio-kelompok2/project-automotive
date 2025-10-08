namespace AutomotiveApp.Application.Interfaces.Utils
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendConfirmationEmailAsync(string email, string userId, string token);
        Task SendPasswordResetEmailAsync(string email, string token);
    }
}
