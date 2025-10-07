using AutoMapper;
using AutomotiveApp.Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public class CreateUserHandler(UserManager<User> userManager, IMapper mapper)
        : IRequestHandler<CreateUser, Guid>
    {
        public async Task<Guid> Handle(CreateUser req, CancellationToken ct)
        {
            User user = mapper.Map<User>(req.UserCreateDto);

            var result = await userManager.CreateAsync(user, req.UserCreateDto.Password);

            if (!result.Succeeded)
            {
                Console.WriteLine($"{result.Errors.Select(c => c.Code)}");
                throw new Exception($"error: {result.Errors.Select(e => e.Description)}");
            }
            
            return user.Id;
        }
    }
}
