using AutomotiveApp.Application.Interfaces.Utils;
using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class ForgotPasswordHandler(UserManager<User> userManager, IEmailService emailService, ILogger<ForgotPasswordHandler> logger)
        : IRequestHandler<ForgotPasswordCommand, bool>
    {
        public async Task<bool> Handle(ForgotPasswordCommand req, CancellationToken cancellationToken)
        {
            string error;
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                error = "Email not found";
                logger.LogWarning("Forgot password failed for Email={Email} Error={Error}",
                    req.Email,
                    error);
                throw new KeyNotFoundException(error);
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                error = "Email needs to be confirmed for password reset";
                logger.LogWarning("Forgot password failed for Email={Email} Error={Error}",
                    req.Email,
                    error);
                throw new InvalidOperationException(error);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            await emailService.SendPasswordResetEmailAsync(req.Email, token);

            logger.LogInformation("POST /api/auth/forgot-password - Forgot password email successfuly sent to Email={Email}, Token={Token}",
                req.Email,
                token);

            return true;
        }
    }
}
