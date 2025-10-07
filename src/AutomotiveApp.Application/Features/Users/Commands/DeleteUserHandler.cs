using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class DeleteUserHandler(UserManager<User> userManager)
        : IRequestHandler<DeleteUser, bool>
    {
        public async Task<bool> Handle(DeleteUser req, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(req.Id.ToString());
            if (user == null)
                throw new KeyNotFoundException($"User with Id: {req.Id} not found");

            var res = await userManager.DeleteAsync(user);
            if (!res.Succeeded)
                throw new InvalidOperationException($"Update password failed: {res.Errors.Select(e => e.Description)}");
            
            return true;
        }
    }
}
