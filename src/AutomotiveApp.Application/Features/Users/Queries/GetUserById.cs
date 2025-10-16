using AutomotiveApp.Shared.Dtos.User;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public record GetUserById(Guid UserId) : IRequest<UserProfileDto>;
}
