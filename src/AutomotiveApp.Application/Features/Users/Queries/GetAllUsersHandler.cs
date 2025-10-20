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
        public async Task<IEnumerable<UserQueryDto>> Handle(GetAllUsers req, CancellationToken ct)
        {
            var users = userManager.Users.ToList();

            var userDtos = mapper.Map<IEnumerable<UserQueryDto>>(users);

            return userDtos;
        }
    }
}