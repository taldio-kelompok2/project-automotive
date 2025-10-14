using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetUsersPagedHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<GetUsersPaged, PaginatedResult<UserQueryDto>>
    {
        public async Task<PaginatedResult<UserQueryDto>> Handle(GetUsersPaged req, CancellationToken ct)
        {
            var query = userManager.Users.Where(u => u.Status);
            var total = await query.CountAsync();
            if (total < 0)
                throw new KeyNotFoundException($"No users found");

            var users = await query
                .Include(u => u.Orders)
                .Include(u => u.Bookings)
                .Include(u => u.Cart)
                .OrderBy(u => true)
                .Skip((req.page - 1) * req.pageSize)
                .Take(req.pageSize)
                .ToListAsync();

            var userDtos = mapper.Map<IEnumerable<UserQueryDto>>(users);

            return new PaginatedResult<UserQueryDto>(userDtos, total);
        }
    }
}
