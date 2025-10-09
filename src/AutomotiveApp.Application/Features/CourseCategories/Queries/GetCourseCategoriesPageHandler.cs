using AutoMapper;
using AutomotiveApp.Application.Features.CourseCategories.Queries;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public class GetCourseCategoriesPagedHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseCategoriesPaged, PaginatedResult<CourseCategoryQueryDto>>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<CourseCategoryQueryDto>> Handle(GetCourseCategoriesPaged request, CancellationToken ct)
        {
            try
            {
                var (items, total) = await _uow.CourseCategoryRepo.GetAllPagedAsync(
                        page: request.Page,
                        itemTaken: request.ItemTaken,
                        ct: ct
                    );
                var mappedItems = _mapper.Map<IEnumerable<CourseCategoryQueryDto>>(items);
                return new PaginatedResult<CourseCategoryQueryDto>(mappedItems, total);
            }
            catch
            {
                throw;
            }
        }
    }
}