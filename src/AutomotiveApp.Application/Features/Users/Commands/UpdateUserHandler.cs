using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class UpdateUserHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<UpdateUser, bool>
    {
        public async Task<bool> Handle(UpdateUser req, CancellationToken ct)
        {
            var existingUser = await userManager.FindByIdAsync(req.Id.ToString());
            if (existingUser == null)
                throw new KeyNotFoundException($"User with ID {req.Id} not found");

            mapper.Map(req.UserUpdateDto, existingUser);
            
            var res = await userManager.UpdateAsync(existingUser);
            if (!res.Succeeded)
                throw new InvalidOperationException($"User update failed: {res.Errors.Select(e => e.Description)}");

            return true;
        }
    }
}
