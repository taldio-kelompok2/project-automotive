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
        public async Task<PaginatedResult<UserQueryDto>> Handle(GetUsersPaged req, CancellationToken cancellationToken)
        {
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                var searchTerm = req.Search.Trim().ToLowerInvariant();

                // safer and null-tolerant
                query = query.Where(u =>
                    EF.Functions.Like((u.UserName ?? string.Empty).ToLowerInvariant(), $"%{searchTerm}%") ||
                    EF.Functions.Like((u.Email ?? string.Empty).ToLowerInvariant(), $"%{searchTerm}%")
                );
            }

            var total = await query.CountAsync(cancellationToken);
            if (total == 0)
                throw new KeyNotFoundException("No users found");

            var users = await query
                .OrderBy(u => u.UserName)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync(cancellationToken);

            var userDtos = mapper.Map<IEnumerable<UserQueryDto>>(users);

            return new PaginatedResult<UserQueryDto>(userDtos, total);
        }
    }
}
