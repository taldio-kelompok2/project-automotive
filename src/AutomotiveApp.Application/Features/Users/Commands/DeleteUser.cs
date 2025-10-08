using MediatR;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public record DeleteUser(Guid Id) : IRequest<bool>;
}
