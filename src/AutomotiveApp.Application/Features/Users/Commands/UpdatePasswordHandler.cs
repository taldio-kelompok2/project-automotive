using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class UpdatePasswordHandler(UserManager<User> userManager)
        : IRequestHandler<UpdatePassword, bool>
    {
        public async Task<bool> Handle(UpdatePassword req, CancellationToken ct)
        {

            var user = await userManager.FindByIdAsync(req.Id.ToString());
            if (user == null)
                throw new KeyNotFoundException($"User with Id: {req.Id} not found");

            var res = await userManager.ChangePasswordAsync(
                user,
                req.UpdatePasswordDto.CurrentPassword,
                req.UpdatePasswordDto.NewPassword);

            if (!res.Succeeded)
                // keep error message same as DTO
                throw new InvalidOperationException($"Passwords do not match");

            return true;
        }
    }
}
