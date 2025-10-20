using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class UpdateUserHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<UpdateUser, bool>
    {
        public async Task<bool> Handle(UpdateUser req, CancellationToken ct)
        {
            var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == req.Id);
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {req.Id} not found");

            mapper.Map(req.UserUpdateDto, existingUser);
            
            var res = await userManager.UpdateAsync(existingUser);
            if (!res.Succeeded)
                throw new InvalidOperationException($"User update failed: {res.Errors.Select(e => e.Description)}");

            var currRole = await userManager.GetRolesAsync(existingUser);
            if (req.UserUpdateDto.Role != currRole.FirstOrDefault() && currRole.FirstOrDefault() != null)
            {
                await userManager.RemoveFromRoleAsync(existingUser, currRole.First());
                await userManager.AddToRoleAsync(existingUser, req.UserUpdateDto.Role);
            }

            return true;
        }
    }
}
