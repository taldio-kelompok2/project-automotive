using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class ResetPasswordHandler(UserManager<User> userManager, ILogger<ResetPasswordHandler> logger)
        : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand req, CancellationToken cancellationToken)
        {
            string error;
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                error = "Email not found";
                logger.LogWarning("Reset password failed for Email={Email} Error={Error}",
                    req.Email,
                    error);
                throw new InvalidOperationException("Email not found");
            }

            var res = await userManager.ResetPasswordAsync(user, req.Token, req.NewPassword);
            if (!res.Succeeded)
            {
                error = res.Errors.Select(e => e.Description).ToString()!;
                logger.LogWarning("Reset password failed for Email={Email} Error={Error}",
                    user.Email,
                    error);
                throw new InvalidOperationException($"Reset password failed: {error}");
            }
            logger.LogInformation("Reset password successful for Email={Email}", user.Email);

            return true;
        }
    }
}
