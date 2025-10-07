using AutomotiveApp.Shared.Dtos.User;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public record UpdatePassword(Guid Id, UpdatePasswordDto UpdatePasswordDto)
        : IRequest<bool>;
}
