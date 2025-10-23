using AutomotiveApp.Shared.Dtos.User;
using MediatR;

namespace AutomotiveApp.Application.Features.Users.Queries
{
    public record GetDashboardTransactionsPaged(
        int Page = 1,
        int PageSize = 6
    ) : IRequest<List<DashboardUserDto>>;
}
