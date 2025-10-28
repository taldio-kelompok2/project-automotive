using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }
            
            return true;
        }
    }
}
