using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Queries
{
    public class GetCourseCategoryByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetCourseCategoryById, CourseCategoryQueryDto>
    {
        public async Task<CourseCategoryQueryDto> Handle(GetCourseCategoryById request, CancellationToken cancellationToken)
        {
            var item = await uow.CourseCategoryRepo.GetByIdAsync(request.Id, ct: cancellationToken);
            return mapper.Map<CourseCategoryQueryDto>(item);
        }
    }
}