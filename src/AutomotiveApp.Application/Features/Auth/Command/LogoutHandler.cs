using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class LogoutHandler(UserManager<User> userManager, ILogger<LogoutHandler> logger)
        : IRequestHandler<LogoutCommand, bool>
    {
        public async Task<bool> Handle(LogoutCommand req, CancellationToken cancellationToken)
        {
            logger.LogInformation("Logout request from UserId={Id}", req.UserId);

            var user = await userManager.FindByIdAsync(req.UserId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.UtcNow;
                await userManager.UpdateAsync(user);
                logger.LogInformation("Logout success for UserId={Id}", req.UserId);
                return true;
            }
            logger.LogInformation("Logout failed for UserId={Id}", req.UserId);
            return false;
        }
    }
}
