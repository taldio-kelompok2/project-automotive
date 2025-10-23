using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Enums;
using AutomotiveApp.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Application.Features.Auth.Query
{
    public class GetCurrentUserHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<GetCurrentUser, CurrentUserResponseDto>
    {
        public async Task<CurrentUserResponseDto> Handle(GetCurrentUser req, CancellationToken ct)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == req.Id, ct)
            ?? throw new NotFoundException<User>(req.Id);

            var mappedUser = mapper.Map<CurrentUserResponseDto>(user);
            mappedUser.IsAuthenticated = true;

            var roles = await userManager.GetRolesAsync(user);
            mappedUser.Role = roles.FirstOrDefault() ?? UserRole.Buyer.ToString();

            return mappedUser;
        }
    }
}
