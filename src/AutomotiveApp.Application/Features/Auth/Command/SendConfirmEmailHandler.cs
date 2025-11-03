using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class SendConfirmEmailHandler(UserManager<User> userManager, IEmailService emailService)
        : IRequestHandler<SendConfirmEmailCommand, bool>
    {
        public async Task<bool> Handle(SendConfirmEmailCommand req, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null) return true; // Don't reveal if user exists

            if (await userManager.IsEmailConfirmedAsync(user))
                throw new InvalidOperationException("Email is already confirmed");

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailService.SendConfirmationEmailAsync(user.Email!, user.Id.ToString(), token);

            return true;
        }
    }
}
