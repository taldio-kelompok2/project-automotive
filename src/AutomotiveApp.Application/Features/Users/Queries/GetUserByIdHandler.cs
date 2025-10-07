using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetUserByIdHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<GetUserById, UserQueryDto>
    {
        public async Task<UserQueryDto> Handle(GetUserById req, CancellationToken ct)
        {

            var user = await userManager.FindByIdAsync(req.UserId.ToString());
            if (user == null)
                throw new KeyNotFoundException($"User with Id: {req.UserId} not found");

            return mapper.Map<UserQueryDto>(user);
        }
    }
}
