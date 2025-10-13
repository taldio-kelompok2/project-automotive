using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record LoginCommand(LoginRequestDto LoginRequestDto)
        : IRequest<AuthResponseDto>;
}
