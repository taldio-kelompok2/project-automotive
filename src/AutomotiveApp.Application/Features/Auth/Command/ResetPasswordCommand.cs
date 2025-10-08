using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<bool>;
}
