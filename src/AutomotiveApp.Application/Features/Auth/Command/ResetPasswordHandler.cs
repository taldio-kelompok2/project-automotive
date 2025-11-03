using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class ResetPasswordHandler(UserManager<User> userManager)
        : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand req, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null)
                throw new InvalidOperationException($"Invalid reset password");

            var res = await userManager.ResetPasswordAsync(user, req.Token, req.NewPassword);
            if (!res.Succeeded)
                throw new InvalidOperationException($"Reset password error: {res.Errors.Select(e => e.Description)}");

            return true;
        }
    }
}
