using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class ConfirmEmailHandler(UserManager<User> userManager, ILogger<ConfirmEmailHandler> logger)
        : IRequestHandler<ConfirmEmailCommand, bool>
    {
        public async Task<bool> Handle(ConfirmEmailCommand req, CancellationToken cancellationToken)
        {
            string error;
            var user = await userManager.FindByIdAsync(req.UserId);
            if (user == null)
            {
                error = "Invalid email confirmation";
                logger.LogWarning("Confirm email failed for UserId={UserId} Error={Error}",
                    req.UserId,
                    error);
                throw new InvalidOperationException(error);
            }

            var res = await userManager.ConfirmEmailAsync(user, req.Token);

            if (!res.Succeeded)
            {
                error = res.Errors.Select(e => e.Description).ToString()!;
                logger.LogWarning("Confirm email failed for Email={Email} Error={Error}",
                    user.Email,
                    error);
                throw new InvalidOperationException($"Email confirmation failed: {error}");
            }
            logger.LogInformation("POST /api/auth/confirm-email - Successfully confirmed Email={Email}", user.Email);

            return true;
        }
    }
}
