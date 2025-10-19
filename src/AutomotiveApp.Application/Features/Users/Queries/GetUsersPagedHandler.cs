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
            var query = userManager.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                var searchTerm = req.Search.Trim().ToLower();

                query = query.Where(u =>
                    u.UserName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm)
                 );
            }
            var total = await query.CountAsync();
            if (total < 0)
                throw new KeyNotFoundException($"No users found");

            var users = await query
                .OrderBy(u => u.UserName)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            var userDtos = mapper.Map<IEnumerable<UserQueryDto>>(users);

            return new PaginatedResult<UserQueryDto>(userDtos, total);
        }
    }
}
