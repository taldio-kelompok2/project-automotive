using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Shared.Dtos.Analytics;
using MediatR;

namespace AutomotiveApp.Application.Features.Analytics.Queries
{
    public sealed class GetCourseAnalyticsOverviewHandler(ICourseRepository courseRepository)
        : IRequestHandler<GetCourseAnalyticsOverview, AnalyticsOverviewDto>
    {
        public async Task<AnalyticsOverviewDto> Handle(GetCourseAnalyticsOverview request, CancellationToken cancellationToken)
        {
            var count = await courseRepository.CountAsync(ct: cancellationToken);

            return new AnalyticsOverviewDto
            {
                CoursesCount = count,
                Items =
                {
                    new AnalyticsCardItemDto
                    {
                        Key = "courses",
                        Label = "Courses",
                        Value = count,
                        Suffix = "+" 
                    }
                }
            };
        }
    }
}
