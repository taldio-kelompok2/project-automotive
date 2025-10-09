using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record RefreshTokenCommand(string RefreshToken, string AccessToken)
        : IRequest<AuthResponseDto>;
}
