using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class CreateUserHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<CreateUser, bool>
    {
        public async Task<bool> Handle(CreateUser req, CancellationToken ct)
        {

            var existingUser = await userManager.FindByEmailAsync(req.UserCreateDto.Email);
            if (existingUser != null)
            {
                throw new Exception($"error: user with email {req.UserCreateDto.Email} already exists");
            }

            User user = mapper.Map<User>(req.UserCreateDto);
            var result = await userManager.CreateAsync(user, req.UserCreateDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new Exception($"error: {errors}");
            }

            await userManager.AddToRoleAsync(user, req.UserCreateDto.Role);
            
            return true;
        }
    }
}
