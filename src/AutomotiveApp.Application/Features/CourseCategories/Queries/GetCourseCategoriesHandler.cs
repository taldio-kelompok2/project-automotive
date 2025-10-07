using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Queries
{
    public class GetCourseCategoriesHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseCategories, IEnumerable<CourseCategoryQueryDto>>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CourseCategoryQueryDto>> Handle(GetCourseCategories request, CancellationToken ct)
        {
            var items = await _uow.CourseCategoryRepo.GetAllAsync(ct: ct);
            var mappedItems = _mapper.Map<IEnumerable<CourseCategoryQueryDto>>(items).ToList();

            return mappedItems;
        }
    }

}