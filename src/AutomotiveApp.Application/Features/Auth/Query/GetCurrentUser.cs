using AutomotiveApp.Shared.Dtos.Auth;
using MediatR;

namespace AutomotiveApp.Application.Features.Auth.Query
{
    public record GetCurrentUser(Guid Id) : IRequest<CurrentUserResponseDto>;
}
