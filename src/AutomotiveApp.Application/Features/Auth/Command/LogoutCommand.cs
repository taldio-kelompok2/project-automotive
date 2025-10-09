using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record LogoutCommand(string UserId)
        : IRequest<bool>;
}
