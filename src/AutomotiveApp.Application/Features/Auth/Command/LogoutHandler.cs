using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class LogoutHandler(UserManager<User> userManager)
        : IRequestHandler<LogoutCommand, bool>
    {
        public async Task<bool> Handle(LogoutCommand req, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(req.UserId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.UtcNow;
                await userManager.UpdateAsync(user);

                return true;
            }

            return false;
        }
    }
}
