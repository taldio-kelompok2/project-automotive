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
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == req.Id && u.Status);
            if (user == null)
                throw new KeyNotFoundException($"User with Id: {req.Id} not found");

            // soft delete
            user.Status = false;

            var res = await userManager.UpdateAsync(user);
            if (!res.Succeeded)
                throw new InvalidOperationException($"Soft delete user failed: {res.Errors.Select(e => e.Description)}");
            
            return true;
        }
    }
}
