using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record ConfirmEmailCommand(string UserId, string Token) : IRequest<bool>;
}
