using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetUserByIdHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<GetUserById, UserProfileDto>
    {
        public async Task<UserProfileDto> Handle(GetUserById req, CancellationToken ct)
        {
            var user = await userManager.Users
                .AsSplitQuery()
                .Include(u => u.Orders)
                .Include(u => u.Bookings)
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.Id == req.UserId && u.Status, ct);
            if (user == null)
                throw new KeyNotFoundException($"User with Id: {req.UserId} not found");

            var res = mapper.Map<UserProfileDto>(user);

            var roles = await userManager.GetRolesAsync(user);
            res.Roles = roles.ToList();

            return res;
        }
    }
}
