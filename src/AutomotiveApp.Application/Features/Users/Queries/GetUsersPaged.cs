using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    // size of page based on admin page
    public record GetUsersPaged(
         int Page = 1,
         int PageSize = 10,
         string? Search = null)
         : IRequest<PaginatedResult<UserQueryDto>>;
}
