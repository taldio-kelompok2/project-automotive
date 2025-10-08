using AutomotiveApp.Shared.Dtos.User;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public record UpdateUser(Guid Id, UserUpdateDto UserUpdateDto)
        : IRequest<bool>;
}
