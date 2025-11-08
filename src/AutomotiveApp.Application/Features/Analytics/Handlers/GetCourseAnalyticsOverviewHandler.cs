using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Shared.Dtos.Analytics;
using MediatR;

namespace AutomotiveApp.Application.Features.Analytics.Queries
{
    
    public sealed class GetCourseAnalyticsOverviewHandler(
    ICourseRepository courseRepository,
    IOrderRepository orderRepository)
    : IRequestHandler<GetCourseAnalyticsOverview, AnalyticsOverviewDto>
    {
        public async Task<AnalyticsOverviewDto> Handle(GetCourseAnalyticsOverview request, CancellationToken ct)
        {
            var courseCount = await courseRepository.CountAsync(ct: ct);
            var orderCount = await orderRepository.CountAsync(ct: ct);
            var partnerCount = 10; // TODO = static data temporary

            return new AnalyticsOverviewDto
            {
                CoursesCount = courseCount,
                OrdersCount = orderCount,
                PartnersCount = partnerCount,
                Items =
                {
                    new AnalyticsCardItemDto
                    {
                        Label = "Courses",
                        Value = courseCount,
                        Suffix = "+"
                    },
                    new AnalyticsCardItemDto
                    {
                        Label = "Professionals",
                        Value = orderCount,
                        Suffix = "+"
                    },
                    new AnalyticsCardItemDto
                    {
                        Label = "Partners",
                        Value = partnerCount,
                        Suffix = "+"
                    }
                }
            };
        }
    }

}
