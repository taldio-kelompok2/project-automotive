using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class SendConfirmEmailHandler(UserManager<User> userManager, IEmailService emailService, ILogger<SendConfirmEmailHandler> logger)
        : IRequestHandler<SendConfirmEmailCommand, bool>
    {
        public async Task<bool> Handle(SendConfirmEmailCommand req, CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending confirmation email to Email={Email}", req.Email);

            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                logger.LogWarning("Send confirmation email failed. Email={Email} not found", req.Email);
                return true; // Don't reveal if user exists
            }

            if (await userManager.IsEmailConfirmedAsync(user))
            {
                logger.LogWarning("Send confirmation email failed. Email={Email} already confirmed", req.Email);
                throw new InvalidOperationException("Email is already confirmed");
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailService.SendConfirmationEmailAsync(user.Email!, user.Id.ToString(), token);

            logger.LogInformation("Confrmation email successfuly sent to Email={Email}, Token={Token}",
                req.Email,
                token);

            return true;
        }
    }
}
