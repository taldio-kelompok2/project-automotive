using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Command
{
    public record SendConfirmEmailCommand(string Email) : IRequest<bool>;
}
