using AutomotiveApp.Application.Interfaces.Repositories;
using MediatR;

namespace AutomotiveApp.Application.Features.Analytics.Queries
{
    public sealed class GetCoursesCountHandler(ICourseRepository courseRepository)
        : IRequestHandler<GetCoursesCount, int>
    {
        public async Task<int> Handle(GetCoursesCount request, CancellationToken cancellationToken)
        {
            var count = await courseRepository.CountAsync(ct: cancellationToken);
            return count;
        }
    }
}
