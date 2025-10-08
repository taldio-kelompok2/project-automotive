using AutomotiveApp.Shared.Dtos.User;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public record CreateUser(UserCreateDto UserCreateDto) 
        : IRequest<Guid>;
}
