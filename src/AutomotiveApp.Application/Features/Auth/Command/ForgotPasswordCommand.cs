using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record ForgotPasswordCommand(string Email) : IRequest<bool>;
}
