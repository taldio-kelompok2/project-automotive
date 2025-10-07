using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    // size of page based on admin page
    public record GetUsersPaged(int page = 1, int pageSize = 10)
        : IRequest<PaginatedResult<UserQueryDto>>;
}
