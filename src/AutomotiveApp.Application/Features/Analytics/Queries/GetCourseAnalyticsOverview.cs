using AutomotiveApp.Shared.Dtos.Analytics;
using MediatR;

namespace AutomotiveApp.Application.Features.Analytics.Queries
{
    public sealed record GetCourseAnalyticsOverview() : IRequest<AnalyticsOverviewDto>;
}
