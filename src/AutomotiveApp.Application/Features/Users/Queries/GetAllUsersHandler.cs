using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetAllUsersHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<GetAllUsers, IEnumerable<UserQueryDto>>
    {
        public async Task<IEnumerable<UserQueryDto>> Handle(GetAllUsers req, CancellationToken cancellationToken)
        {
            var users = userManager.Users.ToList();

            var userDtos = mapper.Map<List<UserQueryDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                var roles = await userManager.GetRolesAsync(users[i]);
                userDtos[i].Roles = [.. roles];
            }

            return userDtos;
        }
    }
}