using AutomotiveApp.Shared.Dtos.User;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Commands
{
    public record UpdateUserProfile(UserProfileUpdateDto UserProfileUpdateDto)
        : IRequest<bool>;
}
