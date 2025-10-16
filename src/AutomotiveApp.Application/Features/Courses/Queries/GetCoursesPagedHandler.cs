using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCoursesPagedHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCoursesPaged, PaginatedResult<CourseQueryDto>>
    {
        public async Task<PaginatedResult<CourseQueryDto>> Handle(GetCoursesPaged request, CancellationToken ct)
        {
            try
            {
                var (items, total) = await uow.CourseRepo.GetCoursesWithCategoryPaged(
                    page: request.Page,
                    itemTaken: request.ItemTaken,
                    isRandom: request.IsRandom,
                    ct: ct
                );

                var mappedItems = mapper.Map<IEnumerable<CourseQueryDto>>(items);
                return new PaginatedResult<CourseQueryDto>(mappedItems, total);
            }
            catch
            {
                throw;
            }
        }
    }
}