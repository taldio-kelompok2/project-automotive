using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public class GetUserByIdHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<GetUserById, UserQueryDto>
    {
        public async Task<UserQueryDto> Handle(GetUserById req, CancellationToken ct)
        {

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == req.UserId && u.Status);
            if (user == null)
                throw new KeyNotFoundException($"User with Id: {req.UserId} not found");

            return mapper.Map<UserQueryDto>(user);
        }
    }
}
