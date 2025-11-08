using MediatR;

namespace AutomotiveApp.Application.Features.Analytics.Queries
{
    public sealed record GetCoursesCount() : IRequest<int>;
}
