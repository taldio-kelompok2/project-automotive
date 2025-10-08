using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public class ConfirmEmailHandler(UserManager<User> userManager)
        : IRequestHandler<ConfirmEmailCommand, bool>
    {
        public async Task<bool> Handle(ConfirmEmailCommand req, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(req.UserId);
            if (user == null)
                throw new InvalidOperationException($"Invalid email confirmation");

            var res = await userManager.ConfirmEmailAsync(user, req.Token);

            if (!res.Succeeded)
                throw new InvalidOperationException($"Email confirmation failed: {res.Errors.Select(e => e.Description)}");

            return true;
        }
    }
}
