using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class ForgotPasswordHandler(UserManager<User> userManager, IEmailService emailService)
        : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null) 
                throw new KeyNotFoundException("Email not found");

            if (!await userManager.IsEmailConfirmedAsync(user))
                throw new InvalidOperationException("Email needs to be confirmed for password reset");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            await emailService.SendPasswordResetEmailAsync(req.Email, token);

            return true;
        }
    }
}
